using System;
using System.Collections.Generic;

using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared.Script;

using MenuAPI;

namespace TestMenu;

public class ExampleMenu : IScript
{
    public ExampleMenu()
    {
        // Setting the menu alignment to be right aligned. This can be changed at any time and it'll update instantly.
        // To test this, checkout one of the checkbox items in this example menu. Clicking it will toggle the menu alignment.
        MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;
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
        // Creating 3 sliders, showing off the 3 possible variations and custom colors.
        MenuSliderItem slider = new MenuSliderItem("Slider", 0, 10, 5, false);
        MenuSliderItem slider2 = new MenuSliderItem("Slider + Bar", 0, 10, 5, true)
        {
            BarColor = System.Drawing.Color.FromArgb(255, 73, 233, 111),
            BackgroundColor = System.Drawing.Color.FromArgb(255, 25, 100, 43)
        };
        MenuSliderItem slider3 = new MenuSliderItem(
            "Slider + Bar + Icons",
            "The icons are currently male/female because that's probably the most common use. But any icon can be used!",
            0,
            10,
            5,
            true
        )
        {
            BarColor = System.Drawing.Color.FromArgb(255, 255, 0, 0),
            BackgroundColor = System.Drawing.Color.FromArgb(255, 100, 0, 0),
            SliderLeftIcon = MenuItem.Icon.MALE,
            SliderRightIcon = MenuItem.Icon.FEMALE
        };

        // adding the sliders to the menu.
        menu.AddMenuItem(slider);
        menu.AddMenuItem(slider2);
        menu.AddMenuItem(slider3);
        // Creating 3 checkboxs, 2 different styles and one has a locked icon and it's 'not enabled' (not enabled meaning you can't toggle it).
        MenuCheckboxItem box = new MenuCheckboxItem(
            "Checkbox - Style 1 (click me!)",
            "This checkbox can toggle the menu position! Try it out.",
            !menu.LeftAligned
        )
        {
            Style = MenuCheckboxItem.CheckboxStyle.Cross
        };
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
        menu.AddMenuItem(box);
        menu.AddMenuItem(box2);
        menu.AddMenuItem(box3);

        // Dynamic list item
        string ChangeCallback(MenuDynamicListItem item, bool left)
        {
            int current = int.TryParse(item.CurrentItem, out int parsed) ? parsed : 0;

            return (left ? current - 1 : current + 1).ToString();
        }
        MenuDynamicListItem dynList = new MenuDynamicListItem(
            "Dynamic list item.",
            "0",
            new MenuDynamicListItem.ChangeItemCallback(ChangeCallback),
            "Description for this dynamic item. Pressing left will make the value smaller, pressing right will make the value bigger."
        );
        menu.AddMenuItem(dynList);
        // List items (first the 3 special variants, then a normal one)
        List<string> colorList = new List<string>();
        for (var i = 0; i < 64; i++)
        {
            colorList.Add($"Color #{i}");
        }
        MenuListItem hairColors = new MenuListItem(
            "Hair Color",
            colorList,
            0,
            "Hair color pallete."
        )
        {
            ShowColorPanel = true
        };

        // Also special
        List<string> makeupColorList = new List<string>();
        for (var i = 0; i < 64; i++)
        {
            makeupColorList.Add($"Color #{i}");
        }
        MenuListItem makeupColors = new MenuListItem("Makeup Color", makeupColorList, 0, "Makeup color pallete.")
        {
            ShowColorPanel = true,
            ColorPanelColorType = MenuListItem.ColorPanelType.Makeup
        };

        // Also special
        List<string> opacityList = new List<string>();
        for (var i = 0; i < 11; i++)
        {
            opacityList.Add($"Opacity {i * 10}%");
        }
        MenuListItem opacity = new MenuListItem("Opacity Panel", opacityList, 0, "Set an opacity for something.")
        {
            ShowOpacityPanel = true
        };

        menu.AddMenuItem(hairColors);
        menu.AddMenuItem(makeupColors);
        menu.AddMenuItem(opacity);
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
            Label = "→→→"
        };
        menu.AddMenuItem(menuButton);
        MenuController.BindMenuItem(menu, submenu, menuButton);

