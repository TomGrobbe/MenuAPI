---
title: "Migration guide"
---

## Migration guide

This page lists everything you have to change when you update a resource to MenuAPI 4.0. Each change has a "before" and "after" example. The [Changelog](../changelog/) explains why each of them changed.

Which part you need depends on where you are coming from:

- **You used MenuAPI for FiveM Enhanced 1.x.** Read [Everyone](#everyone) and [Coming from Enhanced 1.x](#coming-from-enhanced-1x).
- **You used MenuAPI 3.x or older** (the package called `MenuAPI.FiveM`, on the Legacy runtime). Read [Everyone](#everyone) and [Coming from MenuAPI 3.x](#coming-from-menuapi-3x). This is the bigger one, so take your time.

Anything not listed here still works the way it always did.

----

## Everyone

### Your resource needs the NUI page

Menus are always drawn with NUI now. NUI is a small web page that FiveM shows on top of the game. Your resource needs that page, plus a few lines in its `fxmanifest.lua`, or no menu shows up at all.

Follow steps 2 and 3 on the [Setup](../setup/#2-add-the-page-the-menu-is-drawn-on) page. They give you the `index.html` to copy and the lines to add to your manifest:

```lua
ui_page 'index.html'

files {
    'MenuAPI.dll',
    'index.html',
    'menuapi/**/*',
}
```

If you were already using NUI render mode on Enhanced, you already have all of this and can skip ahead.

----

## Coming from Enhanced 1.x

### Render mode does nothing anymore

`MenuController.RenderMode` and `MenuRenderMode` still exist, so your code keeps compiling, but setting them does nothing. You will get a warning saying so. Delete the line whenever it suits you.

```cs
// Before
MenuController.RenderMode = MenuRenderMode.Nui;

// After
// (nothing, NUI is the only way now)
```

### Removed because they only existed for native drawing

These were only used by the old way of drawing. Delete any line that uses them.

- `MenuController.SetDrawOrder`
- `Menu.MenuItemsYOffset`
- `GlareTuning`, the glare position tuner

If you made your own menu item type by inheriting from `MenuItem`, these protected members are gone too: `Width`, `RowHeight`, `GetSpriteSize()` and `GetSpriteX()`. `GetSpriteColour()` now returns a `System.Drawing.Color` instead of three numbers.

### The NUI web files are minified and renamed

This only matters if your `index.html` was written before Enhanced 1.0.20. The css and js files are minified now, which means all spaces and comments are stripped out to make them smaller, and their names end in `.min`. Update the six paths in your `index.html`:

```html
<!-- Before -->
<link rel="stylesheet" href="menuapi/colour-list.css">
<link rel="stylesheet" href="menuapi/menuapi.css">
<script src="menuapi/sprite-cache.js"></script>
<script src="menuapi/glare.js"></script>
<script src="menuapi/colour-list.js"></script>
<script src="menuapi/menuapi.js"></script>

<!-- After -->
<link rel="stylesheet" href="menuapi/colour-list.min.css">
<link rel="stylesheet" href="menuapi/menuapi.min.css">
<script src="menuapi/sprite-cache.min.js"></script>
<script src="menuapi/glare.min.js"></script>
<script src="menuapi/colour-list.min.js"></script>
<script src="menuapi/menuapi.min.js"></script>
```

Same files, same order, same behaviour.

----

## Coming from MenuAPI 3.x

Good news first: you stay on the Legacy runtime, your scripts stay `BaseScript`, and you keep using `CitizenFX.Core` and its `Control` enum. Just update the `MenuAPI.FiveM` package to 4.0. Then work through the changes below.

### The menu toggle key is the player's choice now

`MenuController.MenuToggleKey` and `MenuController.MenuToggleKeyIsValid` are gone. The key that opens the menu is now a FiveM key binding, so players can change it in **Settings, Key Bindings**. You can still pick the key they start with:

```cs
// Before
MenuController.MenuToggleKey = Control.InteractionMenu;

// After (a key name, and it only applies to players who never changed it themselves)
MenuController.MenuToggleKeyDefault = "M";
```

If your resource opens its menus itself and you used to turn the toggle key off, use `EnableMenuToggleKey` instead:

```cs
// Before
MenuController.MenuToggleKey = (Control)(-1);

// After
MenuController.EnableMenuToggleKey = false;
```

Set it in your script's constructor. MenuAPI registers its keys one tick after it starts, and checks this setting at that moment.

[Key bindings](../reference/keybindings/) explains the whole system.

### Custom controls are key bindings now

`Menu.ButtonPressHandlers`, `Menu.ButtonPressHandler` and `Menu.ControlPressCheckType` are gone. You now give a menu its own keys with [AddKeyBinding()](../reference/menu/#addkeybinding). That registers a real FiveM key binding, so the player can change it, and the hint at the bottom of the screen follows whatever key they picked.

```cs
// Before
menu.ButtonPressHandlers.Add(new Menu.ButtonPressHandler(
    Control.FrontendSocialClubSecondary,
    Menu.ControlPressCheckType.JUST_RELEASED,
    new Action<Menu, Control>((m, c) => DoThing()),
    true));

menu.InstructionalButtons.Add(Control.FrontendSocialClubSecondary, "Do the thing");

// After, the key and its hint in one call
menu.AddKeyBinding("dothing", "Do the thing", "K", "R1_INDEX",
    MenuKeyPressType.JUST_RELEASED,
    new Action<Menu, MenuKeyBinding>((m, b) => DoThing()),
    "Do the thing");
```

The arguments are: a short name, the text players see in their key bindings screen, the default keyboard key, the default controller button, when your code runs, your code, and the text of the hint.

The keyboard key is a name like `"K"`, `"SPACE"` or `"DELETE"`. FiveM keeps a [list of every key name](https://docs.fivem.net/docs/game-references/input-mapper-parameter-ids/keyboard/).

`disableControl` has no replacement. A key binding is not one of the game's controls, so there is nothing for MenuAPI to turn off. If your key also does something in the game (<kbd>Space</kbd> also makes the player jump, for example), pick a different default key, or call `DisableControlAction` yourself while your menu is open.

### Hints without a key

`Menu.InstructionalButtons` is gone. The hints along the bottom of the screen are called **instructional buttons**. If you only want to show a hint, without MenuAPI handling the key, add it to `CustomInstructionalButtons`:

```cs
// Before
menu.InstructionalButtons.Add(Control.Jump, "Do the thing");

// After
menu.CustomInstructionalButtons.Add(new Menu.InstructionalButton(
    GetControlInstructionalButton(0, (int)Control.Jump, 1), "Do the thing"));
```

### Select and back hints moved

The select and back hints used to be entries in `Menu.InstructionalButtons`. They are now settings on the menu itself, so they can follow the player's own key bindings.

```cs
// Before
menu.InstructionalButtons[Control.FrontendAccept] = "Choose";
menu.InstructionalButtons.Remove(Control.FrontendCancel);

// After
menu.SelectButtonText = "Choose";
menu.ShowBackInstructionalButton = false;
```

### List item values are a MenuItemList

`MenuListItem.ListItems` used to be a plain `List<string>` that you handed over and kept a reference to. It is now a [MenuItemList](../reference/menuitems/menulistitem/#changing-the-values-later). It works the same way, but it tells the menu when it changes, so the row updates the moment you add or remove a value.

You still create one from a `List<string>`, but the list you pass in is **copied**. Changing your own list afterwards no longer reaches the item, so change it through the item instead.

```cs
// Before
List<string> values = new List<string> { "A", "B" };
MenuListItem item = new MenuListItem("Item", values, 0);

values.Add("C");             // the item picked this up
List<string> current = item.ListItems;

// After
MenuListItem item = new MenuListItem("Item", new List<string> { "A", "B" }, 0);

item.ListItems.Add("C");     // change it through the item
MenuItemList current = item.ListItems;
```

Everything you would call on a `List<string>` is there and works the same: `Add`, `AddRange`, `Insert`, `InsertRange`, `Remove`, `RemoveAt`, `RemoveAll`, `RemoveRange`, `Clear`, `Sort`, `Reverse`, `Contains`, `IndexOf`, `Find`, `FindAll`, `FindIndex`, `Exists`, `TrueForAll`, `ConvertAll`, `GetRange`, `ForEach`, `ToArray`, `Count`, indexing, `foreach` and all of LINQ. If you really need a `List<string>` back, call `item.ListItems.ToList()`.

### Manual garbage collection is gone

`MenuController.EnableManualGCs` no longer exists. Delete the line.

```cs
// Before
MenuController.EnableManualGCs = false;

// After
// (nothing)
```

### Things that can be empty say so now

MenuAPI marks the things that can be `null` (empty, no value at all): `MenuController.GetCurrentMenu()`, `MenuController.MainMenu`, `Menu.ParentMenu`, `Menu.GetCurrentMenuItem()`, `Menu.MenuTitle`, `Menu.MenuSubtitle`, `Menu.CounterPreText`, `MenuItem.Label`, `MenuItem.Description` and `MenuDynamicListItem.CurrentItem`.

Nothing breaks because of this. If your project has nullable checks turned on, you may see new warnings. Each one points at a spot where your code could already crash today, so they are worth fixing.
