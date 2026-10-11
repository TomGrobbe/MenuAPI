---
title: "Key bindings"
---

## Key bindings

Every menu control is a **FiveM key mapping**, registered as soon as your resource loads `MenuAPI.dll`. Keyboard,
controller and mouse bindings all show up in the player's **Settings, Key Bindings** screen under your
resource's name, and the player can rebind them. Their choice is saved on their own machine, so it sticks
between sessions and servers.

:::caution[Coming from the older MenuAPI?]
`MenuController.MenuToggleKey` (a fixed `Control`) and `MenuController.MenuToggleKeyIsValid` are **gone**,
because the key is now the player's choice. To open a menu from your own code, call
[Menu.OpenMenu()](../menu/#openmenu) and set [MenuController.EnableMenuToggleKey](../menucontroller/#properties)
to false. That only turns off the keyboard key, set `EnableMenuToggleKeyOnController` to false too to turn off
the controller hold.
:::

----

### What gets registered

Seven actions, each bound once per device that can trigger it.

|What it does|Keyboard|Controller|Mouse|
|-|-|-|-|
|Opens and closes the menu|<kbd>M</kbd>|Back / View button, held for 400ms||
|Moves up one item|<kbd>↑</kbd>|D-pad up|Scroll wheel up|
|Moves down one item|<kbd>↓</kbd>|D-pad down|Scroll wheel down|
|Moves left on lists and sliders, goes back on anything else|<kbd>←</kbd>|D-pad left||
|Moves right on lists and sliders, selects anything else|<kbd>→</kbd>|D-pad right||
|Selects the highlighted item|<kbd>Enter</kbd>|A / Cross|Left click|
|Goes back one menu, or closes the menu|<kbd>Backspace</kbd> and <kbd>Esc</kbd>|B / Circle|Right click|

Every device has its own binding, for example "Menu up", "Menu up (controller)" and "Menu up (mouse wheel)", so
rebinding one leaves the others alone. A resource can only set one default per binding, but players can add a
secondary binding themselves.

<kbd>Esc</kbd> is a second keyboard binding for back, listed as "Menu back (alternate)". While a menu is open
<kbd>Esc</kbd> does not open the pause menu, <kbd>P</kbd> still does.

:::note
In a [paginated](../pagination/) menu, left and right turn the page. List, dynamic list and slider items still
change their own value.
:::

----

### Changing the default toggle key

```cs
public class MyMenus : IScript // BaseScript on Legacy
{
    public MyMenus()
    {
        MenuController.MenuToggleKeyDefault = "F5";
    }
}
```

Set it in your resource's constructor. MenuAPI registers its key mappings one tick after it starts, so this
works whichever script is constructed first. Setting it later has no effect.

The value is a [keyboard input mapper parameter id](https://docs.fivem.net/docs/game-references/input-mapper-parameter-ids/keyboard/),
such as `"M"`, `"F5"` or `"HOME"`. Empty or null falls back to <kbd>M</kbd>. Only the keyboard toggle key has a
configurable default.

:::caution[It is a default, not a setting]
A default only applies to a player who has **never** bound that command, a saved binding always wins.
Bindings are saved per command name, which contains your resource's name, so a player's binding follows them
to **every** server running your resource. You can not force a key on a player or use different keys per server.
:::

----

### Controllers

Controller bindings are ordinary key mappings in the same settings screen. Their defaults are the buttons the
menu has always used.

The toggle is a **hold**: keep Back / View down for 400ms to open the menu, and again to close it. A tap of
that button is the game's own interaction menu. Rebinding the toggle to another button keeps the hold.

Set [EnableMenuToggleKeyOnController](../menucontroller/#properties) to false to turn off the controller toggle.

----

### The mouse

Left click, right click and both wheel directions are bindings too, so they can be rebound or removed. One
wheel notch moves exactly one item, it never speeds up like a held arrow key.

Scrolling the wheel while holding <kbd>TAB</kbd> on foot is ignored, so the weapon wheel keeps working. That is
the one input MenuAPI still reads from the game's own controls.

----

### Your own keys

A menu can register keys of its own. They show up in the same settings screen under your resource name and
the player can rebind them.

```cs
menu.AddKeyBinding(
    "toggleitems",                  // short name, this ends up in the command name
    "Example: toggle every item",   // what the player reads in their key bindings screen
    "K",                            // the default key
    "R1_INDEX",                     // the default controller button, leave it out for none
    MenuKeyPressType.JUST_RELEASED, // when your code runs
    (m, b) => m.GetMenuItems().ForEach(item => item.Enabled = !item.Enabled),
    "Toggle all");                  // set this to also draw an instructional button
```

The handler only runs while that menu is open. Menus that pass the same `name` share one key, and the first
call's description and defaults are the ones registered. Full reference on the [Menu page](../menu/#key-bindings).

:::caution[This replaces button press handlers]
`Menu.ButtonPressHandlers`, `Menu.ButtonPressHandler`, `Menu.ControlPressCheckType` and the
`Menu.InstructionalButtons` dictionary from MenuAPI 3.x are **gone**, because all four were built around a
fixed `Control`.

```cs
// Before
menu.ButtonPressHandlers.Add(new Menu.ButtonPressHandler(
    Control.FrontendSocialClubSecondary,
    Menu.ControlPressCheckType.JUST_RELEASED,
    new Action<Menu, Control>((m, c) => DoThing()),
    true));

menu.InstructionalButtons.Add(Control.FrontendSocialClubSecondary, "Do the thing");

// After
menu.AddKeyBinding("dothing", "Do the thing", "K", "R1_INDEX",
    handler: (m, b) => DoThing(),
    buttonText: "Do the thing");
```

`disableControl` has no replacement. MenuAPI can not know which game control your key sits on, so if it clashes
with something the game does, pick another default or call `DisableControlAction` yourself while your menu is open.
:::

----

### Command names

Each binding is a console command. The names contain your resource's name in lowercase, so two resources that
both ship MenuAPI never clash. The suffix is the device: `:pad` for controller, `:mouse` for mouse and `:alt`
for a second keyboard key.

```
menuapi:<your-resource-name>:toggle
+menuapi:<your-resource-name>:toggle:pad

+menuapi:<your-resource-name>:up
+menuapi:<your-resource-name>:up:pad
menuapi:<your-resource-name>:up:mouse

+menuapi:<your-resource-name>:down
+menuapi:<your-resource-name>:down:pad
menuapi:<your-resource-name>:down:mouse

+menuapi:<your-resource-name>:left
+menuapi:<your-resource-name>:left:pad

+menuapi:<your-resource-name>:right
+menuapi:<your-resource-name>:right:pad

+menuapi:<your-resource-name>:select
+menuapi:<your-resource-name>:select:pad
+menuapi:<your-resource-name>:select:mouse

+menuapi:<your-resource-name>:back
+menuapi:<your-resource-name>:back:pad
+menuapi:<your-resource-name>:back:mouse
+menuapi:<your-resource-name>:back:alt
```

The keyboard `toggle` command only exists while `EnableMenuToggleKey` is true. Keys from `AddKeyBinding()` follow
the same pattern, `+menuapi:<your-resource-name>:<name>` plus `:pad` when they have a controller button.

A `+` marks a hold command: FiveM runs `+command` on press and `-command` on release. The wheel commands have no
`+` because a notch is never held.

:::caution
**Renaming your resource resets everyone's saved bindings** to the defaults, because the command names contain it.
:::

----

### Instructional buttons

The select and back hints follow the player's own bindings, and switch to controller icons as soon as a
controller is used.

When the game has no icon for a binding, which is common for controller bindings, the hint shows the icon of
the key or button that binding was registered with. Run `menuapi:<your-resource-name>:buttons` in the F8
console while a menu is open to print every binding, its icon and the default it falls back to.

The text next to the hints, and whether they show, is set per menu. See
[Instructional buttons](../menu/#instructional-buttons).