        // Adding items with sprites left & right to the submenu.
        for (var i = 0; i < Enum.GetValues<MenuItem.Icon>().Length; i++)
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
        // Instructional buttons setup for the second (submenu) menu.
        submenu.InstructionalButtons.Add(Control.CharacterWheel, "Right?!");
        submenu.InstructionalButtons.Add(Control.CursorScrollDown, "Cool");
        submenu.InstructionalButtons.Add(Control.CreatorDelete, "Out!");
        submenu.InstructionalButtons.Add(Control.Cover, "This");
        submenu.InstructionalButtons.Add(Control.Context, "Check");
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
            Label = "→→→"
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

        // Create menu with weapon stats panel
        Menu menu4 = new Menu("Weapon Stats", "Weapon Stats Panel") { ShowWeaponStatsPanel = true };
        menu4.AddMenuItem(new MenuItem("dummy item", "You should add at least one item when using weapon stat panels"));
        menu4.SetWeaponStats(0.2f, 0.4f, 0.7f, 0.8f);
        menu4.SetWeaponComponentStats(0.4f, 0f, -0.05f, 0.1f);
        MenuController.AddSubmenu(menu, menu4);
        MenuItem weaponStats = new MenuItem("Weapon stats", "Demo menu for weapon stats components");
        menu.AddMenuItem(weaponStats);
        MenuController.BindMenuItem(menu, menu4, weaponStats);

        // Create menu with vehicle stats panel
        Menu menu5 = new Menu("Vehicle Stats", "Vehicle Stats Panel") { ShowVehicleStatsPanel = true };
        menu5.AddMenuItem(new MenuItem("dummy item", "You should add at least one item when using vehicle stat panels"));
        menu5.SetVehicleStats(0.2f, 0.2f, 0.3f, 0.8f);
        menu5.SetVehicleUpgradeStats(0.4f, -0.025f, 0.05f, 0.1f);
        MenuController.AddSubmenu(menu, menu5);
        MenuItem vehicleStats = new MenuItem("Vehicle stats", "Demo menu for vehicle stats components");
        menu.AddMenuItem(vehicleStats);
        MenuController.BindMenuItem(menu, menu5, vehicleStats);

        // Create a menu to play with the built in select/back instructional buttons. Those two follow
        // whatever the player bound in FiveM's own key bindings screen, so rebinding 'Menu select'
        // there should immediately change the button shown at the bottom of the screen.
        Menu menu6 = new Menu("Key Bindings", "Instructional buttons");
        MenuCheckboxItem showSelectButton = new MenuCheckboxItem(
            "Show the 'select' button",
            "Toggles ShowSelectInstructionalButton for this menu.",
            menu6.ShowSelectInstructionalButton
        );
        MenuCheckboxItem showBackButton = new MenuCheckboxItem(
            "Show the 'back' button",
            "Toggles ShowBackInstructionalButton for this menu.",
            menu6.ShowBackInstructionalButton
        );
        List<string> buttonTexts = new List<string>() { "Select", "Buy", "Confirm", "Do the thing" };
        MenuListItem selectButtonText = new MenuListItem(
            "'Select' button text",
            buttonTexts,
            0,
            "Changes SelectButtonText. The button itself still follows the player's own key binding."
        );
        MenuItem bindingInfo = new MenuItem(
            "How to rebind these",
            "Every keyboard menu control is a FiveM key binding. Open the pause menu, go to Settings, " +
            "Key Bindings, and look for this resource. The menu opens with ~r~M~s~ by default."
        )
        {
            Enabled = false,
            LeftIcon = MenuItem.Icon.LOCK
        };
        menu6.AddMenuItem(bindingInfo);
        menu6.AddMenuItem(showSelectButton);
        menu6.AddMenuItem(showBackButton);
        menu6.AddMenuItem(selectButtonText);

        menu6.OnCheckboxChange += (_menu, _item, _index, _checked) =>
        {
            if (_item == showSelectButton)
            {
                _menu.ShowSelectInstructionalButton = _checked;
            }
            else if (_item == showBackButton)
            {
                _menu.ShowBackInstructionalButton = _checked;
            }
        };
        menu6.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
        {
            if (_listItem == selectButtonText)
            {
                _menu.SelectButtonText = buttonTexts[_newIndex];
            }
        };

        MenuController.AddSubmenu(menu, menu6);
        MenuItem keyBindings = new MenuItem("Key bindings", "Demo menu for the select and back instructional buttons.");
        menu.AddMenuItem(keyBindings);
        MenuController.BindMenuItem(menu, menu6, keyBindings);

