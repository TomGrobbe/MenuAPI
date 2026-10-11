---
title: "Menu"
---

## Menu

A `Menu` is one menu: a banner, a subtitle bar, a list of [menu items](../menuitems/), a description box and optional instructional buttons.

Every menu must be registered with the [MenuController](../menucontroller/), which draws it and handles its controls. Bind a menu to a [MenuItem](../menuitems/menuitem/) of another menu to make that item a submenu button.

Events (`OnItemSelect`, `OnCheckboxChange`, etc.) are members of this class and are documented on the [Events](../events/) page.

----

### Example usage

```cs
using MenuAPI;

// Create a menu with a banner title and a subtitle.
Menu menu = new Menu("Main Menu", "Subtitle");

// Register the menu. The first menu you register automatically becomes
// MenuController.MainMenu, which is the menu that the toggle key opens.
MenuController.AddMenu(menu);

// Add some items to it.
menu.AddMenuItem(new MenuItem("Normal Button", "A simple button with a description."));
menu.AddMenuItem(new MenuCheckboxItem("Checkbox", "A checkbox.", true));

// Listen for button presses.
menu.OnItemSelect += (_menu, _item, _index) =>
{
    Debug.WriteLine($"'{_item.Text}' was pressed (index {_index}).");
};

// Open the menu whenever you want.
menu.OpenMenu();
```

#### Creating a submenu

```cs
Menu menu = new Menu("Main Menu", "Subtitle");
MenuController.AddMenu(menu);

// A submenu is just another Menu. Use AddSubmenu instead of AddMenu so the
// parent/child relation is set up for you (this is what makes 'back' work).
Menu submenu = new Menu("Submenu", "Secondary Menu");
MenuController.AddSubmenu(menu, submenu);

// Create the item that opens the submenu, then bind the two together.
MenuItem submenuButton = new MenuItem("Open submenu", "This opens the submenu.")
{
    Label = "→→→"
};
menu.AddMenuItem(submenuButton);
MenuController.BindMenuItem(menu, submenu, submenuButton);

// Pressing the back/cancel control in the submenu now returns to `menu`.
submenu.AddMenuItem(new MenuItem("I'm inside the submenu!"));
```

----

### Constructors

----

#### Menu(string name)

Creates a menu with only a banner title.

|Parameter|Type|Description|
|-|-|-|
|name|string|The banner text. Pass `null` for a menu without a banner.|

----

#### Menu(string name, string subtitle)

Creates a menu with a banner title and a subtitle.

|Parameter|Type|Description|
|-|-|-|
|name|string|The banner text. Pass `null` for a menu without a banner.|
|subtitle|string|The text in the subtitle bar below the banner.|

```cs
// With a banner and a subtitle.
Menu menu = new Menu("Main Menu", "Subtitle");

// With a banner only.
Menu menu2 = new Menu("Main Menu");

// Without a banner, only a subtitle.
Menu menu3 = new Menu(null, "Only a subtitle, no banner.");
```

----

### Properties

