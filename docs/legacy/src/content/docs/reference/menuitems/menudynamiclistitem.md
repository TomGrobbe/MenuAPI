---
title: "MenuDynamicListItem"
---

## MenuDynamicListItem

A menu item with a dynamic list of values.

Anytime the left or right arrow keys are pressed, a callback function will be executed which will determine what the next value to be displayed should be.

You can also select a menu dynamic list item by pressing enter, which will trigger a separate event that you can listen for.

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

Because the callback decides what the next value is, you can clamp it, step by any amount, or format it however you like.

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
The callback is called directly, without a null check, so `Callback` must never be null. It also has to return a value for every input — returning null makes the item display `N/A`.
:::

----

### Constructors

----

#### MenuDynamicListItem(string text, string initialValue, ChangeItemCallback callback)

Creates a dynamic list item without a description.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The text displayed on the left side of the item.|
|initialValue|string|The value that is displayed before the user changes anything.|
|callback|[ChangeItemCallback](#changeitemcallback)|The function that returns the next value.|

----

#### MenuDynamicListItem(string text, string initialValue, ChangeItemCallback callback, string description)

Creates a dynamic list item with a description.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The text displayed on the left side of the item.|
|initialValue|string|The value that is displayed before the user changes anything.|
|callback|[ChangeItemCallback](#changeitemcallback)|The function that returns the next value.|
|description|string|The description shown below the menu while this item is selected.|

----

### ChangeItemCallback

```cs
public delegate string ChangeItemCallback(MenuDynamicListItem item, bool left);
```

The callback runs every time the user presses left or right on the item. Whatever string you return becomes the new displayed value.

|Parameter|Type|Description|
|-|-|-|
|item|MenuDynamicListItem|The item the user is interacting with. Read `item.CurrentItem` to get the current value.|
|left|boolean|True when the **left** control was pressed, false when the **right** control was pressed.|

|Return type|Description|
|-|-|
|string|The new value to display.|

----

### Properties

:::note
All standard MenuItem class properties are inherited.
The MenuItem properties **RightIcon** and **Label** are not available for MenuDynamicListItems.
:::

|Property|Type|Default value|Description|Optional|
|---|---|---|---|---|
|HideArrowsWhenNotSelected|boolean|false|Hides the left & right arrows when the menu list item is not currently highlighted.|Yes|
|CurrentItem|string|Null|Gets or sets the current text that is being displayed.|**No**|
|Callback|ChangeItemCallback|Null|The callback function to trigger when the left or right arrow key was pressed.|**No**|

----

### Methods

_There are no methods for MenuDynamicListItems._
