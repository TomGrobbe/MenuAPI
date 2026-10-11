---
title: "MenuCheckboxItem"
---

## MenuCheckboxItem

A menu item with 'checkbox' functionality and a forced checkbox icon on the right.

----

### Example usage

```cs
// checkedValue: when this is set to true, the checkbox will be 'checked' by default.
bool checkedValue = false;
MenuCheckboxItem item = new MenuCheckboxItem("Checkbox Text", "Checkbox description.", checkedValue);

// Set the style:
item.Style = CheckboxStyle.Cross;

// Add a menu item to a menu:
menu.AddMenuItem(item);
```

#### Reacting to a checkbox

Toggling a checkbox raises the [OnCheckboxChange](../../events/#oncheckboxchange) event on its parent menu. `Checked` has already been updated by the time your handler runs, and the `newCheckedState` parameter holds the same value.

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
A checkbox can be toggled with the select control **and** with the right control, so do not assume [OnItemSelect](../../events/#onitemselect) will fire. It never fires for checkbox items.
:::

#### A locked checkbox

Disabled checkboxes are greyed out and can not be toggled, which is useful for showing a state the user is not allowed to change.

```cs
menu.AddMenuItem(new MenuCheckboxItem("Premium feature", "Only available to donators.", false)
{
    Enabled = false,
    LeftIcon = MenuItem.Icon.LOCK
});
```

----

### Constructors

----

#### MenuCheckboxItem(string text)

Creates an unchecked checkbox without a description.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The text displayed on the left side of the item.|

----

#### MenuCheckboxItem(string text, bool _checked)

Creates a checkbox without a description, with the given checked state.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The text displayed on the left side of the item.|
|_checked|boolean|The initial checked state.|

----

#### MenuCheckboxItem(string text, string description)

Creates an unchecked checkbox with a description.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The text displayed on the left side of the item.|
|description|string|The description shown below the menu while this item is selected.|

----

#### MenuCheckboxItem(string text, string description, bool _checked)

Creates a checkbox with a description and the given checked state.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The text displayed on the left side of the item.|
|description|string|The description shown below the menu while this item is selected.|
|_checked|boolean|The initial checked state.|

----

### Properties

:::note
All standard MenuItem class properties are inherited.
The MenuItem properties **RightIcon** and **Label** are not available for MenuCheckboxItems.
:::

|Property|Type|Default value|Description|Optional|
|---|---|---|---|---|
|Checked|boolean|false|The checked state for the checkbox.|**No**|
|Style|[CheckboxStyle](#checkbox-styles)|`CheckboxStyle.Tick`|The style of this checkbox item when it is "checked".|**No**|

----

### Methods

_There are no methods available for MenuCheckboxItems._

----

### Checkbox Styles

Available checkbox syltes:

|Style|Available in which game?|Default style|Example|
|-|-|-|:-:|
|CheckboxStyle.Tick|FiveM|Yes|![Style](https://vespura.com/hi/i/20-04-18_13-31-21_ri30C_3284.png)|
|CheckboxStyle.Cross|FiveM|No|![Style](https://vespura.com/hi/i/20-04-18_13-31-56_yoG5Z_3285.png)|
