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
    public class MenuItem
    {
        public enum Icon
        {
            NONE,
            LOCK,
            STAR,
            WARNING,
        }

        public string Text { get; set; }
        public string Label { get; set; }
        public Icon LeftIcon { get; set; }
        public Icon RightIcon { get; set; }
        public bool Enabled { get; set; } = true;
        public string Description { get; set; }
        public int Index { get { if (ParentMenu != null) return ParentMenu.MenuItems.IndexOf(this); return -1; } } //{ get; internal set; }
        public bool Selected { get { if (ParentMenu != null) { return ParentMenu.CurrentIndex == Index; } return false; } }
        public Menu ParentMenu { get; set; }
        public int PositionOnScreen { get; internal set; }
        private const float Width = 500f;
        private const float RowHeight = 38f;

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

            SetScriptGfxAlign(76, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
            BeginTextCommandLineCount("CELL_EMAIL_BCON");
            string[] strings = CitizenFX.Core.UI.Screen.StringToArray(description ?? "");
            foreach (string s in strings)
            {
                AddTextComponentSubstringPlayerName(s);
            }
            ResetScriptGfxAlign();
            Debug.WriteLine(GetTextScreenLineCount(0.0f, 0.1f).ToString());
        }

        /// <summary>
        /// Draws the item on the screen.
        /// </summary>
        internal void Draw(int indexOffset)
        {


            if (ParentMenu != null)
            {
                float yOffset = ParentMenu.MenuItemsYOffset + 1f - (RowHeight * MathUtil.Clamp(ParentMenu.Size, 0, ParentMenu.MaxItemsOnScreen));

                #region Background Rect
                SetScriptGfxAlign(ParentMenu.LeftAligned ? 76 : 82, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                float x = (ParentMenu.Position.X + (Width / 2f)) / Main.ScreenWidth;
                float y = (ParentMenu.Position.Y + ((Index - indexOffset) * RowHeight) + (20f) + yOffset) / Main.ScreenHeight;
                float width = Width / Main.ScreenWidth;
                float height = (RowHeight) / Main.ScreenHeight;

                if (Selected)
                {
                    DrawRect(x, y, width, height, 255, 255, 255, 225);
                }
                ResetScriptGfxAlign();
                #endregion

                #region Left Icon
                float textXOffset = 0f;
                if (LeftIcon != Icon.NONE)
                {
                    textXOffset = 25f;

                    SetScriptGfxAlign(76, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    float spriteX = (20f / Main.ScreenWidth);
                    if (!ParentMenu.LeftAligned)
                    {
                        spriteX = GetSafeZoneSize() - ((Width - 20f) / Main.ScreenWidth);
                    }
                    float spriteY = y;//- (16f / Main.ScreenHeight);
                    float spriteWidth = (38f / Main.ScreenWidth);
                    float spriteHeight = (38f / Main.ScreenHeight);

                    if (LeftIcon == Icon.LOCK)
                    {
                        DrawSprite("commonmenu", "shop_lock", spriteX, spriteY, spriteWidth, spriteHeight, 0f, Selected ? 0 : 255, Selected ? 0 : 255, Selected ? 0 : 255, 255);
                    }

                    ResetScriptGfxAlign();
                }
                #endregion

                #region Right Icon

                #endregion

                #region Text
                int font = 0;
                float textSize = 0.34f;

                SetScriptGfxAlign(76, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
                SetTextFont(font);
                SetTextScale(textSize, textSize);
                SetTextJustification(1);
                BeginTextCommandDisplayText("STRING");
                AddTextComponentSubstringPlayerName(Text ?? "N/A");
                if (Selected)
                {
                    SetTextColour(0, 0, 0, 255);
                }
                float textMinX = (textXOffset / Main.ScreenWidth) + (10f / Main.ScreenWidth);
                float textMaxX = (Width - 10f) / Main.ScreenWidth;
                float textHeight = GetTextScaleHeight(textSize, font);
                float textY = y - ((30f / 2f) / Main.ScreenHeight);
                if (ParentMenu.LeftAligned)
                {
                    SetTextWrap(textMinX, textMaxX);
                    EndTextCommandDisplayText(textMinX, textY);
                }
                else
                {
                    textMinX = (textXOffset / Main.ScreenWidth) + GetSafeZoneSize() - ((Width - 10f) / Main.ScreenWidth);
                    textMaxX = GetSafeZoneSize() - (10f / Main.ScreenWidth);
                    SetTextWrap(textMinX, textMaxX);
                    EndTextCommandDisplayText(textMinX, textY);
                }
                ResetScriptGfxAlign();

                #endregion

                #region Label
                if (!string.IsNullOrEmpty(Label))
                {
                    SetScriptGfxAlign(76, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    BeginTextCommandDisplayText("STRING");
                    SetTextFont(font);
                    SetTextScale(textSize, textSize);
                    SetTextJustification(2);
                    AddTextComponentSubstringPlayerName(Label);
                    if (Selected)
                    {
                        SetTextColour(0, 0, 0, 255);
                    }
                    if (ParentMenu.LeftAligned)
                    {
                        SetTextWrap(0f, (485f / Main.ScreenWidth));
                        EndTextCommandDisplayText(10f / Main.ScreenWidth, textY);
                    }
                    else
                    {
                        SetTextWrap(0f, GetSafeZoneSize() - (10f / Main.ScreenWidth));
                        EndTextCommandDisplayText(0f, textY);
                    }

                    ResetScriptGfxAlign();
                }
                #endregion



            }
        }


        //public float GetDescriptionHeight()
        //{
        //    //float descTextHeight = GetTextScaleHeight(1.35f, 0);// + 0.0027777599170804024f;
        //    float descTextHeight = 27f + 0.0027777599170804024f;

        //    BeginTextCommandLineCount("CELL_EMAIL_BCON");
        //    string[] strings = CitizenFX.Core.UI.Screen.StringToArray(Description);
        //    foreach (string s in strings)
        //    {
        //        AddTextComponentSubstringPlayerName(s.ToUpper());
        //    }
        //    int lines = GetTextScreenLineCount(0f, 0f);
        //    return lines * descTextHeight;
        //}


    }
}