        // Create a menu that shows off SortMenuItems, FilterMenuItems and ResetFilter. The action
        // buttons live in the same menu as the produce, so the sort and filter callbacks have to keep
        // them in place, otherwise you would sort away the buttons you need to press.
        Menu menu7 = new Menu("Sorting & Filtering", "Fruit and veg");
        List<MenuItem> produce = new List<MenuItem>();
        List<MenuItem> fruits = new List<MenuItem>();
        List<MenuItem> vegetables = new List<MenuItem>();

        // Deliberately out of order so sorting visibly does something.
        foreach (var name in new string[] { "Peach", "Apple", "Mango", "Cherry", "Banana", "Strawberry", "Orange" })
        {
            var fruitItem = new MenuItem(name, $"{name} is a fruit.") { Label = "Fruit", LeftIcon = MenuItem.Icon.STAR };
            fruits.Add(fruitItem);
            produce.Add(fruitItem);
        }
        foreach (var name in new string[] { "Onion", "Carrot", "Spinach", "Broccoli", "Leek", "Potato", "Cucumber" })
        {
            var vegItem = new MenuItem(name, $"{name} is a vegetable.") { Label = "Vegetable", LeftIcon = MenuItem.Icon.TICK };
            vegetables.Add(vegItem);
            produce.Add(vegItem);
        }

        MenuItem sortAscending = new MenuItem("Sort A to Z", "Sorts the produce by name. Note that sorting also clears an active filter.");
        MenuItem sortDescending = new MenuItem("Sort Z to A", "Sorts the produce by name, backwards.");
        MenuItem randomizeOrder = new MenuItem("Randomize the order", "Shuffles the produce back into a random order.");
        MenuItem onlyFruits = new MenuItem("Show only fruit", "Filters the list down to the fruit.") { Label = "Filter" };
        MenuItem onlyVegetables = new MenuItem("Show only vegetables", "Filters the list down to the vegetables.") { Label = "Filter" };
        MenuItem clearFilter = new MenuItem("Show everything", "Clears the filter so all the produce comes back.") { Label = "Filter" };

        List<MenuItem> actions = new List<MenuItem>() { sortAscending, sortDescending, randomizeOrder, onlyFruits, onlyVegetables, clearFilter };

        // ReferenceEquals rather than Contains, because List<T>.Contains needs a default equality
        // comparer and those are not available on the FiveM client.
        int ActionOrder(MenuItem item) => actions.FindIndex(a => ReferenceEquals(a, item));
        int ProduceOrder(MenuItem item) => produce.FindIndex(p => ReferenceEquals(p, item));
        bool IsIn(List<MenuItem> list, MenuItem item) => list.FindIndex(i => ReferenceEquals(i, item)) >= 0;

        // Keeps the action buttons pinned to the top in their original order, and hands everything
        // else to the caller's comparison.
        void SortProduce(Comparison<MenuItem> compareProduce)
        {
            menu7.SortMenuItems((a, b) =>
            {
                int aAction = ActionOrder(a);
                int bAction = ActionOrder(b);
                if (aAction >= 0 || bAction >= 0)
                {
                    return (aAction < 0 ? int.MaxValue : aAction).CompareTo(bAction < 0 ? int.MaxValue : bAction);
                }
                return compareProduce(a, b);
            });
            menu7.RefreshIndex();
        }

        foreach (var actionItem in actions)
        {
            menu7.AddMenuItem(actionItem);
        }
        foreach (var produceItem in produce)
        {
            menu7.AddMenuItem(produceItem);
        }

        Random shuffleRandom = new Random();
        menu7.OnItemSelect += (_menu, _item, _index) =>
        {
            if (ReferenceEquals(_item, sortAscending))
            {
                SortProduce((a, b) => string.Compare(a.Text, b.Text, StringComparison.OrdinalIgnoreCase));
            }
            else if (ReferenceEquals(_item, sortDescending))
            {
                SortProduce((a, b) => string.Compare(b.Text, a.Text, StringComparison.OrdinalIgnoreCase));
            }
            else if (ReferenceEquals(_item, randomizeOrder))
            {
                // Shuffle the backing list, then sort by position in it. Sorting with a comparison that
                // returns random values would throw, because List.Sort rejects inconsistent comparers.
                for (int i = produce.Count - 1; i > 0; i--)
                {
                    int j = shuffleRandom.Next(i + 1);
                    (produce[i], produce[j]) = (produce[j], produce[i]);
                }
                SortProduce((a, b) => ProduceOrder(a).CompareTo(ProduceOrder(b)));
            }
            else if (ReferenceEquals(_item, onlyFruits))
            {
                _menu.FilterMenuItems(i => ActionOrder(i) >= 0 || IsIn(fruits, i));
            }
            else if (ReferenceEquals(_item, onlyVegetables))
            {
                _menu.FilterMenuItems(i => ActionOrder(i) >= 0 || IsIn(vegetables, i));
            }
            else if (ReferenceEquals(_item, clearFilter))
            {
                _menu.ResetFilter();
            }
        };

