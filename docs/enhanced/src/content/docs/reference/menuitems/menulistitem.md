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

#### Reacting to a list item

List items raise two different events: one when the value changes, and one when the item is pressed.

```cs
MenuListItem weather = new MenuListItem("Weather", new List<string>() { "CLEAR", "RAIN", "THUNDER" }, 0, "Pick a weather type.");
menu.AddMenuItem(weather);

// Fires every time the user presses left or right on the item.
menu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
{
    if (_listItem == weather)
    {
        Debug.WriteLine($"Weather preview: {_listItem.ListItems[_newIndex]}");
    }
};

// Fires when the user presses select on the item.
menu.OnListItemSelect += (_menu, _listItem, _listIndex, _itemIndex) =>
{
    if (_listItem == weather)
    {
        SetWeatherTypeNow(_listItem.GetCurrentSelection());
    }
};
```

:::note
The list wraps around: pressing right on the last value goes back to the first one, and pressing left on the first value goes to the last one.
:::

#### Changing the values later

`ListItems` is a normal `List<string>`, so you can change it at any time. Remember to keep `ListIndex` in range when you do.

```cs
item.ListItems = newValues;
item.ListIndex = 0;
```

:::caution
Never leave a list item with an empty `ListItems` list. If the list is empty, MenuAPI inserts a single `"N/A"` value to keep the menu from freezing, and the item's value will read `N/A`.
:::

----

### Constructors

----

#### MenuListItem(string text, List&lt;string&gt; items, int index)

Creates a list item without a description.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The text displayed on the left side of the item.|
|items|List&lt;string&gt;|The selectable values. At least one value must be provided.|
|index|int|The index that is selected by default. Must be a valid index for `items`.|

----

#### MenuListItem(string text, List&lt;string&gt; items, int index, string description)

Creates a list item with a description.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The text displayed on the left side of the item.|
|items|List&lt;string&gt;|The selectable values. At least one value must be provided.|
|index|int|The index that is selected by default. Must be a valid index for `items`.|
|description|string|The description shown below the menu while this item is selected.|

----

### Properties

:::note
All standard MenuItem class properties are inherited.
The MenuItem properties **RightIcon** and **Label** are not available for MenuListItems.
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

Returns the value at the current `ListIndex`.

##### Parameters

_This function does not have any parameters_.

##### Return value

|Type|Description|
|-|-|
|string|Returns the currently selected ListItem value, or `null` if `ListIndex` is out of range.|

```cs
string selected = item.GetCurrentSelection();

// This is the same as:
string alsoSelected = item.ListItems[item.ListIndex];
```

----

#### Color panel types

:::note
Color and Opacity panels are only available in FiveM.
:::

Color panels are shown below the menu, right under the item's description text (if present).

|Type|Default|Example|
|-|-|-|
|ColorPanelType.Hair|Yes|![Hair color panel](https://vespura.com/hi/i/20-04-18_14-31-06_0BBvH_3287.png)|
|ColorPanelType.Makeup|No|![Makeup color panel](https://vespura.com/hi/i/20-04-18_14-31-45_9mgFr_3288.png)|

The panel always shows the game's 64 hair or makeup colors, and the highlighted swatch follows `ListIndex`. So give the list 64 values, one per color, or the highlight and your list will drift apart.

```cs
// A hair color palette (64 colors).
List<string> colors = new List<string>();
for (int i = 0; i < 64; i++)
{
    colors.Add($"Color #{i}");
}

MenuListItem hairColors = new MenuListItem("Hair Color", colors, 0, "Hair color palette.")
{
    ShowColorPanel = true
};
menu.AddMenuItem(hairColors);

// The same, but with the makeup palette.
MenuListItem makeupColors = new MenuListItem("Makeup Color", colors, 0, "Makeup color palette.")
{
    ShowColorPanel = true,
    ColorPanelColorType = MenuListItem.ColorPanelType.Makeup
};
menu.AddMenuItem(makeupColors);
```

----

#### Opacity panel example

Opacity panels are also shown below the menu, right under the item's description text (if present).

![Opacity panel](https://vespura.com/hi/i/20-04-18_14-34-02_730Ed_3289.png)

The percentage shown in the panel is `ListIndex * 10`, so give the list 11 values to cover 0% - 100%.

```cs
List<string> opacities = new List<string>();
for (int i = 0; i < 11; i++)
{
    opacities.Add($"Opacity {i * 10}%");
}

menu.AddMenuItem(new MenuListItem("Opacity", opacities, 0, "Set an opacity.")
{
    ShowOpacityPanel = true
});
```

:::note
A list item can show a color panel **or** an opacity panel. If both `ShowOpacityPanel` and `ShowColorPanel` are true, only the opacity panel is drawn.
:::
