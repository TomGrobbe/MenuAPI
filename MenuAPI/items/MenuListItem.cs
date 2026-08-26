namespace MenuAPI;

public class MenuListItem(string text, List<string> items, int index, string? description) : MenuItem(text, description)
{
    public int ListIndex
    {
        get => _listIndex;
        set => MenuNui.Change(ref _listIndex, value);
    }

    public MenuItemList ListItems
    {
        get => _listItems;
        set => MenuNui.Change(ref _listItems, value ?? new MenuItemList());
    }

    public bool HideArrowsWhenNotSelected
    {
        get => _hideArrowsWhenNotSelected;
        set => MenuNui.Change(ref _hideArrowsWhenNotSelected, value);
    }

    public bool ShowOpacityPanel
    {
        get => _showOpacityPanel;
        set => MenuNui.Change(ref _showOpacityPanel, value);
    }

    public bool ShowColorPanel
    {
        get => _showColorPanel;
        set => MenuNui.Change(ref _showColorPanel, value);
    }

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

    public int OpacityPercent
    {
        get => opacityPercent;
        set => MenuNui.Change(ref opacityPercent, Math.Clamp(value, 0, 100));
    }

    internal int ResolvedOpacityPercent =>
        ShowColorPanel ? OpacityPercent : Math.Clamp(ListIndex * 10, 0, 100);
    public enum ColorPanelType
    {
        Hair,
        Makeup
    }
    public int ItemsCount => ListItems.Count;

    public string? GetCurrentSelection()
    {
        if (ItemsCount > 0 && ListIndex >= 0 && ListIndex < ItemsCount)
        {
            return ListItems[ListIndex];
        }
        return null;
    }

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