---
title: "MenuSliderItem"
---

## MenuSliderItem

A menu item with a slider on the right.

----

### Example usage

```cs
// Creating 3 sliders, showing off the 3 possible variations and custom colors.
MenuSliderItem slider = new MenuSliderItem("Slider", 0, 10, 5, false);

MenuSliderItem slider2 = new MenuSliderItem("Slider + Bar", 0, 10, 5, true)
{
    BarColor = System.Drawing.Color.FromArgb(255, 73, 233, 111),
    BackgroundColor = System.Drawing.Color.FromArgb(255, 25, 100, 43)
};

MenuSliderItem slider3 = new MenuSliderItem("Slider + Bar + Icons", "The icons are currently male/female because that's probably the most common use. But any icon can be used!", 0, 10, 5, true)
{
    BarColor = System.Drawing.Color.FromArgb(255, 255, 0, 0),
    BackgroundColor = System.Drawing.Color.FromArgb(255, 100, 0, 0),

    SliderLeftIcon = MenuItem.Icon.MALE,
    SliderRightIcon = MenuItem.Icon.FEMALE
};

// adding the sliders to the menu.
menu.AddMenuItem(slider);
menu.AddMenuItem(slider2);
menu.AddMenuItem(slider3);
```

#### Example preview

![Example](https://vespura.com/hi/i/20-04-18_15-32-46_sjZJl_3290.png)

#### Reacting to a slider

```cs
MenuSliderItem volume = new MenuSliderItem("Volume", 0, 10, 5, true);
menu.AddMenuItem(volume);

// Fires every time the slider moves.
menu.OnSliderPositionChange += (_menu, _sliderItem, _oldPosition, _newPosition, _itemIndex) =>
{
    if (_sliderItem == volume)
    {
        SetVolume(_newPosition / 10f);
    }
};

// Fires when the user presses select on the slider.
menu.OnSliderItemSelect += (_menu, _sliderItem, _sliderPosition, _itemIndex) =>
{
    Debug.WriteLine($"Slider pressed at position {_sliderPosition}.");
};
```

:::note
Sliders do **not** wrap around. Left at `Min` or right at `Max` plays the error sound and does not raise [OnSliderPositionChange](../../events/#onsliderpositionchange).
:::

----

### Constructors

|Constructor|Description|
|-|-|
|`MenuSliderItem(string name, int min, int max, int startPosition)`|No description, no divider.|
|`MenuSliderItem(string name, int min, int max, int startPosition, bool showDivider)`|No description.|
|`MenuSliderItem(string name, string description, int min, int max, int startPosition)`|No divider.|
|`MenuSliderItem(string name, string description, int min, int max, int startPosition, bool showDivider)`|All options set.|

|Parameter|Type|Description|
|-|-|-|
|name|string|The text on the left side of the item.|
|description|string|Shown below the menu while the item is highlighted.|
|min|int|The lowest position. Must be lower than `max`.|
|max|int|The highest position.|
|startPosition|int|The starting position, between `min` and `max` inclusive.|
|showDivider|bool|Draws a white divider line in the middle of the slider. Defaults to false.|

:::note
`Min` and `Max` can only be set through a constructor. `Position` can be changed at any time. A `Position` outside `Min` to `Max` is reset to `(Max - Min) / 2` when the row is drawn.
:::

----

### Properties

:::note
**LeftIcon**, **RightIcon** and **Label** are not available for MenuSliderItems. Use `SliderLeftIcon` and `SliderRightIcon` instead.
:::

|Property|Type|Default value|Description|
|---|---|---|---|
|Min|int|`min` argument|Read only. The lowest position.|
|Max|int|`max` argument|Read only. The highest position.|
|Position|int|`startPosition` argument|The current position.|
|ShowDivider|bool|false|Draws a white divider line in the middle of the slider.|
|SliderLeftIcon|[Icon](../menuitem/#icons)|Icon.NONE|Icon on the left side of the slider.|
|SliderRightIcon|[Icon](../menuitem/#icons)|Icon.NONE|Icon on the right side of the slider.|
|BackgroundColor|[System.Drawing.Color](https://docs.microsoft.com/en-us/dotnet/api/system.drawing.color?view=netframework-4.8)|System.Drawing.Color.FromArgb(255,&nbsp;24,&nbsp;93,&nbsp;151)|The slider background color.|
|BarColor|[System.Drawing.Color](https://docs.microsoft.com/en-us/dotnet/api/system.drawing.color?view=netframework-4.8)|System.Drawing.Color.FromArgb(255,&nbsp;53,&nbsp;165,&nbsp;223)|The slider bar color.|

----

### Methods

MenuSliderItem has no methods of its own.
