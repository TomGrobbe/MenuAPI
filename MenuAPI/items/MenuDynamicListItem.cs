using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

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

    }
}
