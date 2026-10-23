---
title: "Global instructional buttons"
---

## Global instructional buttons

Button hints that belong to your resource instead of a [menu](../menu/#instructional-buttons). They show with or without a menu open, until you hide them. Handy for a noclip, a camera mode or a placement tool.

```cs
GlobalInstructionalButtons buttons = new GlobalInstructionalButtons()
    .AddCombination(() => $"Change Speed ({speed}x)", speedDownControl, speedUpControl)
    .Add("Follow Cam", followCamControl);

buttons.Show();
buttons.Hide();
```

Each set is its own row, stacked above the open menu's row. When the menu closes, the rows slide down into its place. Up to **5** sets can be on screen at once, the menu's row does not count. They hide with the pause menu and the HUD, like the game's own.

Text given as a function is read again about 20 times a second, so it updates by itself.

----

### Members

|Member|Description|
|-|-|
|`Add(text, params int[] controls)`|A hint for one or more controls, drawn in the order given. Pass `(int)Control.Jump`, or the control of your own key mapping.|
|`AddCombination(text, params int[] controls)`|The same, with a `+` between the buttons.|
|`Add(text, MenuKeyBinding binding)`|A hint for a [key binding](../keybindings/).|
|`AddRaw(buttons, text)`|A hint from a raw button string, like [CustomInstructionalButtons](../menu/#instructionalbutton) takes.|
|`Clear()`|Removes every hint.|
|`Show()`|Puts the set on screen. `false` when 5 are already showing.|
|`Hide()`|Takes the set off screen.|
|`Refresh()`|Reads every hint again on this frame.|
|`IsShown`|Whether the set is on screen.|
|`HideWithHud`|Whether the set hides while the HUD is hidden. `true` by default.|

`text` can be a `string` or a `Func<string>`. Every `Add` returns the set, so calls can be chained.
