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
        public int Index { get; internal set; }
        public bool Selected { get; set; }
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
        internal void Draw()
        {
            if (ParentMenu != null)
            {

                float yOffset = ParentMenu.MenuItemsYOffset + 1f - (RowHeight * ParentMenu.Size);

                #region Background Rect
                SetScriptGfxAlign(ParentMenu.LeftAligned ? 76 : 82, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                float x = (ParentMenu.Position.X + (Width / 2f)) / Main.ScreenWidth;
                float y = (ParentMenu.Position.Y + (Index * RowHeight) + (20f) + yOffset) / Main.ScreenHeight;
                float width = Width / Main.ScreenWidth;
                float height = (RowHeight) / Main.ScreenHeight;

                if (Index % 2 == 0)
                {
                    DrawRect(x, y, width, height, 0, 255, 255, 50);
                }
                //else
                //{
                //    DrawRect(x, y, width, height, 0, 0, 0, 190);
                //}
                ResetScriptGfxAlign();
                #endregion

                #region Text

                #endregion

                #region Label

                #endregion

                #region Left Icon

                #endregion

                #region Right Icon

                #endregion



            }
        }


        public float GetDescriptionHeight()
        {
            //float descTextHeight = GetTextScaleHeight(1.35f, 0);// + 0.0027777599170804024f;
            float descTextHeight = 27f + 0.0027777599170804024f;

            BeginTextCommandLineCount("CELL_EMAIL_BCON");
            string[] strings = CitizenFX.Core.UI.Screen.StringToArray(Description);
            foreach (string s in strings)
            {
                AddTextComponentSubstringPlayerName(s.ToUpper());
            }
            int lines = GetTextScreenLineCount(0f, 0f);
            return lines * descTextHeight;
        }


    }
}
