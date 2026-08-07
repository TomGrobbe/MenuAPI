using static CitizenFX.FiveM.Client.Native;

namespace MenuAPI;

public class MenuListItem(string text, List<string> items, int index, string? description) : MenuItem(text, description)
{
    public int ListIndex { get; set; } = index;
    public List<string> ListItems { get; set; } = items;
    public bool HideArrowsWhenNotSelected { get; set; } = false;
    public bool ShowOpacityPanel { get; set; } = false;
    public bool ShowColorPanel { get; set; } = false;
    public ColorPanelType ColorPanelColorType = ColorPanelType.Hair;
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

    internal override void Draw(int indexOffset)
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

        base.Draw(indexOffset);
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

            PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
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

            PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
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