        MenuController.AddSubmenu(menu, menu7);
        MenuItem sortingAndFiltering = new MenuItem("Sorting & filtering", "Demo menu for SortMenuItems, FilterMenuItems and ResetFilter.")
        {
            Label = "→→→"
        };
        menu.AddMenuItem(sortingAndFiltering);
        MenuController.BindMenuItem(menu, menu7, sortingAndFiltering);

        // A paginated menu. Left and right turn the page on the plain buttons, and still change the
        // value on the list and the slider, because those already mean something with arrows.
        Menu menu8 = new Menu("Pagination", "Page 1 / 4");
        menu8.SetPageSize(12);

        MenuListItem pickAPlanet = new MenuListItem(
            "A list item",
            new List<string>() { "Mercury", "Venus", "Earth", "Mars" },
            0,
            "Arrows still change this value, they do not turn the page. Same for the slider below."
        );
        MenuSliderItem aSlider = new MenuSliderItem("A slider item", 0, 10, 5, true);
        MenuItem lockedButton = new MenuItem("A locked button", "Locked rows do not stop you turning the page.")
        {
            Enabled = false,
            LeftIcon = MenuItem.Icon.LOCK
        };

        menu8.AddMenuItem(pickAPlanet);
        menu8.AddMenuItem(aSlider);
        menu8.AddMenuItem(lockedButton);

        for (int i = 1; i <= 45; i++)
        {
            menu8.AddMenuItem(new MenuItem($"Item #{i}", $"Item number {i} of 45. Press left or right to turn the page.")
            {
                Label = $"#{i}"
            });
        }

        menu8.OnPageChange += (_menu, _oldPage, _newPage, _wrapped) =>
        {
            API.Log.Info($"OnPageChange: [{_menu}, {_oldPage}, {_newPage}, wrapped: {_wrapped}]");

            _menu.MenuSubtitle = _wrapped
                ? $"~y~Page {_newPage + 1} / {_menu.PageCount} (wrapped around)"
                : $"Page {_newPage + 1} / {_menu.PageCount}";
        };

        MenuController.AddSubmenu(menu, menu8);
        MenuItem pagination = new MenuItem("Pagination", "Demo menu for SetPageSize and the page navigation.")
        {
            Label = "→→→"
        };
        menu.AddMenuItem(pagination);
        MenuController.BindMenuItem(menu, menu8, pagination);

        // Header styling. Everything here is set on the menu itself, so the banner above the items
        // is the thing that changes while you scroll through them.
        Menu menu9 = new Menu("Header Styling", "Title font & glare")
        {
            MenuTitleFont = MenuFont.Pricedown,
            ShowHeaderGlare = true
        };

        List<int> fontIds = new List<int>()
        {
            MenuFont.ChaletLondon,
            MenuFont.HouseScript,
            MenuFont.Monospace,
            MenuFont.ChaletComprimeCologne,
            MenuFont.Pricedown
        };
        List<string> fontNames = new List<string>()
        {
            "Chalet London",
            "House Script",
            "Monospace",
            "Chalet Comprime Cologne",
            "Pricedown"
        };
        MenuListItem titleFont = new MenuListItem(
            "Title font",
            fontNames,
            fontIds.IndexOf(MenuFont.Pricedown),
            "Sets MenuTitleFont. Each font is drawn at whatever size suits it, you do not have to tune that yourself."
        );

        MenuListItem titleAlignment = new MenuListItem(
            "Title alignment",
            new List<string>() { "Left", "Center", "Right" },
            (int)MenuController.DefaultTitleAlignment,
            "Sets MenuTitleAlignment, which moves the title within the banner."
        );

        MenuCheckboxItem headerGlare = new MenuCheckboxItem(
            "Header glare",
            "Sets ShowHeaderGlare. This is GTA Online's moving glow, drawn with the game's own scaleform. Turn the camera to see it move.",
            true
        );

