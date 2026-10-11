---
title: "Migration Guide (v4)"
---

## Migration Guide (v4)

Everything you need to change when updating to MenuAPI 4.0. Anything not listed still works as before.

- **From Enhanced 1.x:** read [Everyone](#everyone) and [Coming from Enhanced 1.x](#coming-from-enhanced-1x).
- **From MenuAPI 3.x** (`MenuAPI.FiveM` on Legacy): read [Everyone](#everyone) and [Coming from MenuAPI 3.x](#coming-from-menuapi-3x).

----

## Everyone

### Your resource needs the NUI page

Menus are always drawn with NUI now. Add the `index.html` and manifest lines from [Setup](../setup/#2-add-the-page-the-menu-is-drawn-on) steps 2 and 3:

```lua
ui_page 'index.html'

files {
    'MenuAPI.dll',
    'index.html',
    'menuapi/**/*',
}
```

If you already used NUI render mode on Enhanced, you already have this.

### Removed native drawing members

These only existed for the old drawing code. Delete any line that uses them.

- `MenuController.SetDrawOrder`
- `Menu.MenuItemsYOffset`
- On `MenuItem` subclasses: the protected `Width`, `RowHeight`, `GetSpriteSize()` and `GetSpriteX()`. `GetSpriteColour()` now returns a `System.Drawing.Color`.

These still compile with an obsolete warning, but nothing uses them anymore: `Menu.Position`, `Menu.SplitString()`, `MenuItem.PositionOnScreen`, `MenuController.ScreenWidth` and `MenuController.ScreenHeight`.

----

## Coming from Enhanced 1.x

### Render mode does nothing anymore

`MenuController.RenderMode` and `MenuRenderMode` still compile with an obsolete warning, but do nothing. Delete the line.

```cs
// Before
MenuController.RenderMode = MenuRenderMode.Nui;
```

### GlareTuning is removed

The `GlareTuning` class is gone. Delete any code that uses it.

### The NUI web files are minified and renamed

Only if your `index.html` is from before Enhanced 1.0.20. Update the six paths:

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

----

## Coming from MenuAPI 3.x

You stay on Legacy with `BaseScript`, `CitizenFX.Core` and its `Control` enum. Update the `MenuAPI.FiveM` package and work through the changes below.

### The menu toggle key is the player's choice now

`MenuController.MenuToggleKey` and `MenuToggleKeyIsValid` are gone. The toggle is a FiveM key binding that players can change in **Settings, Key Bindings**. You set the default:

```cs
// Before
MenuController.MenuToggleKey = Control.InteractionMenu;

// After, only applies to players who haven't changed it
MenuController.MenuToggleKeyDefault = "M";
```

To turn the toggle off:

```cs
// Before
MenuController.MenuToggleKey = (Control)(-1);

// After
MenuController.EnableMenuToggleKey = false;
```

Set it in your script's constructor, it's read one tick after MenuAPI starts. See [Key bindings](../reference/keybindings/).

### Custom controls are key bindings now

`Menu.ButtonPressHandlers`, `Menu.ButtonPressHandler` and `Menu.ControlPressCheckType` are gone. Use [AddKeyBinding()](../reference/menu/#addkeybinding). It registers a FiveM key binding and its hint follows the player's chosen key.

```cs
// Before
menu.ButtonPressHandlers.Add(new Menu.ButtonPressHandler(
    Control.FrontendSocialClubSecondary,
    Menu.ControlPressCheckType.JUST_RELEASED,
    new Action<Menu, Control>((m, c) => DoThing()),
    true));

menu.InstructionalButtons.Add(Control.FrontendSocialClubSecondary, "Do the thing");

// After
menu.AddKeyBinding("dothing", "Do the thing", "K", "R1_INDEX",
    MenuKeyPressType.JUST_RELEASED,
    new Action<Menu, MenuKeyBinding>((m, b) => DoThing()),
    "Do the thing");
```

Arguments: name, description in the key bindings screen, default [keyboard key](https://docs.fivem.net/docs/game-references/input-mapper-parameter-ids/keyboard/), default controller button, press type, callback, hint text.

`disableControl` has no replacement, a key binding isn't a game control. If your key also does something in game, pick another default or call `DisableControlAction` yourself while the menu is open.

### Hints without a key

`Menu.InstructionalButtons` is gone. For a hint without MenuAPI handling the key, use `CustomInstructionalButtons`:

```cs
// Before
menu.InstructionalButtons.Add(Control.Jump, "Do the thing");

// After
menu.CustomInstructionalButtons.Add(new Menu.InstructionalButton(
    GetControlInstructionalButton(0, (int)Control.Jump, 1), "Do the thing"));
```

### Select and back hints moved

```cs
// Before
menu.InstructionalButtons[Control.FrontendAccept] = "Choose";
menu.InstructionalButtons.Remove(Control.FrontendCancel);

// After
menu.SelectButtonText = "Choose";
menu.ShowBackInstructionalButton = false;
```

### List item values are a MenuItemList

`MenuListItem.ListItems` is now a [MenuItemList](../reference/menuitems/menulistitem/#changing-the-values-later) instead of a `List<string>`. It has the same methods and works with LINQ, and the row updates when it changes. The list you pass to the constructor is **copied**, so change values through `item.ListItems`.

```cs
// Before
List<string> values = new List<string> { "A", "B" };
MenuListItem item = new MenuListItem("Item", values, 0);
values.Add("C");

// After
MenuListItem item = new MenuListItem("Item", new List<string> { "A", "B" }, 0);
item.ListItems.Add("C");
```

Use `item.ListItems.ToList()` if you need a `List<string>`.

### Manual garbage collection is gone

Delete any use of `MenuController.EnableManualGCs`.

### Nullable annotations

Members that can return `null` are now marked as such, for example `MenuController.GetCurrentMenu()`, `Menu.ParentMenu` and `MenuItem.Description`. Nothing breaks, but projects with nullable checks on may see new warnings.