|Property|Type|Default value|Description|
|---|---|---|---|
|MenuTitle|string|-|The banner text. Null or empty draws no banner.|
|MenuSubtitle|string|Null|The subtitle bar text.|
|HeaderTexture|KeyValuePair&lt;string,&nbsp;string&gt;|(empty)|A custom banner from a game texture. `Key` is the streamed texture dictionary (requested for you), `Value` is the texture name. See [Banners](../theming/#banners).|
|HeaderImage|string?|`null`|A banner image from your resource's `menuapi-banners` folder, by name without extension, so `"outlaw"` uses `menuapi-banners/outlaw.png`. Wins over `HeaderTexture`, and falls back to it when the file is missing. See [Point one menu at your own image](../theming/#point-one-menu-at-your-own-image).|
|MenuTitleFont|int?|Null|The banner title font. Null uses [MenuController.DefaultTitleFont](../menucontroller/#properties). See [Header styling](#header-styling).|
|MenuTitleAlignment|[TitleAlignmentOption](#header-styling)?|Null|Where the title sits in the banner. Null uses [MenuController.DefaultTitleAlignment](../menucontroller/#properties).|
|ShowHeaderGlare|boolean?|Null|Draws GTA Online's moving glow over the banner. Null uses [MenuController.DefaultShowHeaderGlare](../menucontroller/#properties).|
|CounterPreText|string|Null|Text in front of the `current / total` counter in the subtitle bar. Setting it forces the counter to show, even when all items fit on screen.|
|Visible|boolean|false|Whether the menu is drawn. Prefer [OpenMenu()](#openmenu) and [CloseMenu()](#closemenu), which also fire the open/close events.|
|EnableInstructionalButtons|boolean|true|Whether this menu's instructional buttons are drawn.|
|ShowSelectInstructionalButton|boolean|true|Draws the built in **select** hint. On a keyboard it follows the player's [key binding](../keybindings/).|
|ShowBackInstructionalButton|boolean|true|Draws the built in **back** hint. On a keyboard it follows the player's [key binding](../keybindings/).|
|SelectButtonText|string|"Select"|Text of the **select** hint. Defaults to the game's translated label.|
|BackButtonText|string|"Back"|Text of the **back** hint. Defaults to the game's translated label.|
|ShowChangeValueInstructionalButton|boolean|true|Draws the built in **change value** hint, only while the highlighted row is a list, dynamic list or slider.|
|ChangeValueButtonText|string|"Change"|Text of the **change value** hint.|
|IgnoreDontOpenMenus|boolean|false|Keeps this menu drawn while [MenuController.DontOpenAnyMenu](../menucontroller/#properties) is true.|
|ShowWeaponStatsPanel|boolean|false|Shows the weapon stats panel. See [Weapon &amp; vehicle stats panels](#weapon--vehicle-stats-panels).|
|ShowVehicleStatsPanel|boolean|false|Shows the vehicle stats panel. See [Weapon &amp; vehicle stats panels](#weapon--vehicle-stats-panels).|
|CustomInstructionalButtons|List&lt;[InstructionalButton](#instructionalbutton)&gt;|(empty)|Extra hints from a raw button string. See [Instructional buttons](#instructional-buttons).|
|KeyBindingHandlers|List&lt;[KeyBindingHandler](#keybindinghandler)&gt;|(empty)|The extra keys this menu listens for. Fill it with [AddKeyBinding()](#addkeybinding), not by hand.|
|Size|int|0|(Getter only) The number of items. With a filter, only the items that passed. When [paginated](../pagination/), only the current page.|
|CurrentIndex|int|0|(Getter only) Index of the highlighted item. Change it with [RefreshIndex()](#refreshindexint-index).|
|MaxItemsOnScreen|int|10|(Getter only) How many items are visible at once. Change it with [SetMaxItemsOnScreen()](#setmaxitemsonscreenint-max).|
|PageSize|int|0|(Getter only) Items per page, or `0` when not paginated. See [Pagination](../pagination/).|
|Paginated|boolean|false|(Getter only) Whether the menu is split into pages.|
|PageIndex|int|0|(Getter only) The current page, from 0.|
|PageCount|int|1|(Getter only) The number of pages.|
|WrapPages|boolean|true|Whether paging past either end wraps around.|
|ShowPageInstructionalButtons|boolean|true|Draws the previous/next page hints, only for a paginated menu with more than one page.|
|PreviousPageButtonText|string|"Previous page"|Text of the previous page hint.|
|NextPageButtonText|string|"Next page"|Text of the next page hint.|
|ViewIndexOffset|int|0|(Getter only) Index of the first visible item.|
|ParentMenu|Menu|Null|(Getter only) The parent menu, or null. Set by [MenuController.AddSubmenu()](../menucontroller/#addsubmenumenu-parent-menu-child).|
|LeftAligned|boolean|true|(Getter only) Shortcut for [MenuController.MenuAlignment](../menucontroller/#menu-alignment).|
|WeaponStats|float[]|{&nbsp;0f,&nbsp;0f,&nbsp;0f,&nbsp;0f&nbsp;}|(Getter only) Set with [SetWeaponStats()](#setweaponstatsfloat-damage-float-firerate-float-accuracy-float-range).|
|WeaponComponentStats|float[]|{&nbsp;0f,&nbsp;0f,&nbsp;0f,&nbsp;0f&nbsp;}|(Getter only) Set with [SetWeaponComponentStats()](#setweaponcomponentstatsfloat-damage-float-firerate-float-accuracy-float-range).|
|VehicleStats|float[]|{&nbsp;0f,&nbsp;0f,&nbsp;0f,&nbsp;0f&nbsp;}|(Getter only) Set with [SetVehicleStats()](#setvehiclestatsfloat-topspeed-float-acceleration-float-braking-float-traction).|
|VehicleUpgradeStats|float[]|{&nbsp;0f,&nbsp;0f,&nbsp;0f,&nbsp;0f&nbsp;}|(Getter only) Set with [SetVehicleUpgradeStats()](#setvehicleupgradestatsfloat-topspeed-float-acceleration-float-braking-float-traction).|

----

### Constants

|Constant|Type|Value|Description|
|-|-|-|-|
|Width|float|500f|Menu width, in 1080p scaled pixels.|

----

### Methods

Methods below return nothing unless a return value is listed.

----

#### SetMaxItemsOnScreen(int max)

Sets how many items are visible at once, clamped between 3 and 10. If the resolution is too small for that many, fewer are shown so the menu never runs off screen.

|Parameter|Type|Description|
|-|-|-|
|max|int|3 to 10.|

```cs
// Only show 5 items at a time.
menu.SetMaxItemsOnScreen(5);
```

:::tip
For menus with hundreds or thousands of items, see [Pagination](../pagination/).
:::

----

#### RefreshIndex()

Highlights the first item and scrolls back to the top.

----

#### RefreshIndex(int index)

Highlights the item at `index` and scrolls it into view.

|Parameter|Type|Description|
|-|-|-|
|index|int|The item to highlight.|

----

#### RefreshIndex(int index, int viewOffset)

Highlights the item at `index` and makes the item at `viewOffset` the first one on screen.

|Parameter|Type|Description|
|-|-|-|
|index|int|The item to highlight.|
|viewOffset|int|The item drawn at the top.|

```cs
// Rebuild the menu, but keep the user roughly where they were.
int index = menu.CurrentIndex;
int offset = menu.ViewIndexOffset;

menu.ClearMenuItems();
BuildItems(menu);

menu.RefreshIndex(index, offset);
```

----

#### AddMenuItem(MenuItem item)

Adds an item to the bottom and sets its `ParentMenu` to this menu.

|Parameter|Type|Description|
|-|-|-|
|item|[MenuItem](../menuitems/menuitem/)|Any item type.|

----

#### InsertMenuItem(int index, MenuItem item)

Adds an item at `index`. The highlighted item stays highlighted, even when the new item lands above it. In a paginated menu the highlight stays at the same spot on the page instead.

The index counts every item, including those a filter hides. An index past the end adds to the bottom. If a filter is active, the new item is not part of it until you call [FilterMenuItems()](#filtermenuitemsfuncmenuitem-bool-predicate) again.

|Parameter|Type|Description|
|-|-|-|
|index|int|Where the item goes. 0 is the top.|
|item|[MenuItem](../menuitems/menuitem/)|The item to add.|

----

#### MoveMenuItem(MenuItem item, int index)

Moves an item already in this menu. Unlike remove and add, a submenu button keeps its submenu. The highlighted item stays highlighted, except in a paginated menu. An active filter is updated too.

The index counts every item, including those a filter hides. Does nothing if the item is not in this menu.

|Parameter|Type|Description|
|-|-|-|
|item|[MenuItem](../menuitems/menuitem/)|The item to move.|
|index|int|Where the item goes. 0 is the top.|

----

#### RemoveMenuItem(int itemIndex)

Removes the item at the index and corrects the current index so the highlight does not jump. Does nothing if the index is out of range.

:::caution
Removing a submenu button also removes its submenu. To keep it, bind it to another button with [BindMenuItem()](../menucontroller/#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem) first.
:::

|Parameter|Type|Description|
|-|-|-|
|itemIndex|int|The index of the item to remove.|

----

#### RemoveMenuItem(MenuItem item)

Removes the item and clears its `ParentMenu`. Does nothing if the item is not in this menu. A submenu button takes its submenu with it, as above.

|Parameter|Type|Description|
|-|-|-|
|item|[MenuItem](../menuitems/menuitem/)|The item to remove.|

----

#### ClearMenuItems()

Removes all items and resets the index and scroll position. Submenus only reachable through the removed buttons are removed too.

----

#### ClearMenuItems(bool dontResetIndex)

Same as [ClearMenuItems()](#clearmenuitems), but can keep the index and scroll position, for rebuilding an open menu.

|Parameter|Type|Description|
|-|-|-|
|dontResetIndex|boolean|When true, the index and scroll position are kept.|

```cs
// Rebuild the items without the menu jumping back to the top.
menu.ClearMenuItems(true);
BuildItems(menu);
```

----

#### GetMenuItems()

Returns the items, only the filtered ones when a filter is active. The list is a **copy**, so changing it does not change the menu.

|Return type|Description|
|-|-|
|List&lt;[MenuItem](../menuitems/menuitem/)&gt;|The (visible) items.|

```cs
// Toggle the 'enabled' state of every item in the menu.
menu.GetMenuItems().ForEach(item => item.Enabled = !item.Enabled);
```

----

#### GetCurrentMenuItem()

|Return type|Description|
|-|-|
|[MenuItem](../menuitems/menuitem/)|The highlighted item, or `null` if the menu is empty or the index is out of range.|

----

#### SelectItem(int index)

Selects the item at `index` as if the user pressed select on it. Disabled items play the error sound, submenu buttons open their submenu.

|Parameter|Type|Description|
|-|-|-|
|index|int|The item to select.|

----

#### SelectItem(MenuItem item)

Selects the item as if the user pressed select on it.

|Parameter|Type|Description|
|-|-|-|
|item|[MenuItem](../menuitems/menuitem/)|The item to select.|

----

#### OpenMenu()

Shows this menu and fires [OnMenuOpen](../events/#onmenuopen).

----

#### CloseMenu()

Hides this menu and fires [OnMenuClose](../events/#onmenuclose).

----

#### GoBack()

Closes this menu and opens its parent, or just closes if there is none.

----

#### GoUp()

Moves the highlight up one, wrapping to the bottom, and fires [OnIndexChange](../events/#onindexchange). Does nothing if the menu is hidden or has fewer than 2 items.

----

#### GoDown()

Moves the highlight down one, wrapping to the top, and fires [OnIndexChange](../events/#onindexchange). Does nothing if the menu is hidden or has fewer than 2 items.

----

#### GoLeft()

Moves a list, dynamic list or slider item left. On other items it goes to the parent menu if [MenuController.NavigateMenuUsingArrows](../menucontroller/#properties) is on. In a [paginated](../pagination/) menu, other items go to the previous page instead.

----

#### GoRight()

Moves a list, dynamic list or slider item right. Other items are selected. In a [paginated](../pagination/) menu, other items go to the next page instead.

----

#### SortMenuItems(Comparison&lt;MenuItem&gt; compare)

Sorts the items. Any active filter is cleared first.

|Parameter|Type|Description|
|-|-|-|
|compare|Comparison&lt;[MenuItem](../menuitems/menuitem/)&gt;|The compare function.|

```cs
// Sort all items alphabetically by their text.
menu.SortMenuItems((a, b) => a.Text.ToLower().CompareTo(b.Text.ToLower()));
```

----

#### FilterMenuItems(Func&lt;MenuItem, bool&gt; predicate)

Hides every item for which the predicate returns false and resets the index and scroll position. Calling it again replaces the previous filter. Hidden items are **not** removed, but [Size](#properties) and [GetMenuItems()](#getmenuitems) only report the visible ones.

|Parameter|Type|Description|
|-|-|-|
|predicate|Func&lt;[MenuItem](../menuitems/menuitem/),&nbsp;bool&gt;|True for items that stay visible.|

```cs
// Only show items that are enabled.
menu.FilterMenuItems(item => item.Enabled);

// Or use ItemData to filter on your own data.
menu.FilterMenuItems(item => item.ItemData?.category == "vehicles");

// Show everything again.
menu.ResetFilter();
```

----

#### ResetFilter()

Clears the filter so all items show again, and resets the index.

----

#### SetWeaponStats(float damage, float fireRate, float accuracy, float range)

Sets the weapon stats panel values, each clamped between 0 and 1. See [Weapon &amp; vehicle stats panels](#weapon--vehicle-stats-panels).

|Parameter|Type|Description|
|-|-|-|
|damage|float|0 to 1.|
|fireRate|float|0 to 1.|
|accuracy|float|0 to 1.|
|range|float|0 to 1.|

----

#### SetWeaponComponentStats(float damage, float fireRate, float accuracy, float range)

Sets the attachment bonus per stat. Each value is **added on top of** the matching [SetWeaponStats()](#setweaponstatsfloat-damage-float-firerate-float-accuracy-float-range) value, the total is clamped between 0 and 1, and the added part is drawn in another colour. Negative values make a stat worse.

|Parameter|Type|Description|
|-|-|-|
|damage|float|Added to damage.|
|fireRate|float|Added to fire rate.|
|accuracy|float|Added to accuracy.|
|range|float|Added to range.|

:::caution
Call `SetWeaponStats()` **before** `SetWeaponComponentStats()`. The total is calculated from the weapon stats at the moment of the call.
:::

----

#### SetVehicleStats(float topSpeed, float acceleration, float braking, float traction)

Sets the vehicle stats panel values, each clamped between 0 and 1. See [Weapon &amp; vehicle stats panels](#weapon--vehicle-stats-panels).

|Parameter|Type|Description|
|-|-|-|
|topSpeed|float|0 to 1.|
|acceleration|float|0 to 1.|
|braking|float|0 to 1.|
|traction|float|0 to 1.|

----

#### SetVehicleUpgradeStats(float topSpeed, float acceleration, float braking, float traction)

Sets the upgrade bonus per stat. Each value is **added on top of** the matching [SetVehicleStats()](#setvehiclestatsfloat-topspeed-float-acceleration-float-braking-float-traction) value, the total is clamped between 0 and 1, and the added part is drawn in blue. A top speed of 0.5 plus 0.2 here shows 0.7 with the last 0.2 in blue. Negative values are downgrades.

|Parameter|Type|Description|
|-|-|-|
|topSpeed|float|Added to top speed.|
|acceleration|float|Added to acceleration.|
|braking|float|Added to braking.|
|traction|float|Added to traction.|

:::caution
Call `SetVehicleStats()` **before** `SetVehicleUpgradeStats()`, for the same reason.
:::

----

### Header styling

You can change the banner title's font, its alignment, and whether GTA Online's moving glow is drawn. This only affects the banner, not the subtitle bar, counter or items.

Each property is nullable. Null uses the [MenuController](../menucontroller/#header-styling-defaults) default, so you can style all menus at once and override per menu.

#### The title font

`MenuTitleFont` is a game font id. The useful ones are in `MenuFont`:

|Value|Font|
|-|-|
|MenuFont.ChaletLondon|`0`. The font the menu items use.|
|MenuFont.HouseScript|`1`. GTA's handwritten banner font. The default.|
|MenuFont.Monospace|`2`. Fixed width, drawn in capitals.|
|MenuFont.ChaletComprimeCologne|`4`. Narrower than Chalet London, for long titles.|
|MenuFont.Pricedown|`7`. The GTA logo font.|

:::note
Chalet London and Pricedown are the real fonts. The others are stand-ins, because the game's fonts cannot be shipped: House Script is Dancing Script, Chalet Comprime Cologne is Antonio, and Monospace is Bebas Neue.
:::

```cs
Menu menu = new Menu("Los Santos Customs", "Vehicle mods")
{
    MenuTitleFont = MenuFont.Pricedown
};
```

Only these five ids are styled. Any other id falls back to the item font at item size, so the title comes out small. Size and vertical offset are set per font, so there is nothing to tune.

#### Where the title sits

|Value|Description|
|-|-|
|Menu.TitleAlignmentOption.Left|Starts at the left edge of the banner.|
|Menu.TitleAlignmentOption.Center|Centred. The default.|
|Menu.TitleAlignmentOption.Right|Ends at the right edge of the banner.|

This is separate from [MenuController.MenuAlignment](../menucontroller/#menu-alignment), which picks the side of the screen. A right aligned menu with a left aligned title puts the title at the left of that menu's banner.

```cs
menu.MenuTitleAlignment = Menu.TitleAlignmentOption.Left;
```

#### The header glare

`ShowHeaderGlare` draws GTA Online's `mp_menu_glare` glow, which drifts as the camera turns.

```cs
Menu menu = new Menu("Nightclub", "Management")
{
    MenuTitleFont = MenuFont.Pricedown,
    ShowHeaderGlare = true
};
```

The page draws it with `glare.min.js`. If it never shows, check that `glare.min.js` is in your `index.html`, see [How menus are drawn](../../nui/#when-something-is-wrong).

----

### Weapon &amp; vehicle stats panels

Both panels are the game's shop panels, drawn under the description box. A menu with a stats panel enabled should have at least one item.

```cs
// Weapon stats panel
Menu weaponMenu = new Menu("Weapon Stats", "Weapon Stats Panel") { ShowWeaponStatsPanel = true };
weaponMenu.AddMenuItem(new MenuItem("Buy weapon", "You should add at least one item when using stat panels."));
weaponMenu.SetWeaponStats(0.2f, 0.4f, 0.7f, 0.8f);
weaponMenu.SetWeaponComponentStats(0.4f, 0f, -0.05f, 0.1f);
MenuController.AddSubmenu(menu, weaponMenu);

// Vehicle stats panel
Menu vehicleMenu = new Menu("Vehicle Stats", "Vehicle Stats Panel") { ShowVehicleStatsPanel = true };
vehicleMenu.AddMenuItem(new MenuItem("Buy vehicle", "You should add at least one item when using stat panels."));
vehicleMenu.SetVehicleStats(0.2f, 0.2f, 0.3f, 0.8f);
vehicleMenu.SetVehicleUpgradeStats(0.4f, -0.025f, 0.05f, 0.1f);
MenuController.AddSubmenu(menu, vehicleMenu);
```

----

### Instructional buttons

The button hints in the bottom right. Each menu has its own set, and they switch instantly between keyboard and controller.

Every menu has **select** and **back**, plus **change value** while the highlighted row is a list, dynamic list or slider (not when it is disabled). All three follow the player's [key bindings](../keybindings/). Give [AddKeyBinding()](#addkeybinding) a `buttonText` to add a hint for your own key, which follows the bindings too.

```cs
// Change the text next to the built in buttons.
menu.SelectButtonText = "Buy";
menu.BackButtonText = "Nevermind";
menu.ChangeValueButtonText = "Pick a colour";

// Hide the built in 'back' button.
menu.ShowBackInstructionalButton = false;

// Add your own, by registering a key for it. This one is rebindable like everything else.
menu.AddKeyBinding("check", "Example: check something", "E", "R1_INDEX",
    handler: (m, b) => Check(),
    buttonText: "Check");

// Or turn them all off for this menu and draw your own.
menu.EnableInstructionalButtons = false;
```

Draw order: select and back, then change value (or the page hints on other rows of a [paginated](../pagination/) menu), then key bindings with a `buttonText`, then `CustomInstructionalButtons`.

:::caution[Coming from an older MenuAPI?]
`Menu.InstructionalButtons`, the dictionary keyed by `Control`, is **gone**. Use a key binding instead. See [Key bindings](../keybindings/#your-own-keys).
:::

#### InstructionalButton

For hints MenuAPI knows nothing about, such as a button combination, add a raw button string to `CustomInstructionalButtons`.

|Field|Type|Description|
|-|-|-|
|controlString|string|The raw button string, e.g. from `GetControlInstructionalButton()`.|
|instructionText|string|The text next to the button.|

```cs
// Show the up + down buttons with the text 'Move'.
string buttons = GetControlInstructionalButton(0, (int)Control.FrontendUp, 1)
               + GetControlInstructionalButton(0, (int)Control.FrontendDown, 1);

menu.CustomInstructionalButtons.Add(new Menu.InstructionalButton(buttons, "Move"));
```

On Enhanced the third parameter of `GetControlInstructionalButton` is a `bool`, so pass `true` instead of `1`.

----

### Key bindings

A menu can listen for its own keys without a tick function. Each is a real FiveM key mapping, so players can rebind it under **Settings, Key Bindings**. Handlers only run while the menu is open and [MenuController.DisableMenuButtons](../menucontroller/#properties) is false.

#### AddKeyBinding()

```cs
public KeyBindingHandler AddKeyBinding(
    string name,
    string description,
    string keyboardKey,
    string? controllerButton = null,
    MenuKeyPressType pressType = MenuKeyPressType.JUST_RELEASED,
    Action<Menu, MenuKeyBinding>? handler = null,
    string? buttonText = null)
```

|Parameter|Type|Description|
|-|-|-|
|name|string|Part of the command name, so keep it short and lowercase. Menus passing the same name share one key.|
|description|string|Shown in the player's key bindings screen.|
|keyboardKey|string|A [keyboard parameter id](https://docs.fivem.net/docs/game-references/input-mapper-parameter-ids/keyboard/), e.g. `"E"` or `"F5"`.|
|controllerButton|string?|A [controller parameter id](https://docs.fivem.net/docs/game-references/input-mapper-parameter-ids/pad_analogbutton/), e.g. `"R1_INDEX"`. Null for no controller button.|
|pressType|[MenuKeyPressType](#menukeypresstype)|When the handler is called.|
|handler|Action&lt;Menu,&nbsp;[MenuKeyBinding](#menukeybinding)&gt;?|Called with this menu and the binding.|
|buttonText|string?|Also draws an [instructional button](#instructional-buttons) for this key.|

```cs
// Toggle the 'enabled' state of every item whenever this key is released.
menu.AddKeyBinding(
    "toggleitems",
    "Example: toggle every item",
    "K",
    "R1_INDEX",
    MenuKeyPressType.JUST_RELEASED,
    (m, b) => m.GetMenuItems().ForEach(item => item.Enabled = !item.Enabled),
    "Toggle all");
```

#### MenuKeyPressType

|Value|Description|
|-|-|
|MenuKeyPressType.JUST_PRESSED|Once, when the key goes down.|
|MenuKeyPressType.JUST_RELEASED|Once, when the key is let go.|
|MenuKeyPressType.PRESSED|Every frame while held.|
|MenuKeyPressType.RELEASED|Every frame while not held.|

#### MenuKeyBinding

The key, passed to your handler and available as `KeyBindingHandler.Binding`.

|Property|Type|Description|
|-|-|-|
|Name|string|The registered name.|
|Command|string|The console command FiveM runs for it, prefix included.|
|Held|boolean|Whether the key is down now, on either device.|
|Icon|string|The instructional button icon for the current device. Empty when the game has none.|

#### KeyBindingHandler

Returned by `AddKeyBinding()`.

|Property|Type|Description|
|-|-|-|
|Binding|[MenuKeyBinding](#menukeybinding)|The key. Read only.|
|PressType|[MenuKeyPressType](#menukeypresstype)|When the handler runs.|
|Handler|Action&lt;Menu,&nbsp;MenuKeyBinding&gt;?|The code to run. Null stops listening.|
|ButtonText|string?|The instructional button text. Null or empty draws nothing.|

:::caution[Coming from an older MenuAPI?]
`Menu.ButtonPressHandlers`, `Menu.ButtonPressHandler`, `Menu.ControlPressCheckType` and the `disableControl` option are **gone**. See [Key bindings](../keybindings/#your-own-keys) for replacements.
:::

----

### Events

All menu events are documented on the [Events](../events/) page.