        menu9.AddMenuItem(titleFont);
        menu9.AddMenuItem(titleAlignment);
        menu9.AddMenuItem(headerGlare);

        menu9.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
        {
            if (_listItem == titleFont)
            {
                _menu.MenuTitleFont = fontIds[_newIndex];
            }
            else if (_listItem == titleAlignment)
            {
                _menu.MenuTitleAlignment = (Menu.TitleAlignmentOption)_newIndex;
            }
        };

        menu9.OnCheckboxChange += (_menu, _item, _index, _checked) =>
        {
            if (_item == headerGlare)
            {
                _menu.ShowHeaderGlare = _checked;
            }
        };

        MenuController.AddSubmenu(menu, menu9);
        MenuItem headerStyling = new MenuItem("Header styling", "Demo menu for the title font, the title alignment and the header glare.")
        {
            Label = "→→→"
        };
        menu.AddMenuItem(headerStyling);
        MenuController.BindMenuItem(menu, menu9, headerStyling);
        /*--------------
         Event handlers
        --------------*/

        menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
        {
            // Code in here gets executed whenever a checkbox is toggled.
            API.Log.Info($"OnCheckboxChange: [{_menu}, {_item}, {_index}, {_checked}]");
            // If the align-menu checkbox is toggled, toggle the menu alignment.
            if (_item == box)
            {
                if (_checked)
                {
                    MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;
                }
                else
                {
                    MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Left;
                }
            }
        };

        menu.OnItemSelect += (_menu, _item, _index) =>
        {
            // Code in here would get executed whenever an item is pressed.
            API.Log.Info($"OnItemSelect: [{_menu}, {_item}, {_index}]");
        };

        menu.OnIndexChange += (_menu, _oldItem, _newItem, _oldIndex, _newIndex) =>
        {
            // Code in here would get executed whenever the up or down key is pressed and the index of the menu is changed.
            API.Log.Info($"OnIndexChange: [{_menu}, {_oldItem}, {_newItem}, {_oldIndex}, {_newIndex}]");
        };

        menu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
        {
            // Code in here would get executed whenever the selected value of a list item changes (when left/right key is pressed).
            API.Log.Info($"OnListIndexChange: [{_menu}, {_listItem}, {_oldIndex}, {_newIndex}, {_itemIndex}]");
        };

        menu.OnListItemSelect += (_menu, _listItem, _listIndex, _itemIndex) =>
        {
            // Code in here would get executed whenever a list item is pressed.
            API.Log.Info($"OnListItemSelect: [{_menu}, {_listItem}, {_listIndex}, {_itemIndex}]");
        };

        menu.OnSliderPositionChange += (_menu, _sliderItem, _oldPosition, _newPosition, _itemIndex) =>
        {
            // Code in here would get executed whenever the position of a slider is changed (when left/right key is pressed).
            API.Log.Info($"OnSliderPositionChange: [{_menu}, {_sliderItem}, {_oldPosition}, {_newPosition}, {_itemIndex}]");
        };

        menu.OnSliderItemSelect += (_menu, _sliderItem, _sliderPosition, _itemIndex) =>
        {
            // Code in here would get executed whenever a slider item is pressed.
            API.Log.Info($"OnSliderItemSelect: [{_menu}, {_sliderItem}, {_sliderPosition}, {_itemIndex}]");
        };

        menu.OnMenuClose += (_menu) =>
        {
            // Code in here gets triggered whenever the menu is closed.
            API.Log.Info($"OnMenuClose: [{_menu}]");
        };

        menu.OnMenuOpen += (_menu) =>
        {
            // Code in here gets triggered whenever the menu is opened.
            API.Log.Info($"OnMenuOpen: [{_menu}]");
        };

        menu.OnDynamicListItemCurrentItemChange += (_menu, _dynamicListItem, _oldCurrentItem, _newCurrentItem) =>
        {
            // Code in here would get executed whenever the value of the current item of a dynamic list item changes.
            API.Log.Info($"OnDynamicListItemCurrentItemChange: [{_menu}, {_dynamicListItem}, {_oldCurrentItem}, {_newCurrentItem}]");
        };

        menu.OnDynamicListItemSelect += (_menu, _dynamicListItem, _currentItem) =>
        {
            // Code in here would get executed whenever a dynamic list item is pressed.
            API.Log.Info($"OnDynamicListItemSelect: [{_menu}, {_dynamicListItem}, {_currentItem}]");
        };
    }
}