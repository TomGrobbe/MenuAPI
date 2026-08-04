using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Client.Extensions;

using static CitizenFX.FiveM.Client.Native;

namespace MenuAPI;

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

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/>'s value was changed.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemCurrentItemChange"/> event occurred.</param>
    /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was changed.</param>
    /// <param name="oldValue">The old <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/>.</param>
    /// <param name="newValue">The new <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/>.</param>
    public delegate void DynamicListItemCurrentItemChangedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string oldValue, string newValue);

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemSelect"/> event occurred.</param>
    /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was selected.</param>
    /// <param name="currentItem">The <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/> in the <see cref="Menu"/>.</param>
    public delegate void DynamicListItemSelectedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string currentItem);
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

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/>'s value was changed.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> dynamicListItem, <see cref="MenuDynamicListItem.CurrentItem"/> oldValue, <see cref="MenuDynamicListItem.CurrentItem"/> newValue.
    /// </summary>
    public event DynamicListItemCurrentItemChangedEvent OnDynamicListItemCurrentItemChange;

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuDynamicListItem"/> dynamicListItem, <see cref="MenuDynamicListItem.CurrentItem"/> itemValue.
    /// </summary>
    public event DynamicListItemSelectedEvent OnDynamicListItemSelect;
    #endregion

    #region virtual voids
    /// <summary>
    /// Triggered when a <see cref="MenuItem"/> is selected.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this event occurred.</param>
    /// <param name="menuItem">The <see cref="MenuItem"/> that was selected.</param>
    /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuItem"/>.</param>
    internal virtual void ItemSelectedEvent(MenuItem menuItem, int itemIndex)
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
    internal virtual void CheckboxChangedEvent(MenuCheckboxItem menuItem, int itemIndex, bool _checked)
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
    internal virtual void ListItemSelectEvent(Menu menu, MenuListItem listItem, int selectedIndex, int itemIndex)
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
    internal virtual void ListItemIndexChangeEvent(Menu menu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)
    {
        OnListIndexChange?.Invoke(menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex);
    }

    /// <summary>
    /// Triggered when a <see cref="Menu"/> is closed.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> that was closed.</param>
    internal virtual void MenuCloseEvent(Menu menu)
    {
        OnMenuClose?.Invoke(menu);
    }

    /// <summary>
    /// Triggered when a <see cref="Menu"/> is opened.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> that has been opened.</param>
    internal virtual void MenuOpenEvent(Menu menu)
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
    internal virtual void IndexChangeEvent(Menu menu, MenuItem oldItem, MenuItem newItem, int oldIndex, int newIndex)
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
    internal virtual void SliderItemChangedEvent(Menu menu, MenuSliderItem sliderItem, int oldPosition, int newPosition, int itemIndex)
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
    internal virtual void SliderSelectedEvent(Menu menu, MenuSliderItem sliderItem, int sliderPosition, int itemIndex)
    {
        OnSliderItemSelect?.Invoke(menu, sliderItem, sliderPosition, itemIndex);
    }

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/>'s value was changed.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemCurrentItemChange"/> event occurred.</param>
    /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was changed.</param>
    /// <param name="oldValue">The old <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/>.</param>
    /// <param name="newValue">The new <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/>.</param>
    internal virtual void DynamicListItemCurrentItemChanged(Menu menu, MenuDynamicListItem dynamicListItem, string oldValue, string newValue)
    {
        OnDynamicListItemCurrentItemChange?.Invoke(menu, dynamicListItem, oldValue, newValue);
    }

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemSelect"/> event occurred.</param>
    /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was selected.</param>
    /// <param name="currentItem">The <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/> in the <see cref="Menu"/>.</param>
    internal virtual void DynamicListItemSelectEvent(Menu menu, MenuDynamicListItem dynamicListItem, string currentItem)
    {
        OnDynamicListItemSelect?.Invoke(menu, dynamicListItem, currentItem);
    }
    #endregion

    #endregion

    #region constants or readonlys
    public const float Width = 500f;
    #endregion

    #region private variables
    private static readonly KeyValuePair<float, float> headerSize = new(Width, 110f);

    private int index = 0;

    private bool visible = false;

    public int ViewIndexOffset { get; private set; } = 0;

    private List<MenuItem> VisibleMenuItems
    {
        get
        {
            // Create a duplicate list, just in case the original list is modified while we're looping through it.
            if (filterActive)
            {
                var items = FilterItems.ToList().GetRange(ViewIndexOffset, Math.Min(MaxItemsOnScreen, Size - ViewIndexOffset));
                return items;
            }
            else
            {
                var items = GetMenuItems().ToList().GetRange(ViewIndexOffset, Math.Min(MaxItemsOnScreen, Size - ViewIndexOffset));
                return items;
            }
        }
    }

    private List<MenuItem> FilterItems { get; set; } = new List<MenuItem>();
    private List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    private readonly int ColorPanelScaleform = RequestScaleformMovie("COLOUR_SWITCHER_02"); // Could probably be improved, but was getting some glitchy results if it wasn't pre-loaded.
    private readonly int OpacityPanelScaleform = RequestScaleformMovie("COLOUR_SWITCHER_01"); // Could probably be improved, but was getting some glitchy results if it wasn't pre-loaded.
    #endregion

    #region Public Variables
    public string MenuTitle { get; set; }

    public string MenuSubtitle { get; set; }

    public KeyValuePair<string, string> HeaderTexture { get; set; } = new KeyValuePair<string, string>();

    public bool IgnoreDontOpenMenus { get; set; } = false;

    public int MaxItemsOnScreen { get; internal set; } = 10;

    public int Size => filterActive ? FilterItems.Count : MenuItems.Count;

    public bool Visible
    {
        get
        {
            return visible;
        }
        set
        {
            if (value)
            {
                MenuController.VisibleMenus.Add(this);
            }
            else
            {
                MenuController.VisibleMenus.Remove(this);
            }
            visible = value;
        }
    }

    public bool LeftAligned => MenuController.MenuAlignment == MenuController.MenuAlignmentOption.Left;
    public KeyValuePair<float, float> Position { get; private set; } = new KeyValuePair<float, float>(0f, 0f);

    public float MenuItemsYOffset { get; private set; } = 0f;

    public string CounterPreText { get; set; }

    public Menu ParentMenu { get; internal set; } = null;

    public int CurrentIndex { get { return index; } internal set { index = Math.Clamp(value, 0, Math.Max(0, Size - 1)); } }

    public bool EnableInstructionalButtons { get; set; } = true;

    /// <summary>
    /// Should contain 4 floats.
    /// </summary>
    public float[] WeaponStats { get; private set; } = new float[4] { 0f, 0f, 0f, 0f };
    /// <summary>
    /// Should contain 4 floats.
    /// </summary>
    public float[] WeaponComponentStats { get; private set; } = new float[4] { 0f, 0f, 0f, 0f };
    /// <summary>
    /// Should contain 4 floats.
    /// </summary>
    public float[] VehicleStats { get; private set; } = new float[4] { 0f, 0f, 0f, 0f };
    /// <summary>
    /// Should contain 4 floats.
    /// </summary>
    public float[] VehicleUpgradeStats { get; private set; } = new float[4] { 0f, 0f, 0f, 0f };

    public bool ShowWeaponStatsPanel { get; set; } = false;
    public bool ShowVehicleStatsPanel { get; set; } = false;

    private readonly string[] weaponStatNames = new string[4] { "PM_DAMAGE", "PM_FIRERATE", "PM_ACCURACY", "PM_RANGE" };
    private readonly string[] vehicleStatNames = new string[4] { "FMMC_VEHST_0", "FMMC_VEHST_1", "FMMC_VEHST_2", "FMMC_VEHST_3" };

    private bool filterActive = false;

    public bool ShowSelectInstructionalButton { get; set; } = true;
    public bool ShowBackInstructionalButton { get; set; } = true;

    public string SelectButtonText { get; set; } = GetLabelText("HUD_INPUT28");
    public string BackButtonText { get; set; } = GetLabelText("HUD_INPUT53");

    // Select and back are not in here because on keyboard they are key mappings the player can rebind,
    // not a fixed Control. This dictionary is for whatever extra controls the resource wants to show.
    public Dictionary<Control, string> InstructionalButtons = new();

    public List<InstructionalButton> CustomInstructionalButtons = new();

    public struct InstructionalButton(string controlString, string instructionText)
    {
        public string controlString = controlString;
        public string instructionText = instructionText;
    }

    public enum ControlPressCheckType
    {
        JUST_RELEASED,
        JUST_PRESSED,
        RELEASED,
        PRESSED
    }

    public struct ButtonPressHandler(Control control, Menu.ControlPressCheckType pressType, Action<Menu, Control> function, bool disableControl)
    {
        // The control to listen for.
        internal Control control = control;
        // The type. 
        internal ControlPressCheckType pressType = pressType;
        // The function to call when the control is triggered.
        internal Action<Menu, Control> function = function;
        // Whether or not the control needs to be disabled if the menu is visible.
        internal bool disableControl = disableControl;
    }
    public List<ButtonPressHandler> ButtonPressHandlers = new();

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
        SetWeaponStats(0f, 0f, 0f, 0f);
        SetWeaponComponentStats(0f, 0f, 0f, 0f);
        SetVehicleStats(0f, 0f, 0f, 0f);
        SetVehicleUpgradeStats(0f, 0f, 0f, 0f);
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
        MaxItemsOnScreen = Math.Clamp(max, 3, 10);
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
        return filterActive ? FilterItems.ToList() : MenuItems.ToList();
    }

    /// <summary>
    /// Gets the currently selected (highlighted) menu item.
    /// </summary>
    /// <returns>Retuns the currently selected menu item. Or null if there are no menu items, or the current menu index is out of range.</returns>
    public MenuItem GetCurrentMenuItem()
    {
        return GetMenuItems().ElementAtOrDefault(CurrentIndex);
    }

    /// <summary>
    /// Removes all menu items.
    /// </summary>
    public void ClearMenuItems()
    {
        CurrentIndex = 0;
        ViewIndexOffset = 0;
        MenuItems.Clear();
        FilterItems.Clear();
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
        MenuItems.Clear();
        FilterItems.Clear();
    }

    /// <summary>
    /// Adds a <see cref="MenuItem"/> to this <see cref="Menu"/>.
    /// </summary>
    /// <param name="item"></param>
    public void AddMenuItem(MenuItem item)
    {
        MenuItems.Add(item);
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
            RemoveMenuItem(MenuItems[itemIndex]);
        }
    }

    /// <summary>
    /// Removes the specified <see cref="MenuItem"/> from this <see cref="Menu"/>.
    /// </summary>
    /// <param name="item"></param>
    public void RemoveMenuItem(MenuItem item)
    {
        if (!MenuItems.Contains(item))
        {
            return;
        }
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
        MenuItems.Remove(item);
    }

    /// <summary>
    /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
    /// </summary>
    /// <param name="index"></param>
    public void SelectItem(int index)
    {
        if (!filterActive)
        {
            if (index > -1 && MenuItems.Count - 1 >= index)
            {
                SelectItem(MenuItems[index]);
            }
        }
        else
        {
            if (index > -1 && FilterItems.Count - 1 >= index)
            {
                SelectItem(FilterItems[index]);
            }
        }
    }

    /// <summary>
    /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
    /// </summary>
    /// <param name="index"></param>
    public void SelectItem(MenuItem item)
    {
        if (item == null)
        {
            return;
        }
        if (!item.Enabled)
        {
            PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
        else
        {
            PlaySoundFrontend(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            item.Select();
            if (MenuController.MenuButtons.TryGetValue(item, out var value))
            {
                // this updates the parent menu.
                MenuController.AddSubmenu(MenuController.GetCurrentMenu(), value);
                MenuController.GetCurrentMenu().CloseMenu();
                value.OpenMenu();
            }
        }
    }

    /// <summary>
    /// Returns to the parent menu. If there's no parent menu, then the current menu just closes.
    /// </summary>
    public void GoBack()
    {
        PlaySoundFrontend(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        CloseMenu();
        ParentMenu?.OpenMenu();
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
        if (!Visible || Size < 2)
        {
            return;
        }
        MenuItem oldItem;

        if (filterActive)
        {
            oldItem = FilterItems[CurrentIndex];
        }
        else
        {
            oldItem = MenuItems[CurrentIndex];
        }

        if (CurrentIndex == 0)
        {
            CurrentIndex = Size - 1;
        }
        else
        {
            CurrentIndex--;
        }

        var currItem = GetCurrentMenuItem();

        if (currItem == null || !VisibleMenuItems.Contains(currItem))
        {
            ViewIndexOffset--;
            if (ViewIndexOffset < 0)
            {
                ViewIndexOffset = Math.Max(Size - MaxItemsOnScreen, 0);
            }
        }

        IndexChangeEvent(this, oldItem, currItem, oldItem.Index, CurrentIndex);
        PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    /// <summary>
    /// Goes down one menu item if possible.
    /// </summary>
    public void GoDown()
    {
        if (!Visible || Size < 2)
        {
            return;
        }

        MenuItem oldItem;

        if (filterActive)
        {
            oldItem = FilterItems[CurrentIndex];
        }
        else
        {
            oldItem = MenuItems[CurrentIndex];
        }

        if (CurrentIndex > 0 && CurrentIndex >= Size - 1)
        {
            CurrentIndex = 0;
        }
        else
        {
            CurrentIndex++;
        }

        var currItem = GetCurrentMenuItem();
        if (currItem == null || !VisibleMenuItems.Contains(currItem))
        {
            ViewIndexOffset++;
            if (CurrentIndex == 0)
            {
                ViewIndexOffset = 0;
            }
        }
        IndexChangeEvent(this, oldItem, currItem, oldItem.Index, CurrentIndex);
        PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    /// <summary>
    /// If the item is a <see cref="MenuListItem"/> or a <see cref="MenuSliderItem"/> then it'll go left if possible.
    /// </summary>
    public void GoLeft()
    {
        if (!MenuController.AreMenuButtonsEnabled)
        {
            return;
        }
        var item = GetCurrentMenuItem();
        if (item != null)
        {
            item.GoLeft();
        }
        // If the item is not any of the above, return to parent menu.
        else if (MenuController.NavigateMenuUsingArrows && !MenuController.DisableBackButton && !(MenuController.PreventExitingMenu && ParentMenu == null))
        {
            GoBack();
        }
    }

    /// <summary>
    /// If the item is a <see cref="MenuListItem"/> or a <see cref="MenuSliderItem"/> then it'll go right if possible.
    /// </summary>
    public void GoRight()
    {
        if (!MenuController.AreMenuButtonsEnabled)
        {
            return;
        }
        var item = GetCurrentMenuItem();
        item?.GoRight();
    }

    /// <summary>
    /// Sorts the menu items using the provided compare function.
    /// </summary>
    /// <param name="compare"></param>
    public void SortMenuItems(Comparison<MenuItem> compare)
    {
        if (filterActive)
        {
            filterActive = false;
            FilterItems.Clear();
        }
        MenuItems.Sort(compare);
    }

    /// <summary>
    /// Filters menu items using the provided filter function.
    /// </summary>
    /// <param name="predicate"></param>
    public void FilterMenuItems(Func<MenuItem, bool> predicate)
    {
        if (filterActive)
        {
            ResetFilter();
        }
        RefreshIndex(0, 0);
        ViewIndexOffset = 0;
        FilterItems = MenuItems.Where(i => predicate.Invoke(i)).ToList();
        filterActive = true;
    }

    /// <summary>
    /// Clears the current menu items filter for this menu.
    /// </summary>
    public void ResetFilter()
    {
        RefreshIndex(0, 0);
        filterActive = false;
        FilterItems.Clear();
    }

    /// <summary>
    /// Values should be between 0 and 1.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="fireRate"></param>
    /// <param name="accuracy"></param>
    /// <param name="range"></param>
    public void SetWeaponStats(float damage, float fireRate, float accuracy, float range)
    {
        WeaponStats = new float[4]
        {
            Math.Clamp(damage, 0f, 1f),
            Math.Clamp(fireRate, 0f, 1f),
            Math.Clamp(accuracy, 0f, 1f),
            Math.Clamp(range, 0f, 1f)
        };
    }

    /// <summary>
    /// Values should be between 0 and 1.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="fireRate"></param>
    /// <param name="accuracy"></param>
    /// <param name="range"></param>
    public void SetWeaponComponentStats(float damage, float fireRate, float accuracy, float range)
    {
        WeaponComponentStats = new float[4]
        {
            Math.Clamp(WeaponStats[0] + damage, 0f, 1f),
            Math.Clamp(WeaponStats[1] + fireRate, 0f, 1f),
            Math.Clamp(WeaponStats[2] + accuracy, 0f, 1f),
            Math.Clamp(WeaponStats[3] + range, 0f, 1f)
        };
    }

    /// <summary>
    /// Values should be between 0 and 1.
    /// </summary>
    /// <param name="topSpeed"></param>
    /// <param name="acceleration"></param>
    /// <param name="braking"></param>
    /// <param name="traction"></param>
    public void SetVehicleStats(float topSpeed, float acceleration, float braking, float traction)
    {
        VehicleStats = new float[4]
        {
            Math.Clamp(topSpeed, 0f, 1f),
            Math.Clamp(acceleration, 0f, 1f),
            Math.Clamp(braking, 0f, 1f),
            Math.Clamp(traction, 0f, 1f)
        };
    }

    /// <summary>
    /// Each upgrade value gets added on top of the already existing vehicle stats.
    /// So if the normal topspeed value is set to 0.5, and you provide 0.2 here, the total
    /// top speed value will be 0.7, where the last section (0.2) will be colored in blue.
    /// The bar can only show values between 0 and 1, so the total value will be clamped between 0 and 1.
    /// </summary>
    /// <param name="topSpeed"></param>
    /// <param name="acceleration"></param>
    /// <param name="braking"></param>
    /// <param name="traction"></param>
    public void SetVehicleUpgradeStats(float topSpeed, float acceleration, float braking, float traction)
    {
        VehicleUpgradeStats = new float[4]
        {
            Math.Clamp(VehicleStats[0] + topSpeed, 0f, 1f),
            Math.Clamp(VehicleStats[1] + acceleration, 0f, 1f),
            Math.Clamp(VehicleStats[2] + braking, 0f, 1f),
            Math.Clamp(VehicleStats[3] + traction, 0f, 1f)
        };
    }
    #endregion

    #region internal/private task functions
    /// <summary>
    /// Processes any custom button press handlers for this menu.
    /// </summary>
    private void ProcessButtonPressHandlers()
    {
        if (ButtonPressHandlers.Count != 0)
        {
            if (!MenuController.DisableMenuButtons)
            {
                foreach (ButtonPressHandler handler in ButtonPressHandlers)
                {
                    if (handler.disableControl)
                    {
                        DisableControlAction(0, (int)handler.control, false);
                    }

                    switch (handler.pressType)
                    {
                        case ControlPressCheckType.JUST_PRESSED:
                            if (IsControlJustPressed(0, (int)handler.control) || IsDisabledControlJustPressed(0, (int)handler.control))
                            {
                                handler.function.Invoke(this, handler.control);
                            }

                            break;
                        case ControlPressCheckType.JUST_RELEASED:
                            if (IsControlJustReleased(0, (int)handler.control) || IsDisabledControlJustReleased(0, (int)handler.control))
                            {
                                handler.function.Invoke(this, handler.control);
                            }

                            break;
                        case ControlPressCheckType.PRESSED:
                            if (IsControlPressed(0, (int)handler.control) || IsDisabledControlPressed(0, (int)handler.control))
                            {
                                handler.function.Invoke(this, handler.control);
                            }

                            break;
                        case ControlPressCheckType.RELEASED:
                            if (!IsControlPressed(0, (int)handler.control) && !IsDisabledControlPressed(0, (int)handler.control))
                            {
                                handler.function.Invoke(this, handler.control);
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draws the menu header.
    /// </summary>
    /// <param name="menuItemsOffset"></param>
    /// <returns>The new menuItemsOffset value</returns>
    private async Task<float> DrawHeader(float menuItemsOffset)
    {
        if (!string.IsNullOrEmpty(MenuTitle))
        {
            #region Draw Header Background
            SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            float x = (Position.Key + (headerSize.Key / 2f)) / MenuController.ScreenWidth;
            float y = (Position.Value + (headerSize.Value / 2f)) / MenuController.ScreenHeight;
            float width = headerSize.Key / MenuController.ScreenWidth;
            float height = headerSize.Value / MenuController.ScreenHeight;

            if (!string.IsNullOrEmpty(HeaderTexture.Key) && !string.IsNullOrEmpty(HeaderTexture.Value))
            {
                if (!HasStreamedTextureDictLoaded(HeaderTexture.Key))
                {
                    RequestStreamedTextureDict(HeaderTexture.Key, false);
                    while (!HasStreamedTextureDictLoaded(HeaderTexture.Key))
                    {
                        await API.Delay(0);
                    }
                }
                DrawSprite(HeaderTexture.Key, HeaderTexture.Value, x, y, width, height, 0f, 255, 255, 255, 255, false, false);
            }
            else
            {
                DrawSprite(MenuController._texture_dict, MenuController._header_texture, x, y, width, height, 0f, 255, 255, 255, 255, false, false);
            }

            ResetScriptGfxAlign();
            #endregion

            #region Draw Header Menu Title
            int font = 1;
            float size = (45f * 27f) / MenuController.ScreenHeight;
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
                EndTextCommandDisplayText(((headerSize.Key / 2f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f), 0);
            }
            else
            {
                EndTextCommandDisplayText(GetSafeZoneSize() - ((headerSize.Key / 2f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f), 0);
            }
            ResetScriptGfxAlign();
            menuItemsOffset = headerSize.Value;
            #endregion
        }
        else
        {
        }
        await Task.FromResult(0);
        return menuItemsOffset;
    }

    /// <summary>
    /// Draws the menu subtitle.
    /// </summary>
    /// <param name="menuItemsOffset"></param>
    /// <returns>The new menuItemsOffset value</returns>
    private float DrawSubtitle(float menuItemsOffset)
    {
        #region draw subtitle background
        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

        float bgHeight = 38f;

        float x = (Position.Key + (headerSize.Key / 2f)) / MenuController.ScreenWidth;
        float y = ((Position.Value + menuItemsOffset + (bgHeight / 2f)) / MenuController.ScreenHeight);
        float width = headerSize.Key / MenuController.ScreenWidth;
        float height = bgHeight / MenuController.ScreenHeight;

        DrawRect(x, y, width, height, 0, 0, 0, 250, false);
        ResetScriptGfxAlign();
        #endregion
        #region draw subtitle text
        if (!string.IsNullOrEmpty(MenuSubtitle))
        {
            int font = 0;
            float size = (14f * 27f) / MenuController.ScreenHeight;

            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            BeginTextCommandDisplayText("STRING");
            SetTextFont(font);
            SetTextScale(size, size);
            SetTextJustification(1);
            // Don't make the text blue if another color is used in the string.
            if (MenuSubtitle.Contains('~') || string.IsNullOrEmpty(MenuTitle))
            {
                AddTextComponentSubstringPlayerName(MenuSubtitle.ToUpper());
            }
            else
            {
                AddTextComponentSubstringPlayerName("~HUD_COLOUR_FREEMODE~" + MenuSubtitle.ToUpper());
            }

            if (LeftAligned)
            {
                EndTextCommandDisplayText(10f / MenuController.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)), 0);
            }
            else
            {
                EndTextCommandDisplayText(GetSafeZoneSize() - ((headerSize.Key - 10f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)), 0);
            }
            ResetScriptGfxAlign();
        }
        #endregion
        #region draw counter + pre-counter text
        string counterText = $"{CounterPreText ?? ""}{CurrentIndex + 1} / {Size}";
        if (!string.IsNullOrEmpty(CounterPreText) || MaxItemsOnScreen < Size)
        {
            int font = 0;
            float size = (14f * 27f) / MenuController.ScreenHeight;

            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            BeginTextCommandDisplayText("STRING");
            SetTextFont(font);
            SetTextScale(size, size);
            SetTextJustification(2);
            if ((MenuSubtitle ?? "").Contains('~') || (CounterPreText ?? "").Contains('~') || string.IsNullOrEmpty(MenuTitle))
            {
                AddTextComponentSubstringPlayerName(counterText.ToUpper());
            }
            else
            {
                AddTextComponentSubstringPlayerName("~HUD_COLOUR_FREEMODE~" + counterText.ToUpper());
            }
            if (LeftAligned)
            {
                SetTextWrap(0f, (485f / MenuController.ScreenWidth));
                EndTextCommandDisplayText(10f / MenuController.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)), 0);
            }
            else
            {
                SetTextWrap(0f, GetSafeZoneSize() - (10f / MenuController.ScreenWidth));
                EndTextCommandDisplayText(0f, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)), 0);
            }

            ResetScriptGfxAlign();
        }
        if (!string.IsNullOrEmpty(MenuSubtitle) || (CounterPreText != null || MaxItemsOnScreen < Size))
        {
            menuItemsOffset += bgHeight - 1f;
        }
        #endregion
        return menuItemsOffset;
    }

    /// <summary>
    /// Draws the background for all visible menu item's as one large rectangle.
    /// </summary>
    /// <param name="menuItemsOffset"></param>
    /// <returns>The new menuItemsOffset value</returns>
    private float DrawBackgroundGradient(float menuItemsOffset)
    {
        if (Size < 1)
        {
            return menuItemsOffset;
        }
        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
        float bgHeight = 38f * Math.Clamp(Size, 0, MaxItemsOnScreen);
        float x = (Position.Key + (headerSize.Key / 2f)) / MenuController.ScreenWidth;
        float y = ((Position.Value + menuItemsOffset + ((bgHeight + 1f) / 2f)) / MenuController.ScreenHeight);
        float width = headerSize.Key / MenuController.ScreenWidth;
        float height = (bgHeight + 1f) / MenuController.ScreenHeight;

        DrawRect(x, y, width, height, 0, 0, 0, 180, false);
        menuItemsOffset += bgHeight - 1f;
        ResetScriptGfxAlign();
        return menuItemsOffset;
    }

    /// <summary>
    /// Draw menu items that are visible in the current view.
    /// </summary>
    private void DrawActiveMenuItems()
    {
        if (Size < 1)
        {
            return;
        }
        foreach (var item in VisibleMenuItems)
        {
            item.Draw(ViewIndexOffset);
        }
    }

    /// <summary>
    /// Draws the up/down arrow indicators whenever the menu contains more items that are not visible in the current view.
    /// </summary>
    /// <returns></returns>
    private float DrawUpDownOverflowIndicators()
    {
        float descriptionYOffset = 0f;
        if (Size < 1 || Size <= MaxItemsOnScreen)
        {
            return descriptionYOffset;
        }
        #region background
        float width = Width / MenuController.ScreenWidth;
        float height = 60f / MenuController.ScreenWidth;
        float x = (Position.Key + (Width / 2f)) / MenuController.ScreenWidth;
        float y = (MenuItemsYOffset / MenuController.ScreenHeight) + (height / 2f) + (6f / MenuController.ScreenHeight);

        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

        DrawRect(x, y, width, height, 0, 0, 0, 180, false);
        descriptionYOffset = height;
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
        SetTextJustification(0);
        if (LeftAligned)
        {
            SetTextWrap(xMin, xMax);
            EndTextCommandDisplayText(xCenter, yTop, 0);
        }
        else
        {
            xMin = GetSafeZoneSize() - ((Width - 10f) / MenuController.ScreenWidth);
            xMax = GetSafeZoneSize() - (10f / MenuController.ScreenWidth);
            xCenter = GetSafeZoneSize() - (250f / MenuController.ScreenWidth);
            SetTextWrap(xMin, xMax);
            EndTextCommandDisplayText(xCenter, yTop, 0);
        }

        BeginTextCommandDisplayText("STRING");
        AddTextComponentSubstringPlayerName("↓");

        SetTextFont(0);
        SetTextScale(1f, (14f * 27f) / MenuController.ScreenHeight);
        SetTextJustification(0);
        if (LeftAligned)
        {
            SetTextWrap(xMin, xMax);
            EndTextCommandDisplayText(xCenter, yBottom, 0);
        }
        else
        {
            SetTextWrap(xMin, xMax);
            EndTextCommandDisplayText(xCenter, yBottom, 0);
        }

        ResetScriptGfxAlign();
        #endregion
        return descriptionYOffset;
    }
    /// <summary>
    /// Draws the menu item description.
    /// </summary>
    /// <param name="menuItemsOffset"></param>
    /// <param name="descriptionYOffset"></param>
    /// <returns>The new descriptionYOffset value</returns>
    private float DrawDescription(float menuItemsOffset, float descriptionYOffset)
    {
        if (Size < 1)
        {
            return descriptionYOffset;
        }
        var currentMenuItem = GetCurrentMenuItem();
        if (currentMenuItem != null && !string.IsNullOrEmpty(currentMenuItem.Description))
        {
            #region description text
            int font = 0;
            float textSize = (14f * 27f) / MenuController.ScreenHeight;

            float textMinX = 0f + (10f / MenuController.ScreenWidth);
            float textMaxX = Width / MenuController.ScreenWidth - (10f / MenuController.ScreenWidth);
            float textY = menuItemsOffset / MenuController.ScreenHeight + (16f / MenuController.ScreenHeight) + descriptionYOffset;
            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            BeginTextCommandDisplayText("CELL_EMAIL_BCON");
            SetTextFont(font);
            SetTextScale(textSize, textSize);
            SetTextJustification(1);
            string text = currentMenuItem.Description;
            foreach (string s in SplitString(text))
            {
                AddTextComponentSubstringPlayerName(s);
            }
            float textHeight = GetTextScaleHeight(textSize, font);
            if (LeftAligned)
            {
                SetTextWrap(textMinX, textMaxX);
                EndTextCommandDisplayText(textMinX, textY, 0);
            }
            else
            {
                textMinX = GetSafeZoneSize() - ((Width - 10f) / MenuController.ScreenWidth);
                textMaxX = GetSafeZoneSize() - (10f / MenuController.ScreenWidth);
                SetTextWrap(textMinX, textMaxX);
                EndTextCommandDisplayText(textMinX, textY, 0);
            }

            ResetScriptGfxAlign();

            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            BeginTextCommandLineCount("CELL_EMAIL_BCON");
            SetTextScale(textSize, textSize);
            SetTextJustification(1);
            SetTextFont(font);
            int lineCount;
            foreach (string s in SplitString(text))
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
            float descX = (Position.Key + (Width / 2f)) / MenuController.ScreenWidth;
            float descY = textY - (6f / MenuController.ScreenHeight) + (descHeight / 2f);

            SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            DrawRect(descX, descY - (descHeight / 2f) + (2f / MenuController.ScreenHeight), descWidth, 4f / MenuController.ScreenHeight, 0, 0, 0, 200, false);
            DrawRect(descX, descY, descWidth, descHeight, 0, 0, 0, 180, false);

            ResetScriptGfxAlign();
            #endregion

            descriptionYOffset += descY + (descHeight / 2f) - (4f / MenuController.ScreenHeight);
        }
        else
        {
            descriptionYOffset += menuItemsOffset / MenuController.ScreenHeight + (2f / MenuController.ScreenHeight) + descriptionYOffset;
        }
        return descriptionYOffset;
    }

    /// <summary>
    /// Draws the weapon or vehicle stats panel.
    /// </summary>
    /// <param name="descriptionYOffset"></param>
    private void DrawWeaponOrVehicleStatsPanel(float descriptionYOffset)
    {
        if (Size < 1)
        {
            return;
        }
        var currentItem = GetCurrentMenuItem();
        if (currentItem == null)
        {
            return;
        }
        if (currentItem is MenuListItem listItem)
        {
            if (listItem.ShowColorPanel || listItem.ShowOpacityPanel)
            {
                return;
            }
        }
        if (!ShowWeaponStatsPanel && !ShowVehicleStatsPanel)
        {
            return;
        }

        float textSize = (14f * 27f) / MenuController.ScreenHeight;
        float width = Width / MenuController.ScreenWidth;
        float height = (140f) / MenuController.ScreenHeight;
        float x = ((Width / 2f) / MenuController.ScreenWidth);
        float y = descriptionYOffset + (height / 2f) + (8f / MenuController.ScreenHeight);
        if (Size > MaxItemsOnScreen)
        {
            y -= (30f / MenuController.ScreenHeight);
        }

        #region background
        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
        DrawRect(x, y, width, height, 0, 0, 0, 180, false);
        ResetScriptGfxAlign();
        #endregion

        float bgStatBarWidth = (Width / 2f) / MenuController.ScreenWidth;
        float bgStatBarX = x + (bgStatBarWidth / 2f) - (10f / MenuController.ScreenWidth);

        if (!LeftAligned)
        {
            bgStatBarX = x - (bgStatBarWidth / 2f) - (10f / MenuController.ScreenWidth);
        }
        float barWidth;
        float componentBarWidth;
        float barY = y - (height / 2f) + (25f / MenuController.ScreenHeight);
        float bgStatBarHeight = 10f / MenuController.ScreenHeight;
        float barX;
        float componentBarX;

        for (int i = 0; i < 4; i++)
        {
            int[] color = new int[3] { 93, 182, 229 };
            barWidth = bgStatBarWidth * (ShowWeaponStatsPanel ? WeaponStats[i] : VehicleStats[i]);
            componentBarWidth = bgStatBarWidth * (ShowWeaponStatsPanel ? WeaponComponentStats[i] : VehicleUpgradeStats[i]);
            if (componentBarWidth < barWidth)
            {
                float diff = barWidth - componentBarWidth;
                barWidth -= diff;
                componentBarWidth += diff;
                color = new int[3] { 224, 50, 50 };
            }
            if (LeftAligned)
            {
                barX = bgStatBarX - (bgStatBarWidth / 2f) + (barWidth / 2f);
                componentBarX = bgStatBarX - (bgStatBarWidth / 2f) + (componentBarWidth / 2f);
            }
            else
            {
                barX = (barWidth * 1.5f) - bgStatBarWidth - (10f / MenuController.ScreenWidth);
                componentBarX = (componentBarWidth * 1.5f) - bgStatBarWidth - (10f / MenuController.ScreenWidth);
            }
            SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
            // bar bg
            DrawRect(bgStatBarX, barY, bgStatBarWidth, bgStatBarHeight, 100, 100, 100, 180, false);
            // component stats
            DrawRect(componentBarX, barY, componentBarWidth, bgStatBarHeight, color[0], color[1], color[2], 255, false);
            // real bar
            DrawRect(barX, barY, barWidth, bgStatBarHeight, 255, 255, 255, 255, false);
            ResetScriptGfxAlign();
            barY += 30f / MenuController.ScreenHeight;
        }

        #region weapon stats text
        float textX = LeftAligned ? x - (width / 2f) + (10f / MenuController.ScreenWidth) : GetSafeZoneSize() - ((Width - 10f) / MenuController.ScreenWidth);
        float textY = y - (height / 2f) + (10f / MenuController.ScreenHeight);

        for (int i = 0; i < 4; i++)
        {
            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
            BeginTextCommandDisplayText(ShowWeaponStatsPanel ? weaponStatNames[i] : vehicleStatNames[i]);
            SetTextJustification(1);
            SetTextScale(textSize, textSize);

            EndTextCommandDisplayText(textX, textY, 0);
            ResetScriptGfxAlign();
            textY += 30f / MenuController.ScreenHeight;
        }
        #endregion
    }

    /// <summary>
    /// Draws the Opacity and Color panels on MenuListItems.
    /// </summary>
    /// <param name="descriptionYOffset"></param>
    private void DrawColorAndOpacityPanel(float descriptionYOffset)
    {
        if (Size < 1)
        {
            return;
        }
        var currentItem = GetCurrentMenuItem();
        if (currentItem == null)
        {
            return;
        }
        if (currentItem is MenuListItem listItem)
        {
            // OPACITY PANEL
            if (listItem.ShowOpacityPanel)
            {
                BeginScaleformMovieMethod(OpacityPanelScaleform, "SET_TITLE");
                PushScaleformMovieMethodParameterString("Opacity");
                PushScaleformMovieMethodParameterString("");
                ScaleformMovieMethodAddParamInt(listItem.ListIndex * 10); // opacity percent
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

            // COLOR PALLETE
            else if (listItem.ShowColorPanel)
            {
                BeginScaleformMovieMethod(ColorPanelScaleform, "SET_TITLE");
                PushScaleformMovieMethodParameterString("Opacity");
                BeginTextCommandScaleformString("FACE_COLOUR");
                AddTextComponentInteger(listItem.ListIndex + 1);
                AddTextComponentInteger(listItem.ItemsCount);
                EndTextCommandScaleformString();
                ScaleformMovieMethodAddParamInt(0); // opacity percent unused
                ScaleformMovieMethodAddParamBool(true);
                EndScaleformMovieMethod();

                BeginScaleformMovieMethod(ColorPanelScaleform, "SET_DATA_SLOT_EMPTY");
                EndScaleformMovieMethod();

                for (int i = 0; i < 64; i++)
                {
                    int r;
                    int g;
                    int b;
                    if (listItem.ColorPanelColorType == MenuListItem.ColorPanelType.Hair)
                    {
                        GetHairRgbColor(i, out r, out g, out b); // _GetHairRgbColor
                    }
                    else
                    {
                        GetMakeupRgbColor(i, out r, out g, out b); // _GetMakeupRgbColor
                    }

                    BeginScaleformMovieMethod(ColorPanelScaleform, "SET_DATA_SLOT");
                    ScaleformMovieMethodAddParamInt(i); // index
                    ScaleformMovieMethodAddParamInt(r); // r
                    ScaleformMovieMethodAddParamInt(g); // g
                    ScaleformMovieMethodAddParamInt(b); // b
                    EndScaleformMovieMethod();
                }

                BeginScaleformMovieMethod(ColorPanelScaleform, "DISPLAY_VIEW");
                EndScaleformMovieMethod();

                BeginScaleformMovieMethod(ColorPanelScaleform, "SET_HIGHLIGHT");
                ScaleformMovieMethodAddParamInt(listItem.ListIndex);
                EndScaleformMovieMethod();

                BeginScaleformMovieMethod(ColorPanelScaleform, "SHOW_OPACITY");
                ScaleformMovieMethodAddParamBool(false);
                ScaleformMovieMethodAddParamBool(true);
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

    /// <summary>
    /// Calls all Draw functions for all visible menu components.
    /// </summary>
    internal async Task Draw()
    {
        if (!(
            IsScreenFadedIn() &&
            !IsPauseMenuActive() &&
            !API.Players.Local.IsDead &&
            !IsPlayerSwitchInProgress()
        ))
        {
            return;
        }
        ProcessButtonPressHandlers();

        MenuItemsYOffset = 0f;
        if (MenuController.SetDrawOrder)
        {
            SetScriptGfxDrawOrder(1);
        }

        MenuItemsYOffset = await DrawHeader(MenuItemsYOffset);

        MenuItemsYOffset = DrawSubtitle(MenuItemsYOffset);

        MenuItemsYOffset = DrawBackgroundGradient(MenuItemsYOffset);

        DrawActiveMenuItems();
        var descriptionYOffset = DrawUpDownOverflowIndicators();
        descriptionYOffset = DrawDescription(MenuItemsYOffset, descriptionYOffset);
        DrawWeaponOrVehicleStatsPanel(descriptionYOffset);
        DrawColorAndOpacityPanel(descriptionYOffset);
        if (MenuController.SetDrawOrder)
        {
            SetScriptGfxDrawOrder(0);
        }
    }
    #endregion
    public static String[] SplitString(String inputString)
    {
        int stringsNeeded = (inputString.Length - 1) / 99 + 1; // division with round up

        String[] outputString = new String[stringsNeeded];
        for (int i = 0; i < stringsNeeded; i++)
        {
            outputString[i] = inputString.Substring(i * 99, Math.Clamp(inputString[(i * 99)..].Length, 0, 99));
        }

        return outputString;
    }
}