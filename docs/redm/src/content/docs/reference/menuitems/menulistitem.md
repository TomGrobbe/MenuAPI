---
title: "MenuListItem"
---

## MenuListItem

A menu item with a list of (string) values, from which one can be selected using the left/right arrow keys.
You can also select a menu list item by pressing enter, which will trigger a separate event that you can listen for.

----

### Example usage

```cs
// A list (of strings) containing all selectable values.
// Note: At least one value must be provided.
List<string> values = new List<string>() {
    "Option 1",
    "Option 2",
    "Option 3"
};

// Specify the index which will be used when the item is visible by default.
// Value must be a valid index value for the given values list.
int currentIndex = 0; // ("Option 1")

MenuListItem item = new MenuListItem("Item Text", values, currentIndex, "Item description");

// Add a menu item to a menu:
menu.AddMenuItem(item);
```

----

### Properties

:::note
All standard MenuItem class properties are inherited.
The MenuItem properties **RightIcon** and **Label** are not available for MenuListItems.
:::

:::caution
Color panels are not available in RedM. The **ShowColorPanel** and **ColorPanelColorType** properties, along with the color panel section below, are FiveM-only.
:::

|Property|Type|Default value|Description|Optional|
|---|---|---|---|---|
|ListIndex|int|0|The currently selected list index.|**No**|
|ListItems|List&lt;string&gt;|-|A list holding a collection of strings which can be selected through this menu list item. At least one string must be present in this list.|**No**|
|HideArrowsWhenNotSelected|boolean|false|Hides the left & right arrows when the menu list item is not currently highlighted.|Yes|
|ShowOpacityPanel|boolean|false|Shows the Opacity Panel.|Yes|
|ShowColorPanel|boolean|false|Shows the Color Panel.|Yes|
|ColorPanelColorType|[ColorPanelType](#color-panel-types)|ColorPanelType.Hair|Shows the Color Panel.|Yes|
|ItemsCount|int|0|Returns how many ListItems there are in this MenuListItem.|Yes|

----

### Methods

----

#### GetCurrentSelection()

##### Parameters

_This function does not have any parameters_.

##### Return value

|Type|Description|
|-|-|
|string|Returns the currently selected ListItem value.|

----

#### Color panel types

:::caution
Color panels are not available in RedM. This section is FiveM-only.
:::

Color panels are shown below the menu, right under the item's description text (if present).

|Type|Default|Example|
|-|-|-|
|ColorPanelType.Hair|Yes|![Hair color panel](https://vespura.com/hi/i/20-04-18_14-31-06_0BBvH_3287.png)|
|ColorPanelType.Makeup|No|![Makeup color panel](https://vespura.com/hi/i/20-04-18_14-31-45_9mgFr_3288.png)|

----

#### Opacity panel example

Opacity panels are also shown below the menu, right under the item's description text (if present).

![Opacity panel](https://vespura.com/hi/i/20-04-18_14-34-02_730Ed_3289.png)
