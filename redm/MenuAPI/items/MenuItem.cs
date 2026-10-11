using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using static CitizenFX.Core.Native.Function;
using static CitizenFX.Core.Native.Hash;

namespace MenuAPI
{
    /// <summary>
    /// A basic menu button with text, an optional label and icons. Every other item type inherits from this class.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// The icons that can be drawn on the left or right side of a <see cref="MenuItem"/>.
        /// </summary>
        public enum Icon
        {
            /// <summary>
            /// No icon.
            /// </summary>
            NONE,
            /// <summary>
            /// A padlock.
            /// </summary>
            LOCK,
            /// <summary>
            /// A tick mark.
            /// </summary>
            TICK,
            /// <summary>
            /// A circle.
            /// </summary>
            CIRCLE,
            /// <summary>
            /// A horse saddle.
            /// </summary>
            SADDLE,
            /// <summary>
            /// An orange star, shown darker while the item is disabled.
            /// </summary>
            STAR,
            /// <summary>
            /// An arrow pointing left.
            /// </summary>
            ARROW_LEFT,
            /// <summary>
            /// An arrow pointing right. Commonly used on submenu buttons.
            /// </summary>
            ARROW_RIGHT,
            /// <summary>
            /// The invite sent icon.
            /// </summary>
            INVITE_SENT,
            /// <summary>
            /// A square selection box.
            /// </summary>
            SELECTION_BOX
        }
        /// <summary>
        /// The text on the left side of the item.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Text shown on the right side of the item. Not available on every item type.
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// The icon on the left side of the item. Defaults to <see cref="Icon.NONE"/>.
        /// </summary>
        public Icon LeftIcon { get; set; }
        /// <summary>
        /// The icon on the right side of the item. Defaults to <see cref="Icon.NONE"/>. Not available on every item type.
        /// </summary>
        public Icon RightIcon { get; set; }
        /// <summary>
        /// When false, the item is greyed out and selecting it only plays an error sound. Defaults to true.
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// Text shown below the menu while this item is highlighted. Long text is wrapped onto new lines of about 50 characters, and null is ignored.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != null)
                {
                    string text = value;
                    int maxLength = 50;
                    List<string> lines = new List<string>();
                    while (text.Length > maxLength)
                    {
                        var substr = text.Substring(0, Math.Min(text.Length - 1, maxLength));
                        var lastIndex = substr.LastIndexOf(" ");
                        if (lastIndex == -1)
                        {
                            lastIndex = Math.Min(text.Length - 1, maxLength);
                        }
                        lines.Add(text.Substring(0, lastIndex));
                        text = text.Substring(lastIndex);
                    }
                    lines.Add(text);
                    text = "";
                    foreach (var str in lines)
                    {
                        text += str + "\n";
                    }
                    _description = text;
                }
            }
        }
        private string _description;
        /// <summary>
        /// The position of this item in its menu, or -1 when it is not in a menu.
        /// </summary>
        public int Index { get { if (ParentMenu != null) return ParentMenu.GetMenuItems().IndexOf(this); return -1; } } //{ get; internal set; }
        /// <summary>
        /// Whether this item is the highlighted item of its menu. False when it is not in a menu.
        /// </summary>
        public bool Selected { get { if (ParentMenu != null) { return ParentMenu.CurrentIndex == Index; } return false; } }
        /// <summary>
        /// The menu this item was added to. Set by <see cref="Menu.AddMenuItem(MenuItem)"/>.
        /// </summary>
        public Menu ParentMenu { get; set; }
        /// <summary>
        /// The index this item had when it was added to its menu.
        /// </summary>
        public int PositionOnScreen { get; internal set; }
        /// <summary>
        /// The width of an item in pixels on a 1080p screen, the same as <see cref="Menu.Width"/>.
        /// </summary>
        protected const float Width = Menu.Width;
        /// <summary>
        /// The height of one item row in pixels on a 1080p screen.
        /// </summary>
        protected const float RowHeight = 38f;

        // Allows you to attach data to a menu item if you want to identify the menu item without having to put identification info in the visible text or description.
        /// <summary>
        /// Any data you want to attach to this item, so you can identify it without putting that in the visible text or description.
        /// </summary>
        public dynamic ItemData { get; set; }

        /// <summary>
        /// Creates a new <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="text"></param>
        public MenuItem(string text) : this(text, null) { }

        /// <summary>
        /// Creates a new <see cref="MenuItem"/>.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="description"></param>
        public MenuItem(string text, string description)
        {
            Text = text;
            Description = description;
        }

        /// <summary>
        /// Get the sprite dictionary name for the given icon.
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        protected string GetSpriteDictionary(Icon icon)
        {
            switch (icon)
            {
                case Icon.LOCK:
                case Icon.TICK:
                case Icon.CIRCLE:
                case Icon.SADDLE:
                case Icon.STAR:
                case Icon.ARROW_LEFT:
                case Icon.ARROW_RIGHT:
                case Icon.INVITE_SENT:
                case Icon.SELECTION_BOX:
                    return "menu_textures";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Get the sprite name for the given icon depending on the selected state of the item.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        protected string GetSpriteName(Icon icon, bool selected)
        {
            switch (icon)
            {
                case Icon.LOCK:
                    return "MENU_ICON_LOCK";
                case Icon.TICK:
                    return "MENU_ICON_TICK";
                case Icon.CIRCLE:
                    return "MENU_ICON_CIRCLE";
                case Icon.SADDLE:
                    return "MENU_ICON_ON_HORSE";
                case Icon.STAR:
                    return "MENU_ICON_INFO_NEW";
                case Icon.ARROW_LEFT:
                    return "SELECTION_ARROW_LEFT";
                case Icon.ARROW_RIGHT:
                    return "SELECTION_ARROW_RIGHT";
                case Icon.INVITE_SENT:
                    return "MENU_ICON_INVITE_SENT";
                case Icon.SELECTION_BOX:
                    return "SELECTION_BOX_SQUARE";
            }
            return "";
        }

        /// <summary>
        /// Get the sprite size for the given icon.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        protected float GetSpriteSize(Icon icon, bool width)
        {
            switch (icon)
            {
                case Icon.TICK:
                    return width ? (16f / MenuController.ScreenWidth) : (24f / MenuController.ScreenHeight);
                case Icon.CIRCLE:
                    return width ? (16f / MenuController.ScreenWidth) : (26f / MenuController.ScreenHeight);
                case Icon.SADDLE:
                    return width ? (16f / MenuController.ScreenWidth) : (24f / MenuController.ScreenHeight);
                case Icon.STAR:
                case Icon.ARROW_LEFT:
                case Icon.ARROW_RIGHT:
                    return width ? (14f / MenuController.ScreenWidth) : (22f / MenuController.ScreenHeight);
                case Icon.SELECTION_BOX:
                case Icon.INVITE_SENT:
                    return width ? (16f / MenuController.ScreenWidth) : (24f / MenuController.ScreenHeight);
                case Icon.LOCK:
                default:
                    return width ? (24f / MenuController.ScreenWidth) : (30f / MenuController.ScreenHeight);
            }
        }

        /// <summary>
        /// Get the sprite color for the provided icon depending on the current state of the item (Enabled &amp; selected values).
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        protected int[] GetSpriteColour(Icon icon, bool selected)
        {
            switch (icon)
            {
                case Icon.STAR:
                    return !Enabled ? new int[3] { 163, 106, 5 } : new int[3] { 237, 154, 9 };
                default:
                    return Enabled ? new int[3] { 255, 255, 255 } : new int[3] { 109, 109, 109 };
                    //return new int[3] { 255, 255, 255 };
            }
        }

        /// <summary>
        /// Get the sprite x position offset for the provided icon and alignment variables.
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="leftAligned"></param>
        /// <param name="leftSide"></param>
        /// <returns></returns>
        protected float GetSpriteX(Icon icon, bool leftAligned, bool leftSide)
        {
            if (leftSide)
            {
                return 30f / MenuController.ScreenWidth;
            }
            return (Width - 30f) / MenuController.ScreenWidth;
        }

        /// <summary>
        /// Handles menu navigation to the right for items that support it, otherwise the item will be selected.
        /// </summary>
        internal virtual void GoRight()
        {
            if (Enabled)
            {
                ParentMenu.SelectItem(this);
            }
        }

        /// <summary>
        /// Handles menu navigation to the left for items that support it, otherwise the menu will navigate to the parent menu.
        /// </summary>
        internal virtual void GoLeft()
        {
            if (MenuController.NavigateMenuUsingArrows && !MenuController.DisableBackButton && !(MenuController.PreventExitingMenu && ParentMenu == null))
            {
                ParentMenu.GoBack();
            }
        }

        /// <summary>
        /// Handles item selection.
        /// </summary>
        internal virtual void Select()
        {
            ParentMenu.ItemSelectedEvent(this, Index);
        }

        /// <summary>
        /// Draws the item on the screen.
        /// </summary>
        internal virtual void Draw(int indexOffset)
        {
            if (ParentMenu == null)
            {
                return;
            }

            int font = 0;
            float textSize = (14f * 27f) / MenuController.ScreenHeight;
            int textColor = Selected ? (Enabled ? 0 : 50) : (Enabled ? 255 : 109);

            float yOffset = ParentMenu.MenuItemsYOffset + 1f - (RowHeight * MathUtil.Clamp(ParentMenu.Size, 0, ParentMenu.MaxItemsOnScreen));
            float textXOffset = 0f;
            float rightTextIconOffset = 0f;

            DrawBackground(indexOffset, yOffset, out float x, out float y);

            float textMaxX = (Width - 10f) / MenuController.ScreenWidth;
            float textY = y - ((30f / 2f) / MenuController.ScreenHeight);

            textXOffset = DrawLeftIcon(textXOffset, y);
            rightTextIconOffset = DrawRightIcon(rightTextIconOffset, y);

            // must be calculated after DrawLeftIcon, otherwise the left icon offset is not taken into account
            // and the item text will be drawn on top of the icon.
            float textMinX = (textXOffset / MenuController.ScreenWidth) + (10f / MenuController.ScreenWidth);
            DrawItemText(font, textSize, textColor, textMinX, textMaxX, textY, textXOffset, y);
        }

        /// <summary>
        /// Drwa the item text
        /// </summary>
        /// <param name="font"></param>
        /// <param name="textSize"></param>
        /// <param name="textColor"></param>
        /// <param name="textMinX"></param>
        /// <param name="textMaxX"></param>
        /// <param name="textY"></param>
        /// <param name="textXOffset"></param>
        /// <param name="y"></param>
        private void DrawItemText(int font, float textSize, int textColor, float textMinX, float textMaxX, float textY, float textXOffset, float y)
        {
            SetTextScale(textSize, textSize);
            textColor = Enabled ? 255 : 109;
            SetTextColor(textColor, textColor, textColor, 255);
            textMinX = ((8f + textXOffset) / MenuController.ScreenWidth) + (10f / MenuController.ScreenWidth);
            textMaxX = (Width - 10f) / MenuController.ScreenWidth;
            textY = y - ((30f / 2f) / MenuController.ScreenHeight);
            font = 23;
            // Cfx native, undocumented.
            Call((CitizenFX.Core.Native.Hash)0xADA9255D, font);
            // API version has incorrect parameter types.
            long _text = Call<long>(_CREATE_VAR_STRING, 10, "LITERAL_STRING", (Text ?? "N/A") + (" " + Label ?? ""));
            DisplayText(_text, textMinX, textY);
        }


        /// <summary>
        /// Draw the right icon if it exists.
        /// </summary>
        /// <param name="rightTextIconOffset"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private float DrawRightIcon(float rightTextIconOffset, float y)
        {
            if (RightIcon == Icon.NONE)
            {
                return rightTextIconOffset;
            }
            string spriteName = GetSpriteName(RightIcon, Selected);
            string spriteDict = GetSpriteDictionary(RightIcon);
            float spriteX = GetSpriteX(RightIcon, true, false);
            float spriteY = y;
            float spriteHeight = GetSpriteSize(RightIcon, false);
            float spriteWidth = GetSpriteSize(RightIcon, true);
            int[] spriteColor = GetSpriteColour(RightIcon, Selected);
            DrawSprite(spriteDict, spriteName, spriteX, spriteY, spriteWidth, spriteHeight, 0f, spriteColor[0], spriteColor[1], spriteColor[2], 255, false);
            return rightTextIconOffset;
        }

        /// <summary>
        /// Draw the left icon if it exists.
        /// </summary>
        /// <param name="textXOffset"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private float DrawLeftIcon(float textXOffset, float y)
        {
            if (LeftIcon == Icon.NONE)
            {
                return textXOffset;
            }
            textXOffset = 25f;
            string spriteName = GetSpriteName(LeftIcon, Selected);
            string spriteDict = GetSpriteDictionary(LeftIcon);
            float spriteX = GetSpriteX(LeftIcon, true, true);
            float spriteY = y;
            float spriteHeight = GetSpriteSize(LeftIcon, false);
            float spriteWidth = GetSpriteSize(LeftIcon, true);
            int[] spriteColor = GetSpriteColour(LeftIcon, Selected);
            DrawSprite(spriteDict, spriteName, spriteX, spriteY, spriteWidth, spriteHeight, 0f, spriteColor[0], spriteColor[1], spriteColor[2], 255, false);
            return textXOffset;
        }

        /// <summary>
        /// Draws the background for the menu item if it is selected and output the x/y values for this item.
        /// </summary>
        /// <param name="indexOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void DrawBackground(int indexOffset, float yOffset, out float x, out float y)
        {
            x = (ParentMenu.Position.Key + (Width / 2f)) / MenuController.ScreenWidth;
            y = (ParentMenu.Position.Value + ((Index - indexOffset) * RowHeight) + (20f) + yOffset) / MenuController.ScreenHeight;

            if (Selected)
            {
                float width = Width / MenuController.ScreenWidth;
                float height = (RowHeight) / MenuController.ScreenHeight;
                DrawSprite(MenuController._texture_dict, MenuController._header_texture, x, y, width, height, 0f, 181, 17, 18, 255, false);
            }
        }
    }
}
