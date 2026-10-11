---
title: "SeparatorMenuItem"
---

## SeparatorMenuItem

A centred heading that labels the rows under it. Pressing it does nothing.

----

### Example usage

```cs
Menu menu = new Menu("Player Options", "Player Options");
MenuController.AddMenu(menu);

menu.AddMenuItem(new SeparatorMenuItem("Protection"));
menu.AddMenuItem(new MenuCheckboxItem("God Mode", "Nothing can hurt you.", false));
menu.AddMenuItem(new MenuCheckboxItem("Invisible", "Nobody can see you.", false));

// The same heading without the arrows around it.
menu.AddMenuItem(new SeparatorMenuItem("Movement", false));
menu.AddMenuItem(new MenuCheckboxItem("Super Jump", "Jump much higher.", false));
menu.AddMenuItem(new MenuCheckboxItem("Fast Run", "Sprint faster.", false));
```

With `ShowArrows` on (the default) the row reads `↓ Protection ↓`, with it off just `Protection`.

----

### How it behaves

- The cursor stops on it like on any other row. It highlights and shows its description, if it has one.
- Select, left and right do nothing, and no error sound plays.
- In a [paginated](../../pagination/) menu, left and right on a separator still turn the page.
- It counts as a row. A menu of 18 items and 3 separators reads `1 / 21`, and the separators take up 3 rows on screen.

:::tip
Since the cursor can rest on a separator, a description is a good place to explain the group below it:

```cs
menu.AddMenuItem(new SeparatorMenuItem("Protection", "Things that stop you taking damage."));
```
:::

----

### Styling

A separator uses the same font, size and white as a normal row, and goes dark on the highlight bar when selected. Use formatting codes to make it stand out:

```cs
// Bold, which is what vMenu's old hand-built spacers looked like.
menu.AddMenuItem(new SeparatorMenuItem("~h~Protection"));

// Grey.
menu.AddMenuItem(new SeparatorMenuItem("~c~Protection"));

// The blue the menu subtitle uses.
menu.AddMenuItem(new SeparatorMenuItem("~HUD_COLOUR_FREEMODE~Protection"));
```

:::caution
Keep separator text short. It is not split across lines for you, so a long heading spills over the row below.
:::

----

### Constructors

|Constructor|Description|
|-|-|
|`SeparatorMenuItem(string text)`|With the down arrows around the text.|
|`SeparatorMenuItem(string text, bool showArrows)`|Chooses whether to draw the arrows.|
|`SeparatorMenuItem(string text, string description)`|With arrows and a description.|
|`SeparatorMenuItem(string text, string description, bool showArrows)`|All options set.|

|Parameter|Type|Description|
|-|-|-|
|text|string|The heading, drawn centred in the row.|
|description|string|Shown below the menu while the cursor is on the separator.|
|showArrows|bool|Whether to draw the text as `↓ text ↓`. Defaults to true.|

----

### Properties

:::note
**LeftIcon**, **RightIcon** and **Label** are never drawn. **Enabled** is set to `false`. Select, left and right do nothing whatever its value.
:::

|Property|Type|Default value|Description|
|---|---|---|---|
|ShowArrows|bool|true|Whether to draw the text as `↓ text ↓`.|

----

### Methods

SeparatorMenuItem has no methods of its own.
