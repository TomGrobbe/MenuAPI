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
#if FIVEM
            Cross,
#endif
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

        private int GetSpriteColour()
        {
            return Enabled ? 255 : 109;
        }

#if FIVEM
        private string GetSpriteName()
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
                if (Selected)
                {
                    return "shop_box_blankb";
                }
                return "shop_box_blank";
            }
        }
#endif

        private float GetSpriteX()
        {
#if FIVEM
            bool leftSide = false;
            bool leftAligned = ParentMenu.LeftAligned;
            if (leftSide)
            {
                if (leftAligned)
                {
                    return 20f / MenuController.ScreenWidth;
                }
                else
                {
                    return GetSafeZoneSize() - ((Width - 20f) / MenuController.ScreenWidth);
                }
            }
            else
            {
                if (leftAligned)
                {
                    return (Width - 20f) / MenuController.ScreenWidth;
                }
                else
                {
                    return GetSafeZoneSize() - (20f / MenuController.ScreenWidth);
                }
            }
#endif
#if REDM
            return (Width - 30f) / MenuController.ScreenWidth;
#endif
        }

        internal override void Draw(int offset)
        {
            RightIcon = Icon.NONE;
            Label = null;
            base.Draw(offset);
#if FIVEM
            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
#endif

            float yOffset = ParentMenu.MenuItemsYOffset + 1f - (RowHeight * MathUtil.Clamp(ParentMenu.Size, 0, ParentMenu.MaxItemsOnScreen));
#if FIVEM
            string name = GetSpriteName();
#endif

            float spriteY = (ParentMenu.Position.Value + ((Index - offset) * RowHeight) + (20f) + yOffset) / MenuController.ScreenHeight;
            float spriteX = GetSpriteX();
#if FIVEM
            float spriteHeight = 45f / MenuController.ScreenHeight;
            float spriteWidth = 45f / MenuController.ScreenWidth;
#endif
            int color = GetSpriteColour();
#if FIVEM
            DrawSprite("commonmenu", name, spriteX, spriteY, spriteWidth, spriteHeight, 0f, color, color, color, 255);
            ResetScriptGfxAlign();
#endif
#if REDM
            float spriteHeight = 24f / MenuController.ScreenHeight;
            float spriteWidth = 16f / MenuController.ScreenWidth;
            DrawSprite("menu_textures", "SELECTION_BOX_SQUARE", spriteX, spriteY, spriteWidth, spriteHeight, 0f, color, color, color, 255, false);
            if (Checked)
            {
                int[] sc = Enabled ? (Selected ? new int[3] { 255, 255, 255 } : new int[3] { 181, 17, 18 }) : (Selected ? new int[3] { 109, 109, 109 } : new int[3] { 110, 10, 10 });
                DrawSprite("generic_textures", "TICK", spriteX, spriteY, spriteWidth, spriteHeight, 0f, sc[0], sc[1], sc[2], 255, false);
            }
#endif
        }

        internal override void GoRight()
        {
            ParentMenu.SelectItem(this);
        }

        internal override void Select()
        {
            Checked = !Checked;
            ParentMenu.CheckboxChangedEvent(this, Index, Checked);
        }
    }
}
