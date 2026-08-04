using static CitizenFX.FiveM.Client.Native;

namespace MenuAPI;

public class MenuDynamicListItem(string text, string initialValue, MenuDynamicListItem.ChangeItemCallback callback, string description) : MenuItem(text, description)
{
    public bool HideArrowsWhenNotSelected { get; set; } = false;
    public string CurrentItem { get; set; } = initialValue;

    public delegate string ChangeItemCallback(MenuDynamicListItem item, bool left);

    public ChangeItemCallback Callback { get; set; } = callback;

    public MenuDynamicListItem(string text, string initialValue, ChangeItemCallback callback) : this(text, initialValue, callback, null) { }

    internal override void Draw(int indexOffset)
    {
        if (HideArrowsWhenNotSelected && !Selected)
        {
            Label = CurrentItem ?? "~r~N/A";
        }
        else
        {
            Label = $"~s~← {CurrentItem ?? "~r~N/A~s~"} ~s~→";
        }
        base.Draw(indexOffset);
    }

    internal override void GoRight()
    {
        string oldValue = CurrentItem;
        string newSelectedItem = Callback(this, false);
        CurrentItem = newSelectedItem;
        ParentMenu.DynamicListItemCurrentItemChanged(ParentMenu, this, oldValue, newSelectedItem);
        PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    internal override void GoLeft()
    {
        string oldValue = CurrentItem;
        string newSelectedItem = Callback(this, true);
        CurrentItem = newSelectedItem;
        ParentMenu.DynamicListItemCurrentItemChanged(ParentMenu, this, oldValue, newSelectedItem);
        PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    internal override void Select()
    {
        ParentMenu.DynamicListItemSelectEvent(ParentMenu, this, CurrentItem);
    }
}