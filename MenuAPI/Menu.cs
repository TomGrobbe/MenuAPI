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
    public delegate void IndexChangedEvent(Menu menu, MenuItem oldItem, MenuItem? newItem, int oldIndex, int newIndex);

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
    public delegate void DynamicListItemCurrentItemChangedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string? oldValue, string newValue);

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemSelect"/> event occurred.</param>
    /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was selected.</param>
    /// <param name="currentItem">The <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/> in the <see cref="Menu"/>.</param>
    public delegate void DynamicListItemSelectedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string? currentItem);

    /// <summary>
    /// Triggered when the <see cref="PageIndex"/> of a paginated <see cref="Menu"/> changes.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnPageChange"/> event occurred.</param>
    /// <param name="oldPage">The <see cref="PageIndex"/> the menu was on.</param>
    /// <param name="newPage">The <see cref="PageIndex"/> the menu is now on.</param>
    /// <param name="wrapped">Whether the move rolled past the first or last page and came out the other end.</param>
    public delegate void PageChangedEvent(Menu menu, int oldPage, int newPage, bool wrapped);
    #endregion

    #region events
    /// <summary>
    /// Triggered when a <see cref="MenuItem"/> is selected.
    /// Parameters: <see cref="Menu"/> parentMenu, <see cref="MenuItem"/> menuItem, <see cref="int"/> itemIndex.
    /// </summary>
    public event ItemSelectEvent? OnItemSelect;

    /// <summary>
    /// Triggered when a <see cref="MenuCheckboxItem"/> was toggled.
    /// Parameters: <see cref="Menu"/> parentMenu, <see cref="MenuCheckboxItem"/> menuItem, <see cref="int"/> itemIndex, <see cref="bool"/> newCheckedState.
    /// </summary>
    public event CheckboxItemChangeEvent? OnCheckboxChange;

    /// <summary>
    /// Triggered when a <see cref="MenuListItem"/> is selected.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> listItem, <see cref="MenuListItem.ListIndex"/> selectedIndex, <see cref="int"/> itemIndex.
    /// </summary>
    public event ListItemSelectedEvent? OnListItemSelect;

    /// <summary>
    /// Triggered when a <see cref="MenuListItem"/>'s index was changed.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> listItem, <see cref="MenuListItem.ListIndex"/> oldSelectionIndex, <see cref="int"/> newSelectionIndex, <see cref="int"/> itemIndex.
    /// </summary>
    public event ListItemIndexChangedEvent? OnListIndexChange;

    /// <summary>
    /// Triggered when a <see cref="Menu"/> is closed.
    /// Parameters: <see cref="Menu"/> closedMenu.
    /// </summary>
    public event MenuClosedEvent? OnMenuClose;

    /// <summary>
    /// Triggered when a <see cref="Menu"/> is opened.
    /// Parameters: <see cref="Menu"/> openedMenu.
    /// </summary>
    public event MenuOpenedEvent? OnMenuOpen;

    /// <summary>
    /// Triggered when the <see cref="CurrentIndex"/> changes.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuItem"/> oldSelectedItem, <see cref="MenuItem"/> newSelectedItem, <see cref="int"/> oldIndex, <see cref="int"/> newIndex.
    /// </summary>
    public event IndexChangedEvent? OnIndexChange;
    /// <summary>
    /// Triggered when the <see cref="MenuSliderItem.Position"/> changes.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuSliderItem"/> sliderItem, <see cref="int"/> oldPosition, <see cref="int"/> newPosition, <see cref="int"/> itemIndex
    /// </summary>
    public event SliderPositionChangedEvent? OnSliderPositionChange;

    /// <summary>
    /// Triggered when a <see cref="MenuSliderItem"/> was selected.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuSliderItem"/> sliderItem, <see cref="int"/> sliderPosition, <see cref="int"/> itemIndex.
    /// </summary>
    public event SliderItemSelectedEvent? OnSliderItemSelect;

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/>'s value was changed.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> dynamicListItem, <see cref="MenuDynamicListItem.CurrentItem"/> oldValue, <see cref="MenuDynamicListItem.CurrentItem"/> newValue.
    /// </summary>
    public event DynamicListItemCurrentItemChangedEvent? OnDynamicListItemCurrentItemChange;

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
    /// Parameters: <see cref="Menu"/> menu, <see cref="MenuDynamicListItem"/> dynamicListItem, <see cref="MenuDynamicListItem.CurrentItem"/> itemValue.
    /// </summary>
    public event DynamicListItemSelectedEvent? OnDynamicListItemSelect;

    /// <summary>
    /// Triggered when the <see cref="PageIndex"/> of a paginated <see cref="Menu"/> changes.
    /// Parameters: <see cref="Menu"/> menu, <see cref="int"/> oldPage, <see cref="int"/> newPage, <see cref="bool"/> wrapped.
    /// </summary>
    public event PageChangedEvent? OnPageChange;
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
    internal virtual void IndexChangeEvent(Menu menu, MenuItem oldItem, MenuItem? newItem, int oldIndex, int newIndex)
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
    internal virtual void DynamicListItemCurrentItemChanged(Menu menu, MenuDynamicListItem dynamicListItem, string? oldValue, string newValue)
    {
        OnDynamicListItemCurrentItemChange?.Invoke(menu, dynamicListItem, oldValue, newValue);
    }

    /// <summary>
    /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemSelect"/> event occurred.</param>
    /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was selected.</param>
    /// <param name="currentItem">The <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/> in the <see cref="Menu"/>.</param>
    internal virtual void DynamicListItemSelectEvent(Menu menu, MenuDynamicListItem dynamicListItem, string? currentItem)
    {
        OnDynamicListItemSelect?.Invoke(menu, dynamicListItem, currentItem);
    }

    /// <summary>
    /// Triggered when the <see cref="PageIndex"/> of a paginated <see cref="Menu"/> changes.
    /// </summary>
    /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnPageChange"/> event occurred.</param>
    /// <param name="oldPage">The <see cref="PageIndex"/> the menu was on.</param>
    /// <param name="newPage">The <see cref="PageIndex"/> the menu is now on.</param>
    /// <param name="wrapped">Whether the move rolled past the first or last page and came out the other end.</param>
    internal virtual void PageChangeEvent(Menu menu, int oldPage, int newPage, bool wrapped)
    {
        OnPageChange?.Invoke(menu, oldPage, newPage, wrapped);
    }
    #endregion

    #endregion

    #region constants or readonlys
    public const float Width = 500f;

    // Plain consts rather than a KeyValuePair: reading a member off a static readonly *struct* field
    // takes its address, which the client's IL verifier rejects and the resource dies on the first
    // menu draw.
    private const float HeaderWidth = Width;
    private const float HeaderHeight = 110f;
    #endregion

    #region private variables

    private int index = 0;

    private bool visible = false;

    public int ViewIndexOffset { get; private set; } = 0;

    private List<MenuItem> VisibleMenuItems
    {
        get
        {
            // GetRange already copies, so the duplicate this needs comes for free. It used to copy
            // the whole list twice more on the way here, once per frame, for nothing.
            var source = ActiveItems;

            return source.GetRange(ViewIndexOffset, Math.Min(MaxItemsOnScreen, Size - ViewIndexOffset));
        }
    }

    /// <summary>Everything the filter left, pages included. What a page is then taken out of.</summary>
    private List<MenuItem> SourceItems => filterActive ? FilterItems : MenuItems;

    /// <summary>
    /// What the menu currently acts on: the filtered list, narrowed to the current page.
    /// </summary>
    // Without pagination this hands back the source list itself, so nothing allocates and every menu
    // that does not use pages behaves exactly as it did. With pagination the slice is cached, because
    // drawing reaches for it dozens of times per frame.
    private List<MenuItem> ActiveItems
    {
        get
        {
            if (!Paginated)
            {
                return SourceItems;
            }

            if (pageItemsDirty)
            {
                var source = SourceItems;
                var start = Math.Clamp(PageIndex * PageSize, 0, source.Count);

                pageItems = source.GetRange(start, Math.Min(PageSize, source.Count - start));
                pageItemsDirty = false;
            }

            return pageItems;
        }
    }

    /// <summary>Where <paramref name="item"/> sits in whichever list is currently active.</summary>
    // Same answer as looking it up in GetMenuItems(), without that method's defensive copy. Drawing
    // reads an item's index around nine times per item per frame, and those copies were the bulk of
    // everything MenuAPI allocated.
    internal int IndexOf(MenuItem item) => ActiveItems.IndexOf(item);

    private List<MenuItem> FilterItems { get; set; } = new List<MenuItem>();
    private List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    private List<MenuItem> pageItems = new List<MenuItem>();
    private bool pageItemsDirty = true;

    /// <summary>Anything that changes what belongs on the current page calls this.</summary>
    private void InvalidatePage() => pageItemsDirty = true;

    // What is currently loaded into ColorPanelScaleform, so the 64 swatches are only re-sent when
    // they would actually differ. See DrawColorAndOpacityPanel. Static because the scaleform is.
    private static MenuListItem? _paletteItem;
    private static MenuListItem.ColorPanelType _paletteType;

    // Static rather than one pair per menu. Only the current menu ever draws a panel, so a resource
    // with fifty menus was asking the game for a hundred scaleform movies and holding them all for
    // the lifetime of the resource. Still requested up front, because loading them on first use was
    // what gave the glitchy results the original comment here warned about.
    private static readonly int ColorPanelScaleform = RequestScaleformMovie("COLOUR_SWITCHER_02");
    private static readonly int OpacityPanelScaleform = RequestScaleformMovie("COLOUR_SWITCHER_01");
    #endregion

    #region Public Variables
    public string? MenuTitle { get; set; }

    public string? MenuSubtitle { get; set; }

    public KeyValuePair<string, string> HeaderTexture { get; set; } = new KeyValuePair<string, string>();

    public bool IgnoreDontOpenMenus { get; set; } = false;

    public int MaxItemsOnScreen { get; internal set; } = 10;

    public int Size => ActiveItems.Count;

    /// <summary>How many items fit on one page. 0 means the menu is not paginated.</summary>
    public int PageSize { get; private set; } = 0;

    /// <summary>Whether this menu is split into pages.</summary>
    public bool Paginated => PageSize > 0;

    /// <summary>The page currently being shown, counting from 0.</summary>
    public int PageIndex { get; private set; } = 0;

    /// <summary>Always at least 1, so an empty paginated menu still reads as "page 1 of 1".</summary>
    public int PageCount => Paginated ? Math.Max(1, (SourceItems.Count + PageSize - 1) / PageSize) : 1;

    /// <summary>Whether paging past the last page comes out at the first, and the other way around.</summary>
    public bool WrapPages { get; set; } = true;

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

            // The single funnel for OpenMenu, CloseMenu, GoBack and CloseAllMenus, so every one of
            // MenuAPI's ticks learns about a menu opening or closing from right here. Safe to call
            // from a setter: it only starts a loop, and a loop always waits a frame before its first
            // iteration, so nothing draws from inside this assignment.
            MenuTicks.Reevaluate();
        }
    }

    public bool LeftAligned => MenuController.MenuAlignment == MenuController.MenuAlignmentOption.Left;
    // Nothing has ever assigned this, so it has always been (0, 0), and every position calculation
    // used to add it anyway. The drawing code no longer reads it. Kept because it is public.
    public KeyValuePair<float, float> Position { get; private set; } = new KeyValuePair<float, float>(0f, 0f);

    public float MenuItemsYOffset { get; private set; } = 0f;

    public string? CounterPreText { get; set; }

    public Menu? ParentMenu { get; internal set; } = null;

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

    /// <summary>Only ever drawn when the menu is paginated, whatever this is set to.</summary>
    public bool ShowPageInstructionalButtons { get; set; } = true;

    public string PreviousPageButtonText { get; set; } = "Previous page";
    public string NextPageButtonText { get; set; } = "Next page";

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
    public Menu(string? name) : this(name, null) { }

    /// <summary>
    /// Creates a new <see cref="Menu"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="subtitle"></param>
    public Menu(string? name, string? subtitle)
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
    /// Splits this menu into pages of <paramref name="size"/> items, navigated with left and right.
    /// Pass 0 or less to turn pagination off again.
    /// </summary>
    /// <remarks>
    /// This sits on top of <see cref="MaxItemsOnScreen"/> rather than replacing it: a page of 48 items
    /// still scrolls ten rows at a time inside itself.
    /// </remarks>
    /// <param name="size">How many items belong on one page.</param>
    public void SetPageSize(int size)
    {
        PageSize = Math.Max(0, size);
        PageIndex = 0;

        InvalidatePage();
        RefreshIndex(0, 0);
    }

    /// <summary>
    /// Jumps to <paramref name="pageIndex"/>, counting from 0. Out of range values are clamped, so
    /// this never wraps. Returns whether the page actually changed.
    /// </summary>
    public bool GoToPage(int pageIndex) => GoToPage(pageIndex, false);

    /// <summary>
    /// Goes to the next page. Wraps around to the first page when <see cref="WrapPages"/> is set.
    /// Returns whether the page actually changed.
    /// </summary>
    public bool NextPage()
    {
        if (!Paginated)
        {
            return false;
        }

        var last = PageCount - 1;

        if (PageIndex < last)
        {
            return GoToPage(PageIndex + 1, false);
        }

        return WrapPages && GoToPage(0, true);
    }

    /// <summary>
    /// Goes to the previous page. Wraps around to the last page when <see cref="WrapPages"/> is set.
    /// Returns whether the page actually changed.
    /// </summary>
    public bool PreviousPage()
    {
        if (!Paginated)
        {
            return false;
        }

        if (PageIndex > 0)
        {
            return GoToPage(PageIndex - 1, false);
        }

        return WrapPages && GoToPage(PageCount - 1, true);
    }

    private bool GoToPage(int pageIndex, bool wrapped)
    {
        if (!Paginated)
        {
            return false;
        }

        var target = Math.Clamp(pageIndex, 0, PageCount - 1);

        if (target == PageIndex)
        {
            return false;
        }

        var oldPage = PageIndex;

        PageIndex = target;

        InvalidatePage();

        // A page is a fresh list, so the cursor starts at the top of it rather than wherever it sat
        // on the page before.
        RefreshIndex(0, 0);

        PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        PageChangeEvent(this, oldPage, PageIndex, wrapped);

        return true;
    }

    /// <summary>
    /// Puts <see cref="PageIndex"/> back in range after the item list changed underneath it.
    /// </summary>
    private void ClampPageIndex()
    {
        if (!Paginated)
        {
            return;
        }

        PageIndex = Math.Clamp(PageIndex, 0, PageCount - 1);
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
        return ActiveItems.ToList();
    }

    /// <summary>
    /// Gets the currently selected (highlighted) menu item.
    /// </summary>
    /// <returns>Retuns the currently selected menu item. Or null if there are no menu items, or the current menu index is out of range.</returns>
    public MenuItem? GetCurrentMenuItem()
    {
        // Straight off the active list rather than through GetMenuItems, which copies it.
        return ActiveItems.ElementAtOrDefault(CurrentIndex);
    }

    /// <summary>
    /// Removes all menu items.
    /// </summary>
    public void ClearMenuItems()
    {
        ClearMenuItems(false);
    }

    /// <summary>
    /// Removes all menu items.
    /// </summary>
    /// <remarks>
    /// A row that opened a menu takes that menu with it, because nothing can reach it once the row is
    /// gone. Rebind the row to keep it.
    /// </remarks>
    public void ClearMenuItems(bool dontResetIndex)
    {
        ClearMenuItems(dontResetIndex, null);
    }

    internal void ClearMenuItems(HashSet<Menu> removed)
    {
        ClearMenuItems(false, removed);
    }

    private void ClearMenuItems(bool dontResetIndex, HashSet<Menu>? removed)
    {
        if (!dontResetIndex)
        {
            CurrentIndex = 0;
            ViewIndexOffset = 0;
            PageIndex = 0;
        }

        // Over a snapshot, and only when some row somewhere opens a menu: unbinding cascades, and a
        // menu bound in a loop back to this one would otherwise empty this list while it is walked.
        foreach (var item in MenuController.HasBoundMenus ? MenuItems.ToArray() : [])
        {
            MenuController.Unbind(item, removed);
        }

        // The back reference is what would otherwise keep this whole menu alive off one stray row.
        foreach (var item in MenuItems)
        {
            item.ParentMenu = null;
        }

        MenuItems.Clear();
        FilterItems.Clear();

        // Holds a copy of the rows, so leaving it would keep every one of them alive until something
        // asked for a page again, which for a menu nobody opens is never.
        pageItems.Clear();

        // Without this an emptied menu would keep reporting a filter that no longer has anything to
        // say, and stay stuck at Size 0 even after it was refilled.
        filterActive = false;

        InvalidatePage();
    }

    /// <summary>Lets go of everything this menu holds. Only <see cref="MenuController.RemoveMenu"/> calls it.</summary>
    internal void Detach(HashSet<Menu> removed)
    {
        // The cached swatches belong to a row of this menu, which is about to go.
        if (ReferenceEquals(_paletteItem?.ParentMenu, this))
        {
            _paletteItem = null;
        }

        ClearMenuItems(removed);

        // A subscriber is usually the object that built the menu, and it keeps the menu alive through
        // the delegate for as long as it stays subscribed.
        OnItemSelect = null;
        OnCheckboxChange = null;
        OnListItemSelect = null;
        OnListIndexChange = null;
        OnMenuClose = null;
        OnMenuOpen = null;
        OnIndexChange = null;
        OnSliderPositionChange = null;
        OnSliderItemSelect = null;
        OnDynamicListItemCurrentItemChange = null;
        OnDynamicListItemSelect = null;
        OnPageChange = null;

        InstructionalButtons.Clear();
        CustomInstructionalButtons.Clear();

        ParentMenu = null;
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

        InvalidatePage();
    }

    /// <summary>
    /// Removes the item at that index.
    /// </summary>
    /// <param name="itemIndex">An index into the items currently on screen, page and filter applied.</param>
    public void RemoveMenuItem(int itemIndex)
    {
        // Read before anything moves, and out of the same list the index was counted in. This used to
        // compare against the filtered count while indexing the unfiltered list.
        var items = ActiveItems;

        if (itemIndex < 0 || itemIndex >= items.Count)
        {
            return;
        }

        RemoveMenuItem(items[itemIndex]);
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
        // Index is -1 for an item the filter or another page is holding back, and the cursor is not
        // sitting after something it cannot see.
        var index = item.Index;

        if (index > -1 && CurrentIndex >= index)
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

        // The filter keeps its own list, so leaving it in there would keep drawing an item the menu
        // no longer has.
        FilterItems.Remove(item);

        // Nothing can reach the menu this row opened now that the row is gone.
        MenuController.Unbind(item);

        item.ParentMenu = null;

        InvalidatePage();
        ClampPageIndex();
    }

    /// <summary>
    /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
    /// </summary>
    /// <param name="index"></param>
    public void SelectItem(int index)
    {
        var items = ActiveItems;

        if (index > -1 && items.Count - 1 >= index)
        {
            SelectItem(items[index]);
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
                // Read once. The old code asked for the current menu twice, so the second call could
                // in principle land after an event handler had already closed it.
                if (MenuController.GetCurrentMenu() is Menu currentMenu)
                {
                    // this updates the parent menu.
                    MenuController.AddSubmenu(currentMenu, value);
                    currentMenu.CloseMenu();
                }
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

        // Cheap insurance: the swatch cache assumes the scaleform still holds what it was given, so
        // a reopen starts from scratch rather than trusting it across a gap.
        _paletteItem = null;

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
        MenuItem oldItem = ActiveItems[CurrentIndex];

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

        MenuItem oldItem = ActiveItems[CurrentIndex];

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

        if (IsPageNavigation(item))
        {
            PreviousPage();
            return;
        }

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

        if (IsPageNavigation(item))
        {
            NextPage();
            return;
        }

        item?.GoRight();
    }

    /// <summary>
    /// Whether left and right should turn the page rather than act on <paramref name="item"/>.
    /// </summary>
    // Items that hold a value keep their arrows, because there is nothing else those arrows could
    // mean on them. Everything else in a paginated menu pages instead of going back or selecting.
    internal bool IsPageNavigation(MenuItem? item) =>
        Paginated && item is not (MenuListItem or MenuSliderItem or MenuDynamicListItem);

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

        InvalidatePage();
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

        // Filtering changes how many items there are, so the page they were split into changes too.
        PageIndex = 0;
        InvalidatePage();
    }

    /// <summary>
    /// Clears the current menu items filter for this menu.
    /// </summary>
    public void ResetFilter()
    {
        RefreshIndex(0, 0);
        filterActive = false;
        FilterItems.Clear();

        PageIndex = 0;
        InvalidatePage();
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

            float x = MenuLayout.RowCenterX;
            float y = MenuLayout.HeaderHeightN / 2f;
            float width = HeaderWidth / MenuLayout.ScreenWidth;
            float height = HeaderHeight / MenuLayout.ScreenHeight;

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
            float size = (45f * 27f) / MenuLayout.ScreenHeight;
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
                EndTextCommandDisplayText(((HeaderWidth / 2f) / MenuLayout.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f), 0);
            }
            else
            {
                EndTextCommandDisplayText(MenuLayout.RightHeaderCenterX, y - (GetTextScaleHeight(size, font) / 2f), 0);
            }
            ResetScriptGfxAlign();
            menuItemsOffset = HeaderHeight;
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

        float x = MenuLayout.RowCenterX;
        float y = ((menuItemsOffset + (bgHeight / 2f)) / MenuLayout.ScreenHeight);
        float width = HeaderWidth / MenuLayout.ScreenWidth;
        float height = bgHeight / MenuLayout.ScreenHeight;

        DrawRect(x, y, width, height, 0, 0, 0, 250, false);
        ResetScriptGfxAlign();
        #endregion
        #region draw subtitle text
        if (!string.IsNullOrEmpty(MenuSubtitle))
        {
            int font = 0;
            float size = MenuLayout.ItemTextSize;

            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            BeginTextCommandDisplayText("STRING");
            SetTextFont(font);
            SetTextScale(size, size);
            SetTextJustification(1);
            // Don't make the text blue if another color is used in the string.
            if (MenuSubtitle.Contains('~') || string.IsNullOrEmpty(MenuTitle))
            {
                AddTextComponentSubstringPlayerName(UpperCase(MenuSubtitle));
            }
            else
            {
                AddTextComponentSubstringPlayerName("~HUD_COLOUR_FREEMODE~" + UpperCase(MenuSubtitle));
            }

            if (LeftAligned)
            {
                EndTextCommandDisplayText(10f / MenuLayout.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuLayout.ScreenHeight)), 0);
            }
            else
            {
                EndTextCommandDisplayText(MenuLayout.RightHeaderTextX, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuLayout.ScreenHeight)), 0);
            }
            ResetScriptGfxAlign();
        }
        #endregion
        #region draw counter + pre-counter text
        string counterText = $"{CounterPreText ?? ""}{CurrentIndex + 1} / {Size}";
        if (!string.IsNullOrEmpty(CounterPreText) || MaxItemsOnScreen < Size)
        {
            int font = 0;
            float size = MenuLayout.ItemTextSize;

            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            BeginTextCommandDisplayText("STRING");
            SetTextFont(font);
            SetTextScale(size, size);
            SetTextJustification(2);
            if ((MenuSubtitle ?? "").Contains('~') || (CounterPreText ?? "").Contains('~') || string.IsNullOrEmpty(MenuTitle))
            {
                AddTextComponentSubstringPlayerName(UpperCase(counterText));
            }
            else
            {
                AddTextComponentSubstringPlayerName("~HUD_COLOUR_FREEMODE~" + UpperCase(counterText));
            }
            if (LeftAligned)
            {
                SetTextWrap(0f, (485f / MenuLayout.ScreenWidth));
                EndTextCommandDisplayText(10f / MenuLayout.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuLayout.ScreenHeight)), 0);
            }
            else
            {
                SetTextWrap(0f, MenuLayout.RightTextMaxX);
                EndTextCommandDisplayText(0f, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuLayout.ScreenHeight)), 0);
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
    /// Uppercases display text, leaving whatever sits between a pair of tildes alone.
    /// </summary>
    /// <remarks>
    /// The subtitle and the counter are drawn in capitals. The game's formatting tokens are lowercase
    /// (<c>~r~</c>, <c>~s~</c>, ...), so uppercasing the whole string turns them into text the game
    /// does not recognise and the colour silently goes missing.
    /// </remarks>
    private static string UpperCase(string text)
    {
        if (!text.Contains('~'))
        {
            return text.ToUpper();
        }

        var characters = text.ToCharArray();
        var insideToken = false;

        for (var i = 0; i < characters.Length; i++)
        {
            if (characters[i] == '~')
            {
                insideToken = !insideToken;
                continue;
            }

            if (!insideToken)
            {
                characters[i] = char.ToUpperInvariant(characters[i]);
            }
        }

        return new string(characters);
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
        float x = MenuLayout.RowCenterX;
        float y = ((menuItemsOffset + ((bgHeight + 1f) / 2f)) / MenuLayout.ScreenHeight);
        float width = HeaderWidth / MenuLayout.ScreenWidth;
        float height = (bgHeight + 1f) / MenuLayout.ScreenHeight;

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
        float width = Width / MenuLayout.ScreenWidth;
        float height = 60f / MenuLayout.ScreenWidth;
        float x = MenuLayout.RowCenterX;
        float y = (MenuItemsYOffset / MenuLayout.ScreenHeight) + (height / 2f) + (6f / MenuLayout.ScreenHeight);

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
        float xMax = Width / MenuLayout.ScreenWidth;
        float xCenter = 250f / MenuLayout.ScreenWidth;
        float yTop = y - (20f / MenuLayout.ScreenHeight);
        float yBottom = y - (10f / MenuLayout.ScreenHeight);

        BeginTextCommandDisplayText("STRING");
        AddTextComponentSubstringPlayerName("↑");

        SetTextFont(0);
        SetTextScale(1f, MenuLayout.ItemTextSize);
        SetTextJustification(0);
        if (LeftAligned)
        {
            SetTextWrap(xMin, xMax);
            EndTextCommandDisplayText(xCenter, yTop, 0);
        }
        else
        {
            xMin = MenuLayout.RightTextMinX;
            xMax = MenuLayout.RightTextMaxX;
            xCenter = MenuLayout.RightDescriptionCenterX;
            SetTextWrap(xMin, xMax);
            EndTextCommandDisplayText(xCenter, yTop, 0);
        }

        BeginTextCommandDisplayText("STRING");
        AddTextComponentSubstringPlayerName("↓");

        SetTextFont(0);
        SetTextScale(1f, MenuLayout.ItemTextSize);
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
            float textSize = MenuLayout.ItemTextSize;

            float textMinX = 0f + (10f / MenuLayout.ScreenWidth);
            float textMaxX = Width / MenuLayout.ScreenWidth - (10f / MenuLayout.ScreenWidth);
            float textY = menuItemsOffset / MenuLayout.ScreenHeight + (16f / MenuLayout.ScreenHeight) + descriptionYOffset;
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
                textMinX = MenuLayout.RightTextMinX;
                textMaxX = MenuLayout.RightTextMaxX;
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
            float descWidth = Width / MenuLayout.ScreenWidth;
            float descHeight = (textHeight + 0.005f) * lineCount + (8f / MenuLayout.ScreenHeight) + (2.5f / MenuLayout.ScreenHeight);
            float descX = MenuLayout.RowCenterX;
            float descY = textY - (6f / MenuLayout.ScreenHeight) + (descHeight / 2f);

            SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            DrawRect(descX, descY - (descHeight / 2f) + (2f / MenuLayout.ScreenHeight), descWidth, 4f / MenuLayout.ScreenHeight, 0, 0, 0, 200, false);
            DrawRect(descX, descY, descWidth, descHeight, 0, 0, 0, 180, false);

            ResetScriptGfxAlign();
            #endregion

            descriptionYOffset += descY + (descHeight / 2f) - (4f / MenuLayout.ScreenHeight);
        }
        else
        {
            descriptionYOffset += menuItemsOffset / MenuLayout.ScreenHeight + (2f / MenuLayout.ScreenHeight) + descriptionYOffset;
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

        float textSize = MenuLayout.ItemTextSize;
        float width = Width / MenuLayout.ScreenWidth;
        float height = (140f) / MenuLayout.ScreenHeight;
        float x = ((Width / 2f) / MenuLayout.ScreenWidth);
        float y = descriptionYOffset + (height / 2f) + (8f / MenuLayout.ScreenHeight);
        if (Size > MaxItemsOnScreen)
        {
            y -= (30f / MenuLayout.ScreenHeight);
        }

        #region background
        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
        DrawRect(x, y, width, height, 0, 0, 0, 180, false);
        ResetScriptGfxAlign();
        #endregion

        float bgStatBarWidth = (Width / 2f) / MenuLayout.ScreenWidth;
        float bgStatBarX = x + (bgStatBarWidth / 2f) - (10f / MenuLayout.ScreenWidth);

        if (!LeftAligned)
        {
            bgStatBarX = x - (bgStatBarWidth / 2f) - (10f / MenuLayout.ScreenWidth);
        }
        float barWidth;
        float componentBarWidth;
        float barY = y - (height / 2f) + (25f / MenuLayout.ScreenHeight);
        float bgStatBarHeight = 10f / MenuLayout.ScreenHeight;
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
                barX = (barWidth * 1.5f) - bgStatBarWidth - (10f / MenuLayout.ScreenWidth);
                componentBarX = (componentBarWidth * 1.5f) - bgStatBarWidth - (10f / MenuLayout.ScreenWidth);
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
            barY += 30f / MenuLayout.ScreenHeight;
        }

        #region weapon stats text
        float textX = LeftAligned ? x - (width / 2f) + (10f / MenuLayout.ScreenWidth) : MenuLayout.RightTextMinX;
        float textY = y - (height / 2f) + (10f / MenuLayout.ScreenHeight);

        for (int i = 0; i < 4; i++)
        {
            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
            BeginTextCommandDisplayText(ShowWeaponStatsPanel ? weaponStatNames[i] : vehicleStatNames[i]);
            SetTextJustification(1);
            SetTextScale(textSize, textSize);

            EndTextCommandDisplayText(textX, textY, 0);
            ResetScriptGfxAlign();
            textY += 30f / MenuLayout.ScreenHeight;
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

                float width = Width / MenuLayout.ScreenWidth;
                float height = ((700f / 500f) * Width) / MenuLayout.ScreenHeight;
                float x = ((Width / 2f) / MenuLayout.ScreenWidth);
                float y = descriptionYOffset + (height / 2f) + (4f / MenuLayout.ScreenHeight);
                if (Size > MaxItemsOnScreen)
                {
                    y -= (30f / MenuLayout.ScreenHeight);
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

                // The swatches themselves only change when the palette does, but filling them costs
                // 64 colour lookups and 64 scaleform calls. The scaleform keeps what it was given,
                // so this only re-sends them when the row or the palette type actually changed.
                // ReferenceEquals rather than !=, matching how the rest of this codebase compares
                // menu objects on a client that cannot load the default equality comparers.
                if (!ReferenceEquals(_paletteItem, listItem) || _paletteType != listItem.ColorPanelColorType)
                {
                    _paletteItem = listItem;
                    _paletteType = listItem.ColorPanelColorType;

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
                }

                BeginScaleformMovieMethod(ColorPanelScaleform, "SET_HIGHLIGHT");
                ScaleformMovieMethodAddParamInt(listItem.ListIndex);
                EndScaleformMovieMethod();

                BeginScaleformMovieMethod(ColorPanelScaleform, "SHOW_OPACITY");
                ScaleformMovieMethodAddParamBool(false);
                ScaleformMovieMethodAddParamBool(true);
                EndScaleformMovieMethod();

                float width = Width / MenuLayout.ScreenWidth;
                float height = ((700f / 500f) * Width) / MenuLayout.ScreenHeight;
                float x = ((Width / 2f) / MenuLayout.ScreenWidth);
                float y = descriptionYOffset + (height / 2f) + (4f / MenuLayout.ScreenHeight);
                if (Size > MaxItemsOnScreen)
                {
                    y -= (30f / MenuLayout.ScreenHeight);
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
        // The pause menu, screen fade, death and player switch checks that used to be here are the
        // same four MenuController.ProcessMenus makes immediately before calling this, so they were
        // eight natives a frame answering a question that had just been answered.
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