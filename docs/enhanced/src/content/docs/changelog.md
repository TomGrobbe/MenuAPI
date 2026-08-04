---
title: "Changelog"
---

---------------

## FiveM Enhanced alphas

:::caution[Heads up]
These are the changes in the MenuAPI FiveM Enhanced alpha packages. If you are moving a resource over from the older (v3, non Enhanced) MenuAPI, this is the list of things you will have to deal with along the way. The final Enhanced release is not ready yet, so more of these can still show up.
:::

### Keyboard controls are now FiveM key bindings

Every keyboard menu control is registered as a proper FiveM key mapping, so players can rebind all of them from their own **Settings, Key Bindings** screen. Full details on the [Key bindings](reference/keybindings/) page.

**Things you have to change in your own code:**

- `MenuController.MenuToggleKey` and `MenuController.MenuToggleKeyIsValid` are **gone**. The key that opens the menu is now the player's choice. A resource can still pick the starting default with the new `MenuController.MenuToggleKeyDefault` (a key name like `"M"` or `"F5"`, set in your constructor), but that only applies to players who have never rebound it.
- `Menu.InstructionalButtons` now starts **empty**. It used to come with a select and a back entry in it. Those two moved to their own properties, because on a keyboard they follow the player's binding instead of a fixed `Control`. Use the new `Menu.SelectButtonText`, `Menu.BackButtonText`, `Menu.ShowSelectInstructionalButton` and `Menu.ShowBackInstructionalButton` instead.

**Things that just behave differently now:**

- Right click goes back, matching left click selecting.
- <kbd>Escape</kbd> no longer opens the pause menu while a MenuAPI menu is open. That was always the intent, but a keyboard/controller check was the wrong way around so it never actually happened. <kbd>P</kbd> still opens the pause menu.
- `MenuController.PreventExitingMenu` really does stop a top level menu being closed with the back button now. The branch that handled it could never be reached.
- A few controls that were meant to be disabled only for controller players were being disabled only for keyboard players, and the other way around. Same wrong check as the Escape issue.

---------------

## Versions v3.0.0 - v3.0.3

Automatic package upload to NuGet added. MenuAPI is now availabe on NuGet! Simply search for MenuAPI.FiveM.Enhanced.

There's also been bug fixes. Things like instructional buttons, different button types, sound effects, different font styles etc.

Full changelog available here: https://github.com/TomGrobbe/MenuAPI/compare/v2.2.0...v3.0.3


---------------

## Versions 1.0.1 - v2.2.0
Lots of bug fixes, new features and more. I know this isn't a great changelog, but it's all nicely listed in git logs.
For a full changelog for these versions see: https://github.com/TomGrobbe/MenuAPI/compare/v1.0.1...v2.2.0
