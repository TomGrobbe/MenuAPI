---
title: "Changelog"
---

---------------

## FiveM Enhanced alphas

:::caution[Heads up]
These are the changes in the MenuAPI FiveM Enhanced alpha packages. If you are moving a resource over from the older (v3, non Enhanced) MenuAPI, this is the list of things you will have to deal with along the way. The final Enhanced release is not ready yet, so more of these can still show up.
:::

### Menus can be split into pages

A menu can now hold thousands of items without the player having to scroll through all of them. Call `menu.SetPageSize(48)` and the menu is split into pages of 48, moved between with left and right. Everything is on the new [Pagination](reference/pagination/) page.

**Things you have to change in your own code:**

- Nothing. Pagination is off by default, and a menu that never calls `SetPageSize()` behaves exactly as it did.

**Things that just behave differently now:**

- In a menu you *have* paginated, left and right turn the page instead of going back and selecting. List, dynamic list and slider items are unaffected, they still change their own value.
- `Size`, `CurrentIndex`, `GetMenuItems()` and `GetCurrentMenuItem()` describe the current page of a paginated menu, not the whole item list.
- Two long standing bugs got fixed on the way. `ClearMenuItems()` used to leave an active filter switched on, so an emptied menu stayed stuck at `Size` 0 even after you refilled it. And `RemoveMenuItem(int)` counted against the filtered list while indexing the unfiltered one, so it could remove the wrong item while a filter was active.

### Menus stop costing anything while they are closed

MenuAPI used to run seven loops that never stopped. Four of them did real work on every single frame even with every menu closed, which added up to roughly 24 wasted calls into the game per frame, forever. All of that work now sits behind a small scheduler, and a loop that is switched off genuinely **ends** instead of running and immediately returning. With no menu open, MenuAPI does almost nothing at all.

You can see this for yourself. Type `menuapi:yourresourcename:ticks` in the console and it prints every loop and whether it is running. You can also read the same thing from code through the new [MenuTicks](reference/ticks/) class, which is handy if your resource has its own debug overlay.

**Things you have to change in your own code:**

- `MenuController.EnableManualGCs` is **gone**, along with the manual garbage collect it controlled. The .NET runtime handles this on its own, and forcing a collect on the game thread was a stutter nobody asked for.
- MenuAPI is now built with **nullable reference types** switched on, so things that really can be null now say so. `MenuController.GetCurrentMenu()`, `MenuController.MainMenu`, `Menu.ParentMenu`, `Menu.GetCurrentMenuItem()`, `Menu.MenuTitle`, `Menu.MenuSubtitle`, `Menu.CounterPreText`, `MenuItem.Label`, `MenuItem.Description` and `MenuDynamicListItem.CurrentItem` are all nullable now. If your own resource has nullable switched on too, you may get new warnings where you use one of these without checking it first. Those warnings are pointing at real crashes waiting to happen, so it is worth fixing them rather than silencing them. Nothing needs to change if you do not use nullable yourself.

**Things that just behave differently now:**

- The controller open gesture is checked ten times a second instead of every frame. It is a 400ms hold, so it still opens at exactly the same moment.
- A few genuine crashes are fixed along the way. Holding a direction key while something else closed the menu could crash, and so could selecting an item that opens a submenu if a handler closed the menu first.
- Menu textures stay loaded while the pause menu is open instead of being thrown away and re-requested. Nothing visible changes, there is just less streaming churn.

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
