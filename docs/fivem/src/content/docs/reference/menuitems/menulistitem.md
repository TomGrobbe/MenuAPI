---
title: "MenuListItem"
---

## MenuListItem

A menu item that picks one string out of a list with left and right. Pressing select raises a separate event.

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

List items raise one event when the value changes and another when the item is pressed. The list wraps around at both ends.

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

#### Changing the values later

`ListItems` is a `MenuItemList`, used like a `List<string>`. It tells the menu when it changes, so the row redraws straight away. An out of range `ListIndex` is wrapped back into range the next time the row is drawn.

```cs
item.ListItems.Add("Option 4");
item.ListItems.RemoveAt(0);

// Or replace the lot. A List<string> can be assigned straight to it.
item.ListItems = newValues;
item.ListIndex = 0;
```

:::caution
The constructor **copies** the list you pass, so later changes to your own list do not reach the item. Change `item.ListItems` instead. See the [Migration Guide (v4)](../../../migration/#list-item-values-are-a-menuitemlist) when moving from an older MenuAPI.
:::

:::caution
Never leave `ListItems` empty. MenuAPI then inserts a single `"N/A"` value to keep the game from freezing, and the item reads `N/A`.
:::

----

### Constructors

|Constructor|Description|
|-|-|
|`MenuListItem(string text, List<string> items, int index)`|Creates a list item without a description.|
|`MenuListItem(string text, List<string> items, int index, string description)`|Creates a list item with a description.|

|Parameter|Type|Description|
|-|-|-|
|text|string|The text on the left side of the item.|
|items|List&lt;string&gt;|The selectable values. Provide at least one. The list is copied.|
|index|int|The index selected at the start. Must be valid for `items`.|
|description|string|Shown below the menu while the item is highlighted.|

----

### Properties

:::note
**RightIcon** and **Label** are not available for MenuListItems.
:::

|Property|Type|Default value|Description|
|---|---|---|---|
|ListIndex|int|`index` argument|The selected index.|
|ListItems|MenuItemList|copy of `items`|The selectable values. Keep at least one. Used like a `List<string>`, and a `List<string>` can be assigned to it.|
|HideArrowsWhenNotSelected|bool|false|Hides the left and right arrows while the item is not highlighted.|
|ShowColorPanel|bool|false|Shows the [color panel](#color-panel-types).|
|ShowOpacityPanel|bool|false|Shows the [opacity panel](#opacity-panel-example).|
|ColorPanelColorType|[ColorPanelType](#color-panel-types)|ColorPanelType.Hair|Which palette the color panel shows.|
|OpacityPercent|int|0|The opacity shown when both panels are on. Clamped to 0 to 100.|
|ItemsCount|int|0|Read only. The number of values in `ListItems`.|

----

### Methods

----

#### GetCurrentSelection()

Returns the value at `ListIndex` as a `string`, or `null` if `ListIndex` is out of range.

```cs
string selected = item.GetCurrentSelection();

// This is the same as:
string alsoSelected = item.ListItems[item.ListIndex];
```

----

#### Color panel types

The color panel is drawn below the menu, under the description. The enum is `MenuListItem.ColorPanelType`.

|Type|Default|Example|
|-|-|-|
|ColorPanelType.Hair|Yes|![Hair color panel](https://vespura.com/hi/i/20-04-18_14-31-06_0BBvH_3287.png)|
|ColorPanelType.Makeup|No|![Makeup color panel](https://vespura.com/hi/i/20-04-18_14-31-45_9mgFr_3288.png)|

The panel always shows the game's 64 hair or makeup colors and highlights the swatch at `ListIndex`. Give the list 64 values, one per color, or the highlight and your list drift apart.

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

The opacity panel is also drawn below the menu, under the description.

![Opacity panel](https://vespura.com/hi/i/20-04-18_14-34-02_730Ed_3289.png)

The percentage shown is `ListIndex * 10`, so 11 values cover 0% to 100%.

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
With both `ShowColorPanel` and `ShowOpacityPanel` on, one panel shows the palette with an opacity bar. `ListIndex` then picks the color, and the opacity comes from `OpacityPercent` instead of `ListIndex * 10`.
:::
