---
title: "MenuController"
---

## MenuController

`MenuController` draws every registered [Menu](../menu/), handles all controls and tracks which menu is open. It starts on its own when your resource loads `MenuAPI.dll` (an `IScript` on Enhanced, a `BaseScript` on Legacy), so you never create one. Every member is static.

----

### Example usage

This class is written for Enhanced, on Legacy use `using CitizenFX.Core;` and `BaseScript`. Other examples in this reference use Legacy names such as `Debug.WriteLine` and `Game`, on Enhanced use their `CitizenFX.FiveM.Client` equivalents.

```cs
using MenuAPI;
using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared.Script;

public class MyMenus : IScript
{
    public MyMenus()
    {
        // Global settings. These can be changed at any time.
        MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;
        MenuController.EnableMenuToggleKeyOnController = false;

        // The key that opens the menu, for players who have not rebound it themselves.
        MenuController.MenuToggleKeyDefault = "F5";

        // The first menu you register becomes MenuController.MainMenu, which is
        // the menu that opens when the user presses the toggle key.
        Menu menu = new Menu("Main Menu", "Subtitle");
        MenuController.AddMenu(menu);

        // Submenus.
        Menu submenu = new Menu("Submenu");
        MenuItem submenuButton = new MenuItem("Open the submenu");
        menu.AddMenuItem(submenuButton);
        MenuController.BindMenuItem(menu, submenu, submenuButton);
    }

    // Somewhere else in your resource:
    private void CloseEverything()
    {
        if (MenuController.IsAnyMenuOpen())
        {
            MenuController.CloseAllMenus();
        }
    }
}
```

----

### Properties

