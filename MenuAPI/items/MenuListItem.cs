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
            Label = $"~s~← {GetCurrentSelection() ?? ""} ~s~→";
            base.Draw(indexOffset);
        }

    }
}
