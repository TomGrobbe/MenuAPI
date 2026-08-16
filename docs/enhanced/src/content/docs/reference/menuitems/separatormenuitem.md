---
title: "SeparatorMenuItem"
---

## SeparatorMenuItem

A heading that labels the rows underneath it. The text is drawn centred, and pressing it does nothing at all.

Use this to break a long menu into readable groups without pushing anything into a submenu.

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

With `ShowArrows` left on you get `↓ Protection ↓`, and with it off you get just `Protection`. Both are centred in the row.

----

### How it behaves

**The cursor scrolls onto it, like any other row.** It highlights, and its description is shown if you gave it one. Scrolling is deliberately not changed: a cursor that jumped over rows would feel like the menu was eating key presses.

**Nothing happens when you press it.** Select does nothing, and so do left and right. There is no error sound either, because scrolling onto a label is not a mistake worth being told off for.

**It counts as a row.** A menu of 18 rows and 3 headings reads `1 / 21` in the counter, and the headings take up 3 of the rows on screen, because that is what they are.

:::tip
Because the cursor can rest on a separator, a description is worth giving it. It is a free place to explain what the whole group below is for:

```cs
menu.AddMenuItem(new SeparatorMenuItem("Protection", "Things that stop you taking damage."));
```
:::

----

### Styling

A separator is drawn in the same font, at the same size and in the same white as a normal row, so it lines up exactly with the rows around it. When the cursor is on it, it goes dark on the white highlight bar, the same as any other selected row.

If you want it to stand out more, put the game's own formatting tokens in the text, the same way you would on any other item:

```cs
// Bold, which is what vMenu's old hand-built spacers looked like.
menu.AddMenuItem(new SeparatorMenuItem("~h~Protection"));

// Grey.
menu.AddMenuItem(new SeparatorMenuItem("~c~Protection"));

// The blue the menu subtitle uses.
menu.AddMenuItem(new SeparatorMenuItem("~HUD_COLOUR_FREEMODE~Protection"));
```

:::caution
Keep separator text short. Like a normal item's text it is not split across lines for you, so a long heading wraps inside the row and spills over the one below it.
:::

----

### Constructors

#### SeparatorMenuItem(string text)

Creates a separator with the down arrows around its text.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The heading, drawn centred in the row.|

----

#### SeparatorMenuItem(string text, bool showArrows)

Creates a separator and says whether to draw the down arrows.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The heading, drawn centred in the row.|
|showArrows|boolean|Whether to draw the text as `↓ text ↓`.|

----

#### SeparatorMenuItem(string text, string description)

Creates a separator with a description, shown below the menu while the cursor is on it.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The heading, drawn centred in the row.|
|description|string|The description shown below the menu while this row is selected.|

----

#### SeparatorMenuItem(string text, string description, bool showArrows)

Creates a separator with all options set.

##### Parameters

|Parameter|Type|Description|
|-|-|-|
|text|string|The heading, drawn centred in the row.|
|description|string|The description shown below the menu while this row is selected.|
|showArrows|boolean|Whether to draw the text as `↓ text ↓`.|

----

### Properties

:::note
All standard MenuItem class properties are inherited. **LeftIcon**, **RightIcon** and **Label** are never drawn, because a separator draws only its own centred text. **Enabled** is set to `false`, which is what stops left and right doing anything, so setting it back to `true` will let those through again while select still does nothing.
:::

|Property|Type|Default value|Description|Optional|
|---|---|---|---|---|
|ShowArrows|boolean|true|Whether to draw the text as `↓ text ↓` rather than on its own.|Yes|

----

### Methods

_There are no methods available for SeparatorMenuItems._
