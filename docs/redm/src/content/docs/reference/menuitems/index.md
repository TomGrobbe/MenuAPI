---
title: "MenuItems"
---

## Menu items

Menu items are the rows inside a [Menu](../menu/). There are four types in the RedM build, and all of them inherit the standard [MenuItem](menuitem/) class, so everything on that page (text, description, icons, enabled state, `ItemData`, …) applies to every item type.

Adding an item to a menu is always the same:

```cs
menu.AddMenuItem(item);
```

----

### The item types

|Type|What it does|Left / right does|
|-|-|-|
|[MenuItem](menuitem/)|A plain button. Also used as a submenu button when you bind a menu to it.|Right presses the item, left goes back.|
|[MenuCheckboxItem](menucheckboxitem/)|A button with a checkbox on the right.|Right toggles the checkbox.|
|[MenuListItem](menulistitem/)|Picks one value out of a fixed list of strings.|Moves through the list (wraps around).|
|[MenuDynamicListItem](menudynamiclistitem/)|Like a list item, but a callback decides what the next value is. Use this for endless or expensive-to-build lists.|Calls your callback for the new value.|

:::caution
`MenuSliderItem` is FiveM only and does not exist in the RedM build of MenuAPI. RedM also has far fewer [icons](menuitem/#icons), and no color or opacity panels.
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
```

----

### Reacting to menu items

Every item type raises its own event on the parent menu. `OnItemSelect` only fires for plain `MenuItem`s — see [Events](../events/#which-event-fires-for-which-item) for the full list.

```cs
menu.OnItemSelect += (_menu, _item, _index) => { /* plain buttons */ };
menu.OnCheckboxChange += (_menu, _item, _index, _checked) => { /* checkboxes */ };
menu.OnListIndexChange += (_menu, _item, _oldIndex, _newIndex, _itemIndex) => { /* list items */ };
```
