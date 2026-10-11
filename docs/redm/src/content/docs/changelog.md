---
title: "Changelog"
---

---------------

## 4.0.0

MenuAPI now has one version number for everything. The RedM, FiveM Enhanced and FiveM Legacy packages always come out together, with the same version. That is why RedM jumps from 3.2 to 4.0, even though very little changed for RedM itself.

### The toggle key works again

The built in toggle key never opened a menu in RedM. MenuAPI checked the key against the control numbers FiveM uses, and RedM controls are hashes, so every RedM key failed that check without any error. Now any RedM control works, and setting it to `(Control)0` turns the key off. See [The menu toggle key](../reference/menucontroller/#the-menu-toggle-key).

Nothing else changed, so updating from 3.x needs no code changes.

---------------

## Versions v3.0.0 - v3.0.3
### RedM support

Main difference here is that there are now 2 DLL files. One for RedM, and one for FiveM. Obviously choose the one matching your project.

Automatic package upload to NuGet added. MenuAPI is now availabe on NuGet! Simply search for MenuAPI.FiveM for the FiveM version, or MenuAPI.RedM for the RedM version.

There's also been bug fixes and old functionalities re-added from the "normal" (FiveM) version of MenuAPI, which were removed at first for RedM because they weren't supported. But now they are! Things like instructional buttons, different button types, sound effects, different font styles etc.

Full changelog available here: https://github.com/TomGrobbe/MenuAPI/compare/v2.2.0...v3.0.3


---------------

## Versions 1.0.1 - v2.2.0
Lots of bug fixes, new features and more. I know this isn't a great changelog, but it's all nicely listed in git logs.
For a full changelog for these versions see: https://github.com/TomGrobbe/MenuAPI/compare/v1.0.1...v2.2.0
