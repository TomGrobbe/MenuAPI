# MenuAPI documentation (FiveM Enhanced)

The MenuAPI docs for **FiveM Enhanced**, built with [Astro Starlight](https://starlight.astro.build/)
and a custom "cartoon" theme (shared with the vMenu docs). This branch (`fivem-enhanced`) owns only
the Enhanced doc set; the Legacy and RedM doc sets live on their own branches.

The docs are published into the hub repo (`TomGrobbe/TomGrobbe.github.io`) and served at:

| URL | Content |
| --- | --- |
| `/menuapi/enhanced/`, `/mapi/enhanced/` | FiveM (Enhanced) docs |

`/mapi/` is kept for backwards compatibility with the old documentation URLs.

## Layout

- `docs/enhanced/`: the Astro Starlight project for the FiveM Enhanced docs.

Its base path is env-driven (`DOCS_BASE`) so it can be built for both the `/menuapi` and `/mapi` prefixes.

## Local development

Requires Node 20.19+ or 22.12+.

```sh
cd docs/enhanced
npm install
npm run dev
```

## How it deploys

`.github/workflows/docs.yml` builds the Enhanced project twice (once per prefix) and pushes the
result into the hub repo under `/menuapi/enhanced/` and `/mapi/enhanced/` only. It runs on pushes to
`fivem-enhanced` that touch `docs/**`, and via the manual **Run workflow** button.

Because the Legacy/RedM docs deploy from a different branch into the *same* hub repo, this workflow
only ever replaces the `enhanced/` subfolders; it never wipes the whole `/menuapi` or `/mapi` prefix.
The other branch's deploy is likewise scoped to its own subfolders, so the two never clobber each other.

## One-time setup

1. Add a **`HUB_DEPLOY_TOKEN`** secret to this repo (a fine-grained token scoped to the
   `TomGrobbe/TomGrobbe.github.io` repo with Contents: Read and write).
2. Leave this repo's own GitHub Pages **off** (docs are served from the hub site).

The `.github/workflows/build.yml` pipeline ignores `docs/**`, so documentation commits never trigger
a package build.
