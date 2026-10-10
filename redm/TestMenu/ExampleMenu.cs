using System;
using System.Collections.Generic;
using MenuAPI;
using CitizenFX.Core;

namespace TestMenu
{
    public class ExampleMenu : BaseScript
    {
        public ExampleMenu()
        {
            // Creating the first menu.
            Menu menu = new Menu("Main Menu", "Subtitle");
            MenuController.AddMenu(menu);

            // Adding a new button by directly creating one inline.
            // You could also just store it and then add it but we don't need to do that in this example.
            menu.AddMenuItem(
                new MenuItem(
                    "Normal Button",
                    "This is a simple button with a simple description. Scroll down for more button types!"
                )
                {
                    Enabled = false,
                    LeftIcon = MenuItem.Icon.TICK
                }
            );
            MenuCheckboxItem box2 = new MenuCheckboxItem(
                "Checkbox - Style 2",
                "This checkbox does nothing right now.",
                true
            )
            {
                Style = MenuCheckboxItem.CheckboxStyle.Tick
            };

            MenuCheckboxItem box3 = new MenuCheckboxItem(
                "Checkbox (unchecked + locked)",
                "Make this menu right aligned. If you set this to false, then the menu will move to the left.",
                false
            )
            {
                Enabled = false,
                LeftIcon = MenuItem.Icon.LOCK
            };

            // Adding the checkboxes to the menu.
            menu.AddMenuItem(box2);
            menu.AddMenuItem(box3);

            // Dynamic list item
            string ChangeCallback(MenuDynamicListItem item, bool left)
            {
                if (left)
                    return (int.Parse(item.CurrentItem) - 1).ToString();
                return (int.Parse(item.CurrentItem) + 1).ToString();
            }
            MenuDynamicListItem dynList = new MenuDynamicListItem(
                "Dynamic list item.",
                "0",
                new MenuDynamicListItem.ChangeItemCallback(ChangeCallback),
                "Description for this dynamic item. Pressing left will make the value smaller, pressing right will make the value bigger."
            );
            menu.AddMenuItem(dynList);
            // Normal
            List<string> normalList = new List<string>() { "Item #1", "Item #2", "Item #3" };
            MenuListItem normalListItem = new MenuListItem(
                "Normal List Item",
                normalList,
                0,
                "And another simple description for yet another simple (list) item. Nothing special about this one."
            );

            // Adding the lists to the menu.
            menu.AddMenuItem(normalListItem);

            // Creating a submenu, adding it to the menus list, and creating and binding a button for it.
            Menu submenu = new Menu("Submenu", "Secondary Menu");
            MenuController.AddSubmenu(menu, submenu);

            MenuItem menuButton = new MenuItem(
                "Submenu",
                "This button is bound to a submenu. Clicking it will take you to the submenu."
            )
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };
            menu.AddMenuItem(menuButton);
            MenuController.BindMenuItem(menu, submenu, menuButton);

            // Adding items with sprites left & right to the submenu.
            for (var i = 0; i < Enum.GetValues(typeof(MenuItem.Icon)).Length; i++)
            {
                var tmpItem = new MenuItem(
                    $"Icon.{Enum.GetName(typeof(MenuItem.Icon), ((MenuItem.Icon)i))}",
                    "This menu item has a left and right sprite. Press ~r~HOME~s~ to toggle the 'enabled' state on these items."
                )
                {
                    Label = $"(#{i})",
                    RightIcon = (MenuItem.Icon)i,
                    LeftIcon = (MenuItem.Icon)i
                };

                submenu.AddMenuItem(tmpItem);
            }
            submenu.ButtonPressHandlers.Add(
                new Menu.ButtonPressHandler(Control.FrontendSocialClubSecondary,
                Menu.ControlPressCheckType.JUST_RELEASED,
                new Action<Menu, Control>((m, c) =>
                {
                    m.GetMenuItems().ForEach(a => a.Enabled = !a.Enabled);
                }), true)
            );
            // Create a third menu without a banner.
            Menu menu3 = new Menu(null, "Only a subtitle, no banner.");

