---
title: "MenuItems"
---

## Menu items

Menu items are the rows inside a [Menu](../menu/). All six types inherit [MenuItem](menuitem/), so everything on that page applies to every type. Add any item with:

```cs
menu.AddMenuItem(item);
```

----

### The item types

|Type|What it does|Left / right does|
|-|-|-|
|[MenuItem](menuitem/)|A plain button. Also used to open a submenu.|Right presses the item, left goes back.|
|[MenuCheckboxItem](menucheckboxitem/)|A button with a checkbox on the right.|Right toggles the checkbox.|
|[MenuListItem](menulistitem/)|Picks one value out of a fixed list of strings.|Moves through the list, wrapping around.|
|[MenuDynamicListItem](menudynamiclistitem/)|A callback returns the next value. For endless or expensive lists.|Calls your callback for the new value.|
|[MenuSliderItem](menuslideritem/)|A slider between a minimum and maximum position.|Moves the slider, without wrapping.|
|[SeparatorMenuItem](separatormenuitem/)|A centred heading for the rows under it. Pressing it does nothing.|Nothing.|

:::note
`RightIcon` and `Label` are unavailable on types that draw their own content on the right. Each page lists its limitations.
:::

----

### Example usage

```cs
Menu menu = new Menu("Item Types", "One of each");
MenuController.AddMenu(menu);

// A plain button with an icon and text on the right.
menu.AddMenuItem(new MenuItem("Button", "A plain button.")
{
    Label = "Right text",
    LeftIcon = MenuItem.Icon.STAR
});

// A disabled (greyed out) button.
menu.AddMenuItem(new MenuItem("Locked button", "You can't press me.")
{
    Enabled = false,
    LeftIcon = MenuItem.Icon.LOCK
});

// A checkbox.
menu.AddMenuItem(new MenuCheckboxItem("Checkbox", "Toggle me.", true));

// A list item.
menu.AddMenuItem(new MenuListItem("List", new List<string>() { "One", "Two", "Three" }, 0, "Pick one."));

// A dynamic list item: the callback decides what the next value is.
string ChangeCallback(MenuDynamicListItem item, bool left)
{
    int value = int.Parse(item.CurrentItem);
    return (left ? value - 1 : value + 1).ToString();
}
menu.AddMenuItem(new MenuDynamicListItem("Dynamic list", "0", new MenuDynamicListItem.ChangeItemCallback(ChangeCallback), "Counts up and down."));

// A slider.
menu.AddMenuItem(new MenuSliderItem("Slider", 0, 10, 5, true));

// A centred heading for whatever rows come after it. Pressing it does nothing.
menu.AddMenuItem(new SeparatorMenuItem("Section heading"));
```

----

### Reacting to menu items

Each item type raises its own event on the parent menu. `OnItemSelect` only fires for plain `MenuItem`s. See [Events](../events/#which-event-fires-for-which-item) for the full list.

```cs
menu.OnItemSelect += (_menu, _item, _index) => { /* plain buttons */ };
menu.OnCheckboxChange += (_menu, _item, _index, _checked) => { /* checkboxes */ };
menu.OnListIndexChange += (_menu, _item, _oldIndex, _newIndex, _itemIndex) => { /* list items */ };
```
