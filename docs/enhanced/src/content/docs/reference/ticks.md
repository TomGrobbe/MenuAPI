---
title: "MenuTicks"
---

## MenuTicks

A "tick" is just a loop that runs over and over while your resource is alive. Most of what a menu does is a tick: drawing itself, watching for a key press, reacting to the arrow keys. FiveM runs these once per frame, so around 60 times a second.

That is fine when a menu is on screen. It is a waste when nothing is open, and for most players nothing is open almost all of the time.

So MenuAPI keeps its loops in a small scheduler. Every loop has a name and a condition, and when the condition is not met the loop **stops completely**. It does not keep running and returning early, it ends. That means a closed menu really does cost nothing.

`MenuTicks` is the window into that scheduler. You can look, but you cannot touch: **only MenuAPI itself can add, start or stop a loop here.** This is not a general purpose tick registry for your own code to hook into, it exists so MenuAPI can manage its own work and so you can see what it is doing.

----

### Seeing what is running

The quickest way is the console command. It is named after your resource, because two resources that both ship MenuAPI would otherwise fight over the same command name:

```
menuapi:yourresourcename:ticks
```

With every menu closed you should see something like this:

```
[MenuAPI] 8 ticks registered:
[MenuAPI]   Menu.Scheduler (per frame): running
[MenuAPI]   Menu.Draw (per frame): stopped
[MenuAPI]   Menu.InstructionalButtons (per frame): stopped
[MenuAPI]   Menu.Select (per frame): stopped
[MenuAPI]   Menu.Navigate (per frame): stopped
[MenuAPI]   Menu.OnscreenKeyboard (per frame): stopped
[MenuAPI]   Menu.Toggle (per frame): running
[MenuAPI]   Menu.ToggleController (every 100ms): running
```

Only the ones that watch for the menu being opened are running, plus the scheduler's own bookkeeping. Open a menu and those five flip to `running`, while `Menu.ToggleController` stops, because there is no point watching for "open the menu" when a menu is already open.

----

### The ticks

|Name|Rate|Runs while|
|---|---|---|
|Menu.Scheduler|per frame|Always. Notices that a menu opened or closed and switches the rest on or off to match. Its body is a single check on a flag, so it costs nothing when nothing changed.|
|Menu.Draw|per frame|A menu is open. Draws it, and disables the game controls that would otherwise fight with the menu.|
|Menu.InstructionalButtons|per frame|A menu is open. Draws the button hints along the bottom of the screen.|
|Menu.Select|per frame|A menu is open. Handles select and back, including the mouse buttons.|
|Menu.Navigate|per frame|A menu is open. Handles up, down, left and right, including holding a key to repeat.|
|Menu.OnscreenKeyboard|per frame|A menu is open. Watches for the game's text input box so the menu does not react to you typing.|
|Menu.Toggle|per frame|Always. Watches for the menu toggle key. It reads a flag that the key binding sets rather than asking the game anything, so an idle frame costs nothing.|
|Menu.ToggleController|every 100ms|No menu is open, and the controller toggle is enabled. This one has to ask the game whether a button is held, which is why it is the only one that is deliberately slowed down. The gesture is a 400ms hold, so ten checks a second is plenty.|

`Menu.Scheduler` waits a frame on purpose. Opening a submenu closes the parent and opens the child in one go, so reacting to each half on its own would briefly see "no menu is open" and throw away the button hints and menu textures, only to rebuild them an instant later. Waiting one frame lets a close and an open that belong together cancel each other out.

Some things are deliberately **not** conditions here: whether the game is paused, whether the screen has faded out, whether the player is dead or switching. Those change from one frame to the next with nothing to announce them, so the loops keep running and simply draw nothing on the frames where they should not. Making them conditions would mean re-checking everything every frame, which is exactly the cost this whole design removes.

----

### Reading it from code

Useful if your resource has its own debug overlay and you want MenuAPI's loops in it.

```cs
using MenuAPI;

// Everything MenuAPI has registered, running or not.
foreach (MenuTickHandle tick in MenuTicks.Handles)
{
    API.Log.Info($"{tick.Name} at {tick.Rate} is {(tick.IsRunning ? "running" : "stopped")}");
}

// Or let MenuAPI format the lines for you.
foreach (string line in MenuTicks.Describe())
{
    API.Log.Info(line);
}

// Or just dump the lot to the console.
MenuTicks.Dump();
```

To keep something on screen up to date, subscribe to `Changed`:

```cs
MenuTicks.Changed += () => _needsRedraw = true;
```

:::caution
Do not do real work straight from `Changed`. Opening or closing a menu flips several ticks at once, and the event fires once **per tick that changed**, so you would redraw five times for one menu opening. Set a flag like the example above and act on it once.
:::

----

### MenuTicks

|Member|Type|Description|
|---|---|---|
|Handles|IReadOnlyList&lt;MenuTickHandle&gt;|(Getter only) Every tick MenuAPI has registered, running or not.|
|Changed|event Action|Fires whenever a tick starts or stops. See the warning above about coalescing.|
|Describe()|IEnumerable&lt;string&gt;|One `"name (rate): running"` line per tick.|
|Dump()|void|Writes every one of those lines to the console. This is what the console command calls.|

### MenuTickHandle

One registered tick. You can only ever read one of these, never create or control one.

|Member|Type|Description|
|---|---|---|
|Name|string|The tick's name, for example `Menu.Draw`.|
|Rate|MenuTickRate|How long it waits between iterations.|
|IsRunning|boolean|Whether its loop is alive right now.|

### MenuTickRate

|Member|Type|Description|
|---|---|---|
|Milliseconds|long|The wait between iterations. `0` means once per frame.|
|ToString()|string|Either `"per frame"` or something like `"every 100ms"`.|

The wait happens **after** an iteration finishes rather than on a fixed timer, so a slow iteration delays the next one instead of overlapping with it.
