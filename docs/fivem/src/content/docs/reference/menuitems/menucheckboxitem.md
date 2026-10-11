---
title: "MenuCheckboxItem"
---

## MenuCheckboxItem

A menu item with a checkbox on the right.

----

### Example usage

```cs
// checkedValue: when this is set to true, the checkbox will be 'checked' by default.
bool checkedValue = false;
MenuCheckboxItem item = new MenuCheckboxItem("Checkbox Text", "Checkbox description.", checkedValue);

// Set the style:
item.Style = MenuCheckboxItem.CheckboxStyle.Cross;

// Add a menu item to a menu:
menu.AddMenuItem(item);
```

#### Reacting to a checkbox

Toggling raises [OnCheckboxChange](../../events/#oncheckboxchange) on the parent menu. `Checked` is already updated when your handler runs, and `newCheckedState` holds the same value.

```cs
MenuCheckboxItem godMode = new MenuCheckboxItem("God mode", "Makes you invincible.", false);
menu.AddMenuItem(godMode);

menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
{
    if (_item == godMode)
    {
        SetPlayerInvincible(Game.Player.Handle, _checked);
    }
};
```

:::note
Both select and right toggle a checkbox. [OnItemSelect](../../events/#onitemselect) never fires for checkbox items.
:::

#### A locked checkbox

A disabled checkbox is greyed out and cannot be toggled.

```cs
menu.AddMenuItem(new MenuCheckboxItem("Premium feature", "Only available to donators.", false)
{
    Enabled = false,
    LeftIcon = MenuItem.Icon.LOCK
});
```

----

### Constructors

|Constructor|Description|
|-|-|
|`MenuCheckboxItem(string text)`|Unchecked, no description.|
|`MenuCheckboxItem(string text, bool _checked)`|No description, with the given checked state.|
|`MenuCheckboxItem(string text, string description)`|Unchecked, with a description.|
|`MenuCheckboxItem(string text, string description, bool _checked)`|With a description and the given checked state.|

`text` is drawn on the left side of the row. `description` is shown below the menu while the item is highlighted.

----

### Properties

:::note
**RightIcon** and **Label** are not available for MenuCheckboxItems.
:::

|Property|Type|Default value|Description|
|---|---|---|---|
|Checked|bool|false|The checked state.|
|Style|[CheckboxStyle](#checkbox-styles)|`CheckboxStyle.Tick`|What the checkbox shows when checked.|

----

### Methods

MenuCheckboxItem has no methods of its own.

----

### Checkbox Styles

The enum is `MenuCheckboxItem.CheckboxStyle`.

|Style|Default|Example|
|-|-|:-:|
|CheckboxStyle.Tick|Yes|![Style](https://vespura.com/hi/i/20-04-18_13-31-21_ri30C_3284.png)|
|CheckboxStyle.Cross|No|![Style](https://vespura.com/hi/i/20-04-18_13-31-56_yoG5Z_3285.png)|