|Property|Type|Default value|Description|
|---|---|---|---|
|MainMenu|Menu|null|The menu the [toggle key](#the-menu-toggle-key) opens. Set to the first menu you register, and can be changed at any time. When null, the first registered menu opens.|
|Menus|List&lt;[Menu](../menu/)&gt;|(empty)|Getter only. Every menu registered with [AddMenu()](#addmenumenu-menu) or [AddSubmenu()](#addsubmenumenu-parent-menu-child).|
|MenuAlignment|[MenuAlignmentOption](#menu-alignment)|MenuAlignmentOption.Left|Which side of the screen menus are drawn on. See [Menu alignment](#menu-alignment).|
|DefaultTitleFont|int|MenuFont.HouseScript|Banner title font for menus that do not set [MenuTitleFont](../menu/#header-styling). See [Header styling defaults](#header-styling-defaults).|
|DefaultTitleAlignment|[TitleAlignmentOption](../menu/#header-styling)|TitleAlignmentOption.Center|Banner title alignment for menus that do not set [MenuTitleAlignment](../menu/#header-styling).|
|DefaultShowHeaderGlare|boolean|false|GTA Online's moving banner glow, for menus that do not set [ShowHeaderGlare](../menu/#header-styling).|
|MenuToggleKeyDefault|string|"M"|The keyboard toggle key for players who never rebound it. Set it in your constructor. See [Changing the default toggle key](../keybindings/#changing-the-default-toggle-key).|
|EnableMenuToggleKeyOnController|boolean|true|Whether the controller toggle works, while `EnableMenuToggleKey` is true. It is a 400ms hold on the back/select button by default, and the player can rebind the button.|
|EnableMenuToggleKey|boolean|true|Whether MenuAPI registers its toggle key, on keyboard and controller. Set it in your constructor, because it is read when the key mappings are registered one tick after MenuAPI starts. See [The menu toggle key](#the-menu-toggle-key).|
|DisableMenuButtons|boolean|false|When true, the open menu stays on screen but ignores all input, including the toggle key.|
|AreMenuButtonsEnabled|boolean|false|Getter only. False when no menu is open, the pause menu is open, the screen is faded out, a player switch is in progress, the player is dead, the F8 console is open, or `DisableMenuButtons` is true.|
|DontOpenAnyMenu|boolean|false|When true, open menus are closed, the toggle key does nothing and no menu is drawn or takes input. A menu with [IgnoreDontOpenMenus](../menu/#properties) set to true is not closed, but it is still hidden until this is false again.|
|PreventExitingMenu|boolean|false|When true, back and the toggle key can not close a top level menu. Submenus can still go back to their parent. Give the player a button that closes the menu.|
|DisableBackButton|boolean|false|When true, back does nothing, in submenus too.|
|NavigateMenuUsingArrows|boolean|true|When true, left on an item without left/right behaviour acts like back.|
|ScreenWidth|float|1920f|Getter only. 1080 times the screen aspect ratio.|
|ScreenHeight|float|1080f|Getter only. Always 1080.|

----

### Constants

|Constant|Type|Value|Description|
|-|-|-|-|
|_texture_dict|string|"commonmenu"|The streamed texture dictionary used for the default menu sprites.|
|_header_texture|string|"interaction_bgd"|The texture name used for the default menu banner.|

----

### Methods

All methods are static.

----

#### AddMenu(Menu menu)

Registers `menu`. A menu is not drawn and takes no input until it is registered. Registering the same menu twice does nothing. If [MainMenu](#properties) is null, `menu` becomes the main menu.

```cs
Menu menu = new Menu("Main Menu", "Subtitle");
MenuController.AddMenu(menu);
```

----

#### AddSubmenu(Menu parent, Menu child)

Registers `child` if needed and sets its [ParentMenu](../menu/#properties) to `parent`, which is where back returns to. It does not add a button that opens `child`, use [BindMenuItem()](#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem) for that.

----

#### BindMenuItem(Menu parentMenu, Menu childMenu, MenuItem menuItem)

Makes `menuItem` a submenu button: selecting it closes `parentMenu` and opens `childMenu`. Calls [AddSubmenu()](#addsubmenumenu-parent-menu-child) for you.

Binding the same item again replaces its menu. The old menu is removed, unless another item still opens it or it is [MainMenu](#properties).

```cs
Menu menu = new Menu("Main Menu", "Subtitle");
MenuController.AddMenu(menu);

Menu submenu = new Menu("Submenu", "Secondary Menu");

MenuItem submenuButton = new MenuItem("Open submenu", "This opens the submenu.")
{
    Label = "→→→"
};
menu.AddMenuItem(submenuButton);

// Registers `submenu`, sets its parent, and binds the button to it.
MenuController.BindMenuItem(menu, submenu, submenuButton);
```

----

#### RemoveMenu(Menu menu)

Removes `menu`, and every menu that could only be reached through its items, all the way down. Passing null does nothing.

The menu is closed if open (raising [OnMenuClose](../events/#onmenuclose)), its items are cleared and all its event handlers are dropped, so your own reference is the only one left. If it was open, its parent menu is opened. If it was [MainMenu](#properties), the first remaining menu takes its place.

```cs
Menu submenu = new Menu("Submenu");
MenuItem button = new MenuItem("Open the submenu");
menu.AddMenuItem(button);
MenuController.BindMenuItem(menu, submenu, button);

// Later on, when the submenu is no longer needed:
MenuController.RemoveMenu(submenu);
```

:::note
Removing a submenu button with [RemoveMenuItem()](../menu/#removemenuitemmenuitem-item) or [ClearMenuItems()](../menu/#clearmenuitems) already removes the menu behind it.
:::

----

#### RemoveAllMenus()

Removes every menu and clears the menu list, the open menus, the submenu bindings and [MainMenu](#properties). Use it on resource shutdown or to rebuild your menus from scratch.

```cs
// Start over with a clean slate.
MenuController.RemoveAllMenus();
```

----

#### GetCurrentMenu()

Returns the open [Menu](../menu/), or `null` if no menu is open.

```cs
Menu current = MenuController.GetCurrentMenu();
if (current != null)
{
    Debug.WriteLine($"'{current.MenuTitle}' is open, on item {current.CurrentIndex}.");
}
```

----

#### IsAnyMenuOpen()

Returns true if at least one menu is open.

```cs
// Don't let the player shoot while a menu is open.
if (MenuController.IsAnyMenuOpen())
{
    Game.DisableControlThisFrame(0, Control.Attack);
}
```

----

#### CloseAllMenus()

Closes every open menu. Each one raises its own [OnMenuClose](../events/#onmenuclose) event.

----

#### RefreshNui()

Redraws the open menu. Call it when item text is built from something MenuAPI does not own, such as a label you resolve yourself, and that changes. Everything MenuAPI owns already triggers a redraw.

----

### Menu alignment

Can be changed at any time and takes effect immediately. Both sides follow the safe zone size.

|Value|Description|
|-|-|
|MenuAlignmentOption.Left|Left side of the screen. The default.|
|MenuAlignmentOption.Right|Right side of the screen.|

```cs
// Right align all menus.
MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;

// Let the user toggle it with a checkbox.
MenuCheckboxItem alignBox = new MenuCheckboxItem("Right aligned menu", "Move the menu to the other side.", false);
menu.AddMenuItem(alignBox);

menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
{
    if (_item == alignBox)
    {
        MenuController.MenuAlignment = _checked
            ? MenuController.MenuAlignmentOption.Right
            : MenuController.MenuAlignmentOption.Left;
    }
};
```

:::caution
Right alignment is not supported on 17:9 and wider aspect ratios. There the value is forced to `MenuAlignmentOption.Left` and an error is logged. Read the property back, or use [Menu.LeftAligned](../menu/#properties), to get the alignment actually in use.
:::

----

### Header styling defaults

The fallbacks for menus that do not set their own title font, title alignment or glare (see [Header styling](../menu/#header-styling)). Set them before building your menus to style your whole resource in one place.

```cs
MenuController.DefaultTitleFont = MenuFont.Pricedown;
MenuController.DefaultShowHeaderGlare = true;

// Both of these get the Pricedown title and the glare, without asking for either.
Menu main = new Menu("Los Santos Customs", "Vehicle mods");
Menu wheels = new Menu("Wheels", "Pick a set");

// And a single menu can still say no.
Menu warning = new Menu("Are you sure?", "This cannot be undone")
{
    MenuTitleFont = MenuFont.ChaletLondon,
    ShowHeaderGlare = false
};
```

Changing a default at runtime immediately affects every menu that has not overridden it, including menus already built or on screen.

----

### The menu toggle key

The toggle key opens [MainMenu](#properties). It defaults to <kbd>M</kbd> on a keyboard and to holding the back/select button for 400ms on a controller. Players can rebind both in **Settings, Key Bindings**, the controller toggle stays a hold. See [Key bindings](../keybindings/).

```cs
// Decide which menu the toggle key opens.
MenuController.MainMenu = myOtherMenu;

// Pick a different default key. Only affects players who never rebound it.
MenuController.MenuToggleKeyDefault = "F5";
```

- Pressing it while a menu is open closes that menu, unless [PreventExitingMenu](#properties) is true.
- When [MainMenu](#properties) is null, the first registered menu opens. With no menus, nothing happens.

To open menus yourself, call [Menu.OpenMenu()](../menu/#openmenu) from your own command or key mapping and set [EnableMenuToggleKey](#properties) to false in your constructor. That turns off the toggle on keyboard and controller.

:::caution[Coming from the older MenuAPI?]
`MenuController.MenuToggleKey` and `MenuController.MenuToggleKeyIsValid` are **gone**, the key is now the player's choice. Use `MenuToggleKeyDefault` to pick the key players start with, or set `EnableMenuToggleKey` to false where you used to set the key to `-1`. See the [Migration Guide (v4)](../../migration/#the-menu-toggle-key-is-the-players-choice-now).
:::
