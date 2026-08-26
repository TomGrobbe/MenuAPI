import fs from "node:fs";
import path from "node:path";
import zlib from "node:zlib";

const DDS_MAGIC = 0x20534444;
const HEADER_SIZE = 128;

const PF_FLAGS = 80;
const PF_FOURCC = 84;
const PF_RGB_BIT_COUNT = 88;
const PF_FLAG_FOURCC = 0x4;

function decode(buffer) {
    if (buffer.readUInt32LE(0) !== DDS_MAGIC) {
        throw new Error("not a DDS file");
    }

    const height = buffer.readUInt32LE(12);
    const width = buffer.readUInt32LE(16);
    const compressed = (buffer.readUInt32LE(PF_FLAGS) & PF_FLAG_FOURCC) !== 0;
    const fourCC = buffer.toString("ascii", PF_FOURCC, PF_FOURCC + 4);
    const pixels = Buffer.alloc(width * height * 4);

    if (!compressed) {
        const bits = buffer.readUInt32LE(PF_RGB_BIT_COUNT);

        if (bits !== 32) {
            throw new Error(`unsupported uncompressed depth ${bits}`);
        }

        for (let i = 0; i < width * height; i++) {
            const at = HEADER_SIZE + (i * 4);

            pixels[(i * 4) + 0] = buffer[at + 2];
            pixels[(i * 4) + 1] = buffer[at + 1];
            pixels[(i * 4) + 2] = buffer[at + 0];
            pixels[(i * 4) + 3] = buffer[at + 3];
        }

        return { width, height, pixels };
    }

    if (fourCC !== "DXT1" && fourCC !== "DXT3" && fourCC !== "DXT5") {
        throw new Error(`unsupported format ${fourCC}`);
    }

    const blockBytes = fourCC === "DXT1" ? 8 : 16;
    const blocksWide = Math.max(1, Math.ceil(width / 4));
    const blocksHigh = Math.max(1, Math.ceil(height / 4));

    let at = HEADER_SIZE;

    for (let by = 0; by < blocksHigh; by++) {
        for (let bx = 0; bx < blocksWide; bx++) {
            const block = buffer.subarray(at, at + blockBytes);

            at += blockBytes;

            const colour = fourCC === "DXT1" ? block : block.subarray(8);
            const alpha = decodeAlpha(fourCC, block);

            writeColourBlock(pixels, width, height, bx, by, colour, alpha, fourCC === "DXT1");
        }
    }

    return { width, height, pixels };
}

function decodeAlpha(fourCC, block) {
    if (fourCC === "DXT1") {
        return null;
    }

    const out = new Uint8Array(16);

    if (fourCC === "DXT3") {
        for (let i = 0; i < 16; i++) {
            const nibble = (block[i >> 1] >> ((i & 1) * 4)) & 0xf;

            out[i] = (nibble * 255) / 15;
        }

        return out;
    }

    const a0 = block[0];
    const a1 = block[1];
    const ramp = new Uint8Array(8);

    ramp[0] = a0;
    ramp[1] = a1;

    if (a0 > a1) {
        for (let i = 2; i < 8; i++) {
            ramp[i] = ((8 - i) * a0 + (i - 1) * a1) / 7;
        }
    } else {
        for (let i = 2; i < 6; i++) {
            ramp[i] = ((6 - i) * a0 + (i - 1) * a1) / 5;
        }

        ramp[6] = 0;
        ramp[7] = 255;
    }

    for (let half = 0; half < 2; half++) {
        const base = 2 + (half * 3);
        const bits = block[base] | (block[base + 1] << 8) | (block[base + 2] << 16);

        for (let i = 0; i < 8; i++) {
            out[(half * 8) + i] = ramp[(bits >> (i * 3)) & 0x7];
        }
    }

    return out;
}

