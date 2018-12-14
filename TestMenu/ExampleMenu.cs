﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MenuAPI;

namespace TestMenu
{
    public class ExampleMenu : BaseScript
    {
        public ExampleMenu()
        {
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Right;

            Menu menu = new Menu("Main Menu", "Subtitle") { Visible = true };
            Menu menu2 = new Menu("New Menu", "Subutitle 2");
            Menu menu3 = new Menu(null, "WHO NEEDS A BANNER!?");

            MenuController.AddMenu(menu);
            MenuController.AddSubmenu(menu, menu2);
            MenuController.AddSubmenu(menu2, menu3);

            MenuItem item1 = new MenuItem("First Item") { Label = "→→→" };
            MenuItem item2 = new MenuItem("Second Item");
            MenuItem item3 = new MenuItem("3");
            MenuItem item4 = new MenuItem("4");
            MenuItem item5 = new MenuItem("5", "Hey look, i've got a description! Cool!") { Label = "→→→" };

            item4.Enabled = false;
            item4.LeftIcon = MenuItem.Icon.LOCK;

            MenuCheckboxItem box = new MenuCheckboxItem("Right Align Menu", "Make this menu right aligned. If you set this to false, then the menu will move to the left.", !menu.LeftAligned)
            {
                Style = MenuCheckboxItem.CheckboxStyle.Tick
            };

            MenuCheckboxItem box2 = new MenuCheckboxItem("Another Checkbox Style", "There are two checkbox styles to choose from. A checkmark, and a cross.", false)
            {
                Style = MenuCheckboxItem.CheckboxStyle.Cross
            };

            //menu.AddMenuItem(new MenuListItem("Left Text", new List<string>() { "Item one", "Item Two", "Item Three" }, 0, "Description for this stuffs.") { ShowOpacityPanel = true });
            //menu.AddMenuItem(new MenuListItem("Wow!", new List<string>() { "Item one", "Item Two", "Item Three" }, 0, "Holy crap! It's a list item with a right icon! Very cool!") { RightIcon = MenuItem.Icon.HEALTH_HEART, ShowOpacityPanel = true });
            var colorList = new List<string>();
            for (var i = 0; i < 64; i++)
            {
                colorList.Add($"Color #{i}");
            }
            var hairColors = new MenuListItem("Hair Color", colorList, 0, "Choose a hair color.")
            {
                ShowColorPanel = true
            };
            menu.AddMenuItem(hairColors);

            var makeupColorList = new List<string>();
            for (var i = 0; i < 64; i++)
            {
                makeupColorList.Add($"Color #{i}");
            }
            var makeupColors = new MenuListItem("Makeup Color", makeupColorList, 0, "Choose a makeup color.")
            {
                ShowColorPanel = true,
                ColorPanelColorType = MenuListItem.ColorPanelType.Makeup
            };
            menu.AddMenuItem(makeupColors);

            var opacityList = new List<string>();
            for (var i = 0; i < 11; i++)
            {
                opacityList.Add($"Opacity {i * 10}%");
            }
            var opacity = new MenuListItem("Makeup Opacity", opacityList, 0, "Set a makeup opacity.")
            {
                ShowOpacityPanel = true
            };
            menu.AddMenuItem(opacity);



            menu.AddMenuItem(item1);
            menu.AddMenuItem(box);
            menu.AddMenuItem(box2);
            menu.AddMenuItem(item4);
            menu2.AddMenuItem(item2);
            menu2.AddMenuItem(item3);
            menu2.AddMenuItem(item5);

            MenuController.BindMenuItem(menu, menu2, item1);
            MenuController.BindMenuItem(menu2, menu3, item5);

            menu.OnCheckboxChange += (_menu, _item, _index, _checked) =>
            {
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
                Debug.WriteLine($"Pressed item with text {_item.Text} and index {_index} in menu with title {_menu.MenuTitle}");
            };

            menu.AddMenuItem(new MenuItem("Test Item #1", "Description (1). abc abc abc abc"));
            menu.AddMenuItem(new MenuItem("Test Item #2", "Description (2). abc abc abc abc abc abc abc abc abc abc abc abc"));
            menu.AddMenuItem(new MenuItem("Test Item #3", "Description ∑ (3). abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc") { Label = "Right Label", RightIcon = MenuItem.Icon.MASK });
            menu.AddMenuItem(new MenuItem("Test Item #4", "Description (4). abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc"));
            menu.AddMenuItem(new MenuItem("Test Item #5", "Description (5). abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc"));

            for (var i = 0; i < 14; i++)
            {
                var tmpitem = new MenuItem($"Item #{i}", $"Some cool description, idk what to put here tbh, something cool i guess but this sucks. Description: #{i}.") { LeftIcon = (MenuItem.Icon)i, RightIcon = (MenuItem.Icon)(27 - i) };
                menu3.AddMenuItem(tmpitem);
            }
        }
    }
}
