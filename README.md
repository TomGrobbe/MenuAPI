# MenuAPI

**C# Menu API for FiveM and RedM**

[![Discord](https://discordapp.com/api/guilds/285424882534187008/widget.png)](https://vespura.com/discord) [![CodeFactor](https://www.codefactor.io/repository/github/tomgrobbe/menuapi/badge)](https://www.codefactor.io/repository/github/tomgrobbe/menuapi) [![Patreon](https://img.shields.io/badge/donate-Patreon-orange.svg)](https://www.patreon.com/vespura)

Designed specifically as a replacement of **NativeUI**, MenuAPI features performance improvements, improved stability, better features, less bugs, full safezone alignment support for both left and right aligned menus and less (in my opinion) unnecessary features.

This has been coded from the ground up. Using decompiled scripts from GTA V to figure out what some undocumented natives were used for.

## Which package do I need?

MenuAPI comes in three packages. Pick the one that matches the game and the C# runtime your resource uses. All three always have the same version number, and they come out together.

| Package | Game | What it is built for |
| --- | --- | --- |
| `MenuAPI.FiveM.Enhanced` | FiveM Enhanced | .NET 10 with the new `CitizenFX.FiveM.Client` runtime |
| `MenuAPI.FiveM` | FiveM | .NET Framework 4.5.2 with the classic `CitizenFX.Core` runtime |
| `MenuAPI.RedM` | RedM | .NET Framework 4.5.2 with the RedM `CitizenFX.Core` runtime |

The two FiveM packages are built from the exact same code, so they offer the same features. Both draw the menu with NUI, which is a small web page that FiveM shows on top of the game. Your resource needs a `ui_page` and has to list the `menuapi` folder in its `fxmanifest.lua` for that to work. The TestMenu folder in this repository has a working example of both manifests.

The RedM package is the older MenuAPI, kept as it was, with a few fixes.

## Installation

Please refer to the documentation for installation instructions: [FiveM](https://docs.vespura.com/menuapi/fivem/setup/) or [RedM](https://docs.vespura.com/menuapi/redm/setup/).

## Building it yourself

There are two solutions in this repository.

1. `MenuAPI.FiveM.slnx` builds both FiveM packages. One project builds the same code twice, once for .NET 10 and once for .NET Framework 4.5.2. The few things that differ between the two FiveM runtimes live in the `MenuAPI/platform` folder.
2. `redm/MenuAPI.RedM.slnx` builds the RedM package. It is completely separate and shares nothing with the FiveM side.

Run `dotnet build` on either one. For the smaller web files, install Node and run `npm ci` once first. Without it the build still works, it just ships the web files unminified.

## Versions and branches

Work happens on the `develop` branch. Every push there publishes a prerelease of all three packages to NuGet, like `4.0.1-develop.57`. Merging into `main` publishes a real release: the next version number, a git tag, a GitHub release with the zips, all three packages on NuGet and a message on Discord.

By default every release raises the last number (4.0.0 becomes 4.0.1). Put `+semver: minor` or `+semver: major` in a commit message to raise the middle or first number instead. When every commit since the last release says `+semver: none`, a push to `main` releases nothing.

## Documentation

Documentation is available [here](https://docs.vespura.com/menuapi/).

Feel like contributing to the documentation? The docs source lives in the [`docs/`](docs/) folder of this repository, thanks!

## Copyright / License

Copyright © Tom Grobbe 2018-2026.

MenuAPI is licensed under the **[GNU Lesser General Public License v3.0 or later](LICENSE.md)** (`LGPL-3.0-or-later`).

The LGPL is written as a set of additional permissions on top of the GPL, so both texts are included: [LICENSE.md](LICENSE.md) (LGPL-3.0) and [LICENSE.GPL.md](LICENSE.GPL.md) (GPL-3.0).

In short — this is not legal advice, read the license itself:

- You may link MenuAPI into your own resource and ship it, **including closed-source and paid resources**. Your own code does not become LGPL.
- If you **modify MenuAPI itself** and distribute it, you must release those modifications under the LGPL.
- You must credit MenuAPI, keep the copyright and license notices intact, and make clear that MenuAPI is used and available under the LGPL.
- Your users must be able to relink your resource against a modified MenuAPI. Shipping `MenuAPI.dll` as its own file (rather than merging it into your assembly) satisfies this.
- It comes with **absolutely no warranty**. Nothing is guaranteed to work, I do not take responsibility for any bugs or damages caused by this code. Use at your own risk.

### Full notice

```
MenuAPI
Copyright (C) 2018-2026 Tom Grobbe

This library is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with this library.  If not, see <https://www.gnu.org/licenses/>.
```
