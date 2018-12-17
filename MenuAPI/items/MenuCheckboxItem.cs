using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


namespace MenuAPI
{
    public class MenuCheckboxItem : MenuItem
    {
        public bool Checked { get; set; } = false;
        public CheckboxStyle Style { get; set; } = CheckboxStyle.Tick;
        public enum CheckboxStyle
        {
            Cross,
            Tick
        }

        /// <summary>
        /// Creates a basic <see cref="MenuCheckboxItem"/>.
        /// </summary>
        /// <param name="text"></param>
        public MenuCheckboxItem(string text) : this(text, null) { }
        /// <summary>
        /// Creates a basic <see cref="MenuCheckboxItem"/> and sets the checked state to <param name="_checked"></param>'s state.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="_checked"></param>
        public MenuCheckboxItem(string text, bool _checked) : this(text, null, _checked) { }
        /// <summary>
        /// Creates a basic <see cref="MenuCheckboxItem"/> and adds an item description.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="description"></param>
        public MenuCheckboxItem(string text, string description) : this(text, description, false) { }
        /// <summary>
        /// Creates a new <see cref="MenuCheckboxItem"/> with all parameters set.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="description"></param>
        /// <param name="_checked"></param>
        public MenuCheckboxItem(string text, string description, bool _checked) : base(text, description)
        {
            Checked = _checked;
        }


        int GetSpriteColour()
        {
            return 255;
        }

        string GetSpriteName()
        {
            if (Checked)
            {
                if (Style == CheckboxStyle.Tick)
                {
                    if (Selected)
                    {
                        return "shop_box_tickb";
                    }
                    return "shop_box_tick";
                }
                else
                {
                    if (Selected)
                    {
                        return "shop_box_crossb";
                    }
                    return "shop_box_cross";
                }
            }
            else
            {
                if (base.Selected)
                {
                    return "shop_box_blankb";
                }
                return "shop_box_blank";
            }
        }

        float GetSpriteX()
        {
            bool leftSide = false;
            bool leftAligned = ParentMenu.LeftAligned;
            return leftSide ? (leftAligned ? (20f / MenuController.ScreenWidth) : GetSafeZoneSize() - ((Width - 20f) / MenuController.ScreenWidth)) : (leftAligned ? (Width - 20f) / MenuController.ScreenWidth : (GetSafeZoneSize() - (20f / MenuController.ScreenWidth)));
        }

        internal override void Draw(int offset)
        {
            RightIcon = Icon.NONE;
            Label = null;

            base.Draw(offset);

            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            float yOffset = ParentMenu.MenuItemsYOffset + 1f - (RowHeight * MathUtil.Clamp(ParentMenu.Size, 0, ParentMenu.MaxItemsOnScreen));

            string name = GetSpriteName();
            float spriteY = (ParentMenu.Position.Y + ((Index - offset) * RowHeight) + (20f) + yOffset) / MenuController.ScreenHeight;
            float spriteX = GetSpriteX();
            float spriteHeight = 45f / MenuController.ScreenHeight;
            float spriteWidth = 45f / MenuController.ScreenWidth;
            int color = GetSpriteColour();

            DrawSprite("commonmenu", name, spriteX, spriteY, spriteWidth, spriteHeight, 0f, color, color, color, 255);
            ResetScriptGfxAlign();

        }
    }
}
