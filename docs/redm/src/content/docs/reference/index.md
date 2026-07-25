---
title: "API Reference"
---

## API Reference

A full reference of every public class, property, method and event in MenuAPI.

If you have not set up MenuAPI yet, start at [Setup](../setup/).

:::note
This is the RedM reference. RedM supports a smaller feature set than FiveM: there are no slider items, no color/opacity panels, no weapon or vehicle stats panels, no right aligned menus, and far fewer icons. Each page lists its own FiveM only members.
:::

----

### The classes

|Class|What it is|
|-|-|
|[Menu](menu/)|A single menu: a banner, a subtitle, a list of items and a description box. This is also where all [events](events/) live.|
|[MenuController](menucontroller/)|The static controller that draws your menus and handles all controls. You register your menus here.|
|[Menu Items](menuitems/)|Everything you can put inside a menu: buttons, checkboxes, lists and dynamic lists.|
|[Events](events/)|Every event a menu can raise, with the parameters they pass to your handler.|

----

### Quick start

This is a complete, working menu. Everything else in this reference builds on these few calls.

```cs
using System;
using System.Collections.Generic;
using CitizenFX.Core;
using MenuAPI;

namespace MyResource
{
    public class MyMenu : BaseScript
    {
        public MyMenu()
        {
            // 1. Create a menu and register it with the controller.
            Menu menu = new Menu("My Resource", "Main Menu");
            MenuController.AddMenu(menu);

            // 2. Add some items.
            MenuItem button = new MenuItem("A button", "Press enter on me.");
            MenuCheckboxItem checkbox = new MenuCheckboxItem("A checkbox", "Toggle me.", false);
            MenuListItem list = new MenuListItem("A list", new List<string>() { "One", "Two", "Three" }, 0);

            menu.AddMenuItem(button);
            menu.AddMenuItem(checkbox);
            menu.AddMenuItem(list);

            // 3. React to what the user does.
            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item == button)
                {
                    Debug.WriteLine("The button was pressed!");
                }
            };

            menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
            {
                Debug.WriteLine($"The checkbox is now {_checked}.");
            };

            // 4. Open it yourself. The built-in toggle key does not work in RedM,
            //    see MenuController -> The menu toggle key.
            RegisterCommand("mymenu", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (!MenuController.IsAnyMenuOpen())
                {
                    menu.OpenMenu();
                }
            }), false);
        }
    }
}
```

----

### Where to look for what

|I want to…|See|
|-|-|
|Create a menu, add or remove items, sort or filter them|[Menu](menu/)|
|Open, close or navigate a menu from code|[Menu](menu/#methods)|
|Create a submenu|[MenuController.BindMenuItem()](menucontroller/#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem)|
|Open menus myself instead of using the toggle key|[MenuController](menucontroller/#the-menu-toggle-key)|
|Stop the user from closing a menu|[MenuController.PreventExitingMenu](menucontroller/#properties)|
|Change the UI prompts at the bottom of the screen|[Menu.InstructionalButtons](menu/#instructional-buttons)|
|Run my own code when a control is pressed|[Menu.ButtonPressHandlers](menu/#button-press-handlers)|
|Use the `Control` enum in RedM|[MenuController](menucontroller/#the-menu-toggle-key)|
|Add an icon or right-hand text to an item|[MenuItem](menuitems/menuitem/)|
|Attach my own data to an item|[MenuItem.ItemData](menuitems/menuitem/#properties)|
|Know which event fires for which item type|[Events](events/#which-event-fires-for-which-item)|
