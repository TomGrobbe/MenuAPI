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
menu.AddMenuItem(item);
```

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