function writeColourBlock(pixels, width, height, bx, by, colour, alpha, punchThrough) {
    const c0 = colour.readUInt16LE(0);
    const c1 = colour.readUInt16LE(2);
    const bits = colour.readUInt32LE(4);

    const r = new Uint8Array(4);
    const g = new Uint8Array(4);
    const b = new Uint8Array(4);
    const a = new Uint8Array(4).fill(255);

    unpack565(c0, r, g, b, 0);
    unpack565(c1, r, g, b, 1);

    const opaque = !punchThrough || c0 > c1;

    if (opaque) {
        for (const channel of [r, g, b]) {
            channel[2] = ((2 * channel[0]) + channel[1]) / 3;
            channel[3] = (channel[0] + (2 * channel[1])) / 3;
        }
    } else {
        for (const channel of [r, g, b]) {
            channel[2] = (channel[0] + channel[1]) / 2;
            channel[3] = 0;
        }

        a[3] = 0;
    }

    for (let y = 0; y < 4; y++) {
        for (let x = 0; x < 4; x++) {
            const px = (bx * 4) + x;
            const py = (by * 4) + y;

            if (px >= width || py >= height) {
                continue;
            }

            const i = (y * 4) + x;
            const code = (bits >> (i * 2)) & 0x3;
            const at = ((py * width) + px) * 4;

            pixels[at + 0] = r[code];
            pixels[at + 1] = g[code];
            pixels[at + 2] = b[code];
            pixels[at + 3] = alpha ? alpha[i] : a[code];
        }
    }
}

function unpack565(value, r, g, b, index) {
    const r5 = (value >> 11) & 0x1f;
    const g6 = (value >> 5) & 0x3f;
    const b5 = value & 0x1f;

    r[index] = (r5 << 3) | (r5 >> 2);
    g[index] = (g6 << 2) | (g6 >> 4);
    b[index] = (b5 << 3) | (b5 >> 2);
}

function encodePng({ width, height, pixels }) {
    const raw = Buffer.alloc((width * 4 + 1) * height);

    for (let y = 0; y < height; y++) {
        raw[y * (width * 4 + 1)] = 0;
        pixels.copy(raw, (y * (width * 4 + 1)) + 1, y * width * 4, (y + 1) * width * 4);
    }

    const ihdr = Buffer.alloc(13);

    ihdr.writeUInt32BE(width, 0);
    ihdr.writeUInt32BE(height, 4);
    ihdr[8] = 8;
    ihdr[9] = 6;
    ihdr[10] = 0;
    ihdr[11] = 0;
    ihdr[12] = 0;

    return Buffer.concat([
        Buffer.from([0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a]),
        chunk("IHDR", ihdr),
        chunk("IDAT", zlib.deflateSync(raw, { level: 9 })),
        chunk("IEND", Buffer.alloc(0)),
    ]);
}

function chunk(type, data) {
    const out = Buffer.alloc(data.length + 12);

    out.writeUInt32BE(data.length, 0);
    out.write(type, 4, "ascii");
    data.copy(out, 8);
    out.writeUInt32BE(crc32(out.subarray(4, out.length - 4)), out.length - 4);

    return out;
}

const CRC_TABLE = (() => {
    const table = new Uint32Array(256);

    for (let i = 0; i < 256; i++) {
        let value = i;

        for (let bit = 0; bit < 8; bit++) {
            value = value & 1 ? 0xedb88320 ^ (value >>> 1) : value >>> 1;
        }

        table[i] = value >>> 0;
    }

    return table;
})();

function crc32(buffer) {
    let crc = 0xffffffff;

    for (const byte of buffer) {
        crc = CRC_TABLE[(crc ^ byte) & 0xff] ^ (crc >>> 8);
    }

    return (crc ^ 0xffffffff) >>> 0;
}

const source = process.argv[2];
const target = process.argv[3] ?? "MenuAPI/ui/sprites";

if (!source) {
    console.error('usage: node tools/dds-to-png.mjs "<dump folder>" [output folder]');
    process.exit(1);
}

let converted = 0;
let failed = 0;

for (const entry of fs.readdirSync(source, { withFileTypes: true })) {
    if (!entry.isDirectory()) {
        continue;
    }

    const from = path.join(source, entry.name);
    const to = path.join(target, entry.name);

    fs.mkdirSync(to, { recursive: true });

    for (const file of fs.readdirSync(from)) {
        if (!file.toLowerCase().endsWith(".dds")) {
            continue;
        }

        const out = path.join(to, `${path.basename(file, path.extname(file))}.png`);

        try {
            fs.writeFileSync(out, encodePng(decode(fs.readFileSync(path.join(from, file)))));
            converted++;
        } catch (error) {
            console.error(`  ${entry.name}/${file}: ${error.message}`);
            failed++;
        }
    }

    console.log(`${entry.name}`);
}

console.log(`\n${converted} converted, ${failed} failed, into ${target}`);
