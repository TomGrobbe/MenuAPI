---
title: "Events"
---

## Events

Every event is a member of [Menu](../menu/), so you subscribe on the menu the item belongs to, not on the item. Lambdas and methods both work.

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
`OnItemSelect` is *not* a catch-all. Each item type raises **only its own** select event, so pressing a checkbox raises `OnCheckboxChange`, pressing a list item raises `OnListItemSelect`, and so on.
:::

|Item type|Pressing select|Pressing left / right|
|-|-|-|
|[MenuItem](../menuitems/menuitem/)|[OnItemSelect](#onitemselect)|Right selects the item, left goes back to the parent menu.|
|[MenuCheckboxItem](../menuitems/menucheckboxitem/)|[OnCheckboxChange](#oncheckboxchange)|Right toggles the checkbox (raising `OnCheckboxChange`), left goes back to the parent menu.|
|[MenuListItem](../menuitems/menulistitem/)|[OnListItemSelect](#onlistitemselect)|[OnListIndexChange](#onlistindexchange)|
|[MenuDynamicListItem](../menuitems/menudynamiclistitem/)|[OnDynamicListItemSelect](#ondynamiclistitemselect)|[OnDynamicListItemCurrentItemChange](#ondynamiclistitemcurrentitemchange)|
|[MenuSliderItem](../menuitems/menuslideritem/)|[OnSliderItemSelect](#onslideritemselect)|[OnSliderPositionChange](#onsliderpositionchange)|

Not tied to an item type: [OnIndexChange](#onindexchange), [OnMenuOpen](#onmenuopen), [OnMenuClose](#onmenuclose) and [OnPageChange](../pagination/#the-onpagechange-event).

:::note
In a [paginated](../pagination/) menu, left and right turn the page on every item type except list, dynamic list and slider items.
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

A [MenuItem](../menuitems/menuitem/) was pressed. For an item bound with [MenuController.BindMenuItem()](../menucontroller/#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem), this fires **before** the submenu opens. Disabled items never raise it, they play the error sound instead.

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
        SpawnVehicle(_item.ItemData.model);
    }
};
```

----

### OnCheckboxChange

A [MenuCheckboxItem](../menuitems/menucheckboxitem/) was toggled with select or right. `Checked` is already updated when this fires.

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
        SetPlayerInvincible(Game.Player.Handle, _checked);
    }
};
```

----

### OnListItemSelect

A [MenuListItem](../menuitems/menulistitem/) was pressed with select. Changing its value with left or right does not raise this.

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|listItem|[MenuListItem](../menuitems/menulistitem/)|The list item that was pressed.|
|selectedIndex|int|The selected index inside that list item's `ListItems`.|
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

The value of a [MenuListItem](../menuitems/menulistitem/) was changed with left or right. Lists wrap around at both ends.

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
    if (_listItem == hairColors)
    {
        SetPedHairColor(PlayerPedId(), _newIndex, 0);
    }
};
```

----

### OnSliderPositionChange

The position of a [MenuSliderItem](../menuitems/menuslideritem/) was changed with left or right. Sliders do **not** wrap: at `Min` or `Max` the error sound plays and this does not fire.

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|sliderItem|[MenuSliderItem](../menuitems/menuslideritem/)|The slider that was changed.|
|oldPosition|int|The previous slider position.|
|newPosition|int|The new slider position.|
|itemIndex|int|The index of the slider in the menu.|

```cs
menu.OnSliderPositionChange += (_menu, _sliderItem, _oldPosition, _newPosition, _itemIndex) =>
{
    if (_sliderItem == volumeSlider)
    {
        SetVolume(_newPosition / 10f);
    }
};
```

----

### OnSliderItemSelect

A [MenuSliderItem](../menuitems/menuslideritem/) was pressed with select.

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|sliderItem|[MenuSliderItem](../menuitems/menuslideritem/)|The slider that was pressed.|
|sliderPosition|int|The current slider position.|
|itemIndex|int|The index of the slider in the menu.|

----

### OnDynamicListItemCurrentItemChange

The value of a [MenuDynamicListItem](../menuitems/menudynamiclistitem/) was changed with left or right. Fires right after the item's callback returned the new value.

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|dynamicListItem|[MenuDynamicListItem](../menuitems/menudynamiclistitem/)|The dynamic list item that was changed.|
|oldValue|string?|The value before the change.|
|newValue|string|The value the callback returned.|

```cs
menu.OnDynamicListItemCurrentItemChange += (_menu, _item, _oldValue, _newValue) =>
{
    Debug.WriteLine($"'{_item.Text}' changed from {_oldValue} to {_newValue}.");
};
```

----

### OnDynamicListItemSelect

A [MenuDynamicListItem](../menuitems/menudynamiclistitem/) was pressed with select.

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|dynamicListItem|[MenuDynamicListItem](../menuitems/menudynamiclistitem/)|The dynamic list item that was pressed.|
|currentItem|string?|The value currently displayed.|

----

### OnIndexChange

The highlighted item changed, by up, down or the scroll wheel. The list wraps around at both ends. It does not fire in a menu with fewer than 2 items.

|Parameter|Type|Description|
|-|-|-|
|menu|[Menu](../menu/)|The menu in which this event occurred.|
|oldItem|[MenuItem](../menuitems/menuitem/)|The item that was highlighted before.|
|newItem|[MenuItem](../menuitems/menuitem/)?|The item that is highlighted now. Can be null.|
|oldIndex|int|The previous index.|
|newIndex|int|The new index.|

```cs
// Load a preview of whatever the user is currently looking at.
menu.OnIndexChange += (_menu, _oldItem, _newItem, _oldIndex, _newIndex) =>
{
    if (_newItem?.ItemData != null)
    {
        PreviewVehicle(_newItem.ItemData.model);
    }
};
```

----

### OnMenuOpen

The menu was opened, by the player or by [Menu.OpenMenu()](../menu/#openmenu).

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

The menu was closed, by the player or by [Menu.CloseMenu()](../menu/#closemenu), [Menu.GoBack()](../menu/#goback) or [MenuController.CloseAllMenus()](../menucontroller/#closeallmenus).

:::note
Opening a submenu closes the parent menu first, so this also fires when the player goes *into* a submenu.
:::

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

All delegate types are nested in `Menu`.

|Event|Delegate|
|-|-|
|OnItemSelect|`void ItemSelectEvent(Menu menu, MenuItem menuItem, int itemIndex)`|
|OnCheckboxChange|`void CheckboxItemChangeEvent(Menu menu, MenuCheckboxItem menuItem, int itemIndex, bool newCheckedState)`|
|OnListItemSelect|`void ListItemSelectedEvent(Menu menu, MenuListItem listItem, int selectedIndex, int itemIndex)`|
|OnListIndexChange|`void ListItemIndexChangedEvent(Menu menu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)`|
|OnSliderPositionChange|`void SliderPositionChangedEvent(Menu menu, MenuSliderItem sliderItem, int oldPosition, int newPosition, int itemIndex)`|
|OnSliderItemSelect|`void SliderItemSelectedEvent(Menu menu, MenuSliderItem sliderItem, int sliderPosition, int itemIndex)`|
|OnDynamicListItemCurrentItemChange|`void DynamicListItemCurrentItemChangedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string? oldValue, string newValue)`|
|OnDynamicListItemSelect|`void DynamicListItemSelectedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string? currentItem)`|
|OnIndexChange|`void IndexChangedEvent(Menu menu, MenuItem oldItem, MenuItem? newItem, int oldIndex, int newIndex)`|
|OnMenuOpen|`void MenuOpenedEvent(Menu menu)`|
|OnMenuClose|`void MenuClosedEvent(Menu menu)`|
|OnPageChange|`void PageChangedEvent(Menu menu, int oldPage, int newPage, bool wrapped)`|
