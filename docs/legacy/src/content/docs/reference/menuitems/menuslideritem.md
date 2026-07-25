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

----

### Properties

:::note
All standard MenuItem class properties are inherited.
The MenuItem properties **LeftIcon**, **RightIcon** and **Label** are not available for MenuSliderItems.
MenuSliderItems are only available in FiveM.
:::

|Property|Type|Default value|Description|Optional|
|---|---|---|---|---|
|Min|int|0|The minimum slider range, value must be an integer equal to or higher than 0, but less than the Max parameter.|**No**|
|Max|int|10|The maximum slider range, value must be an integer higher than the min value.|**No**|
|ShowDivider|boolean|false|Whether or not to show the white divider line in the middle of the slider.|Yes|
|Position|int|0|The default slider position, value must be an integer between the Min and Max value (inclusive).|**No**|
|SliderLeftIcon|[Icon](../menuitem/#icons)|Icon.NONE|An icon to show on the left side of the slider.|Yes|
|SliderRightIcon|[Icon](../menuitem/#icons)|Icon.NONE|An icon to show on the right side of the slider.|Yes|
|BackgroundColor|[System.Drawing.Color](https://docs.microsoft.com/en-us/dotnet/api/system.drawing.color?view=netframework-4.8)|System.Drawing.Color.FromArgb(255,&nbsp;24,&nbsp;93,&nbsp;151)|The slider background color.|Yes|
|BarColor|[System.Drawing.Color](https://docs.microsoft.com/en-us/dotnet/api/system.drawing.color?view=netframework-4.8)|System.Drawing.Color.FromArgb(255,&nbsp;53,&nbsp;165,&nbsp;223)|The slider bar color.|Yes|

----

### Methods

_There are no methods available for MenuSliderItems._
