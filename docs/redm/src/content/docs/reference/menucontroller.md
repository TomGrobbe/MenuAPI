---
title: "MenuController"
---

## MenuController

The `MenuController` is the part of MenuAPI that actually runs: it draws every registered [Menu](../menu/), handles all controls, and keeps track of which menu is currently open.

You never create one yourself. It is a `BaseScript` that starts automatically as soon as `MenuAPI.dll` is loaded by your resource, and every member on it is **static**, so you always use it as `MenuController.Something`.

Its main job for you is registering menus ([AddMenu()](#addmenumenu-menu), [AddSubmenu()](#addsubmenumenu-parent-menu-child) and [BindMenuItem()](#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem)) and giving you a handful of global switches to control how menus behave.

----

### Example usage

```cs
using MenuAPI;

public class MyMenus : BaseScript
{
    public MyMenus()
    {
        // Global settings. These can be changed at any time.
        MenuController.MenuToggleKey = Control.PlayerMenu;

        // The first menu you register becomes MenuController.MainMenu, which is
        // the menu that opens when the user presses the toggle key.
        Menu menu = new Menu("Main Menu", "Subtitle");
        MenuController.AddMenu(menu);

        // Submenus.
        Menu submenu = new Menu("Submenu");
        MenuItem submenuButton = new MenuItem("Open the submenu")
        {
            RightIcon = MenuItem.Icon.ARROW_RIGHT
        };
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

:::note
RedM supports a smaller feature set than FiveM. Members that only exist in the FiveM build of MenuAPI are listed under [FiveM only members](#fivem-only-members).
:::

All properties are **static**.

|Property|Type|Default value|Description|Optional|
|---|---|---|---|---|
|MainMenu|Menu|Null|The menu that is opened when the user presses the [menu toggle key](#the-menu-toggle-key). This is set automatically to the first menu you register, but you can change it at any time.|Yes|
|Menus|List&lt;[Menu](../menu/)&gt;|(empty)|(Getter only) Every menu that has been registered with [AddMenu()](#addmenumenu-menu) or [AddSubmenu()](#addsubmenumenu-parent-menu-child).|Yes|
|MenuToggleKey|Control|Control.PlayerMenu|The control that opens [MainMenu](#properties). See [The menu toggle key](#the-menu-toggle-key).|Yes|
|MenuToggleKeyIsValid|boolean|false|(Getter only) Whether the current `MenuToggleKey` is a control the game accepts. When this is false, the toggle key is ignored entirely. See [The menu toggle key](#the-menu-toggle-key).|Yes|
|DisableMenuButtons|boolean|false|When true, all menu controls are ignored. The menu stays on screen, it just does not respond to input. Useful while you are doing something that should not be interrupted.|Yes|
|AreMenuButtonsEnabled|boolean|false|(Getter only) Whether menu controls are currently being processed. This is false when no menu is open, the pause menu is active, the screen is faded out, the player is dead, or `DisableMenuButtons` is true.|Yes|
|DontOpenAnyMenu|boolean|false|When true, menus stop being drawn and can not be opened. Menus with [IgnoreDontOpenMenus](../menu/#properties) set to true are excluded.|Yes|
|PreventExitingMenu|boolean|false|When true, the user can not close the menu with the back/cancel control. Submenus can still go back to their parent.|Yes|
|DisableBackButton|boolean|false|When true, the back/cancel control does nothing at all, not even in submenus.|Yes|
|NavigateMenuUsingArrows|boolean|true|When true, pressing left on an item that has no left/right behaviour goes back to the parent menu.|Yes|
|SetDrawOrder|boolean|true|Whether MenuAPI sets the script graphics draw order while drawing. Turn this off if the menu draws over (or under) your own UI in the wrong order.|Yes|
|EnableManualGCs|boolean|true|Whether MenuAPI triggers a manual garbage collect every 60 seconds while no menu is open. Turn this off if your resource manages GCs itself.|Yes|
|ScreenWidth|float|-|(Getter only) The width of the screen, scaled to a 1080p height. The RedM build assumes a fixed 16:9 aspect ratio.|Yes|
|ScreenHeight|float|1080f|(Getter only) The height of the screen. This is always 1080.|Yes|

:::caution
`PreventExitingMenu` should only be used for menus where the user really must not walk away halfway through. Always give them a button to close the menu themselves. Be nice to your users!
:::

----

### Constants

|Constant|Type|Value|Description|
|-|-|-|-|
|_texture_dict|string|"menu_textures"|The streamed texture dictionary used for the default menu sprites.|
|_header_texture|string|"translate_bg_1a"|The texture name used for the default menu banner.|

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

Binding the same item again replaces the menu it was bound to.

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
    RightIcon = MenuItem.Icon.ARROW_RIGHT
};
menu.AddMenuItem(submenuButton);

// Registers `submenu`, sets its parent, and binds the button to it.
MenuController.BindMenuItem(menu, submenu, submenuButton);
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
// Don't let the player attack while a menu is open.
if (MenuController.IsAnyMenuOpen())
{
    DisableControlAction(0, (uint)Control.Attack, true);
}
```

----

#### CloseAllMenus()

Closes every open menu. Each closed menu triggers its own [OnMenuClose](../events/#onmenuclose) event and disables its UI prompts.

MenuAPI also calls this automatically when your resource stops, so the UI prompts do not get stuck on screen.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

### The menu toggle key

MenuAPI handles all menu controls for you, including one control that opens [MainMenu](#properties). By default that is `Control.PlayerMenu`.

:::caution
**The built-in toggle key currently does not work in RedM.** The toggle key is only processed while [MenuToggleKeyIsValid](#properties) is true, and that check expects the small numeric control IDs that FiveM uses. RedM controls are 32-bit hashes, so no RedM control ever passes it.

Open your menu yourself instead — see the example below. Everything else (navigating, selecting, going back) works normally.
:::

```cs
// Open the menu from your own command or key mapping.
RegisterCommand("mymenu", new Action<int, List<object>, string>((source, args, raw) =>
{
    if (!MenuController.IsAnyMenuOpen())
    {
        menu.OpenMenu();
    }
}), false);
```

- The toggle control is disabled every frame while MenuAPI is running, so the game's own action for it does not also fire.
- [MainMenu](#properties) must not be null when the toggle key is used. If it is, an error is printed to the console and no menu opens.

:::note
RedM's `CitizenFX.Core` does not ship a `Control` enum, so MenuAPI provides its own. It is included in `MenuAPI.dll` under the usual `CitizenFX.Core.Control` name, so `using CitizenFX.Core;` is all you need to use it. Note that RedM controls are hashes (`uint`), so native calls need an explicit cast, e.g. `DisableControlAction(0, (uint)Control.Attack, true)`.
:::

----

### FiveM only members

These members are part of the FiveM build of MenuAPI and are **not** available (or have no effect) in RedM:

|Member|Notes|
|-|-|
|MenuAlignment, MenuAlignmentOption|Right aligned menus are FiveM only, so neither the property nor the enum exists in the RedM build.|
|EnableMenuToggleKeyOnController|This property exists in the RedM build, but has no effect: RedM has no separate controller handling for the toggle key.|
