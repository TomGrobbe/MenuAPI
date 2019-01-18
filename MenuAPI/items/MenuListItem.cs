using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
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

    }
}
