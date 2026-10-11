---
title: "Menu"
---

## Menu

A `Menu` is one self contained menu: a banner, a subtitle bar, a list of [menu items](../menuitems/), a description box and (optionally) instructional buttons.

Every menu has to be registered with the [MenuController](../menucontroller/), which takes care of drawing it and of handling all controls for you. Menus can be nested: bind any menu to a [MenuItem](../menuitems/menuitem/) of another menu and that item becomes a submenu button.

All events (`OnItemSelect`, `OnCheckboxChange`, etc.) are members of this class. They are documented on their own page: [Events](../events/).

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
    RightIcon = MenuItem.Icon.ARROW_RIGHT
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

Creates a new menu with only a banner title.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|name|string|The text displayed on the menu banner. Pass `null` to create a menu without a banner.|

----

#### Menu(string name, string subtitle)

Creates a new menu with a banner title and a subtitle.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|name|string|The text displayed on the menu banner. Pass `null` to create a menu without a banner.|
|subtitle|string|The text displayed in the subtitle bar, directly below the banner.|

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

:::note
RedM supports a smaller feature set than FiveM. Members that only exist in the FiveM build of MenuAPI are listed under [FiveM only members](#fivem-only-members).
:::

|Property|Type|Default value|Description|Optional|
|---|---|---|---|---|
|MenuTitle|string|-|The text displayed on the menu banner. If this is null or empty, no banner is drawn.|Yes|
|MenuSubtitle|string|Null|The text displayed in the subtitle bar below the banner.|Yes|
|HeaderTexture|KeyValuePair&lt;string,&nbsp;string&gt;|(empty)|A custom banner image. The `Key` is the streamed texture dictionary, the `Value` is the texture name. The texture dictionary is requested for you. When this is not set, the default banner is used.|Yes|
|CounterPreText|string|Null|Text placed in front of the `current / total` counter in the top right of the subtitle bar. Setting this forces the counter to be shown, even when all items already fit on screen.|Yes|
|Visible|boolean|false|Whether this menu is currently being drawn. Prefer [OpenMenu()](#openmenu) and [CloseMenu()](#closemenu), because those also trigger the open/close events (and enable/disable the UI prompts).|Yes|
|EnableInstructionalButtons|boolean|true|Whether the instructional buttons (UI prompts) for this menu are shown while the menu is open.|Yes|
|IgnoreDontOpenMenus|boolean|false|When true, this menu keeps being drawn even while [MenuController.DontOpenAnyMenu](../menucontroller/#properties) is set to true.|Yes|
|InstructionalButtons|List&lt;[InstructionalButton](#instructionalbutton)&gt;|Select, Back, Up&nbsp;/&nbsp;Down|The instructional buttons for this menu. See [Instructional buttons](#instructional-buttons).|Yes|
|ButtonPressHandlers|List&lt;[ButtonPressHandler](#buttonpresshandler)&gt;|(empty)|Custom control handlers that run while this menu is open. See [Button press handlers](#button-press-handlers).|Yes|
|Size|int|0|(Getter only) The number of items in this menu. When a filter is active, this is the number of items that passed the filter.|Yes|
|CurrentIndex|int|0|(Getter only) The index of the currently highlighted menu item. Use [RefreshIndex()](#refreshindexint-index) to change it.|Yes|
|MaxItemsOnScreen|int|10|(Getter only) How many items are visible on screen at a time. Use [SetMaxItemsOnScreen()](#setmaxitemsonscreenint-max) to change it.|Yes|
|ViewIndexOffset|int|0|(Getter only) The index of the first item that is currently visible on screen.|Yes|
|ParentMenu|Menu|Null|(Getter only) The parent of this menu, or null if it has none. Set by [MenuController.AddSubmenu()](../menucontroller/#addsubmenumenu-parent-menu-child).|Yes|
|Position|KeyValuePair&lt;float,&nbsp;float&gt;|(0f,&nbsp;20f)|(Getter only) The current x/y position of the menu on screen.|Yes|
|MenuItemsYOffset|float|0f|(Getter only) The y offset (in pixels) of the first menu item, relative to the top of the menu. Recalculated every frame.|Yes|

----

### Constants

|Constant|Type|Value|Description|
|-|-|-|-|
|Width|float|300f|The width of a menu, in 1080p scaled pixels. (This is 500f in FiveM.)|

----

### Methods

----

#### SetMaxItemsOnScreen(int max)

Sets how many menu items are visible on screen at a time. The value is clamped between 3 and 10.

:::note
If the screen resolution is too small to fit this many items, MenuAPI automatically reduces the amount of visible items so the menu never runs off screen.
:::

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|max|int|A value between 3 and 10 (inclusive).|

##### Return value

_This function does not return anything_.

```cs
// Only show 5 items at a time.
menu.SetMaxItemsOnScreen(5);
```

----

#### RefreshIndex()

Resets the highlighted item back to the first item, and scrolls the menu back to the top.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### RefreshIndex(int index)

Highlights the item at `index` and scrolls the menu so that item is visible.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|index|int|The index of the item to highlight.|

##### Return value

_This function does not return anything_.

----

#### RefreshIndex(int index, int viewOffset)

Highlights the item at `index` and scrolls the menu so that the item at `viewOffset` is the first item on screen. Use this when you want full control over the scroll position.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|index|int|The index of the item to highlight.|
|viewOffset|int|The index of the item that should be drawn at the top of the menu.|

##### Return value

_This function does not return anything_.

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

Adds a menu item to the bottom of this menu, and sets that item's `ParentMenu` to this menu.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|item|[MenuItem](../menuitems/menuitem/)|The item to add. Any item type can be used, since they all inherit `MenuItem`.|

##### Return value

_This function does not return anything_.

----

#### RemoveMenuItem(int itemIndex)

Removes the item at the given index. The current index is corrected so the highlighted item does not jump around. Does nothing if the index is out of range.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|itemIndex|int|The index of the item to remove.|

##### Return value

_This function does not return anything_.

----

#### RemoveMenuItem(MenuItem item)

Removes the given item from this menu. Does nothing if the item is not in this menu.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|item|[MenuItem](../menuitems/menuitem/)|The item to remove.|

##### Return value

_This function does not return anything_.

----

#### ClearMenuItems()

Removes all menu items and resets the index and scroll position.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### ClearMenuItems(bool dontResetIndex)

Removes all menu items, optionally keeping the current index and scroll position. Useful when you rebuild the contents of a menu while it is open.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|dontResetIndex|boolean|When true, the current index and scroll position are kept.|

##### Return value

_This function does not return anything_.

```cs
// Rebuild the items without the menu jumping back to the top.
menu.ClearMenuItems(true);
BuildItems(menu);
```

----

#### GetMenuItems()

Returns the items in this menu. When a filter is active, only the items that passed the filter are returned.

:::note
This returns a **copy** of the internal list, so adding to or removing from the returned list does not change the menu. Use [AddMenuItem()](#addmenuitemmenuitem-item) and [RemoveMenuItem()](#removemenuitemmenuitem-item) for that.
:::

##### Parameters

_This function does not have any parameters_.

##### Return value

|Type|Description|
|-|-|
|List&lt;[MenuItem](../menuitems/menuitem/)&gt;|All (visible) menu items in this menu.|

```cs
// Toggle the 'enabled' state of every item in the menu.
menu.GetMenuItems().ForEach(item => item.Enabled = !item.Enabled);
```

----

#### GetCurrentMenuItem()

Returns the currently highlighted menu item.

##### Parameters

_This function does not have any parameters_.

##### Return value

|Type|Description|
|-|-|
|[MenuItem](../menuitems/menuitem/)|The currently highlighted item, or `null` if the menu is empty or the current index is out of range.|

----

#### SelectItem(int index)

Selects the item at the given index, exactly as if the user had pressed the select control on it. Disabled items play the error sound instead, and submenu buttons open their submenu.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|index|int|The index of the item to select.|

##### Return value

_This function does not return anything_.

----

#### SelectItem(MenuItem item)

Selects the given item, exactly as if the user had pressed the select control on it.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|item|[MenuItem](../menuitems/menuitem/)|The item to select.|

##### Return value

_This function does not return anything_.

----

#### OpenMenu()

Makes this menu visible and triggers the [OnMenuOpen](../events/#onmenuopen) event. This also prepares and enables the UI prompts for this menu's [instructional buttons](#instructional-buttons).

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### CloseMenu()

Hides this menu, triggers the [OnMenuClose](../events/#onmenuclose) event and disables this menu's UI prompts.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### GoBack()

Closes this menu and opens its parent menu. If there is no parent menu, the menu simply closes.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### GoUp()

Moves the highlighted item up by one, wrapping around to the bottom of the list. Triggers the [OnIndexChange](../events/#onindexchange) event. Does nothing if the menu is not visible or has fewer than 2 items.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### GoDown()

Moves the highlighted item down by one, wrapping around to the top of the list. Triggers the [OnIndexChange](../events/#onindexchange) event. Does nothing if the menu is not visible or has fewer than 2 items.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### GoLeft()

Moves the currently highlighted item to the left, if it supports that (list and dynamic list items). If the item does not support it and [MenuController.NavigateMenuUsingArrows](../menucontroller/#properties) is enabled, this returns to the parent menu instead.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### GoRight()

Moves the currently highlighted item to the right, if it supports that (list and dynamic list items).

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

#### SortMenuItems(Comparison&lt;MenuItem&gt; compare)

Sorts the items in this menu using the provided compare function. Any active filter is cleared first.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|compare|Comparison&lt;[MenuItem](../menuitems/menuitem/)&gt;|The compare function used to sort the items.|

##### Return value

_This function does not return anything_.

```cs
// Sort all items alphabetically by their text.
menu.SortMenuItems((a, b) => a.Text.ToLower().CompareTo(b.Text.ToLower()));
```

----

#### FilterMenuItems(Func&lt;MenuItem, bool&gt; predicate)

Hides every item for which the predicate returns false. The index and scroll position are reset. Only one filter can be active at a time; calling this again replaces the previous filter.

While a filter is active, [Size](#properties) and [GetMenuItems()](#getmenuitems) only report the items that passed the filter, but the hidden items are **not** removed from the menu.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|predicate|Func&lt;[MenuItem](../menuitems/menuitem/),&nbsp;bool&gt;|Returns true for every item that should stay visible.|

##### Return value

_This function does not return anything_.

```cs
// Only show items that are enabled.
menu.FilterMenuItems(item => item.Enabled);

// Or use ItemData to filter on your own data.
menu.FilterMenuItems(item => item.ItemData?.category == "horses");

// Show everything again.
menu.ResetFilter();
```

----

#### ResetFilter()

Clears the active filter, making all items visible again, and resets the index.

##### Parameters

_This function does not have any parameters_.

##### Return value

_This function does not return anything_.

----

### Instructional buttons

In RedM, instructional buttons are drawn as native **UI prompts**. Every menu has its own set, and by default that is **select**, **back** and **up / down**.

The prompts are registered and enabled when the menu is opened, and disabled again when it is closed. Set [EnableInstructionalButtons](#properties) to false before opening the menu if you want to handle prompts yourself.

```cs
// Add your own prompt.
menu.InstructionalButtons.Add(
    new Menu.InstructionalButton(new Control[1] { Control.Reload }, "Reset")
);

// Add a prompt for a combination of controls.
menu.InstructionalButtons.Add(
    new Menu.InstructionalButton(
        new Control[2] { Control.FrontendLeft, Control.FrontendRight },
        "Change value"
    )
);

// Remove the default 'up / down' prompt.
Menu.InstructionalButton upDown = menu.InstructionalButtons
    .Find(b => b.GetTextString() == "Up / Down");

upDown.Dispose();
menu.InstructionalButtons.Remove(upDown);

// Or turn them off entirely for this menu.
menu.EnableInstructionalButtons = false;
```

:::caution
Always call [Dispose()](#dispose) on a prompt you remove from the list, otherwise it stays registered with the game and can get stuck on screen. MenuAPI closes all menus when your resource stops for this same reason.
:::

#### InstructionalButton

##### InstructionalButton(Control[] controls, string text)

Creates a new prompt for one or more controls.

|Parameter|Type|Description|
|-|-|-|
|controls|Control[]|The controls this prompt listens for. Pass more than one to show a combination.|
|text|string|The text displayed next to the prompt.|

##### Properties

|Property|Type|Description|
|-|-|-|
|IsPrepared|boolean|(Getter only) Whether the prompt has been registered with the game and is ready to be displayed.|

##### Prepare()

Registers the prompt with the game. This is done for you when the menu is opened. Does nothing if the prompt is already prepared.

##### SetEnabled(bool visible, bool enabled)

Shows/hides and enables/disables the prompt on screen. The prompt has to be prepared first.

|Parameter|Type|Description|
|-|-|-|
|visible|boolean|Whether the prompt is visible.|
|enabled|boolean|Whether the prompt can be triggered.|

##### Dispose()

Removes the prompt from the game. `Prepare()` has to be called again before the prompt can be used.

##### GetTextString()

Returns the text of this prompt.

|Type|Description|
|-|-|
|string|The prompt text.|

##### GetControls()

Returns the controls of this prompt.

|Type|Description|
|-|-|
|Control[]|The controls this prompt listens for.|

----

### Button press handlers

Button press handlers let you run your own code when a control is pressed while this menu is open, without having to write your own tick function. They are only processed while the menu is open and while [MenuController.DisableMenuButtons](../menucontroller/#properties) is false.

#### ButtonPressHandler

|Parameter|Type|Description|
|-|-|-|
|control|Control|The control to listen for.|
|pressType|[ControlPressCheckType](#controlpresschecktype)|How the control should be checked.|
|function|Action&lt;Menu,&nbsp;Control&gt;|The function to run. It receives the menu and the control that triggered it.|
|disableControl|boolean|Whether the control should be disabled (blocking the game's own action) while this menu is open.|

#### ControlPressCheckType

|Value|Description|
|-|-|
|ControlPressCheckType.JUST_PRESSED|Triggers once, on the frame the control is pressed down.|
|ControlPressCheckType.JUST_RELEASED|Triggers once, on the frame the control is released.|
|ControlPressCheckType.PRESSED|Triggers every frame while the control is held down.|
|ControlPressCheckType.RELEASED|Triggers every frame while the control is not held down.|

```cs
// Toggle the 'enabled' state of every item whenever this control is released.
menu.ButtonPressHandlers.Add(
    new Menu.ButtonPressHandler(
        Control.FrontendSocialClubSecondary,
        Menu.ControlPressCheckType.JUST_RELEASED,
        new Action<Menu, Control>((m, c) =>
        {
            m.GetMenuItems().ForEach(item => item.Enabled = !item.Enabled);
        }),
        true
    )
);
```

:::note
RedM's `CitizenFX.Core` does not ship a `Control` enum, so MenuAPI provides its own. It is included in `MenuAPI.dll` under the usual `CitizenFX.Core.Control` name, so `using CitizenFX.Core;` is all you need to use it.
:::

----

### FiveM only members

These members are part of the FiveM build of MenuAPI and are **not** available (or have no effect) in RedM:

|Member|Notes|
|-|-|
|LeftAligned|Right aligned menus are FiveM only, so this property does not exist in the RedM build.|
|CustomInstructionalButtons|FiveM only. In RedM, add your own [InstructionalButton](#instructionalbutton) to `InstructionalButtons` instead.|
|SetWeaponStats(), SetWeaponComponentStats()|FiveM only. The weapon stats panel is not drawn in RedM.|
|SetVehicleStats(), SetVehicleUpgradeStats()|FiveM only. The vehicle stats panel is not drawn in RedM.|
|ShowWeaponStatsPanel, ShowVehicleStatsPanel|These properties exist in the RedM build, but setting them has no effect because the panels are never drawn.|
|WeaponStats, WeaponComponentStats, VehicleStats, VehicleUpgradeStats|These properties exist in the RedM build, but always contain zeroes because there is no way to set them.|

----

### Events

All menu events are documented on the [Events](../events/) page.
