---
title: "Changelog"
---

---------------

## FiveM Enhanced

:::caution[Heads up]
These are the changes in MenuAPI for FiveM Enhanced. If you are moving a resource over from the older (v3, non Enhanced) MenuAPI, this is the list of things you will have to deal with along the way.
:::

### Controllers and the mouse are rebindable too

:::danger[Breaking change]
`Menu.ButtonPressHandlers`, `Menu.ButtonPressHandler`, `Menu.ControlPressCheckType` and the
`Menu.InstructionalButtons` dictionary have been **removed**. Every one of them was built around a fixed
`Control`, which is exactly what this change is getting rid of. Use
[Menu.AddKeyBinding()](reference/menu/#addkeybinding) instead, and see the
[Migration guide](migration/#custom-controls-are-key-bindings-now).
:::


Every menu control is now a FiveM key binding, on all three devices. The controller buttons and the mouse used
to be read straight from the game's controls, which meant they were fixed and a player who did not like them
was stuck with them. They now sit in the same **Settings, Key Bindings** screen the keyboard controls have
always been in, each with its own row, so they can be changed or removed one at a time.

The defaults are the buttons the menu has always used, so nothing moves for a player who was happy with them.
The controller toggle is still a 400ms hold of the back/select button, on purpose, because a tap of that
button is the game's own interaction menu.

**Things that just behave differently now:**

- <kbd>Esc</kbd> goes back one menu, or closes the menu, the same as <kbd>Backspace</kbd> does. It used to do
  nothing at all while a menu was open, since MenuAPI stops it opening the pause menu.
- Holding the controller toggle button while a menu is open **closes** it. It used to only ever open a menu,
  which meant a controller could open a menu it then could not close.
- The scroll wheel moves one item per notch instead of scrolling faster the longer you keep scrolling. A wheel
  notch can not be held down, so a binding can only see the notch itself.
- Nothing polls the game's controls for menu input any more, with one exception: scrolling the wheel while
  holding <kbd>TAB</kbd> on foot is still left to the weapon wheel, and that check has to ask the game.

Lists, dynamic lists and sliders now get their own hint too. Whenever one of those rows is highlighted, a
left/right hint appears next to select and back, so it is obvious that the row has a value to change. Its text
is [ChangeValueButtonText](reference/menu/#properties) and it can be turned off per menu.

See [Key bindings](reference/keybindings/) for the full list.

### The menu is drawn without describing it sixty times a second

The NUI renderer used to build a full description of the open menu on every single frame, purely to
find out whether anything about it had changed. Nothing usually had, so almost all of that work, and
the few kilobytes of garbage that came with it, was thrown straight away again.

Now every property that is on screen says so when you write to it, and the description is only built
when something actually said it changed. A menu nobody is touching costs nothing at all beyond the
draw itself.

**Things you have to change in your own code:**

- `MenuListItem.ListItems` is now a `MenuItemList` instead of a `List<string>`. It is used the same
  way, and a `List<string>` can still be assigned straight to it, so most code needs no change. The
  two things that do: a variable typed `List<string> values = item.ListItems;` has to become
  `MenuItemList` (or call `.ToList()`), and the list you pass to the constructor is now **copied**,
  so change the values through `item.ListItems` rather than through your own copy of the list. See
  the [Migration guide](migration/).

**Things that just behave differently now:**

- If you build part of a row's text from something MenuAPI does not own, such as a label you resolve
  yourself, call `MenuController.RefreshNui()` when it changes. Everything MenuAPI owns already says
  so on its own.
- The instructional buttons bar fills its slots a few times a second instead of every frame. Swapping
  between keyboard and controller still changes the icons, it just no longer costs a dozen scaleform
  calls per frame to notice.

### Menus can be split into groups with a heading

There is a new item type, [SeparatorMenuItem](reference/menuitems/separatormenuitem/). It is a heading that labels the rows underneath it, so a long menu can be broken into readable groups without pushing anything into a submenu.

The text is centred properly, using the game's own text justification, and it sits on exactly the same baseline as the rows around it. `ShowArrows` is on by default and draws it as `↓ Protection ↓`, and turning it off leaves just the centred word.

The cursor scrolls onto a separator like any other row, and it highlights like any other row. What it will not do is act: select, left and right are all ignored, and without the error sound a disabled row would have made, because scrolling onto a label is not a mistake. Give it a description and that gets shown too, which is a free place to say what the whole group below is for.

Scrolling deliberately does not skip over them. A cursor that jumped rows feels like the menu is eating key presses, and it is worse to use than the row it was trying to save you from.

This is the thing people were building by hand: a disabled item whose text had been padded with spaces until it looked roughly centred. That never actually lined up, the padding had to be recalculated for the text, and pressing it gave you an error sound.

**Things you have to change in your own code:** nothing, `SeparatorMenuItem` is a new type. Nothing about how menus scroll or count their rows has changed.

**Things that just behave differently now:**

- If you were faking a heading with a space padded disabled `MenuItem`, swap it for a `SeparatorMenuItem` and drop both the padding and the arrows you were typing into the text yourself. Do not swap it if you were using that trick as a "there is nothing here" message in an otherwise empty menu, since a real heading is not what that is. Keep a disabled plain item for that.

### Menu banners can have their own font, and GTA Online's glare

The banner at the top of a menu used to be fixed. The only thing you could change about it was swapping the whole image out for your own. It now has three settings, all covered on the [Header styling](reference/menu/#header-styling) page.

`MenuTitleFont` picks the font the title is drawn in, from the game's own set. `MenuFont` has names for the ones worth using, so `menu.MenuTitleFont = MenuFont.Pricedown` gets you the Grand Theft Auto logo font. Any font id the game knows works, including one your resource registered itself, so a custom font goes straight in. The size and the vertical position are worked out per font, because fonts disagree about both and a shared number would leave half of them sitting crooked.

`MenuTitleAlignment` moves the title to the left, the centre or the right of the banner. This is separate from `MenuController.MenuAlignment`, which is about which side of the *screen* the menu is on.

`ShowHeaderGlare` draws the soft moving glow GTA Online has behind its own pause menu title, the one that drifts as you turn the camera. It is the game's own `mp_menu_glare` scaleform, so nothing is streamed and no NUI is involved. It is loaded the first time a menu asks for it and released when every menu closes.

All three exist twice: on a menu, where they are nullable, and on `MenuController` as `DefaultTitleFont`, `DefaultTitleAlignment` and `DefaultShowHeaderGlare`. Set the defaults once and every menu follows them, and any single menu can still say otherwise.

**Things you have to change in your own code:** nothing. All three are opt in.

**Things that just behave differently now:**

- An untouched banner is drawn very slightly smaller than before. The title size was one hardcoded number picked for the default font, and it is now the measured size for that font, which came out a little lower. Nothing moves, it is the same font in the same place.

### Menus can be removed again

Until now, anything you handed to MenuAPI stayed forever. There was no way to remove a menu, so a resource that rebuilt part of its menu structure while running slowly filled memory with menus nobody could open anymore. There are now two ways to clean up: `MenuController.RemoveMenu(menu)` for a single menu, and `MenuController.RemoveAllMenus()` for everything.

Removing a menu closes it if it is open, empties its buttons, forgets the event handlers you attached to it, and takes it out of `MenuController.Menus`. After that your own variable is the only thing still pointing at it, so letting go of that variable is enough for the game to clean it up.

The nice part is that you usually will not have to call any of this. Removing a button now takes the submenu behind it with it, because once the button is gone there is no way to reach that submenu. So `RemoveMenuItem()` and `ClearMenuItems()` clean up after themselves, which is exactly what a resource rebuilding a menu from live data was quietly leaking before.

**Things you have to change in your own code:** nothing.

**Things that just behave differently now:**

- Removing or clearing a button that was bound with `BindMenuItem()` also removes the menu it opened, unless another button still opens that same menu. If you were deliberately keeping a menu alive by holding your own reference to it while removing its button, bind it to a new button before removing the old one.
- Binding an already bound button to a different menu removes the menu it used to open, on the same "nothing points at it anymore" rule.
- Removing a button clears its `ParentMenu`, so an item you have taken out of a menu reports `Index` as -1 instead of still pointing into the menu it used to be in.
- `RemoveAllMenus()` clears `MainMenu` too, so register a menu again before expecting the toggle key to open anything.

### Coloured subtitles actually come out coloured

The subtitle and the counter are drawn in capitals, and MenuAPI was uppercasing the whole string to do it. The game's formatting tokens are lowercase, so `~r~` was going to the game as `~R~`, which it does not recognise, and the colour silently went missing. Uppercasing now skips whatever sits between a pair of tildes, so `menu.MenuSubtitle = "~r~Out of date"` renders red like it always should have.

**Things you have to change in your own code:** nothing. If you were working around this, you can stop.

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
