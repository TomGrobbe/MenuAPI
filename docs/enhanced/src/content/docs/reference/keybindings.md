---
title: "Key bindings"
---

## Key bindings

MenuAPI handles all menu controls for you. On a keyboard it does that through **FiveM key mappings**, which
means every menu control shows up in the player's own **Settings, Key Bindings** screen and they can change
it to whatever key they like. You do not have to write any code for this, it happens as soon as your resource
loads `MenuAPI.dll`.

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

Seven bindings, all on the keyboard.

|What it does|Shown in the settings as|Default key|
|-|-|-|
|Opens and closes the menu|Open / close menu|<kbd>M</kbd>|
|Moves up one item|Menu up|<kbd>↑</kbd>|
|Moves down one item|Menu down|<kbd>↓</kbd>|
|Moves left on lists and sliders, goes back on anything else|Menu left|<kbd>←</kbd>|
|Moves right on lists and sliders, selects anything else|Menu right|<kbd>→</kbd>|
|Selects the highlighted item|Menu select|<kbd>Enter</kbd>|
|Goes back one menu, or closes the menu|Menu back|<kbd>Backspace</kbd>|

They are grouped under your resource's name in the key bindings screen, so players can tell them apart from
any other resource's bindings.

:::note
A resource can only hand FiveM **one** default key per binding, which is why the table above lists a single
key for each row. Players are not stuck with one though: in the key bindings screen everything can have a
primary **and** a secondary key, so anyone who wants two keys for the same action can simply set both.
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

Only the toggle key can be changed this way. The navigation keys are left alone on purpose, since they are
much less likely to clash with anything and players can rebind them themselves anyway.

:::caution[It is a default, not a setting]
A default only applies to a player who has **never** bound that command. Once someone has a saved binding,
changing the default does nothing for them, because their own choice wins.

Bindings are also saved per command name, and the command name comes from your resource's name, so a player's
binding follows them to **every** server running your resource. There is no way to force a particular key on
a player, and no way to have different keys on different servers.
:::

----

### The mouse

On top of the bindings above, MenuAPI also reads a few mouse inputs directly. These are fixed, they do not
appear in the key bindings screen and they can not be changed or removed.

|Mouse input|What it does|
|-|-|
|Scroll wheel up|Moves up one item|
|Scroll wheel down|Moves down one item|
|Left click|Selects the highlighted item|
|Right click|Goes back one menu, or closes the menu|

Scrolling the wheel while holding <kbd>TAB</kbd> on foot is left alone, so the weapon wheel keeps working.

----

### Controllers

Controller input is completely separate and is **not** rebindable. It still works the way it always has.

- **Opening the menu**: hold the back/select button (the interaction menu button) for 400ms. Set
  [EnableMenuToggleKeyOnController](../menucontroller/#properties) to false to turn this off.
- **Navigating**: the d-pad and left stick move around, the accept button selects, and the cancel button
  goes back.

The reason it is left alone is that a controller's buttons are already consistent across every game, so
there is far less reason to let people move them around, and the 400ms hold on the toggle button is there
on purpose so a quick tap does not open the menu by accident.

----

### Command names

Behind the scenes each binding is a console command, and FiveM command names are shared by every resource on
the server. To make sure two resources that both ship MenuAPI never fight over the same names, the names are
built from your resource's own name, lowercased:

```
menuapi:<your-resource-name>:toggle
+menuapi:<your-resource-name>:up
+menuapi:<your-resource-name>:down
+menuapi:<your-resource-name>:left
+menuapi:<your-resource-name>:right
+menuapi:<your-resource-name>:select
+menuapi:<your-resource-name>:back
```

The `+` on most of them is FiveM's way of saying "this one cares about being held down": pressing the key
runs `+command` and letting go runs `-command`. That is how holding an arrow key keeps scrolling.

:::caution
Because the names contain your resource's name, **renaming your resource resets everyone's saved bindings**
back to the defaults. That is a FiveM limitation, not something MenuAPI can work around. Pick a resource name
and stick with it.
:::

----

### Instructional buttons

The select and back hints drawn at the bottom of the screen follow the player's own bindings automatically.
If someone binds select to <kbd>J</kbd>, the hint shows <kbd>J</kbd>. On a controller the hints switch to the
controller's glyphs instead, the moment a controller is used.

You can change the text next to them, or hide them, per menu. See
[Instructional buttons](../menu/#instructional-buttons).
