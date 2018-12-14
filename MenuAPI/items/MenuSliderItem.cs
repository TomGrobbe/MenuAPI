using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    public class MenuSliderItem : MenuItem
    {
        public int Min { get; private set; } = 0;
        public int Max { get; private set; } = 10;
        public bool ShowDivider { get; set; }
        public int Position { get; set; } = 0;

        public Icon SliderLeftIcon { get; set; } = Icon.NONE;
        public Icon SliderRightIcon { get; set; } = Icon.NONE;

        public Color BackgroundColor { get; set; } = Color.FromArgb(255, 24, 93, 151);
        public Color BarColor { get; set; } = Color.FromArgb(255, 53, 165, 223);

        public MenuSliderItem(string name, int min, int max, int startPosition) : this(name, min, max, startPosition, false) { }
        public MenuSliderItem(string name, int min, int max, int startPosition, bool showDivider) : this(name, null, min, max, startPosition, showDivider) { }
        public MenuSliderItem(string name, string description, int min, int max, int startPosition) : this(name, description, min, max, startPosition, false) { }
        public MenuSliderItem(string name, string description, int min, int max, int startPosition, bool showDivider) : base(name, description)
        {
            Min = min;
            Max = max;
            ShowDivider = showDivider;
            Position = startPosition;
        }

        /// <summary>
        /// Maps '<see cref="float"/> <paramref name="val"/>' to be a value between '<see cref="float"/> <paramref name="out_min"/>' and '<see cref="float"/> <paramref name="out_max"/>'.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="in_min"></param>
        /// <param name="in_max"></param>
        /// <param name="out_min"></param>
        /// <param name="out_max"></param>
        /// <returns></returns>
        private float Map(float val, float in_min, float in_max, float out_min, float out_max)
        {
            return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        internal override void Draw(int indexOffset)
        {

            RightIcon = SliderRightIcon;
            Label = null;

            base.Draw(indexOffset);

            if (Position > Max || Position < Min)
            {
                Position = (Max - Min) / 2;
            }


            float yOffset = ParentMenu.MenuItemsYOffset + 1f - (RowHeight * MathUtil.Clamp(ParentMenu.Size, 0, ParentMenu.MaxItemsOnScreen));

            float width = 150f / MenuController.ScreenWidth;
            float height = 10f / MenuController.ScreenHeight;
            float y = (ParentMenu.Position.Y + ((Index - indexOffset) * RowHeight) + (20f) + yOffset) / MenuController.ScreenHeight;
            float x = (ParentMenu.Position.X + (Width)) / MenuController.ScreenWidth - (width / 2f) - (8f / MenuController.ScreenWidth);
            if (!ParentMenu.LeftAligned)
            {
                x = (width / 2f) - (8f / MenuController.ScreenWidth);
            }

            if (SliderLeftIcon != Icon.NONE && SliderRightIcon != Icon.NONE)
            {
                x -= 40f / MenuController.ScreenWidth;

                var leftColor = GetSpriteColour(SliderLeftIcon, Selected);
                var rightColor = GetSpriteColour(SliderRightIcon, Selected);


                SetScriptGfxAlign(ParentMenu.LeftAligned ? 76 : 82, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                string textureDictionary = "commonmenu";
                if (SliderLeftIcon == Icon.MALE || SliderLeftIcon == Icon.FEMALE)
                {
                    textureDictionary = "mpleaderboard";
                }

                if (ParentMenu.LeftAligned)
                {
                    // left sprite left aligned.
                    DrawSprite(textureDictionary, GetSpriteName(SliderLeftIcon, Selected), x - (width / 2f + (4f / MenuController.ScreenWidth)) - (GetSpriteSize(SliderLeftIcon, true) / 2f), y, GetSpriteSize(SliderLeftIcon, true), GetSpriteSize(SliderLeftIcon, false), 0f, leftColor, leftColor, leftColor, 255);

                    // right sprite is managed by the regular function in MenuItem that handles the right icon.
                }
                else
                {
                    // left sprite right aligned.
                    DrawSprite(textureDictionary, GetSpriteName(SliderLeftIcon, Selected), x - (width + (4f / MenuController.ScreenWidth)) - GetSpriteSize(SliderLeftIcon, true) - (20f / MenuController.ScreenWidth), y, GetSpriteSize(SliderLeftIcon, true), GetSpriteSize(SliderLeftIcon, false), 0f, leftColor, leftColor, leftColor, 255);

                    // right sprite is managed by the regular function in MenuItem that handles the right icon.
                }



                ResetScriptGfxAlign();
            }

            SetScriptGfxAlign(ParentMenu.LeftAligned ? 76 : 82, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
            #region drawing background bar and foreground bar

            // background
            DrawRect(x, y, width, height, BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A);

            float xOffset = Map((float)Position, (float)Min, (float)Max, -((width / 4f) * MenuController.ScreenWidth), ((width / 4f) * MenuController.ScreenWidth)) / MenuController.ScreenWidth;

            // bar (foreground)
            if (!ParentMenu.LeftAligned)
                DrawRect(x - (width / 2f) + xOffset, y, width / 2f, height, BarColor.R, BarColor.G, BarColor.B, BarColor.A);
            else
                DrawRect(x + xOffset, y, width / 2f, height, BarColor.R, BarColor.G, BarColor.B, BarColor.A);

            #endregion

            #region drawing divider
            if (ShowDivider)
            {
                if (!ParentMenu.LeftAligned)
                    DrawRect(x - width + (4f / MenuController.ScreenWidth), y, 4f / MenuController.ScreenWidth, RowHeight / MenuController.ScreenHeight / 2f, 255, 255, 255, 255);
                else
                    DrawRect(x + (2f / MenuController.ScreenWidth), y, 4f / MenuController.ScreenWidth, RowHeight / MenuController.ScreenHeight / 2f, 255, 255, 255, 255);
            }
            #endregion
            ResetScriptGfxAlign();



        }
    }
}
