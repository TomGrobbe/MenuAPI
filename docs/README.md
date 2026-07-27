# MenuAPI documentation

The MenuAPI docs, built with [Astro Starlight](https://starlight.astro.build/) and
a custom "cartoon" theme (shared with the vMenu docs). The docs are published into
the hub repo (`TomGrobbe/TomGrobbe.github.io`) and served at:

| URL | Content |
| --- | --- |
| `/menuapi/`, `/mapi/` | Version chooser (FiveM Legacy, FiveM Enhanced, RedM) |
| `/menuapi/legacy/`, `/mapi/legacy/` | FiveM (Legacy) docs |
| `/menuapi/redm/`, `/mapi/redm/` | RedM docs |
| `/menuapi/enhanced/`, `/mapi/enhanced/` | FiveM (Enhanced) docs (deployed from the `fivem-enhanced` branch) |

`/mapi/` is a full copy of `/menuapi/`, kept for backwards compatibility with the
old documentation URLs. (Today under `wiki.vespura.com`, later `docs.vespura.com`.)

## Layout

- `docs/legacy/` — Astro Starlight project for the FiveM docs.
- `docs/redm/` — Astro Starlight project for the RedM docs (a copy of legacy,
  adjusted for RedM's reduced feature set).
- `docs/landing/` — the static chooser page. (The `enhanced/` docs are built and deployed
  from the `fivem-enhanced` branch, not here.)

Each project's base path is env-driven (`DOCS_BASE`) so it can be built for both
the `/menuapi` and `/mapi` prefixes.

## Local development

Requires Node 20.19+ or 22.12+.

```sh
cd docs/legacy   # or docs/redm
npm install
npm run dev
```

## How it deploys

`.github/workflows/docs.yml` builds each project twice (once per prefix), assembles
`/menuapi/**` and `/mapi/**`, and pushes them into the hub repo. It runs on pushes
to `master` that touch `docs/**`, and via the manual **Run workflow** button.

When publishing, it replaces only the folders this branch owns and deliberately keeps
each prefix's `enhanced/` subfolder intact, because those docs are deployed separately
from the `fivem-enhanced` branch into the same hub repo. The two deploys never overlap.

## One-time setup

1. Add a **`HUB_DEPLOY_TOKEN`** secret to this repo (the same fine-grained token
   used for vMenu, scoped to the `TomGrobbe/TomGrobbe.github.io` repo with
   Contents: Read and write). Secrets are per repo.
2. Leave this repo's own GitHub Pages **off** (docs are served from the hub site).

The `.github/workflows/build.yml` pipeline (which publishes to NuGet on `master`)
ignores `docs/**`, so documentation commits never trigger a package publish.
