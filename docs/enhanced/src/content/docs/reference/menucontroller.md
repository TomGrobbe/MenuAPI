---
title: "MenuController"
---

## MenuController

The `MenuController` is the part of MenuAPI that actually runs: it draws every registered [Menu](../menu/), handles all controls, and keeps track of which menu is currently open.

You never create one yourself. It is an `IScript` that starts automatically as soon as `MenuAPI.dll` is loaded by your resource, and every member on it is **static**, so you always use it as `MenuController.Something`.

Its main job for you is registering menus ([AddMenu()](#addmenumenu-menu), [AddSubmenu()](#addsubmenumenu-parent-menu-child) and [BindMenuItem()](#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem)) and giving you a handful of global switches to control how menus behave.

----

### Example usage

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

All properties are **static**.

|Property|Type|Default value|Description|Optional|
|---|---|---|---|---|
|MainMenu|Menu|Null|The menu that is opened when the user presses the [menu toggle key](#the-menu-toggle-key). This is set automatically to the first menu you register, but you can change it at any time. When it is null, the first registered menu is opened instead.|Yes|
|Menus|List&lt;[Menu](../menu/)&gt;|(empty)|(Getter only) Every menu that has been registered with [AddMenu()](#addmenumenu-menu) or [AddSubmenu()](#addsubmenumenu-parent-menu-child).|Yes|
|MenuAlignment|[MenuAlignmentOption](#menu-alignment)|MenuAlignmentOption.Left|Whether menus are drawn on the left or the right side of the screen. See [Menu alignment](#menu-alignment).|Yes|
|DefaultTitleFont|int|MenuFont.HouseScript|The font every menu banner title is drawn in, unless that menu sets [MenuTitleFont](../menu/#header-styling) itself. See [Header styling defaults](#header-styling-defaults).|Yes|
|DefaultTitleAlignment|[TitleAlignmentOption](../menu/#header-styling)|TitleAlignmentOption.Center|Where every menu banner title sits, unless that menu sets [MenuTitleAlignment](../menu/#header-styling) itself. See [Header styling defaults](#header-styling-defaults).|Yes|
|DefaultShowHeaderGlare|boolean|false|Whether GTA Online's moving glow is drawn over every menu banner, unless that menu sets [ShowHeaderGlare](../menu/#header-styling) itself. See [Header styling defaults](#header-styling-defaults).|Yes|
|MenuToggleKeyDefault|string|"M"|The key that opens the menu for players who have never rebound it themselves. Set it in your constructor. See [Changing the default toggle key](../keybindings/#changing-the-default-toggle-key).|Yes|
|EnableMenuToggleKeyOnController|boolean|true|Whether the menu can also be toggled with a controller. The controller binding can not be changed: it is always the back/select button, held for 400ms. See [Key bindings](../keybindings/).|Yes|
|DisableMenuButtons|boolean|false|When true, all menu controls are ignored. The menu stays on screen, it just does not respond to input. Useful while you are doing something that should not be interrupted.|Yes|
|AreMenuButtonsEnabled|boolean|false|(Getter only) Whether menu controls are currently being processed. This is false when no menu is open, the game is paused, the screen is faded out, a player switch is in progress, the player is dead, or `DisableMenuButtons` is true.|Yes|
|DontOpenAnyMenu|boolean|false|When true, menus stop being drawn and can not be opened. Menus with [IgnoreDontOpenMenus](../menu/#properties) set to true are excluded.|Yes|
|PreventExitingMenu|boolean|false|When true, the user can not close the menu with the back/cancel control. Submenus can still go back to their parent.|Yes|
|DisableBackButton|boolean|false|When true, the back/cancel control does nothing at all, not even in submenus.|Yes|
|NavigateMenuUsingArrows|boolean|true|When true, pressing left on an item that has no left/right behaviour goes back to the parent menu.|Yes|
|SetDrawOrder|boolean|true|Whether MenuAPI sets the script graphics draw order while drawing. Turn this off if the menu draws over (or under) your own UI in the wrong order.|Yes|
|ScreenWidth|float|1920f|(Getter only) The width of the screen, scaled to a 1080p height. This changes with the aspect ratio.|Yes|
|ScreenHeight|float|1080f|(Getter only) The height of the screen. This is always 1080.|Yes|

:::caution
`PreventExitingMenu` should only be used for menus where the user really must not walk away halfway through. Always give them a button to close the menu themselves. Be nice to your users!
:::

----

### Constants

|Constant|Type|Value|Description|
|-|-|-|-|
|_texture_dict|string|"commonmenu"|The streamed texture dictionary used for the default menu sprites.|
|_header_texture|string|"interaction_bgd"|The texture name used for the default menu banner.|

----

### Methods

All methods are **static**.

----

#### AddMenu(Menu menu)

Registers a menu with the controller. A menu is not drawn and does not respond to controls until it has been registered.

If [MainMenu](#properties) is still null, the menu you pass here becomes the main menu.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu to register. Registering the same menu twice does nothing.|

##### Return value

_This function does not return anything_.

```cs
Menu menu = new Menu("Main Menu", "Subtitle");
MenuController.AddMenu(menu);
```

----

#### AddSubmenu(Menu parent, Menu child)

Registers `child` (if it was not registered yet) and sets its [ParentMenu](../menu/#properties) to `parent`. That parent/child relation is what makes the back/cancel control return to the previous menu.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|parent|[Menu](../menu/)|The menu that the child returns to.|
|child|[Menu](../menu/)|The submenu.|

##### Return value

_This function does not return anything_.

:::note
`AddSubmenu()` only sets up the relation. To give the user a button that opens the submenu, use [BindMenuItem()](#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem) instead, which does both.
:::

----

#### BindMenuItem(Menu parentMenu, Menu childMenu, MenuItem menuItem)

Turns `menuItem` into a submenu button: pressing it closes `parentMenu` and opens `childMenu`. This also calls [AddSubmenu()](#addsubmenumenu-parent-menu-child) for you.

Binding the same item again replaces the menu it was bound to. The menu it used to open is removed for you, unless another button still opens it, because with nothing pointing at it nobody could ever get to it again.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|parentMenu|[Menu](../menu/)|The menu that `menuItem` belongs to.|
|childMenu|[Menu](../menu/)|The menu to open when the item is pressed.|
|menuItem|[MenuItem](../menuitems/menuitem/)|The item that opens the submenu.|

##### Return value

_This function does not return anything_.

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

Takes `menu` back out of MenuAPI again. Use it when a menu is finished with and you do not want it hanging around.

It also removes every menu that could only be reached through one of `menu`'s buttons, all the way down. That sounds drastic, but think about it: if the only button that opened a submenu has just gone, there is no route to that submenu anymore, so keeping it would only waste memory.

Along the way it closes the menu if it happens to be open, empties its buttons, and forgets every event handler you attached to it. Once that is done, the only thing still pointing at the menu is your own variable, so as soon as you stop using that variable the game can clean it up.

If the menu was open when you removed it, the player is put back on its parent menu rather than being left staring at nothing.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu to remove. Passing null does nothing.|

##### Return value

_This function does not return anything_.

```cs
Menu submenu = new Menu("Submenu");
MenuItem button = new MenuItem("Open the submenu");
menu.AddMenuItem(button);
MenuController.BindMenuItem(menu, submenu, button);

// Later on, when the submenu is no longer needed:
MenuController.RemoveMenu(submenu);
```

:::note
You rarely need to call this yourself for submenus. Removing the button that opens one, with [RemoveMenuItem()](../menu/#removemenuitemmenuitem-item) or [ClearMenuItems()](../menu/#clearmenuitems), already removes the menu behind it.
:::

----

#### RemoveAllMenus()

Removes every registered menu in one go, and empties everything MenuAPI was keeping track of: the menu list, the open menus, the submenu bindings and [MainMenu](#properties).

This is the one to call when your resource is shutting down, or when you want to throw away your whole menu structure and build a fresh one from scratch.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

```cs
// Start over with a clean slate.
MenuController.RemoveAllMenus();
```

----

#### GetCurrentMenu()

Returns the menu that is currently open.

##### Parameters

_This function does not have any parameters_.

##### Return value

|Type|Description|
|-|-|
|[Menu](../menu/)|The currently open menu, or `null` if no menu is open.|

```cs
Menu current = MenuController.GetCurrentMenu();
if (current != null)
{
    Debug.WriteLine($"'{current.MenuTitle}' is open, on item {current.CurrentIndex}.");
}
```

----

#### IsAnyMenuOpen()

Returns whether any menu is currently open.

##### Parameters

_This function does not have any parameters_.

##### Return value

|Type|Description|
|-|-|
|boolean|True if at least one menu is open.|

```cs
// Don't let the player shoot while a menu is open.
if (MenuController.IsAnyMenuOpen())
{
    Game.DisableControlThisFrame(0, Control.Attack);
}
```

----

#### CloseAllMenus()

Closes every open menu. Each closed menu triggers its own [OnMenuClose](../events/#onmenuclose) event.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

### Menu alignment

Menus can be drawn on the left or the right side of the screen, and this can be changed at any time during runtime. The menu updates instantly, and both alignments scale with the safezone size automatically.

|Value|Description|
|-|-|
|MenuAlignmentOption.Left|Menus are drawn on the left side of the screen. This is the default.|
|MenuAlignmentOption.Right|Menus are drawn on the right side of the screen.|

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
Right aligned menus are not supported on 17:9 and 21:9 aspect ratios. On those resolutions the value is forced back to `MenuAlignmentOption.Left` and a warning is printed to the console. Always read the property back if you need to know the alignment that is actually being used, or use [Menu.LeftAligned](../menu/#properties).
:::

:::note
Menu alignment is only available in FiveM.
:::

----

### Header styling defaults

Every menu banner can have its own title font, its own title alignment and its own glare, and each of those is described in full under [Header styling](../menu/#header-styling). The three properties here are what a menu falls back to when it has not been told otherwise.

That means you can set the look of your whole resource in one place. Do it before you build your menus and every one of them picks it up:

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

Changing a default at runtime affects every menu that has not overridden it, immediately, including menus you built earlier and menus that are on screen right now.

----

### The menu toggle key

MenuAPI handles all menu controls for you, including one key that opens [MainMenu](#properties). On a keyboard that key defaults to <kbd>M</kbd>, and **the player can rebind it themselves** from FiveM's **Settings, Key Bindings** screen. See [Key bindings](../keybindings/) for the full list and how it works.

```cs
// Decide which menu the toggle key opens.
MenuController.MainMenu = myOtherMenu;

// Pick a different default key. Only affects players who never rebound it.
MenuController.MenuToggleKeyDefault = "F5";
```

- Pressing the toggle key while a menu is open closes that menu, unless [PreventExitingMenu](#properties) is true.
- When [MainMenu](#properties) is null, the first registered menu is opened instead. If no menus are registered at all, nothing happens.
- The controller binding can **not** be changed: it is always the interaction menu button, held for 400ms. Set [EnableMenuToggleKeyOnController](#properties) to false to disable it.

If you would rather open menus yourself, call [Menu.OpenMenu()](../menu/#openmenu) from your own command or key mapping.

:::caution[Coming from the older MenuAPI?]
`MenuController.MenuToggleKey` and `MenuController.MenuToggleKeyIsValid` are **gone**. Which key opens the menu is now the player's choice, not the resource's, so there is nothing to set. Delete any code that assigned them.
:::
