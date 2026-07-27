
using CitizenFX.Core;
using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Client.Extensions;
using static CitizenFX.FiveM.Client.Native;

namespace MenuAPI
{
    public class MenuDynamicListItem : MenuItem
    {
        public bool HideArrowsWhenNotSelected { get; set; } = false;
        public string CurrentItem { get; set; } = null;

        public delegate string ChangeItemCallback(MenuDynamicListItem item, bool left);

        public ChangeItemCallback Callback { get; set; }

        public MenuDynamicListItem(string text, string initialValue, ChangeItemCallback callback) : this(text, initialValue, callback, null) { }
        public MenuDynamicListItem(string text, string initialValue, ChangeItemCallback callback, string description) : base(text, description)
        {
            CurrentItem = initialValue;
            Callback = callback;
        }

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
}
