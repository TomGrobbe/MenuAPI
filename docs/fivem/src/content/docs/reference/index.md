---
title: "API Reference"
---

## API Reference

Every public class, property, method and event in MenuAPI. Not set up yet? Start at [Setup](../setup/).

----

### The classes

|Class|What it is|
|-|-|
|[Menu](menu/)|A single menu: banner, subtitle, items and description box. Also where all [events](events/) live.|
|[MenuController](menucontroller/)|The static controller that registers, draws and controls your menus.|
|[Pagination](pagination/)|Splitting a long menu into pages, moved between with left and right.|
|[GlobalInstructionalButtons](globalinstructionalbuttons/)|Button hints that belong to your resource instead of a menu, shown with or without a menu open.|
|[Menu Items](menuitems/)|Buttons, checkboxes, lists, dynamic lists and sliders.|
|[Events](events/)|Every event a menu can raise, with its parameters.|
|[MenuTicks](ticks/)|A read only view of the loops MenuAPI runs, and when each one stops.|

----

### Quick start

Examples in this reference use Legacy names (`BaseScript`, `Debug.WriteLine`). [Setup](../setup/#4-make-your-first-menu) shows the Enhanced equivalents.

```cs
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

            // 4. Open it. (The menu toggle key, M by default, already opens the
            //    first registered menu for you, so this is only needed if you
            //    want your own trigger.)
            menu.OpenMenu();
        }
    }
}
```

----

### Where to look for what

|I want to…|See|
|-|-|
|Create a menu, add or remove items, sort or filter them|[Menu](menu/)|
|Show hundreds or thousands of items without endless scrolling|[Pagination](pagination/)|
|Open, close or navigate a menu from code|[Menu](menu/#methods)|
|Create a submenu|[MenuController.BindMenuItem()](menucontroller/#bindmenuitemmenu-parentmenu-menu-childmenu-menuitem-menuitem)|
|Know which keys open and control the menu, and how players rebind them|[Key bindings](keybindings/)|
|Open menus myself instead of using the toggle key|[Menu.OpenMenu()](menu/#openmenu)|
|Move menus to the right side of the screen|[MenuController.MenuAlignment](menucontroller/#menu-alignment)|
|Stop the user from closing a menu|[MenuController.PreventExitingMenu](menucontroller/#properties)|
|Change the instructional buttons at the bottom of the screen|[Menu instructional buttons](menu/#instructional-buttons)|
|Run my own code when a key is pressed|[Key bindings](keybindings/#your-own-keys)|
|Add an icon or right-hand text to an item|[MenuItem](menuitems/menuitem/)|
|Attach my own data to an item|[MenuItem.ItemData](menuitems/menuitem/#properties)|
|Know which event fires for which item type|[Events](events/#which-event-fires-for-which-item)|
|Show a weapon or vehicle stats panel|[Menu](menu/#weapon--vehicle-stats-panels)|
|See what MenuAPI is actually running, and when|[MenuTicks](ticks/)|
