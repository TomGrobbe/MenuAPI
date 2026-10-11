using System.Collections.Generic;
using static CitizenFX.Core.Native.Function;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    /// <summary>
    /// A menu item that lets the user scroll through a list of strings with left and right.
    /// </summary>
    public class MenuListItem : MenuItem
    {
        /// <summary>
        /// The index of the selected entry in <see cref="ListItems"/>. Wraps around when it goes out of range.
        /// </summary>
        public int ListIndex { get; set; } = 0;
        /// <summary>
        /// The strings the user can scroll through. When empty, an N/A entry is added so the item can still be drawn.
        /// </summary>
        public List<string> ListItems { get; set; } = new List<string>();
        /// <summary>
        /// Hides the left and right arrows while this item is not highlighted. Defaults to false.
        /// </summary>
        public bool HideArrowsWhenNotSelected { get; set; } = false;
        /// <summary>
        /// Has no effect in RedM, the opacity panel is never drawn.
        /// </summary>
        public bool ShowOpacityPanel { get; set; } = false;
        /// <summary>
        /// Has no effect in RedM, the color panel is never drawn.
        /// </summary>
        public bool ShowColorPanel { get; set; } = false;
        /// <summary>
        /// Has no effect in RedM, the color panel is never drawn.
        /// </summary>
        public ColorPanelType ColorPanelColorType = ColorPanelType.Hair;
        /// <summary>
        /// The color palettes of the color panel. Kept for FiveM compatibility, RedM does not draw the panel.
        /// </summary>
        public enum ColorPanelType
        {
            /// <summary>
            /// The hair color palette.
            /// </summary>
            Hair,
            /// <summary>
            /// The makeup color palette.
            /// </summary>
            Makeup
        }
        /// <summary>
        /// The number of entries in <see cref="ListItems"/>.
        /// </summary>
        public int ItemsCount => ListItems.Count;

        /// <summary>
        /// Returns the selected entry of <see cref="ListItems"/>, or null when <see cref="ListIndex"/> is out of range.
        /// </summary>
        public string GetCurrentSelection()
        {
            if (ItemsCount > 0 && ListIndex >= 0 && ListIndex < ItemsCount)
            {
                return ListItems[ListIndex];
            }
            return null;
        }

        /// <summary>
        /// Creates a new <see cref="MenuListItem"/>.
        /// </summary>
        /// <param name="text">The text on the left side of the item.</param>
        /// <param name="items">The strings the user can scroll through.</param>
        /// <param name="index">The index of the entry selected at the start.</param>
        public MenuListItem(string text, List<string> items, int index) : this(text, items, index, null) { }
        /// <summary>
        /// Creates a new <see cref="MenuListItem"/> with a description.
        /// </summary>
        /// <param name="text">The text on the left side of the item.</param>
        /// <param name="items">The strings the user can scroll through.</param>
        /// <param name="index">The index of the entry selected at the start.</param>
        /// <param name="description">The text shown below the menu while this item is highlighted.</param>
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
                // Has invalid parameter types in API.
                Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_RIGHT", "HUD_SHOP_SOUNDSET", 1);
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
                // Has invalid parameter types in API.
                Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_LEFT", "HUD_SHOP_SOUNDSET", 1);
            }
        }

        internal override void Select()
        {
            ParentMenu.ListItemSelectEvent(ParentMenu, this, ListIndex, Index);
        }
    }
}
