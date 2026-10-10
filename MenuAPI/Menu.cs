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

    #endregion

    #region private variables

    private int index = 0;

    private int viewIndexOffset = 0;

    private int pageIndex = 0;

    private bool visible = false;

    public int ViewIndexOffset
    {
        get => viewIndexOffset;
        private set
        {
            if (viewIndexOffset == value)
            {
                return;
            }

            viewIndexOffset = value;

            MenuNui.Invalidate();
        }
    }

    internal List<MenuItem> VisibleMenuItems
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
                var start = MenuMath.Clamp(PageIndex * PageSize, 0, source.Count);

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
    private void InvalidatePage()
    {
        pageItemsDirty = true;

        MenuNui.Invalidate();
    }
    #endregion

    #region Public Variables
    public string? MenuTitle
    {
        get => _menuTitle;
        set => MenuNui.Change(ref _menuTitle, value);
    }

    private string? _menuTitle;

    public string? MenuSubtitle
    {
        get => _menuSubtitle;
        set => MenuNui.Change(ref _menuSubtitle, value);
    }

    private string? _menuSubtitle;

    public KeyValuePair<string, string> HeaderTexture
    {
        get => _headerTexture;
        set => MenuNui.Change(ref _headerTexture, value);
    }

    private KeyValuePair<string, string> _headerTexture = new KeyValuePair<string, string>();

    /// <summary>
    /// Banner image from the resource's <c>menuapi-banners</c> folder, without the extension. A full
    /// <c>http</c>, <c>https</c> or <c>nui</c> url is taken as it is instead, so the image may live in
    /// another resource. Wins over <see cref="HeaderTexture"/>, falls back to it when missing.
    /// </summary>
    public string? HeaderImage
    {
        get => _headerImage;
        set => MenuNui.Change(ref _headerImage, value);
    }

    private string? _headerImage;

    #region Menu title styling
    // Nullable, and falling back to the matching MenuController.Default*, so a resource can style
    // every menu it owns by setting the defaults once and still override a single menu afterwards.
    // These only affect the title. The subtitle, the counter and the item rows are left alone.
    //
    // How big the title is drawn and where exactly it sits are deliberately not exposed. They are
    // per font values that were picked by eye to sit correctly on the banner, and a menu whose title
    // hangs off its own header helps nobody.

    /// <summary>The font the title is drawn in. See <see cref="MenuFont"/>.</summary>
    public int? MenuTitleFont
    {
        get => _menuTitleFont;
        set => MenuNui.Change(ref _menuTitleFont, value);
    }

    private int? _menuTitleFont;

    /// <summary>Where the title sits inside the header.</summary>
    public TitleAlignmentOption? MenuTitleAlignment
    {
        get => _menuTitleAlignment;
        set => MenuNui.Change(ref _menuTitleAlignment, value);
    }

    private TitleAlignmentOption? _menuTitleAlignment;

    public enum TitleAlignmentOption
    {
        Left,
        Center,
        Right
    }

    /// <summary>Whether GTA Online's moving header glow is drawn over the banner.</summary>
    public bool? ShowHeaderGlare
    {
        get => _showHeaderGlare;
        set => MenuNui.Change(ref _showHeaderGlare, value);
    }

    private bool? _showHeaderGlare;

    internal int ResolvedTitleFont => MenuTitleFont ?? MenuController.DefaultTitleFont;

    internal TitleAlignmentOption ResolvedTitleAlignment => MenuTitleAlignment ?? MenuController.DefaultTitleAlignment;

    internal bool ResolvedShowHeaderGlare => ShowHeaderGlare ?? MenuController.DefaultShowHeaderGlare;
    #endregion

    public bool IgnoreDontOpenMenus { get; set; } = false;

    public int MaxItemsOnScreen
    {
        get => _maxItemsOnScreen;
        internal set => MenuNui.Change(ref _maxItemsOnScreen, value);
    }

    private int _maxItemsOnScreen = 10;

    public int Size => ActiveItems.Count;

    /// <summary>How many items fit on one page. 0 means the menu is not paginated.</summary>
    public int PageSize { get; private set; } = 0;

    /// <summary>Whether this menu is split into pages.</summary>
    public bool Paginated => PageSize > 0;

    /// <summary>The page currently being shown, counting from 0.</summary>
    public int PageIndex
    {
        get => pageIndex;
        private set
        {
            if (pageIndex == value)
            {
                return;
            }

            pageIndex = value;

            MenuNui.Invalidate();
        }
    }

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

            MenuNui.Invalidate();

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

    public string? CounterPreText
    {
        get => _counterPreText;
        set => MenuNui.Change(ref _counterPreText, value);
    }

    private string? _counterPreText;

    public Menu? ParentMenu { get; internal set; } = null;

    public int CurrentIndex
    {
        get => index;
        internal set
        {
            var clamped = MenuMath.Clamp(value, 0, Math.Max(0, Size - 1));

            if (index == clamped)
            {
                return;
            }

            index = clamped;

            MenuNui.Invalidate();
        }
    }

    public bool EnableInstructionalButtons { get; set; } = true;

    /// <summary>
    /// Should contain 4 floats.
    /// </summary>
    public float[] WeaponStats
    {
        get => _weaponStats;
        private set => MenuNui.Change(ref _weaponStats, value);
    }

    private float[] _weaponStats = new float[4] { 0f, 0f, 0f, 0f };
    /// <summary>
    /// Should contain 4 floats.
    /// </summary>
    public float[] WeaponComponentStats
    {
        get => _weaponComponentStats;
        private set => MenuNui.Change(ref _weaponComponentStats, value);
    }

    private float[] _weaponComponentStats = new float[4] { 0f, 0f, 0f, 0f };
    /// <summary>
    /// Should contain 4 floats.
    /// </summary>
    public float[] VehicleStats
    {
        get => _vehicleStats;
        private set => MenuNui.Change(ref _vehicleStats, value);
    }

    private float[] _vehicleStats = new float[4] { 0f, 0f, 0f, 0f };
    /// <summary>
    /// Should contain 4 floats.
    /// </summary>
    public float[] VehicleUpgradeStats
    {
        get => _vehicleUpgradeStats;
        private set => MenuNui.Change(ref _vehicleUpgradeStats, value);
    }

    private float[] _vehicleUpgradeStats = new float[4] { 0f, 0f, 0f, 0f };

    public bool ShowWeaponStatsPanel
    {
        get => _showWeaponStatsPanel;
        set => MenuNui.Change(ref _showWeaponStatsPanel, value);
    }

    public bool ShowVehicleStatsPanel
    {
        get => _showVehicleStatsPanel;
        set => MenuNui.Change(ref _showVehicleStatsPanel, value);
    }

    private bool _showWeaponStatsPanel = false;
    private bool _showVehicleStatsPanel = false;

    internal string StatLabelKey(int index) =>
        ShowWeaponStatsPanel ? weaponStatNames[index] : vehicleStatNames[index];

    private readonly string[] weaponStatNames = new string[4] { "PM_DAMAGE", "PM_FIRERATE", "PM_ACCURACY", "PM_RANGE" };
    private readonly string[] vehicleStatNames = new string[4] { "FMMC_VEHST_0", "FMMC_VEHST_1", "FMMC_VEHST_2", "FMMC_VEHST_3" };

    private bool filterActive = false;

    public bool ShowSelectInstructionalButton { get; set; } = true;
    public bool ShowBackInstructionalButton { get; set; } = true;

    public string SelectButtonText { get; set; } = Native.GetLabelText("HUD_INPUT28");
    public string BackButtonText { get; set; } = Native.GetLabelText("HUD_INPUT53");

    /// <summary>Only ever drawn when the menu is paginated, whatever this is set to.</summary>
    public bool ShowPageInstructionalButtons { get; set; } = true;

    public string PreviousPageButtonText { get; set; } = "Previous page";
    public string NextPageButtonText { get; set; } = "Next page";

    /// <summary>Only drawn while the highlighted row is a list or a slider.</summary>
    public bool ShowChangeValueInstructionalButton { get; set; } = true;

    public string ChangeValueButtonText { get; set; } = "Change";

    public List<InstructionalButton> CustomInstructionalButtons = new();

    public struct InstructionalButton(string controlString, string instructionText)
    {
        public string controlString = controlString;
        public string instructionText = instructionText;
    }

    /// <summary>The keys this menu listens for. Add them with <see cref="AddKeyBinding"/>.</summary>
    public List<KeyBindingHandler> KeyBindingHandlers = new();

    /// <summary>One <see cref="MenuKeyBinding"/> and what this menu does with it.</summary>
    public sealed class KeyBindingHandler(
        MenuKeyBinding binding,
        MenuKeyPressType pressType,
        Action<Menu, MenuKeyBinding>? handler,
        string? buttonText)
    {
        /// <summary>The key itself, which the player can rebind.</summary>
        public MenuKeyBinding Binding { get; } = binding;

        public MenuKeyPressType PressType { get; set; } = pressType;

        public Action<Menu, MenuKeyBinding>? Handler { get; set; } = handler;

        /// <summary>The text next to its instructional button. Null or empty draws nothing.</summary>
        public string? ButtonText { get; set; } = buttonText;
    }

    /// <summary>Registers a rebindable key for this menu, and calls <paramref name="handler"/> while the menu is open.</summary>
    public KeyBindingHandler AddKeyBinding(
        string name,
        string description,
        string keyboardKey,
        string? controllerButton = null,
        MenuKeyPressType pressType = MenuKeyPressType.JUST_RELEASED,
        Action<Menu, MenuKeyBinding>? handler = null,
        string? buttonText = null)
    {
        MenuKeyBinding binding = MenuInput.RegisterCustom(name, description, keyboardKey, controllerButton);

        KeyBindingHandler entry = new(binding, pressType, handler, buttonText);

        KeyBindingHandlers.Add(entry);

        return entry;
    }

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
        MaxItemsOnScreen = MenuMath.Clamp(max, 3, 10);
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

        var target = MenuMath.Clamp(pageIndex, 0, PageCount - 1);

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

        Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
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

        var target = MenuMath.Clamp(PageIndex, 0, PageCount - 1);

        if (target == PageIndex)
        {
            return;
        }

        PageIndex = target;

        // The cached slice is of the page just left.
        InvalidatePage();
    }

    /// <summary>
    /// Resets the index to 0
    /// </summary>
    public void RefreshIndex() => RefreshIndex(0, 0);
    public void RefreshIndex(int index) => RefreshIndex(index, index >= MaxItemsOnScreen ? index - MaxItemsOnScreen + 1 : 0);
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

        KeyBindingHandlers.Clear();
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
    /// Inserts a <see cref="MenuItem"/> into this <see cref="Menu"/> at <paramref name="index"/>. The highlighted item stays highlighted, except in a paginated menu. While a filter is active the new item stays hidden until <see cref="FilterMenuItems"/> runs again.
    /// </summary>
    /// <param name="index">Position among all items, the ones a filter hides included. Clamped to the list.</param>
    /// <param name="item"></param>
    public void InsertMenuItem(int index, MenuItem item)
    {
        index = MenuMath.Clamp(index, 0, MenuItems.Count);

        var shiftCursor = !filterActive && !Paginated && MenuItems.Count > 0 && index <= CurrentIndex;

        MenuItems.Insert(index, item);
        item.ParentMenu = this;
        item.PositionOnScreen = item.Index;

        InvalidatePage();

        if (shiftCursor)
        {
            CurrentIndex++;
            KeepCursorInView();
        }
    }

    /// <summary>
    /// Moves a <see cref="MenuItem"/> that is already in this <see cref="Menu"/>, also within an active filter. A submenu button keeps the menu it opens. The highlighted item stays highlighted, except in a paginated menu.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index">Position among all items, the ones a filter hides included. Clamped to the list.</param>
    public void MoveMenuItem(MenuItem item, int index)
    {
        var from = MenuItems.IndexOf(item);

        if (from < 0)
        {
            return;
        }

        var current = Paginated ? null : GetCurrentMenuItem();

        MenuItems.RemoveAt(from);
        MenuItems.Insert(MenuMath.Clamp(index, 0, MenuItems.Count), item);

        if (filterActive && FilterItems.Remove(item))
        {
            var position = 0;

            foreach (var other in MenuItems)
            {
                if (ReferenceEquals(other, item))
                {
                    break;
                }

                if (position < FilterItems.Count && ReferenceEquals(FilterItems[position], other))
                {
                    position++;
                }
            }

            FilterItems.Insert(position, item);
        }

        InvalidatePage();

        if (current is not null)
        {
            CurrentIndex = SourceItems.IndexOf(current);
            KeepCursorInView();
        }
    }

    private void KeepCursorInView()
    {
        var max = MaxItemsOnScreen;
        var offset = Math.Min(ViewIndexOffset, Math.Max(Size - max, 0));

        ViewIndexOffset = MenuMath.Clamp(offset, Math.Max(CurrentIndex - max + 1, 0), CurrentIndex);
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

    private static bool IsWeaponWheelActive => Native.IsHudComponentActive(19);

    /// <summary>
    /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
    /// </summary>
    /// <param name="index"></param>
    public void SelectItem(int index)
    {
        if (IsWeaponWheelActive)
        {
            return;
        }

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
        if (item == null || IsWeaponWheelActive)
        {
            return;
        }
        // Before the Enabled branch, which would otherwise answer with the error sound. A separator
        // is a label, not a row that was taken away.
        if (item is SeparatorMenuItem)
        {
            return;
        }
        if (!item.Enabled)
        {
            Native.PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
        else
        {
            Native.PlaySoundFrontend(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            item.Select();

            MenuNui.Invalidate();
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
        if (IsWeaponWheelActive)
        {
            return;
        }

        Native.PlaySoundFrontend(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
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
        if (!Visible || Size < 2 || IsWeaponWheelActive)
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
        Native.PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    /// <summary>
    /// Goes down one menu item if possible.
    /// </summary>
    public void GoDown()
    {
        if (!Visible || Size < 2 || IsWeaponWheelActive)
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
        Native.PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    /// <summary>
    /// If the item is a <see cref="MenuListItem"/> or a <see cref="MenuSliderItem"/> then it'll go left if possible.
    /// </summary>
    public void GoLeft()
    {
        if (!MenuController.AreMenuButtonsEnabled || IsWeaponWheelActive)
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

            MenuNui.Invalidate();
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
        if (!MenuController.AreMenuButtonsEnabled || IsWeaponWheelActive)
        {
            return;
        }
        var item = GetCurrentMenuItem();

        if (IsPageNavigation(item))
        {
            NextPage();
            return;
        }

        if (item != null)
        {
            item.GoRight();

            MenuNui.Invalidate();
        }
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
            ClampStat(damage),
            ClampStat(fireRate),
            ClampStat(accuracy),
            ClampStat(range)
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
            ClampStat(WeaponStats[0] + damage),
            ClampStat(WeaponStats[1] + fireRate),
            ClampStat(WeaponStats[2] + accuracy),
            ClampStat(WeaponStats[3] + range)
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
            ClampStat(topSpeed),
            ClampStat(acceleration),
            ClampStat(braking),
            ClampStat(traction)
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
            ClampStat(VehicleStats[0] + topSpeed),
            ClampStat(VehicleStats[1] + acceleration),
            ClampStat(VehicleStats[2] + braking),
            ClampStat(VehicleStats[3] + traction)
        };
    }

    private static float ClampStat(float value) => MenuMath.IsFinite(value) ? MenuMath.Clamp(value, 0f, 1f) : 0f;
    #endregion

    #region internal/private task functions
    // Answers what MenuInput.DrainUnhandled needs to know: whether a press on this binding is going
    // to reach a handler, so everything else can be thrown away instead of firing later.
    internal bool Handles(MenuKeyBinding binding)
    {
        if (KeyBindingHandlers.Count == 0 || MenuController.DisableMenuButtons)
        {
            return false;
        }

        foreach (KeyBindingHandler entry in KeyBindingHandlers)
        {
            if (entry.Handler is not null && ReferenceEquals(entry.Binding, binding))
            {
                return true;
            }
        }

        return false;
    }

    internal void ProcessKeyBindings()
    {
        if (KeyBindingHandlers.Count == 0 || MenuController.DisableMenuButtons)
        {
            return;
        }

        for (var index = 0; index < KeyBindingHandlers.Count; index++)
        {
            var entry = KeyBindingHandlers[index];

            if (entry.Handler is null)
            {
                continue;
            }

            bool triggered = entry.PressType switch
            {
                MenuKeyPressType.JUST_PRESSED => entry.Binding.ConsumePress(),
                MenuKeyPressType.JUST_RELEASED => entry.Binding.ConsumeRelease(),
                MenuKeyPressType.PRESSED => entry.Binding.Held,
                MenuKeyPressType.RELEASED => !entry.Binding.Held,
                _ => false
            };

            if (triggered)
            {
                entry.Handler.Invoke(this, entry.Binding);
            }
        }
    }
    #endregion
    public static String[] SplitString(String inputString)
    {
        int stringsNeeded = (inputString.Length - 1) / 99 + 1; // division with round up

        String[] outputString = new String[stringsNeeded];
        for (int i = 0; i < stringsNeeded; i++)
        {
            outputString[i] = inputString.Substring(i * 99, MenuMath.Clamp(inputString.Length - (i * 99), 0, 99));
        }

        return outputString;
    }
}