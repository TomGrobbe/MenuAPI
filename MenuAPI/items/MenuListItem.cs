namespace MenuAPI;

/// <summary>
/// An item that picks one value out of a list with left and right. The list wraps around at both ends.
/// Changing the value raises <see cref="Menu.OnListIndexChange"/>, pressing it raises <see cref="Menu.OnListItemSelect"/>.
/// </summary>
/// <param name="text">The text on the left side of the item.</param>
/// <param name="items">The values to pick from. Provide at least one. The list is copied, so change <see cref="ListItems"/> afterwards instead.</param>
/// <param name="index">The index selected at the start.</param>
/// <param name="description">Shown below the menu while the item is highlighted. Null for none.</param>
public class MenuListItem(string text, List<string> items, int index, string? description) : MenuItem(text, description)
{
    /// <summary>
    /// The index of the selected value in <see cref="ListItems"/>. An out of range index is wrapped back into range when the item is drawn.
    /// </summary>
    public int ListIndex
    {
        get => _listIndex;
        set => MenuNui.Change(ref _listIndex, value);
    }

    /// <summary>
    /// The values to pick from. Keep at least one, an empty list gets a single <c>N/A</c> value when the item is drawn.
    /// A <see cref="List{T}"/> of strings can be assigned to it, and setting null gives an empty list.
    /// </summary>
    public MenuItemList ListItems
    {
        get => _listItems;
        set => MenuNui.Change(ref _listItems, value ?? new MenuItemList());
    }

    /// <summary>
    /// Hides the left and right arrows while the item is not highlighted. False by default.
    /// </summary>
    public bool HideArrowsWhenNotSelected
    {
        get => _hideArrowsWhenNotSelected;
        set => MenuNui.Change(ref _hideArrowsWhenNotSelected, value);
    }

    /// <summary>
    /// Shows the opacity panel below the menu while the item is highlighted. The opacity shown is <see cref="ListIndex"/> times 10 percent,
    /// unless <see cref="ShowColorPanel"/> is on too, then it is <see cref="OpacityPercent"/>. False by default.
    /// </summary>
    public bool ShowOpacityPanel
    {
        get => _showOpacityPanel;
        set => MenuNui.Change(ref _showOpacityPanel, value);
    }

    /// <summary>
    /// Shows the color panel below the menu while the item is highlighted, with the swatch at <see cref="ListIndex"/> highlighted.
    /// The palette has 64 colors, so give the list 64 values. False by default.
    /// </summary>
    public bool ShowColorPanel
    {
        get => _showColorPanel;
        set => MenuNui.Change(ref _showColorPanel, value);
    }

    /// <summary>
    /// Which palette the color panel shows. Defaults to <see cref="ColorPanelType.Hair"/>.
    /// </summary>
    public ColorPanelType ColorPanelColorType
    {
        get => _colorPanelColorType;
        set => MenuNui.Change(ref _colorPanelColorType, value);
    }

    private int _listIndex = index;
    private MenuItemList _listItems = new(items);
    private bool _hideArrowsWhenNotSelected = false;
    private bool _showOpacityPanel = false;
    private bool _showColorPanel = false;
    private ColorPanelType _colorPanelColorType = ColorPanelType.Hair;

    private int opacityPercent;

    /// <summary>
    /// The opacity shown when both <see cref="ShowColorPanel"/> and <see cref="ShowOpacityPanel"/> are on. Clamped to 0 to 100.
    /// </summary>
    public int OpacityPercent
    {
        get => opacityPercent;
        set => MenuNui.Change(ref opacityPercent, MenuMath.Clamp(value, 0, 100));
    }

    internal int ResolvedOpacityPercent =>
        ShowColorPanel ? OpacityPercent : MenuMath.Clamp(ListIndex * 10, 0, 100);
    /// <summary>
    /// The palettes the color panel can show.
    /// </summary>
    public enum ColorPanelType
    {
        /// <summary>The game's 64 hair colors.</summary>
        Hair,
        /// <summary>The game's 64 makeup colors.</summary>
        Makeup
    }
    /// <summary>
    /// The number of values in <see cref="ListItems"/>.
    /// </summary>
    public int ItemsCount => ListItems.Count;

    /// <summary>
    /// Gets the selected value.
    /// </summary>
    /// <returns>The value at <see cref="ListIndex"/>, or null when the index is out of range.</returns>
    public string? GetCurrentSelection()
    {
        if (ItemsCount > 0 && ListIndex >= 0 && ListIndex < ItemsCount)
        {
            return ListItems[ListIndex];
        }
        return null;
    }

    /// <summary>
    /// Creates a <see cref="MenuListItem"/> without a description.
    /// </summary>
    /// <param name="text">The text on the left side of the item.</param>
    /// <param name="items">The values to pick from. Provide at least one. The list is copied.</param>
    /// <param name="index">The index selected at the start.</param>
    public MenuListItem(string text, List<string> items, int index) : this(text, items, index, null) { }

    internal override void PrepareForDisplay()
    {
        if (ItemsCount < 1)
        {
            // Add a dummy item to prevent the other while loops from freezing the game.
            ListItems.Add("N/A");
        }

        while (ListIndex < 0)
        {
            ListIndex += ItemsCount;
        }

        while (ListIndex >= ItemsCount)
        {
            ListIndex -= ItemsCount;
        }

        if (HideArrowsWhenNotSelected && !Selected)
        {
            Label = GetCurrentSelection() ?? "~r~N/A";
        }
        else
        {
            Label = $"~s~← {GetCurrentSelection() ?? "~r~N/A~s~"} ~s~→";
        }
    }

    internal override void GoRight()
    {
        if (ItemsCount > 0)
        {
            int oldIndex = ListIndex;
            int newIndex = oldIndex;
            if (ListIndex >= ItemsCount - 1)
            {
                newIndex = 0;
            }
            else
            {
                newIndex++;
            }
            ListIndex = newIndex;
            if (ParentMenu is Menu parent)
            {
                parent.ListItemIndexChangeEvent(parent, this, oldIndex, newIndex, Index);
            }

            Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
    }

    internal override void GoLeft()
    {
        if (ItemsCount > 0)
        {
            int oldIndex = ListIndex;
            int newIndex = oldIndex;
            if (ListIndex < 1)
            {
                newIndex = ItemsCount - 1;
            }
            else
            {
                newIndex--;
            }
            ListIndex = newIndex;

            if (ParentMenu is Menu parent)
            {
                parent.ListItemIndexChangeEvent(parent, this, oldIndex, newIndex, Index);
            }

            Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
    }

    internal override void Select()
    {
        if (ParentMenu is Menu parent)
        {
            parent.ListItemSelectEvent(parent, this, ListIndex, Index);
        }
    }
}