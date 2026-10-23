---
title: "MenuTicks"
---

## MenuTicks

MenuAPI runs its per frame work in named loops, each with a condition. When the condition is false the loop **ends completely**, it does not keep running and return early. With every menu closed, almost nothing runs.

`MenuTicks` is read only. **Only MenuAPI itself can add, start or stop a tick.** It is not a general purpose tick registry for your own code.

----

### Seeing what is running

The console command carries your resource name, so two resources that ship MenuAPI do not clash:

```
menuapi:yourresourcename:ticks
```

With every menu closed:

```
[MenuAPI] 10 ticks registered:
[MenuAPI]   Menu.Scheduler (per frame): running
[MenuAPI]   Menu.Layout (every 500ms): stopped
[MenuAPI]   Menu.Textures (every 250ms): stopped
[MenuAPI]   Menu.Draw (per frame): stopped
[MenuAPI]   Menu.InstructionalButtons (per frame): stopped
[MenuAPI]   Menu.InstructionalButtonsData (every 300ms): stopped
[MenuAPI]   Menu.Select (per frame): stopped
[MenuAPI]   Menu.Navigate (per frame): stopped
[MenuAPI]   Menu.OnscreenKeyboard (per frame): stopped
[MenuAPI]   Menu.Toggle (per frame): running
```

Opening a menu flips the other eight to `running`. Closing every menu stops them again.

----

### The ticks

|Name|Rate|Runs while|
|---|---|---|
|Menu.Scheduler|per frame|Always. Switches the other ticks on or off when a menu opens or closes. Costs one flag check when nothing changed.|
|Menu.Layout|every 500ms|A menu is open. Reads screen size and safe zone.|
|Menu.Textures|every 250ms|A menu is open. Streams game textures the menu needs, like your own banner texture, and releases them once every menu is closed.|
|Menu.Draw|per frame|A menu is open. Sends changes to the [NUI page](../../nui/) and disables conflicting game controls. Hides the page when the last menu closes.|
|Menu.InstructionalButtons|per frame|A menu is open, or [GlobalInstructionalButtons](../globalinstructionalbuttons/) are shown. Hides the button hints when something covers the game, like the pause menu, and shows them again after.|
|Menu.InstructionalButtonsData|every 50ms|A menu is open, or global instructional buttons are shown. Works out which hints and key icons to show, reads the text of the global ones again, and sends it all to the [NUI page](../../nui/) when something changed.|
|Menu.Select|per frame|A menu is open. Handles select and back, including mouse buttons.|
|Menu.Navigate|per frame|A menu is open. Handles up, down, left and right, including hold to repeat.|
|Menu.OnscreenKeyboard|per frame|A menu is open. Stops the menu reacting while the game's text input box is open.|
|Menu.Toggle|per frame|Always. Watches for the menu toggle key on keyboard and controller. Reads a flag set by the key binding, so an idle frame costs no natives.|

`Menu.Scheduler` waits one frame before switching ticks. Opening a submenu closes the parent and opens the child in one go, and the delay stops that from tearing down and rebuilding the button hints and textures.

Pause menu, screen fade, death and player switch are **not** tick conditions. The loops keep running and draw nothing on those frames.

----

### Reading it from code

```cs
using MenuAPI;

// Everything MenuAPI has registered, running or not.
foreach (MenuTickHandle tick in MenuTicks.Handles)
{
    Debug.WriteLine($"{tick.Name} at {tick.Rate} is {(tick.IsRunning ? "running" : "stopped")}");
}

// Or let MenuAPI format the lines for you.
foreach (string line in MenuTicks.Describe())
{
    Debug.WriteLine(line);
}

// Or just dump the lot to the console.
MenuTicks.Dump();
```

On Enhanced, use `API.Log.Info` instead of `Debug.WriteLine`.

To keep a debug overlay up to date, subscribe to `Changed`:

```cs
MenuTicks.Changed += () => _needsRedraw = true;
```

:::caution
`Changed` fires once **per tick that changed**, so opening a menu raises it eight times. Set a flag and act on it once.
:::

----

### MenuTicks

|Member|Type|Description|
|---|---|---|
|Handles|IReadOnlyList&lt;MenuTickHandle&gt;|(Getter only) Every registered tick, running or not.|
|Changed|event Action|Fires whenever a tick starts or stops.|
|Describe()|IEnumerable&lt;string&gt;|One `"name (rate): running"` line per tick.|
|Dump()|void|Writes those lines to the console. The console command calls this.|

### MenuTickHandle

Read only, you cannot create or control one.

|Member|Type|Description|
|---|---|---|
|Name|string|The tick's name, for example `Menu.Draw`.|
|Rate|MenuTickRate|How long it waits between iterations.|
|IsRunning|boolean|Whether its loop is alive right now.|

A tick that throws five times in a row logs an error and stops until a menu next opens or closes.

### MenuTickRate

|Member|Type|Description|
|---|---|---|
|Milliseconds|long|The wait between iterations. `0` means once per frame.|
|ToString()|string|`"per frame"` or something like `"every 100ms"`.|

The wait starts **after** an iteration finishes, so a slow iteration delays the next one instead of overlapping it.
