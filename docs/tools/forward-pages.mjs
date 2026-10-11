// Usage: node forward-pages.mjs <built site dir> <output dir> <target base>
// Writes a forwarding page into <output dir> for every page of the built site, so old links keep working.

import fs from 'node:fs';
import path from 'node:path';

const [source, output, target] = process.argv.slice(2);

if (!source || !output || !target) {
  console.error('Usage: node forward-pages.mjs <built site dir> <output dir> <target base>');
  process.exit(1);
}

const base = target.endsWith('/') ? target : `${target}/`;

function* pages(dir, relative = '') {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    if (entry.name.startsWith('_') || entry.name === 'pagefind') {
      continue;
    }

    const child = path.join(relative, entry.name);
    if (entry.isDirectory()) {
      yield* pages(path.join(dir, entry.name), child);
    } else if (entry.name === 'index.html') {
      yield relative;
    }
  }
}

let count = 0;
for (const relative of pages(source)) {
  const url = base + (relative ? `${relative.split(path.sep).join('/')}/` : '');
  const html = `<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>This page has moved</title>
  <meta name="robots" content="noindex">
  <link rel="canonical" href="${url}">
  <meta http-equiv="refresh" content="0; url=${url}">
  <script>location.replace(${JSON.stringify(url)} + location.hash);</script>
</head>
<body>
  <p>This page has moved to <a href="${url}">${url}</a>.</p>
</body>
</html>
`;

  const file = path.join(output, relative, 'index.html');
  fs.mkdirSync(path.dirname(file), { recursive: true });
  fs.writeFileSync(file, html);
  count++;
}

console.log(`Wrote ${count} forwarding pages to ${output}, pointing at ${base}.`);
