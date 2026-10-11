# MenuAPI documentation

This folder holds the MenuAPI documentation websites. Each site is built with [Astro Starlight](https://starlight.astro.build/), a tool that turns a folder of Markdown files into a website. They all share a custom "cartoon" look, the same one the vMenu docs use.

## What is where

| Folder | Website address | What it is |
| --- | --- | --- |
| `fivem/` | `/menuapi/fivem/` | The docs for MenuAPI 4.0 and newer on FiveM. One site covers both FiveM runtimes, Enhanced and Legacy. |
| `redm/` | `/menuapi/redm/` | The docs for MenuAPI on RedM. |
| `archive/legacy-v3/` | `/menuapi/legacy/` | The old docs for MenuAPI 3.x and older on FiveM. These are frozen, so please don't change them. Every page shows a banner that points people to the new FiveM docs. |
| `landing/` | `/menuapi/` | The page where people pick between FiveM and RedM. It's a single html file. |
| `tools/forward-pages.mjs` | `/menuapi/enhanced/` | A small script that fills the old Enhanced address with forwarding pages. Anyone with an old link to an Enhanced page lands on the same page in the new FiveM docs. |

Everything is also published a second time under `/mapi/` instead of `/menuapi/`. That was the address of the very first docs, and it's kept so old links keep working.

## Enhanced and Legacy on one site

MenuAPI works the same on both FiveM runtimes, so the FiveM site describes them together. Where the two differ, a page shows two tabs, one for Enhanced and one for Legacy. Once a reader picks a tab, every other page remembers that choice.

Tabs only work in `.mdx` files, which are Markdown files that can also use components. To add tabs to a page, rename it from `.md` to `.mdx` and copy how `fivem/src/content/docs/setup.mdx` does it. The `syncKey="runtime"` part is what makes the choice carry over between pages.

## Working on the docs

You need Node 20.19 or newer, or Node 22.12 or newer.

```sh
cd docs/fivem   # or docs/redm
npm install
npm run dev
```

`npm run dev` starts a local copy of the site and prints its address. The page reloads by itself whenever you save a file.

## How it gets published

`.github/workflows/docs.yml` publishes the docs. It runs whenever something in `docs/` changes on the `main` branch. You can also start it yourself with the **Run workflow** button on the Actions tab.

The workflow does the following:

1. It builds the three sites twice, once for `/menuapi/` and once for `/mapi/`. Astro writes the address into every link, which is why each address needs its own build.
2. It adds the landing page and the forwarding pages.
3. It copies everything into the hub repository, `TomGrobbe/TomGrobbe.github.io`. That one repository serves all the docs sites. Only the `menuapi/` and `mapi/` folders are replaced there, and everything else in the hub, like the vMenu docs, is left alone.

Changes that only touch `docs/` never start a package build. `build.yml` ignores this folder.

## One time setup

1. This repository needs a secret called **`HUB_DEPLOY_TOKEN`**. It's a GitHub token that can write to the `TomGrobbe/TomGrobbe.github.io` repository (Contents: Read and write). vMenu uses the same kind of token, but secrets are per repository, so it has to be added here too.
2. Leave GitHub Pages turned **off** for this repository. The hub site serves the docs instead.
