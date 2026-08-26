using CitizenFX.FiveM.Client;

namespace MenuAPI;

public class MenuDynamicListItem(string text, string? initialValue, MenuDynamicListItem.ChangeItemCallback callback, string? description) : MenuItem(text, description)
{
    public bool HideArrowsWhenNotSelected
    {
        get => _hideArrowsWhenNotSelected;
        set => MenuNui.Change(ref _hideArrowsWhenNotSelected, value);
    }

    public string? CurrentItem
    {
        get => _currentItem;
        set => MenuNui.Change(ref _currentItem, value);
    }

    private bool _hideArrowsWhenNotSelected = false;
    private string? _currentItem = initialValue;

    public delegate string ChangeItemCallback(MenuDynamicListItem item, bool left);

    public ChangeItemCallback Callback { get; set; } = callback;

    public MenuDynamicListItem(string text, string? initialValue, ChangeItemCallback callback) : this(text, initialValue, callback, null) { }

    internal override void PrepareForDisplay()
    {
        if (HideArrowsWhenNotSelected && !Selected)
        {
            Label = CurrentItem ?? "~r~N/A";
        }
        else
        {
            Label = $"~s~← {CurrentItem ?? "~r~N/A~s~"} ~s~→";
        }
    }

    internal override void GoRight()
    {
        string? oldValue = CurrentItem;
        string newSelectedItem = Callback(this, false);
        CurrentItem = newSelectedItem;

        if (ParentMenu is Menu parent)
        {
            parent.DynamicListItemCurrentItemChanged(parent, this, oldValue, newSelectedItem);
        }

        Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    internal override void GoLeft()
    {
        string? oldValue = CurrentItem;
        string newSelectedItem = Callback(this, true);
        CurrentItem = newSelectedItem;

        if (ParentMenu is Menu parent)
        {
            parent.DynamicListItemCurrentItemChanged(parent, this, oldValue, newSelectedItem);
        }

        Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    internal override void Select()
    {
        if (ParentMenu is Menu parent)
        {
            parent.DynamicListItemSelectEvent(parent, this, CurrentItem);
        }
    }
}
