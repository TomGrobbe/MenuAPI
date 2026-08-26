import fs from "node:fs";
import http from "node:http";
import path from "node:path";
import { fileURLToPath } from "node:url";

const here = path.dirname(fileURLToPath(import.meta.url));
const ui = path.resolve(here, "../../MenuAPI/ui");
const port = Number(process.argv[2] ?? 8730);

const TYPES = {
    ".css": "text/css",
    ".html": "text/html; charset=utf-8",
    ".js": "text/javascript",
    ".png": "image/png",
    ".woff": "font/woff",
    ".woff2": "font/woff2",
};

if (!fs.existsSync(ui)) {
    console.error(`cannot find ${ui}`);
    process.exit(1);
}

function resolve(url) {
    const clean = decodeURIComponent(url.split("?")[0]).replace(/^\/+/, "") || "index.html";

    const target = clean.startsWith("menuapi/")
        ? path.join(ui, clean.slice("menuapi/".length))
        : path.join(here, clean);

    const resolved = path.resolve(target);

    return resolved.startsWith(path.resolve(ui)) || resolved.startsWith(path.resolve(here))
        ? resolved
        : null;
}

http.createServer((request, response) => {
    const file = resolve(request.url ?? "/");

    if (!file || !fs.existsSync(file) || !fs.statSync(file).isFile()) {
        response.writeHead(404).end("not found");

        return;
    }

    response.writeHead(200, {
        "content-type": TYPES[path.extname(file).toLowerCase()] ?? "application/octet-stream",
        "cache-control": "no-store",
    });

    response.end(fs.readFileSync(file));
}).listen(port, () => {
    console.log(`MenuAPI NUI preview on http://localhost:${port}`);
    console.log(`serving menuapi/ from ${ui}`);
});
