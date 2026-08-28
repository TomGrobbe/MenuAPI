---
title: "Key bindings"
---

## Key bindings

MenuAPI handles all menu controls for you, and it does that through **FiveM key mappings**. Every menu control,
on the keyboard, on a controller and on the mouse, shows up in the player's own **Settings, Key Bindings**
screen, and they can change it to whatever they like. You do not have to write any code for this, it happens as
soon as your resource loads `MenuAPI.dll`.

A "key mapping" is FiveM's built in system for rebindable keys. Your resource registers a named action with a
default key, and from then on the player is in charge of which key actually triggers it. Their choice is
saved on their own machine, so it sticks between sessions and between servers.

:::caution[Coming from the older MenuAPI?]
In the older (v3, non Enhanced) MenuAPI the key that opened the menu was `MenuController.MenuToggleKey`, a
`Control` value that the resource picked and the player could never change. That property and
`MenuController.MenuToggleKeyIsValid` are **gone**. There is nothing to replace them with, because the key is
now the player's choice rather than yours. If you want to open a menu from your own code, call
[Menu.OpenMenu()](../menu/#openmenu).
:::

----

### What gets registered

Seven actions, each one bound on every device that can trigger it.

|What it does|Keyboard|Controller|Mouse|
|-|-|-|-|
|Opens and closes the menu|<kbd>M</kbd>|Back / View button, held for 400ms||
|Moves up one item|<kbd>↑</kbd>|D-pad up|Scroll wheel up|
|Moves down one item|<kbd>↓</kbd>|D-pad down|Scroll wheel down|
|Moves left on lists and sliders, goes back on anything else|<kbd>←</kbd>|D-pad left||
|Moves right on lists and sliders, selects anything else|<kbd>→</kbd>|D-pad right||
|Selects the highlighted item|<kbd>Enter</kbd>|A / Cross|Left click|
|Goes back one menu, or closes the menu|<kbd>Backspace</kbd> and <kbd>Esc</kbd>|B / Circle|Right click|

FiveM saves one binding per name, so an action that more than one device can trigger needs one binding for
each of them. "Menu up", "Menu up (controller)" and "Menu up (mouse wheel)" are three separate rows in the key
bindings screen, and rebinding one leaves the other two alone. That is what lets a player move their controller
buttons around without touching their keyboard keys.

Back is the one action with two keyboard keys, <kbd>Backspace</kbd> and <kbd>Esc</kbd>, because <kbd>Esc</kbd>
is what most people reach for to get out of a menu. It is listed as "Menu back (alternate)" and can be rebound
or cleared like any other row. While a menu is open <kbd>Esc</kbd> no longer opens the pause menu, <kbd>P</kbd>
still does.

Everything is grouped under your resource's name, so players can tell your bindings apart from any other
resource's.

:::note
In a [paginated](../pagination/) menu, left and right turn the page instead of going back and selecting. Lists
and sliders are unaffected, they still change their own value.
:::

:::note
A resource can only hand FiveM **one** default per binding, which is why every cell in the table above lists a
single key or button. Players are not stuck with one though: in the key bindings screen everything can have a
primary **and** a secondary binding, so anyone who wants two keys for the same action can simply set both.
:::

----

### Changing the default toggle key

The key that opens the menu is the one a server owner is most likely to care about, so your resource can pick
its own default instead of <kbd>M</kbd>:

```cs
public class MyMenus : IScript
{
    public MyMenus()
    {
        MenuController.MenuToggleKeyDefault = "F5";
    }
}
```

Set it in your resource's constructor. MenuAPI registers its key mappings one tick after it starts,
specifically so this works no matter whether your script or MenuAPI's was created first. Setting it later
than that has no effect, because the mapping has already been registered by then.

The value is a [keyboard input mapper parameter id](https://docs.fivem.net/docs/game-references/input-mapper-parameter-ids/keyboard/),
the same sort of string as `"M"`, `"F5"` or `"HOME"`. If you leave it empty or null, <kbd>M</kbd> is used.

Only the keyboard toggle key can be changed this way. Everything else is left alone on purpose, since those
are much less likely to clash with anything and players can rebind them themselves anyway.

:::caution[It is a default, not a setting]
A default only applies to a player who has **never** bound that command. Once someone has a saved binding,
changing the default does nothing for them, because their own choice wins.

Bindings are also saved per command name, and the command name comes from your resource's name, so a player's
binding follows them to **every** server running your resource. There is no way to force a particular key on
a player, and no way to have different keys on different servers.
:::

----

### Controllers

Controller bindings are ordinary key mappings, exactly like the keyboard ones, and they sit in the same
settings screen. The defaults are the buttons the menu has always used, so a player who is happy with those
never has to open that screen at all.

The toggle is the one that behaves a little differently. It is a **hold** rather than a press: keep the
Back / View button down for 400ms and the menu opens, hold it again and the menu closes. That is deliberate,
because a quick tap of that button is the game's own interaction menu, and a menu that opened on a tap would
fight with it. Rebinding the toggle to some other controller button keeps the hold, since the hold belongs to
the action rather than to the button.

Set [EnableMenuToggleKeyOnController](../menucontroller/#properties) to false if you do not want a controller
to be able to open the menu at all.

----

### The mouse

Left click, right click and both scroll wheel directions are bindings too, so they can be rebound or removed
like anything else.

The wheel is a special case. A wheel notch can not be held down, so it moves exactly one item per notch
instead of speeding up the longer you hold it, the way the arrow keys do.

Scrolling the wheel while holding <kbd>TAB</kbd> on foot is left alone, so the weapon wheel keeps working.
That is the one piece of input MenuAPI still reads from the game's own controls, because a binding can only
say which button was used, never why it was used.

----

### Your own keys

A menu can listen for keys of its own. They are registered exactly like the menu controls are, so they show up
in the same settings screen under your resource name and the player can move them wherever they like.

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

The handler only runs while that menu is open. Two menus that pass the same `name` share one key, which is how
you give the same action the same key everywhere. The full reference is on the
[Menu page](../menu/#key-bindings).

:::caution[This replaces button press handlers]
`Menu.ButtonPressHandlers`, `Menu.ButtonPressHandler`, `Menu.ControlPressCheckType` and the
`Menu.InstructionalButtons` dictionary are **gone**, because all four were built around a fixed `Control` that
a player could never change.

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

The one thing with no replacement is `disableControl`. A binding is not one of the game's controls, so MenuAPI
cannot know which control your key happens to sit on, and it has nothing to disable. If your key clashes with
something the game does with the same key, either pick a quieter default or disable that control yourself
while your menu is open, with `DisableControlAction` from your own tick.
:::

----

### Command names

Behind the scenes each binding is a console command, and FiveM command names are shared by every resource on
the server. To make sure two resources that both ship MenuAPI never fight over the same names, the names are
built from your resource's own name, lowercased. The device a binding belongs to is a suffix on the end,
`:pad` for the controller, `:mouse` for the mouse and `:alt` for a second key on the same action:

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

The `+` on most of them is FiveM's way of saying "this one cares about being held down": pressing the button
runs `+command` and letting go runs `-command`. That is how holding an arrow key keeps scrolling, and it is
how the controller toggle knows for how long you held it. The two wheel commands have no `+`, because a wheel
notch is never held.

:::caution
Because the names contain your resource's name, **renaming your resource resets everyone's saved bindings**
back to the defaults. That is a FiveM limitation, not something MenuAPI can work around. Pick a resource name
and stick with it.
:::

----

### Instructional buttons

The select and back hints drawn at the bottom of the screen follow the player's own bindings automatically.
If someone binds select to <kbd>J</kbd>, the hint shows <kbd>J</kbd>. The hints swap to the controller's icons
the moment a controller is used.

The game does not have an icon for every binding, and controller bindings in particular often come back with
nothing to draw. When that happens the hint falls back to the icon of the key or button that binding was
registered on, so a player who has never changed anything always sees the button they are actually pressing,
and only somebody who rebound that one control can end up looking at the old icon. To see what the game hands
back on your own machine, run `menuapi:<your-resource-name>:buttons` in the F8 console while a menu is open,
and it prints every binding, its icon, and the default it falls back to.

You can change the text next to them, or hide them, per menu. See
[Instructional buttons](../menu/#instructional-buttons).
