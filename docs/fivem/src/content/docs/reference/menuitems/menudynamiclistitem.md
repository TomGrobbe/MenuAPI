---
title: "MenuDynamicListItem"
---

## MenuDynamicListItem

A list item without a fixed list. Left and right call your callback, and the string it returns becomes the displayed value. Pressing select raises a separate event.

----

### Example usage

```cs
// The title for the menu item.
string title = "Dynamic list item.";

// The initial selected list item text.
string initialSelectedText = "0";

// Create the callback function for when the left/right arrow
// keys are pressed. Returning a new string value which will become the new
// selected text.
string ChangeCallback(MenuDynamicListItem item, bool left)
{
    // Left will be true when the left arrow key was pressed
    // and false if the right arrow key was pressed.
    if (left)
        return (int.Parse(item.CurrentItem) - 1).ToString();

    return (int.Parse(item.CurrentItem) + 1).ToString();
}

// Use the ChangeCallback function from above to create a new callback delegate.
MenuDynamicListItem.ChangeItemCallback callback = new MenuDynamicListItem.ChangeItemCallback(ChangeCallback);

// optional description
string description = "Description for this dynamic item. Pressing left will make the value smaller, pressing right will make the value bigger.";

// Create the dynamic list item using the variables above.
MenuDynamicListItem dynList = new MenuDynamicListItem(title, initialSelectedText, callback, description);

// Add a menu item to a menu:
menu.AddMenuItem(dynList);
```

#### A clamped counter

The callback can clamp, step or format the value however you like.

```cs
string VolumeCallback(MenuDynamicListItem item, bool left)
{
    int value = int.Parse(item.CurrentItem.Replace("%", ""));
    value = MathUtil.Clamp(left ? value - 5 : value + 5, 0, 100);
    return $"{value}%";
}

menu.AddMenuItem(new MenuDynamicListItem(
    "Volume",
    "50%",
    new MenuDynamicListItem.ChangeItemCallback(VolumeCallback),
    "Changes in steps of 5%, between 0% and 100%."
));
```

#### Reacting to a dynamic list item

```cs
// Fires after your callback returned a new value.
menu.OnDynamicListItemCurrentItemChange += (_menu, _item, _oldValue, _newValue) =>
{
    Debug.WriteLine($"'{_item.Text}' went from {_oldValue} to {_newValue}.");
};

// Fires when the user presses select on the item.
menu.OnDynamicListItemSelect += (_menu, _item, _currentItem) =>
{
    Debug.WriteLine($"'{_item.Text}' was pressed while showing {_currentItem}.");
};
```

:::caution
`Callback` is called without a null check, so it must never be null. Returning null from it makes the item show `N/A`.
:::

----

### Constructors

|Constructor|Description|
|-|-|
|`MenuDynamicListItem(string text, string initialValue, ChangeItemCallback callback)`|Creates a dynamic list item without a description.|
|`MenuDynamicListItem(string text, string initialValue, ChangeItemCallback callback, string description)`|Creates a dynamic list item with a description.|

|Parameter|Type|Description|
|-|-|-|
|text|string|The text on the left side of the item.|
|initialValue|string|The value shown before the user changes anything.|
|callback|[ChangeItemCallback](#changeitemcallback)|Returns the next value.|
|description|string|Shown below the menu while the item is highlighted.|

----

### ChangeItemCallback

```cs
public delegate string ChangeItemCallback(MenuDynamicListItem item, bool left);
```

Runs on every left or right press. The returned string becomes the new `CurrentItem`.

|Parameter|Type|Description|
|-|-|-|
|item|MenuDynamicListItem|The item being changed. `item.CurrentItem` holds the current value.|
|left|bool|True for the **left** control, false for the **right** control.|

----

### Properties

:::note
**RightIcon** and **Label** are not available for MenuDynamicListItems.
:::

|Property|Type|Default value|Description|
|---|---|---|---|
|HideArrowsWhenNotSelected|bool|false|Hides the left and right arrows while the item is not highlighted.|
|CurrentItem|string|`initialValue` argument|The value being displayed. Can be set.|
|Callback|ChangeItemCallback|`callback` argument|Called on left and right to get the next value.|

----

### Methods

MenuDynamicListItem has no methods of its own.
