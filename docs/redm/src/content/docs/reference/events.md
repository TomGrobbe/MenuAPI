---
title: "Events"
---

## Events

Every event in MenuAPI is a member of a [Menu](../menu/), so you always subscribe on the menu that the item belongs to — not on the item itself.

```cs
menu.OnItemSelect += (_menu, _item, _index) =>
{
    Debug.WriteLine($"'{_item.Text}' was pressed.");
};
```

MenuAPI uses plain delegates, so a lambda, a `new EventHandler(...)` style delegate or a normal method all work:

```cs
// Lambda
menu.OnMenuOpen += (_menu) => Debug.WriteLine("Opened!");

// Method group
menu.OnMenuOpen += HandleMenuOpen;

private void HandleMenuOpen(Menu menu)
{
    Debug.WriteLine($"'{menu.MenuTitle}' was opened.");
}
```

----

### Which event fires for which item?

:::caution
Each item type raises **only its own** select event. `OnItemSelect` is *not* a catch-all: pressing a checkbox raises `OnCheckboxChange`, pressing a list item raises `OnListItemSelect`, and so on.
:::

|Item type|Pressing select|Pressing left / right|
|-|-|-|
|[MenuItem](../menuitems/menuitem/)|[OnItemSelect](#onitemselect)|Right selects the item, left goes back to the parent menu.|
|[MenuCheckboxItem](../menuitems/menucheckboxitem/)|[OnCheckboxChange](#oncheckboxchange)|Right toggles the checkbox (so it also raises `OnCheckboxChange`).|
|[MenuListItem](../menuitems/menulistitem/)|[OnListItemSelect](#onlistitemselect)|[OnListIndexChange](#onlistindexchange)|
|[MenuDynamicListItem](../menuitems/menudynamiclistitem/)|[OnDynamicListItemSelect](#ondynamiclistitemselect)|[OnDynamicListItemCurrentItemChange](#ondynamiclistitemcurrentitemchange)|

Two more events are not tied to a specific item type: [OnIndexChange](#onindexchange) (the highlighted item changed) and [OnMenuOpen](#onmenuopen) / [OnMenuClose](#onmenuclose).

:::note
The FiveM build also has `OnSliderPositionChange` and `OnSliderItemSelect`. Those events, and the slider items they belong to, do not exist in the RedM build.
:::

----

### Full example

```cs
Menu menu = new Menu("Main Menu", "Subtitle");
MenuController.AddMenu(menu);

MenuItem button = new MenuItem("Normal Button", "A simple button.");
MenuCheckboxItem box = new MenuCheckboxItem("Checkbox", "A checkbox.", false);
MenuListItem list = new MenuListItem("List", new List<string>() { "One", "Two", "Three" }, 0);

menu.AddMenuItem(button);
menu.AddMenuItem(box);
menu.AddMenuItem(list);

menu.OnItemSelect += (_menu, _item, _index) =>
{
    if (_item == button)
    {
        Debug.WriteLine("The normal button was pressed!");
    }
};

menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
{
    Debug.WriteLine($"'{_item.Text}' is now {(_checked ? "checked" : "unchecked")}.");
};

menu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
{
    Debug.WriteLine($"List changed to '{_listItem.GetCurrentSelection()}'.");
};

menu.OnListItemSelect += (_menu, _listItem, _listIndex, _itemIndex) =>
{
    Debug.WriteLine($"'{_listItem.ListItems[_listIndex]}' was picked.");
};

menu.OnMenuOpen += (_menu) => Debug.WriteLine("Menu opened.");
menu.OnMenuClose += (_menu) => Debug.WriteLine("Menu closed.");
```

----

### OnItemSelect

Triggered when a [MenuItem](../menuitems/menuitem/) is pressed. This only fires for plain menu items, not for checkbox or list items.

If the item is bound to a submenu with [MenuController.BindMenuItem()](../menucontroller/#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem), this event fires **before** the submenu opens.

Disabled items (`Enabled = false`) never raise this event; they play the error sound instead.

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|menuItem|[MenuItem](../menuitems/menuitem/)|The item that was pressed.|
|itemIndex|int|The index of that item in the menu.|

```cs
menu.OnItemSelect += (_menu, _item, _index) =>
{
    // Comparing against the item you created is the most reliable check.
    if (_item == deleteButton)
    {
        DeleteTheThing();
    }

    // Or use ItemData when your items are created in a loop.
    if (_item.ItemData != null)
    {
        SpawnHorse(_item.ItemData.model);
    }
};
```

----

### OnCheckboxChange

Triggered when a [MenuCheckboxItem](../menuitems/menucheckboxitem/) is toggled, either by pressing select on it or by pressing right on it. The `Checked` property has already been updated when this fires.

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|menuItem|[MenuCheckboxItem](../menuitems/menucheckboxitem/)|The checkbox that was toggled.|
|itemIndex|int|The index of that checkbox in the menu.|
|newCheckedState|boolean|The new checked state.|

```cs
menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
{
    if (_item == godModeBox)
    {
        SetEntityInvincible(PlayerPedId(), _checked);
    }
};
```

----

### OnListItemSelect

Triggered when a [MenuListItem](../menuitems/menulistitem/) is pressed (select), not when its value is changed with left/right.

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|listItem|[MenuListItem](../menuitems/menulistitem/)|The list item that was pressed.|
|selectedIndex|int|The currently selected index inside that list item's `ListItems`.|
|itemIndex|int|The index of the list item in the menu.|

```cs
menu.OnListItemSelect += (_menu, _listItem, _listIndex, _itemIndex) =>
{
    if (_listItem == weatherList)
    {
        SetWeather(_listItem.ListItems[_listIndex]);
    }
};
```

----

### OnListIndexChange

Triggered every time the value of a [MenuListItem](../menuitems/menulistitem/) is changed with the left or right control. List items wrap around, so going right on the last value moves back to the first one.

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|listItem|[MenuListItem](../menuitems/menulistitem/)|The list item that was changed.|
|oldSelectionIndex|int|The previously selected index.|
|newSelectionIndex|int|The newly selected index.|
|itemIndex|int|The index of the list item in the menu.|

```cs
// Live preview: update something as soon as the value changes.
menu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
{
    if (_listItem == timeOfDayList)
    {
        SetTimeOfDay(_newIndex);
    }
};
```

----

### OnDynamicListItemCurrentItemChange

Triggered every time the value of a [MenuDynamicListItem](../menuitems/menudynamiclistitem/) is changed with the left or right control, right after the item's callback has returned the new value.

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|dynamicListItem|[MenuDynamicListItem](../menuitems/menudynamiclistitem/)|The dynamic list item that was changed.|
|oldValue|string|The value before the change.|
|newValue|string|The value the callback returned.|

```cs
menu.OnDynamicListItemCurrentItemChange += (_menu, _item, _oldValue, _newValue) =>
{
    Debug.WriteLine($"'{_item.Text}' changed from {_oldValue} to {_newValue}.");
};
```

----

### OnDynamicListItemSelect

Triggered when a [MenuDynamicListItem](../menuitems/menudynamiclistitem/) is pressed (select).

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|dynamicListItem|[MenuDynamicListItem](../menuitems/menudynamiclistitem/)|The dynamic list item that was pressed.|
|currentItem|string|The value that is currently being displayed.|

----

### OnIndexChange

Triggered whenever the highlighted item changes, so every time the user presses up or down. The list wraps around at both ends.

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|oldItem|[MenuItem](../menuitems/menuitem/)|The item that was highlighted before.|
|newItem|[MenuItem](../menuitems/menuitem/)|The item that is highlighted now.|
|oldIndex|int|The previous index.|
|newIndex|int|The new index.|

```cs
// Load a preview of whatever the user is currently looking at.
menu.OnIndexChange += (_menu, _oldItem, _newItem, _oldIndex, _newIndex) =>
{
    if (_newItem?.ItemData != null)
    {
        PreviewHorse(_newItem.ItemData.model);
    }
};
```

----

### OnMenuOpen

Triggered when a menu is opened, either by the user or by a call to [Menu.OpenMenu()](../menu/#openmenu).

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu that was opened.|

```cs
// Refresh the contents every time the menu is opened.
menu.OnMenuOpen += (_menu) =>
{
    _menu.ClearMenuItems();
    BuildItems(_menu);
};
```

----

### OnMenuClose

Triggered when a menu is closed, either by the user or by a call to [Menu.CloseMenu()](../menu/#closemenu), [Menu.GoBack()](../menu/#goback) or [MenuController.CloseAllMenus()](../menucontroller/#closeallmenus).

:::note
Opening a submenu closes the parent menu first, so this event also fires when the user navigates *into* a submenu.
:::

#### Parameters

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu that was closed.|

```cs
menu.OnMenuClose += (_menu) =>
{
    SaveSettings();
};
```

----

### Delegate signatures

If you prefer to declare your handlers explicitly, these are the delegate types the events use. They are all nested in `Menu`.

|Event|Delegate|
|-|-|
|OnItemSelect|`void ItemSelectEvent(Menu menu, MenuItem menuItem, int itemIndex)`|
|OnCheckboxChange|`void CheckboxItemChangeEvent(Menu menu, MenuCheckboxItem menuItem, int itemIndex, bool newCheckedState)`|
|OnListItemSelect|`void ListItemSelectedEvent(Menu menu, MenuListItem listItem, int selectedIndex, int itemIndex)`|
|OnListIndexChange|`void ListItemIndexChangedEvent(Menu menu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)`|
|OnDynamicListItemCurrentItemChange|`void DynamicListItemCurrentItemChangedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string oldValue, string newValue)`|
|OnDynamicListItemSelect|`void DynamicListItemSelectedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string currentItem)`|
|OnIndexChange|`void IndexChangedEvent(Menu menu, MenuItem oldItem, MenuItem newItem, int oldIndex, int newIndex)`|
|OnMenuOpen|`void MenuOpenedEvent(Menu menu)`|
|OnMenuClose|`void MenuClosedEvent(Menu menu)`|
