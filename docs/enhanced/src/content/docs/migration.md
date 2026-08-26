---
title: "Migration guide"
---

## Migration guide

Moving a resource from the older (v3, non Enhanced) MenuAPI to the Enhanced one. This page is just
the things that stop compiling or stop behaving the way they used to, with a before and after for
each. The [Changelog](changelog/) explains why each of them changed.

Everything not listed here still works the way it always did.

----

### The menu toggle key is the player's choice now

`MenuController.MenuToggleKey` and `MenuController.MenuToggleKeyIsValid` are gone. You can still pick
the key players start with, but they can rebind it in **Settings, Key Bindings**.

```cs
// Before
MenuController.MenuToggleKey = Control.SelectCharacterMichael;

// After (a key name, and only for players who have never rebound it)
MenuController.MenuToggleKeyDefault = "M";
```

----

### Select and back instructional buttons moved

`Menu.InstructionalButtons` used to come with a select and a back entry already in it. It now starts
empty, and those two live on the menu itself so they can follow the player's own key binding.

```cs
// Before
menu.InstructionalButtons[Control.FrontendAccept] = "Choose";
menu.InstructionalButtons.Remove(Control.FrontendCancel);

// After
menu.SelectButtonText = "Choose";
menu.ShowBackInstructionalButton = false;
```

Your own extra buttons still go in `Menu.InstructionalButtons` exactly as before.

----

### List item values are a MenuItemList

`MenuListItem.ListItems` used to be a plain `List<string>` that you handed over and kept a reference
to. It is now a [MenuItemList](reference/menuitems/menulistitem/#changing-the-values-later), which
works the same way but tells the menu when it changes, so the row redraws the moment you add or
remove a value.

You still build one from a `List<string>`, and the list you pass in is **copied**, so changing your
own copy afterwards no longer reaches the item. Change it through the item instead.

```cs
// Before
List<string> values = new List<string> { "A", "B" };
MenuListItem item = new MenuListItem("Item", values, 0);

values.Add("C");             // the item picked this up
List<string> current = item.ListItems;

// After
MenuListItem item = new MenuListItem("Item", new List<string> { "A", "B" }, 0);

item.ListItems.Add("C");     // change it through the item
MenuItemList current = item.ListItems;
```

Everything you would call on a `List<string>` is there and works the same: `Add`, `AddRange`,
`Insert`, `InsertRange`, `Remove`, `RemoveAt`, `RemoveAll`, `RemoveRange`, `Clear`, `Sort`,
`Reverse`, `Contains`, `IndexOf`, `Find`, `FindAll`, `FindIndex`, `Exists`, `TrueForAll`,
`ConvertAll`, `GetRange`, `ForEach`, `ToArray`, `Count`, indexing, `foreach` and all of LINQ. If you
genuinely need a `List<string>` back, call `item.ListItems.ToList()`.

----

### Manual garbage collection is gone

`MenuController.EnableManualGCs` no longer exists. Delete the line, .NET handles this itself.

```cs
// Before
MenuController.EnableManualGCs = false;

// After
// (nothing)
```

----

### Nullable reference types

MenuAPI is built with nullable reference types on, so things that can be null now say so:
`MenuController.GetCurrentMenu()`, `MenuController.MainMenu`, `Menu.ParentMenu`,
`Menu.GetCurrentMenuItem()`, `Menu.MenuTitle`, `Menu.MenuSubtitle`, `Menu.CounterPreText`,
`MenuItem.Label`, `MenuItem.Description` and `MenuDynamicListItem.CurrentItem`.

Nothing breaks. If your resource has nullable switched on too you may get new warnings, and each one
is pointing at a crash that could already happen today, so they are worth fixing rather than
silencing.
