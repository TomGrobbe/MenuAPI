using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    public class Menu
    {
        #region Setting up events

        #region delegates
        /// <summary>
        /// Triggered when a <see cref="MenuItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this event occurred.</param>
        /// <param name="menuItem">The <see cref="MenuItem"/> that was selected.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuItem"/>.</param>
        public delegate void ItemSelectEvent(Menu menu, MenuItem menuItem, int itemIndex);

        /// <summary>
        /// Triggered when a <see cref="MenuCheckboxItem"/> was toggled.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this event occurred.</param>
        /// <param name="menuItem">The <see cref="MenuCheckboxItem"/> that was toggled.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuCheckboxItem"/>.</param>
        /// <param name="newCheckedState">The new <see cref="MenuCheckboxItem.Checked"/> state of this <see cref="MenuCheckboxItem"/>.</param>
        public delegate void CheckboxItemChangeEvent(Menu menu, MenuCheckboxItem menuItem, int itemIndex, bool newCheckedState);

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnListItemSelect"/> event occurred.</param>
        /// <param name="listItem">The <see cref="MenuListItem"/> that was selected.</param>
        /// <param name="selectedIndex">The <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of the <see cref="MenuListItem"/> in the <see cref="Menu"/>.</param>
        public delegate void ListItemSelectedEvent(Menu menu, MenuListItem listItem, int selectedIndex, int itemIndex);

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/>'s index was changed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnListIndexChange"/> event occurred.</param>
        /// <param name="listItem">The <see cref="MenuListItem"/> that was changed.</param>
        /// <param name="oldSelectionIndex">The old <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="newSelectionIndex">The new <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of the <see cref="MenuListItem"/> in the <see cref="Menu"/>.</param>
        public delegate void ListItemIndexChangedEvent(Menu menu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex);

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is closed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> that was closed.</param>
        public delegate void MenuClosedEvent(Menu menu);

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is opened.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> that has been opened.</param>
        public delegate void MenuOpenedEvent(Menu menu);

        /// <summary>
        /// Triggered when the <see cref="CurrentIndex"/> changes.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnIndexChange"/></param> event occurred.
        /// <param name="oldItem">The old <see cref="MenuItem"/> that was previously selected.</param>
        /// <param name="newItem">The new <see cref="MenuItem"/> that is now selected.</param>
        /// <param name="oldIndex">The old <see cref="MenuItem.Index"/> of this item.</param>
        /// <param name="newIndex">The new <see cref="MenuItem.Index"/> of this item.</param>
        public delegate void IndexChangedEvent(Menu menu, MenuItem oldItem, MenuItem newItem, int oldIndex, int newIndex);

        /// <summary>
        /// Triggered when the <see cref="MenuSliderItem.Position"/> changes.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnSliderPositionChange"/> event occurred.</param>
        /// <param name="sliderItem">The <see cref="MenuSliderItem>"/> that was changed.</param>
        /// <param name="oldPosition">The old position of the slider bar.</param>
        /// <param name="newPosition">The new position of the slider bar.</param>
        /// <param name="itemIndex">The index of this <see cref="MenuSliderItem"/>.</param>
        public delegate void SliderPositionChangedEvent(Menu menu, MenuSliderItem sliderItem, int oldPosition, int newPosition, int itemIndex);

        /// <summary>
        /// Triggered when a <see cref="MenuSliderItem"/> was selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnSliderItemSelect"/> event occurred.</param>
        /// <param name="sliderItem">The <see cref="MenuSliderItem>"/> that was pressed.</param>
        /// <param name="sliderPosition">The current position of the slider bar.</param>
        /// <param name="itemIndex">The index of this <see cref="MenuSliderItem"/>.</param>
        public delegate void SliderItemSelectedEvent(Menu menu, MenuSliderItem sliderItem, int sliderPosition, int itemIndex);
        #endregion

        #region events
        /// <summary>
        /// Triggered when a <see cref="MenuItem"/> is selected.
        /// Parameters: <see cref="Menu"/> parentMenu, <see cref="MenuItem"/> menuItem, <see cref="int"/> itemIndex.
        /// </summary>
        public event ItemSelectEvent OnItemSelect;

        /// <summary>
        /// Triggered when a <see cref="MenuCheckboxItem"/> was toggled.
        /// Parameters: <see cref="Menu"/> parentMenu, <see cref="MenuCheckboxItem"/> menuItem, <see cref="int"/> itemIndex, <see cref="bool"/> newCheckedState.
        /// </summary>
        public event CheckboxItemChangeEvent OnCheckboxChange;

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/> is selected.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> listItem, <see cref="MenuListItem.ListIndex"/> selectedIndex, <see cref="int"/> itemIndex.
        /// </summary>
        public event ListItemSelectedEvent OnListItemSelect;

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/>'s index was changed.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> listItem, <see cref="MenuListItem.ListIndex"/> oldSelectionIndex, <see cref="int"/> newSelectionIndex, <see cref="int"/> itemIndex.
        /// </summary>
        public event ListItemIndexChangedEvent OnListIndexChange;

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is closed.
        /// Parameters: <see cref="Menu"/> closedMenu.
        /// </summary>
        public event MenuClosedEvent OnMenuClose;

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is opened.
        /// Parameters: <see cref="Menu"/> openedMenu.
        /// </summary>
        public event MenuOpenedEvent OnMenuOpen;

        /// <summary>
        /// Triggered when the <see cref="CurrentIndex"/> changes.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuItem"/> oldSelectedItem, <see cref="MenuItem"/> newSelectedItem, <see cref="int"/> oldIndex, <see cref="int"/> newIndex.
        /// </summary>
        public event IndexChangedEvent OnIndexChange;

        /// <summary>
        /// Triggered when the <see cref="MenuSliderItem.Position"/> changes.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuSliderItem"/> sliderItem, <see cref="int"/> oldPosition, <see cref="int"/> newPosition, <see cref="int"/> itemIndex
        /// </summary>
        public event SliderPositionChangedEvent OnSliderPositionChange;

        /// <summary>
        /// Triggered when a <see cref="MenuSliderItem"/> was selected.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuSliderItem"/> sliderItem, <see cref="int"/> sliderPosition, <see cref="int"/> itemIndex.
        /// </summary>
        public event SliderItemSelectedEvent OnSliderItemSelect;
        #endregion

        #region virtual voids
        /// <summary>
        /// Triggered when a <see cref="MenuItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this event occurred.</param>
        /// <param name="menuItem">The <see cref="MenuItem"/> that was selected.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuItem"/>.</param>
        protected virtual void ItemSelectedEvent(MenuItem menuItem, int itemIndex)
        {
            OnItemSelect?.Invoke(this, menuItem, itemIndex);
        }

        /// <summary>
        /// Triggered when a <see cref="MenuCheckboxItem"/> was toggled.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this event occurred.</param>
        /// <param name="menuItem">The <see cref="MenuCheckboxItem"/> that was toggled.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuCheckboxItem"/>.</param>
        /// <param name="newCheckedState">The new <see cref="MenuCheckboxItem.Checked"/> state of this <see cref="MenuCheckboxItem"/>.</param>
        protected virtual void CheckboxChangedEvent(MenuCheckboxItem menuItem, int itemIndex, bool _checked)
        {
            OnCheckboxChange?.Invoke(this, menuItem, itemIndex, _checked);
        }

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnListItemSelect"/> event occurred.</param>
        /// <param name="listItem">The <see cref="MenuListItem"/> that was selected.</param>
        /// <param name="selectedIndex">The <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of the <see cref="MenuListItem"/> in the <see cref="Menu"/>.</param>
        protected virtual void ListItemSelectEvent(Menu menu, MenuListItem listItem, int selectedIndex, int itemIndex)
        {
            OnListItemSelect?.Invoke(menu, listItem, selectedIndex, itemIndex);
        }

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/>'s index was changed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnListIndexChange"/> event occurred.</param>
        /// <param name="listItem">The <see cref="MenuListItem"/> that was changed.</param>
        /// <param name="oldSelectionIndex">The old <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="newSelectionIndex">The new <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of the <see cref="MenuListItem"/> in the <see cref="Menu"/>.</param>
        protected virtual void ListItemIndexChangeEvent(Menu menu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)
        {
            OnListIndexChange?.Invoke(menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex);
        }

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is closed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> that was closed.</param>
        protected virtual void MenuCloseEvent(Menu menu)
        {
            OnMenuClose?.Invoke(menu);
        }

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is opened.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> that has been opened.</param>
        protected virtual void MenuOpenEvent(Menu menu)
        {
            OnMenuOpen?.Invoke(menu);
        }

        /// <summary>
        /// Triggered when the <see cref="CurrentIndex"/> changes.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnIndexChange"/> event occurred.</param>
        /// <param name="oldItem">The old <see cref="MenuItem"/> that was previously selected.</param>
        /// <param name="newItem">The new <see cref="MenuItem"/> that is now selected.</param>
        /// <param name="oldIndex">The old <see cref="MenuItem.Index"/> of this item.</param>
        /// <param name="newIndex">The new <see cref="MenuItem.Index"/> of this item.</param>
        protected virtual void IndexChangeEvent(Menu menu, MenuItem oldItem, MenuItem newItem, int oldIndex, int newIndex)
        {
            OnIndexChange?.Invoke(menu, oldItem, newItem, oldIndex, newIndex);
        }

        /// <summary>
        /// Triggered when the <see cref="MenuSliderItem.Position"/> changes.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnSliderPositionChange"/> event occurred.</param>
        /// <param name="sliderItem">The <see cref="MenuSliderItem>"/> that was changed.</param>
        /// <param name="oldPosition">The old position of the slider bar.</param>
        /// <param name="newPosition">The new position of the slider bar.</param>
        /// <param name="itemIndex">The index of this <see cref="MenuSliderItem"/>.</param>
        protected virtual void SliderItemChangedEvent(Menu menu, MenuSliderItem sliderItem, int oldPosition, int newPosition, int itemIndex)
        {
            OnSliderPositionChange?.Invoke(menu, sliderItem, oldPosition, newPosition, itemIndex);
        }

        /// <summary>
        /// Triggered when a <see cref="MenuSliderItem"/> was selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnSliderItemSelect"/> event occurred.</param>
        /// <param name="sliderItem">The <see cref="MenuSliderItem>"/> that was pressed.</param>
        /// <param name="sliderPosition">The current position of the slider bar.</param>
        /// <param name="itemIndex">The index of this <see cref="MenuSliderItem"/>.</param>
        protected virtual void SliderSelectedEvent(Menu menu, MenuSliderItem sliderItem, int sliderPosition, int itemIndex)
        {
            OnSliderItemSelect?.Invoke(menu, sliderItem, sliderPosition, itemIndex);
        }

        #endregion

        #endregion

        #region constants or readonlys
        public const float Width = 500f;
        #endregion

        #region private variables
        private static SizeF headerSize = new SizeF(Width, 110f);

        public int ViewIndexOffset { get; private set; } = 0;

        private List<MenuItem> VisibleMenuItems
        {
            get
            {
                // Create a duplicate list, just in case the original list is modified while we're looping through it.
                var items = GetMenuItems().ToList().GetRange(ViewIndexOffset, Math.Min(MaxItemsOnScreen, Size - ViewIndexOffset));
                return items;
            }
        }

        private List<MenuItem> _MenuItems { get; set; } = new List<MenuItem>();

        private readonly int ColorPanelScaleform = RequestScaleformMovie("COLOUR_SWITCHER_02"); // Could probably be improved, but was getting some glitchy results if it wasn't pre-loaded.
        private readonly int OpacityPanelScaleform = RequestScaleformMovie("COLOUR_SWITCHER_01"); // Could probably be improved, but was getting some glitchy results if it wasn't pre-loaded.
        #endregion

        #region Public Variables
        public string MenuTitle { get; set; }

        public string MenuSubtitle { get; set; }

        public KeyValuePair<string, string> HeaderTexture { get; set; } = new KeyValuePair<string, string>();

        public bool IgnoreDontOpenMenus { get; set; } = false;

        public int MaxItemsOnScreen { get; internal set; } = 10;

        public int Size => _MenuItems.Count;

        public bool Visible { get; set; } = false;

        public bool LeftAligned => MenuController.MenuAlignment == MenuController.MenuAlignmentOption.Left;

        public PointF Position { get; private set; } = new PointF(0f, 0f);

        public float MenuItemsYOffset { get; private set; } = 0f;

        public string CounterPreText { get; set; }

        public Menu ParentMenu { get; internal set; } = null;

        public int CurrentIndex { get; internal set; } = 0;

        public bool EnableInstructionalButtons { get; set; } = true;

        public Dictionary<Control, string> InstructionalButtons = new Dictionary<Control, string>() { { Control.FrontendAccept, GetLabelText("HUD_INPUT28") }, { Control.FrontendCancel, GetLabelText("HUD_INPUT53") } };

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="Menu"/>.
        /// </summary>
        /// <param name="name"></param>
        public Menu(string name) : this(name, null) { }

        /// <summary>
        /// Creates a new <see cref="Menu"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subtitle"></param>
        public Menu(string name, string subtitle)
        {
            MenuTitle = name;
            MenuSubtitle = subtitle;
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Sets the max amount of visible items on screen at a time.
        /// Min = 3, max = 10.
        /// </summary>
        /// <param name="max">A value between 3 and 10 (inclusive).</param>
        public void SetMaxItemsOnScreen(int max)
        {
            if (max < 11 && max > 2)
            {
                MaxItemsOnScreen = max;
            }
        }

        /// <summary>
        /// Resets the index to 0
        /// </summary>
        public void RefreshIndex() => RefreshIndex(0, 0);
        public void RefreshIndex(int index) => RefreshIndex(index, index > MaxItemsOnScreen ? index - MaxItemsOnScreen : 0);
        public void RefreshIndex(int index, int viewOffset) { CurrentIndex = index; ViewIndexOffset = viewOffset; }

        /// <summary>
        /// Returns the menu items in this menu.
        /// </summary>
        /// <returns></returns>
        public List<MenuItem> GetMenuItems()
        {
            return _MenuItems.ToList();
        }

        /// <summary>
        /// Removes all menu items.
        /// </summary>
        public void ClearMenuItems()
        {
            CurrentIndex = 0;
            ViewIndexOffset = 0;
            _MenuItems.Clear();
        }
        /// <summary>
        /// Removes all menu items.
        /// </summary>
        public void ClearMenuItems(bool dontResetIndex)
        {
            if (!dontResetIndex)
            {
                CurrentIndex = 0;
                ViewIndexOffset = 0;
            }
            _MenuItems.Clear();
        }

        /// <summary>
        /// Adds a <see cref="MenuItem"/> to this <see cref="Menu"/>.
        /// </summary>
        /// <param name="item"></param>
        public void AddMenuItem(MenuItem item)
        {
            _MenuItems.Add(item);
            item.PositionOnScreen = item.Index;
            item.ParentMenu = this;
        }

        /// <summary>
        /// Removes the item at that index.
        /// </summary>
        /// <param name="itemIndex"></param>
        public void RemoveMenuItem(int itemIndex)
        {
            if (CurrentIndex >= itemIndex)
            {
                if (Size > CurrentIndex)
                {
                    CurrentIndex--;
                }
                else
                {
                    CurrentIndex = 0;
                }
            }
            if (itemIndex < Size && itemIndex > -1)
            {
                RemoveMenuItem(_MenuItems[itemIndex]);
                RemoveMenuItem(_MenuItems[itemIndex]);
            }
        }

        /// <summary>
        /// Removes the specified <see cref="MenuItem"/> from this <see cref="Menu"/>.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveMenuItem(MenuItem item)
        {
            if (_MenuItems.Contains(item))
            {
                if (CurrentIndex >= item.Index)
                {
                    if (Size > CurrentIndex)
                    {
                        CurrentIndex--;
                    }
                    else
                    {
                        CurrentIndex = 0;
                    }
                }
                _MenuItems.Remove(item);
            }
        }

        /// <summary>
        /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(int index)
        {
            if (index > -1 && _MenuItems.Count - 1 >= index)
            {
                SelectItem(_MenuItems[index]);
            }
        }

        /// <summary>
        /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(MenuItem item)
        {
            if (item != null && item.Enabled)
            {
                if (item is MenuCheckboxItem checkbox)
                {
                    checkbox.Checked = !checkbox.Checked;
                    CheckboxChangedEvent(checkbox, item.Index, checkbox.Checked);
                }
                else if (item is MenuListItem listItem)
                {
                    ListItemSelectEvent(this, listItem, listItem.ListIndex, listItem.Index);
                }
                else if (item is MenuSliderItem slider)
                {
                    SliderSelectedEvent(this, slider, slider.Position, slider.Index);
                }
                else
                {
                    ItemSelectedEvent(item, item.Index);
                }
                PlaySoundFrontend(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                if (MenuController.MenuButtons.ContainsKey(item))
                {
                    // this updates the parent menu.
                    MenuController.AddSubmenu(MenuController.GetCurrentMenu(), MenuController.MenuButtons[item]);

                    MenuController.GetCurrentMenu().CloseMenu();
                    MenuController.MenuButtons[item].OpenMenu();
                }
            }
            else if (item != null && !item.Enabled)
                PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }

        /// <summary>
        /// Returns to the parent menu. If there's no parent menu, then the current menu just closes.
        /// </summary>
        public void GoBack()
        {
            PlaySoundFrontend(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            CloseMenu();
            if (ParentMenu != null)
            {
                ParentMenu.OpenMenu();
            }
        }

        /// <summary>
        /// Closes the menu. Also triggers the <see cref="OnMenuClose"/> event.
        /// </summary>
        public void CloseMenu()
        {
            Visible = false;
            MenuCloseEvent(this);
        }

        /// <summary>
        /// Opens the menu and triggers the <see cref="OnMenuOpen"/> event.
        /// </summary>
        public void OpenMenu()
        {
            Visible = true;
            MenuOpenEvent(this);
        }

        /// <summary>
        /// Goes up one menu item if possible.
        /// </summary>
        public void GoUp()
        {
            if (Visible && Size > 1)
            {
                var oldItem = _MenuItems[CurrentIndex];
                CurrentIndex--; if (CurrentIndex < 0)
                {
                    CurrentIndex = Size - 1;
                }
                if (!VisibleMenuItems.Contains(_MenuItems[CurrentIndex]))
                {
                    ViewIndexOffset--;
                    if (ViewIndexOffset < 0)
                    {
                        ViewIndexOffset = Math.Max(Size - MaxItemsOnScreen, 0);
                    }
                }

                IndexChangeEvent(this, oldItem, _MenuItems[CurrentIndex], oldItem.Index, CurrentIndex);
                PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            }
        }

        /// <summary>
        /// Goes down one menu item if possible.
        /// </summary>
        public void GoDown()
        {
            if (Visible && Size > 1)
            {
                var oldItem = _MenuItems[CurrentIndex];
                CurrentIndex++;
                if (CurrentIndex >= Size)
                {
                    CurrentIndex = 0;
                }
                if (!VisibleMenuItems.Contains(_MenuItems[CurrentIndex]))
                {
                    ViewIndexOffset++;
                    if (CurrentIndex == 0)
                    {
                        ViewIndexOffset = 0;
                    }
                }
                IndexChangeEvent(this, oldItem, _MenuItems[CurrentIndex], oldItem.Index, CurrentIndex);
                PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            }
        }

        /// <summary>
        /// If the item is a <see cref="MenuListItem"/> or a <see cref="MenuSliderItem"/> then it'll go left if possible.
        /// </summary>
        public void GoLeft()
        {
            if (MenuController.AreMenuButtonsEnabled)
            {
                var item = _MenuItems.ElementAt(CurrentIndex);
                if (item.Enabled && item is MenuListItem listItem)
                {
                    if (listItem.ItemsCount > 0)
                    {
                        int oldIndex = listItem.ListIndex;
                        int newIndex = oldIndex;
                        if (listItem.ListIndex < 1)
                        {
                            newIndex = listItem.ItemsCount - 1;
                        }
                        else
                        {
                            newIndex--;
                        }
                        listItem.ListIndex = newIndex;
                        ListItemIndexChangeEvent(this, listItem, oldIndex, newIndex, listItem.Index);
                        PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
                else if (item.Enabled && item is MenuSliderItem slider)
                {
                    if (slider.Position > slider.Min)
                    {
                        SliderItemChangedEvent(this, slider, slider.Position, slider.Position - 1, slider.Index);
                        slider.Position--;
                        PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                    else
                    {
                        PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
                else if (item.Enabled && item is MenuDynamicListItem dynList)
                {
                    string newSelectedItem = dynList.Callback(dynList, true);
                    dynList.CurrentItem = newSelectedItem;
                    PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                }
            }
        }

        /// <summary>
        /// If the item is a <see cref="MenuListItem"/> or a <see cref="MenuSliderItem"/> then it'll go right if possible.
        /// </summary>
        public void GoRight()
        {
            if (MenuController.AreMenuButtonsEnabled)
            {
                var item = _MenuItems.ElementAt(CurrentIndex);
                if (item.Enabled && item is MenuListItem listItem)
                {
                    if (listItem.ItemsCount > 0)
                    {
                        int oldIndex = listItem.ListIndex;
                        int newIndex = oldIndex;
                        if (listItem.ListIndex >= listItem.ItemsCount - 1)
                        {
                            newIndex = 0;
                        }
                        else
                        {
                            newIndex++;
                        }
                        listItem.ListIndex = newIndex;
                        ListItemIndexChangeEvent(this, listItem, oldIndex, newIndex, listItem.Index);
                        PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
                else if (item.Enabled && item is MenuSliderItem slider)
                {
                    if (slider.Position < slider.Max)
                    {
                        SliderItemChangedEvent(this, slider, slider.Position, slider.Position + 1, slider.Index);
                        slider.Position++;
                        PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                    else
                    {
                        PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
                else if (item.Enabled && item is MenuDynamicListItem dynList)
                {
                    string newSelectedItem = dynList.Callback(dynList, false);
                    dynList.CurrentItem = newSelectedItem;
                    PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                }
            }
        }

        /// <summary>
        /// Allows you to sort the menu items using your own compare function.
        /// </summary>
        /// <param name="compare"></param>
        public void SortMenuItems(Comparison<MenuItem> compare)
        {
            _MenuItems.Sort(compare);
        }

        #endregion
        #region internal task functions
        /// <summary>
        /// Draws the menu title + subtitle, calls all Draw functions for all menu items and draws the description for the selected item.
        /// </summary>
        /// <returns></returns>
        internal async void Draw()
        {
            if (!Game.IsPaused && IsScreenFadedIn() && !IsPlayerSwitchInProgress() && !Game.PlayerPed.IsDead)
            {
                // impossible to reach this code, but i don't like Visual studio warnings.
                if (CurrentIndex == -2) { await BaseScript.Delay(0); }

                MenuItemsYOffset = 0f;

                #region Draw Header
                if (!string.IsNullOrEmpty(MenuTitle))
                {
                    #region Draw Header Background
                    SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    float x = (Position.X + (headerSize.Width / 2f)) / MenuController.ScreenWidth;
                    float y = (Position.Y + (headerSize.Height / 2f)) / MenuController.ScreenHeight;
                    float width = headerSize.Width / MenuController.ScreenWidth;
                    float height = headerSize.Height / MenuController.ScreenHeight;

                    if (!string.IsNullOrEmpty(HeaderTexture.Key) && !string.IsNullOrEmpty(HeaderTexture.Value))
                    {
                        if (!HasStreamedTextureDictLoaded(HeaderTexture.Key))
                        {
                            RequestStreamedTextureDict(HeaderTexture.Key, false);
                            while (!HasStreamedTextureDictLoaded(HeaderTexture.Key))
                            {
                                await BaseScript.Delay(0);
                            }
                        }
                        DrawSprite(HeaderTexture.Key, HeaderTexture.Value, x, y, width, height, 0f, 255, 255, 255, 255);
                    }
                    else
                    {
                        DrawSprite(MenuController._texture_dict, MenuController._header_texture, x, y, width, height, 0f, 255, 255, 255, 255);
                    }


                    ResetScriptGfxAlign();
                    #endregion

                    #region Draw Header Menu Title
                    int font = 1;
                    float size = (45f * 27f) / MenuController.ScreenHeight;
                    //float size = 1.1f;

                    SetScriptGfxAlign(76, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    BeginTextCommandDisplayText("STRING");
                    SetTextFont(font);
                    SetTextColour(255, 255, 255, 255);
                    SetTextScale(size, size);
                    SetTextJustification(0);
                    AddTextComponentSubstringPlayerName(MenuTitle);
                    if (LeftAligned)
                    {
                        EndTextCommandDisplayText(((headerSize.Width / 2f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f));
                    }
                    else
                    {
                        EndTextCommandDisplayText(GetSafeZoneSize() - ((headerSize.Width / 2f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f));
                    }

                    ResetScriptGfxAlign();

                    MenuItemsYOffset = headerSize.Height;
                    #endregion
                }
                #endregion

                #region Draw Subtitle
                {
                    #region draw subtitle background
                    SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    float bgHeight = 38f;


                    float x = (Position.X + (headerSize.Width / 2f)) / MenuController.ScreenWidth;
                    float y = ((Position.Y + MenuItemsYOffset + (bgHeight / 2f)) / MenuController.ScreenHeight);
                    float width = headerSize.Width / MenuController.ScreenWidth;
                    float height = bgHeight / MenuController.ScreenHeight;

                    DrawRect(x, y, width, height, 0, 0, 0, 250);
                    ResetScriptGfxAlign();
                    #endregion

                    #region draw subtitle text
                    if (!string.IsNullOrEmpty(MenuSubtitle))
                    {
                        int font = 0;
                        float size = (14f * 27f) / MenuController.ScreenHeight;
                        //float size = 0.34f;

                        SetScriptGfxAlign(76, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        BeginTextCommandDisplayText("STRING");
                        SetTextFont(font);
                        SetTextScale(size, size);
                        SetTextJustification(1);
                        AddTextComponentSubstringPlayerName("~HUD_COLOUR_HB_BLUE~" + MenuSubtitle.ToUpper());
                        if (LeftAligned)
                        {
                            EndTextCommandDisplayText(10f / MenuController.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)));
                        }
                        else
                        {
                            EndTextCommandDisplayText(GetSafeZoneSize() - ((headerSize.Width - 10f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)));
                        }

                        ResetScriptGfxAlign();
                    }
                    #endregion

                    #region draw counter + pre-counter text
                    string counterText = $"{CounterPreText ?? ""}{CurrentIndex + 1} / {Size}";
                    if (CounterPreText != null || MaxItemsOnScreen < Size)
                    {
                        int font = 0;
                        float size = (14f * 27f) / MenuController.ScreenHeight;
                        //float size = 0.34f;

                        SetScriptGfxAlign(76, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        BeginTextCommandDisplayText("STRING");
                        SetTextFont(font);
                        SetTextScale(size, size);
                        SetTextJustification(2);
                        AddTextComponentSubstringPlayerName("~HUD_COLOUR_HB_BLUE~" + counterText.ToUpper());
                        if (LeftAligned)
                        {
                            SetTextWrap(0f, (485f / MenuController.ScreenWidth));
                            EndTextCommandDisplayText(10f / MenuController.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)));
                        }
                        else
                        {
                            SetTextWrap(0f, GetSafeZoneSize() - (10f / MenuController.ScreenWidth));
                            EndTextCommandDisplayText(0f, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)));
                        }

                        ResetScriptGfxAlign();
                    }
                    if (!string.IsNullOrEmpty(MenuSubtitle) || (CounterPreText != null || MaxItemsOnScreen < Size))
                    {
                        MenuItemsYOffset += bgHeight - 1f;
                    }

                    #endregion
                }
                #endregion

                #region Draw menu items background gradient
                if (Size > 0)
                {
                    SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    float bgHeight = 38f * MathUtil.Clamp(_MenuItems.Count, 0, MaxItemsOnScreen);


                    float x = (Position.X + (headerSize.Width / 2f)) / MenuController.ScreenWidth;
                    float y = ((Position.Y + MenuItemsYOffset + ((bgHeight + 1f) / 2f)) / MenuController.ScreenHeight);
                    float width = headerSize.Width / MenuController.ScreenWidth;
                    float height = (bgHeight + 1f) / MenuController.ScreenHeight;

                    //DrawSprite(MenuController._texture_dict, "gradient_bgd", x, y, width, height, 0f, 255, 255, 255, 255);
                    DrawRect(x, y, width, height, 0, 0, 0, 180);
                    ResetScriptGfxAlign();
                    MenuItemsYOffset += bgHeight - 1f;
                }
                #endregion

                #region Draw menu items that are visible in the current view.
                if (Size > 0)
                {
                    foreach (var item in VisibleMenuItems)
                    {
                        item.Draw(ViewIndexOffset);
                    }
                }
                #endregion

                #region Up Down overflow Indicator
                float descriptionYOffset = 0f;
                if (Size > 0)
                {
                    if (Size > MaxItemsOnScreen)
                    {
                        #region background
                        float width = Width / MenuController.ScreenWidth;
                        float height = 60f / MenuController.ScreenWidth;
                        float x = (Position.X + (Width / 2f)) / MenuController.ScreenWidth;
                        float y = MenuItemsYOffset / MenuController.ScreenHeight + (height / 2f) + (6f / MenuController.ScreenHeight);

                        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        DrawRect(x, y, width, height, 0, 0, 0, 180);
                        descriptionYOffset = height;// + (1f / MenuController.ScreenHeight);
                        ResetScriptGfxAlign();
                        #endregion

                        #region up/down icons
                        SetScriptGfxAlign(76, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
                        float xMin = 0f;
                        float xMax = Width / MenuController.ScreenWidth;
                        float xCenter = 250f / MenuController.ScreenWidth;
                        float yTop = y - (20f / MenuController.ScreenHeight);
                        float yBottom = y - (10f / MenuController.ScreenHeight);

                        BeginTextCommandDisplayText("STRING");
                        AddTextComponentSubstringPlayerName("↑");

                        SetTextFont(0);
                        SetTextScale(1f, (14f * 27f) / MenuController.ScreenHeight);
                        //SetTextScale(0.35f, 0.35f);
                        SetTextJustification(0);
                        if (LeftAligned)
                        {
                            SetTextWrap(xMin, xMax);
                            EndTextCommandDisplayText(xCenter, yTop);
                        }
                        else
                        {
                            xMin = GetSafeZoneSize() - ((Width - 10f) / MenuController.ScreenWidth);
                            xMax = GetSafeZoneSize() - (10f / MenuController.ScreenWidth);
                            xCenter = GetSafeZoneSize() - (250f / MenuController.ScreenWidth);
                            SetTextWrap(xMin, xMax);
                            EndTextCommandDisplayText(xCenter, yTop);
                        }

                        BeginTextCommandDisplayText("STRING");
                        AddTextComponentSubstringPlayerName("↓");

                        SetTextFont(0);
                        SetTextScale(1f, (14f * 27f) / MenuController.ScreenHeight);
                        //SetTextScale(0.35f, 0.35f);
                        SetTextJustification(0);
                        if (LeftAligned)
                        {
                            SetTextWrap(xMin, xMax);
                            EndTextCommandDisplayText(xCenter, yBottom);
                        }
                        else
                        {
                            SetTextWrap(xMin, xMax);
                            EndTextCommandDisplayText(xCenter, yBottom);
                        }

                        ResetScriptGfxAlign();
                        #endregion
                    }
                }

                #endregion

                #region Draw Description
                if (Size > 0)
                {
                    if (!string.IsNullOrEmpty(_MenuItems[CurrentIndex].Description))
                    {
                        #region description text
                        int font = 0;
                        float textSize = (14f * 27f) / MenuController.ScreenHeight;
                        //float textSize = 0.35f;

                        float textMinX = 0f + (10f / MenuController.ScreenWidth);
                        float textMaxX = Width / MenuController.ScreenWidth - (10f / MenuController.ScreenWidth);
                        float textY = MenuItemsYOffset / MenuController.ScreenHeight + (16f / MenuController.ScreenHeight) + descriptionYOffset;

                        SetScriptGfxAlign(76, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        BeginTextCommandDisplayText("CELL_EMAIL_BCON");
                        SetTextFont(font);
                        SetTextScale(textSize, textSize);
                        SetTextJustification(1);
                        string text = _MenuItems[CurrentIndex].Description;
                        foreach (string s in CitizenFX.Core.UI.Screen.StringToArray(text))
                        {
                            AddTextComponentSubstringPlayerName(s);
                        }
                        float textHeight = GetTextScaleHeight(textSize, font);
                        if (LeftAligned)
                        {
                            SetTextWrap(textMinX, textMaxX);
                            EndTextCommandDisplayText(textMinX, textY);
                        }
                        else
                        {
                            textMinX = GetSafeZoneSize() - ((Width - 10f) / MenuController.ScreenWidth);
                            textMaxX = GetSafeZoneSize() - (10f / MenuController.ScreenWidth);
                            SetTextWrap(textMinX, textMaxX);
                            EndTextCommandDisplayText(textMinX, textY);
                        }

                        ResetScriptGfxAlign();

                        SetScriptGfxAlign(76, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        BeginTextCommandLineCount("CELL_EMAIL_BCON");
                        SetTextScale(textSize, textSize);
                        SetTextJustification(1);
                        SetTextFont(font);
                        int lineCount = 1;
                        foreach (string s in CitizenFX.Core.UI.Screen.StringToArray(text))
                        {
                            AddTextComponentSubstringPlayerName(s);
                        }
                        if (LeftAligned)
                        {
                            SetTextWrap(textMinX, textMaxX);
                            lineCount = GetTextScreenLineCount(textMinX, textY);
                        }
                        else
                        {
                            SetTextWrap(textMinX, textMaxX);
                            lineCount = GetTextScreenLineCount(textMinX, textY);
                        }

                        ResetScriptGfxAlign();

                        #endregion

                        #region background
                        float descWidth = Width / MenuController.ScreenWidth;
                        float descHeight = (textHeight + 0.005f) * lineCount + (8f / MenuController.ScreenHeight) + (2.5f / MenuController.ScreenHeight);
                        float descX = (Position.X + (Width / 2f)) / MenuController.ScreenWidth;
                        float descY = textY - (6f / MenuController.ScreenHeight) + (descHeight / 2f);

                        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        DrawRect(descX, descY - (descHeight / 2f) + (2f / MenuController.ScreenHeight), descWidth, 4f / MenuController.ScreenHeight, 0, 0, 0, 200);
                        DrawRect(descX, descY, descWidth, descHeight, 0, 0, 0, 180);

                        ResetScriptGfxAlign();
                        #endregion

                        descriptionYOffset += descY + (descHeight / 2f) - (4f / MenuController.ScreenHeight);
                    }
                    else
                    {
                        descriptionYOffset += MenuItemsYOffset / MenuController.ScreenHeight + (2f / MenuController.ScreenHeight) + descriptionYOffset;
                    }
                }

                #endregion

                #region Draw Color and opacity palletes
                if (Size > 0)
                {
                    var currentItem = _MenuItems[CurrentIndex];
                    if (currentItem is MenuListItem listItem)
                    {
                        /// OPACITY PANEL
                        if (listItem.ShowOpacityPanel)
                        {
                            PushScaleformMovieFunction(OpacityPanelScaleform, "SET_TITLE");
                            PushScaleformMovieMethodParameterString("Opacity");
                            PushScaleformMovieMethodParameterString("");
                            PushScaleformMovieMethodParameterInt(listItem.ListIndex * 10); // opacity percent
                            EndScaleformMovieMethod();

                            float width = Width / MenuController.ScreenWidth;
                            float height = ((700f / 500f) * Width) / MenuController.ScreenHeight;
                            float x = ((Width / 2f) / MenuController.ScreenWidth);
                            float y = descriptionYOffset + (height / 2f) + (4f / MenuController.ScreenHeight);
                            if (Size > MaxItemsOnScreen)
                            {
                                y -= (30f / MenuController.ScreenHeight);
                            }

                            SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
                            DrawScaleformMovie(OpacityPanelScaleform, x, y, width, height, 255, 255, 255, 255, 0);
                            ResetScriptGfxAlign();
                        }

                        /// COLOR PALLETE
                        else if (listItem.ShowColorPanel)
                        {
                            PushScaleformMovieFunction(ColorPanelScaleform, "SET_TITLE");
                            PushScaleformMovieMethodParameterString("Opacity");
                            BeginTextCommandScaleformString("FACE_COLOUR");
                            AddTextComponentInteger(listItem.ListIndex + 1);
                            AddTextComponentInteger(listItem.ItemsCount);
                            EndTextCommandScaleformString();
                            PushScaleformMovieMethodParameterInt(0); // opacity percent unused
                            PushScaleformMovieMethodParameterBool(true);
                            EndScaleformMovieMethod();

                            PushScaleformMovieFunction(ColorPanelScaleform, "SET_DATA_SLOT_EMPTY");
                            EndScaleformMovieMethod();

                            for (int i = 0; i < 64; i++)
                            {
                                var r = 0;
                                var g = 0;
                                var b = 0;
                                if (listItem.ColorPanelColorType == MenuListItem.ColorPanelType.Hair)
                                {
                                    GetHairRgbColor(i, ref r, ref g, ref b); // _GetHairRgbColor
                                }
                                else
                                {
                                    GetMakeupRgbColor(i, ref r, ref g, ref b); // _GetMakeupRgbColor
                                }

                                PushScaleformMovieFunction(ColorPanelScaleform, "SET_DATA_SLOT");
                                PushScaleformMovieMethodParameterInt(i); // index
                                PushScaleformMovieMethodParameterInt(r); // r
                                PushScaleformMovieMethodParameterInt(g); // g
                                PushScaleformMovieMethodParameterInt(b); // b
                                EndScaleformMovieMethod();
                            }

                            PushScaleformMovieFunction(ColorPanelScaleform, "DISPLAY_VIEW");
                            EndScaleformMovieMethod();

                            PushScaleformMovieFunction(ColorPanelScaleform, "SET_HIGHLIGHT");
                            PushScaleformMovieMethodParameterInt(listItem.ListIndex);
                            EndScaleformMovieMethod();

                            PushScaleformMovieFunction(ColorPanelScaleform, "SHOW_OPACITY");
                            PushScaleformMovieMethodParameterBool(false);
                            PushScaleformMovieMethodParameterBool(true);
                            EndScaleformMovieMethod();

                            float width = Width / MenuController.ScreenWidth;
                            float height = ((700f / 500f) * Width) / MenuController.ScreenHeight;
                            float x = ((Width / 2f) / MenuController.ScreenWidth);
                            float y = descriptionYOffset + (height / 2f) + (4f / MenuController.ScreenHeight);
                            if (Size > MaxItemsOnScreen)
                            {
                                y -= (30f / MenuController.ScreenHeight);
                            }

                            SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
                            DrawScaleformMovie(ColorPanelScaleform, x, y, width, height, 255, 255, 255, 255, 0);
                            ResetScriptGfxAlign();
                        }
                    }
                }

                #endregion
            }
        }
        #endregion


    }
}