            // you can use AddSubmenu or AddMenu, both will work but if you want to link this menu from another menu,
            // you should use AddSubmenu.
            MenuController.AddSubmenu(menu, menu3);
            MenuItem thirdSubmenuBtn = new MenuItem(
                "Another submenu",
                "This is just a submenu without a banner. No big deal. This also has a very long description to test multiple " +
                "lines and see if they work properly. Let's find out if it works as intended."
            )
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };
            menu.AddMenuItem(thirdSubmenuBtn);
            MenuController.BindMenuItem(menu, menu3, thirdSubmenuBtn);
            menu3.AddMenuItem(new MenuItem("Nothing here!"));
            menu3.AddMenuItem(new MenuItem("Nothing here!"));
            menu3.AddMenuItem(new MenuItem("Nothing here!"));
            menu3.AddMenuItem(new MenuItem("Nothing here!") { LeftIcon = MenuItem.Icon.TICK });

            for (var i = 0; i < 10; i++)
            {
                menu.AddMenuItem(new MenuItem($"Item #{i + 1}.", "With an invisible description."));
            }

            /*--------------
             Event handlers
            --------------*/

            menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
            {
                // Code in here gets executed whenever a checkbox is toggled.
                Debug.WriteLine($"OnCheckboxChange: [{_menu}, {_item}, {_index}, {_checked}]");
            };

            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                // Code in here would get executed whenever an item is pressed.
                Debug.WriteLine($"OnItemSelect: [{_menu}, {_item}, {_index}]");
            };

            menu.OnIndexChange += (_menu, _oldItem, _newItem, _oldIndex, _newIndex) =>
            {
                // Code in here would get executed whenever the up or down key is pressed and the index of the menu is changed.
                Debug.WriteLine($"OnIndexChange: [{_menu}, {_oldItem}, {_newItem}, {_oldIndex}, {_newIndex}]");
            };

            menu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                // Code in here would get executed whenever the selected value of a list item changes (when left/right key is pressed).
                Debug.WriteLine($"OnListIndexChange: [{_menu}, {_listItem}, {_oldIndex}, {_newIndex}, {_itemIndex}]");
            };

            menu.OnListItemSelect += (_menu, _listItem, _listIndex, _itemIndex) =>
            {
                // Code in here would get executed whenever a list item is pressed.
                Debug.WriteLine($"OnListItemSelect: [{_menu}, {_listItem}, {_listIndex}, {_itemIndex}]");
            };


            menu.OnMenuClose += (_menu) =>
            {
                // Code in here gets triggered whenever the menu is closed.
                Debug.WriteLine($"OnMenuClose: [{_menu}]");
            };

            menu.OnMenuOpen += (_menu) =>
            {
                // Code in here gets triggered whenever the menu is opened.
                Debug.WriteLine($"OnMenuOpen: [{_menu}]");
            };

            menu.OnDynamicListItemCurrentItemChange += (_menu, _dynamicListItem, _oldCurrentItem, _newCurrentItem) =>
            {
                // Code in here would get executed whenever the value of the current item of a dynamic list item changes.
                Debug.WriteLine($"OnDynamicListItemCurrentItemChange: [{_menu}, {_dynamicListItem}, {_oldCurrentItem}, {_newCurrentItem}]");
            };

            menu.OnDynamicListItemSelect += (_menu, _dynamicListItem, _currentItem) =>
            {
                // Code in here would get executed whenever a dynamic list item is pressed.
                Debug.WriteLine($"OnDynamicListItemSelect: [{_menu}, {_dynamicListItem}, {_currentItem}]");
            };

            // Opens the menu without the toggle key, or closes it when one is already open.
            CitizenFX.Core.Native.API.RegisterCommand("testmenu", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (MenuController.IsAnyMenuOpen())
                {
                    Debug.WriteLine("[TestMenu] /testmenu: a menu is open, closing all menus.");
                    MenuController.CloseAllMenus();
                    return;
                }

                Debug.WriteLine($"[TestMenu] /testmenu: opening {menu.MenuTitle}. MainMenu is {(MenuController.MainMenu == null ? "null" : MenuController.MainMenu.MenuTitle)}, {MenuController.Menus.Count} menus registered.");
                menu.OpenMenu();
                Debug.WriteLine($"[TestMenu] /testmenu: menu visible after opening: {menu.Visible}, any menu open: {MenuController.IsAnyMenuOpen()}.");
            }), false);
        }
    }
}
