using static CitizenFX.Core.Native.Function;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    /// <summary>
    /// A list item without a fixed list. Every press of left or right calls <see cref="Callback"/>, which returns the next value to show.
    /// </summary>
    public class MenuDynamicListItem : MenuItem
    {
        /// <summary>
        /// Hides the left and right arrows while this item is not highlighted. Defaults to false.
        /// </summary>
        public bool HideArrowsWhenNotSelected { get; set; } = false;
        /// <summary>
        /// The value currently shown on the item. Null shows N/A.
        /// </summary>
        public string CurrentItem { get; set; } = null;

        /// <summary>
        /// Called when left or right is pressed on a <see cref="MenuDynamicListItem"/>. The returned string becomes the new <see cref="CurrentItem"/>.
        /// </summary>
        /// <param name="item">The item the user is changing.</param>
        /// <param name="left">True when left was pressed, false when right was pressed.</param>
        /// <returns>The new value to show.</returns>
        public delegate string ChangeItemCallback(MenuDynamicListItem item, bool left);

        /// <summary>
        /// The function that returns the next value. It is called without a null check, so it must be set.
        /// </summary>
        public ChangeItemCallback Callback { get; set; }

        /// <summary>
        /// Creates a new <see cref="MenuDynamicListItem"/>.
        /// </summary>
        /// <param name="text">The text on the left side of the item.</param>
        /// <param name="initialValue">The value shown before the user changes it.</param>
        /// <param name="callback">The function that returns the next value.</param>
        public MenuDynamicListItem(string text, string initialValue, ChangeItemCallback callback) : this(text, initialValue, callback, null) { }
        /// <summary>
        /// Creates a new <see cref="MenuDynamicListItem"/> with a description.
        /// </summary>
        /// <param name="text">The text on the left side of the item.</param>
        /// <param name="initialValue">The value shown before the user changes it.</param>
        /// <param name="callback">The function that returns the next value.</param>
        /// <param name="description">The text shown below the menu while this item is highlighted.</param>
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
            // Has invalid parameter types in API.
            Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_RIGHT", "HUD_SHOP_SOUNDSET", 1);
        }

        internal override void GoLeft()
        {
            string oldValue = CurrentItem;
            string newSelectedItem = Callback(this, true);
            CurrentItem = newSelectedItem;
            ParentMenu.DynamicListItemCurrentItemChanged(ParentMenu, this, oldValue, newSelectedItem);
            // Has invalid parameter types in API.
            Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_RIGHT", "HUD_SHOP_SOUNDSET", 1);
        }

        internal override void Select()
        {
            ParentMenu.DynamicListItemSelectEvent(ParentMenu, this, CurrentItem);
        }
    }
}
