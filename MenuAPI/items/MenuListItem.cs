using System.Collections.Generic;
using static CitizenFX.Core.Native.Function;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    public class MenuListItem : MenuItem
    {
        public int ListIndex { get; set; } = 0;
        public List<string> ListItems { get; set; } = new List<string>();
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

        public string GetCurrentSelection()
        {
            if (ItemsCount > 0 && ListIndex >= 0 && ListIndex < ItemsCount)
            {
                return ListItems[ListIndex];
            }
            return null;
        }

        public MenuListItem(string text, List<string> items, int index) : this(text, items, index, null) { }
        public MenuListItem(string text, List<string> items, int index, string description) : base(text, description)
        {
            ListItems = items;
            ListIndex = index;
        }

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
                ParentMenu.ListItemIndexChangeEvent(ParentMenu, this, oldIndex, newIndex, Index);
#if FIVEM
                PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
#endif
#if REDM
                // Has invalid parameter types in API.
                Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_RIGHT", "HUD_SHOP_SOUNDSET", 1);
#endif
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

                ParentMenu.ListItemIndexChangeEvent(ParentMenu, this, oldIndex, newIndex, Index);
#if FIVEM
                PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
#endif
#if REDM
                // Has invalid parameter types in API.
                Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_LEFT", "HUD_SHOP_SOUNDSET", 1);
#endif
            }
        }

        internal override void Select()
        {
            ParentMenu.ListItemSelectEvent(ParentMenu, this, ListIndex, Index);
        }
    }
}
