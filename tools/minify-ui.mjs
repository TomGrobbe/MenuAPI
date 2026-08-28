import { mkdir, readFile, readdir, rm, writeFile } from "node:fs/promises";
import { dirname, join, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const root = resolve(dirname(fileURLToPath(import.meta.url)), "..");
const source = join(root, "MenuAPI", "ui");
const target = join(root, "MenuAPI", "ui-dist");

const BANNER = "/*! MenuAPI (FiveM Enhanced) | LGPL-3.0-or-later | https://github.com/TomGrobbe/MenuAPI */\n";

async function loadEsbuild() {
    try {
        return await import("esbuild");
    } catch {
        return null;
    }
}

function outputName(name) {
    const dot = name.lastIndexOf(".");

    return `${name.slice(0, dot)}.min${name.slice(dot)}`;
}

async function main() {
    const entries = await readdir(source, { withFileTypes: true });

    const files = entries
        .filter(entry => entry.isFile() && (entry.name.endsWith(".css") || entry.name.endsWith(".js")))
        .map(entry => entry.name)
        .sort();

    if (files.length === 0) {
        throw new Error(`No .css or .js files found in ${source}`);
    }

    const esbuild = await loadEsbuild();

    if (!esbuild) {
        console.warn("[minify-ui] esbuild is not installed, copying the sources unminified instead. Run `npm ci` for smaller files.");
    }

    await rm(target, { recursive: true, force: true });
    await mkdir(target, { recursive: true });

    let before = 0;
    let after = 0;

    for (const name of files) {
        const code = await readFile(join(source, name), "utf8");

        let minified = code;

        if (esbuild) {
            // Safe only because esbuild leaves top level names alone when not bundling, and
            // these scripts hand each other classes through the shared global scope.
            const result = await esbuild.transform(code, {
                loader: name.endsWith(".css") ? "css" : "js",
                minify: true,
                legalComments: "none",
                target: "chrome110",
            });

            for (const warning of result.warnings) {
                console.warn(`[minify-ui] ${name}: ${warning.text}`);
            }

            minified = result.code;
        }

        const output = BANNER + minified;

        await writeFile(join(target, outputName(name)), output, "utf8");

        before += Buffer.byteLength(code);
        after += Buffer.byteLength(output);

        console.log(`[minify-ui] ${name} -> ${outputName(name)}  ${Buffer.byteLength(code)} -> ${Buffer.byteLength(output)} bytes`);
    }

    const saved = before === 0 ? 0 : Math.round(((before - after) / before) * 100);

    console.log(`[minify-ui] ${files.length} files, ${before} -> ${after} bytes (${saved}% smaller)`);
}

await main();
