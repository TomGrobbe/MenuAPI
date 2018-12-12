using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    public class Menu
    {
        #region Setting up events
        public delegate void ItemSelectEvent(Menu parentMenu, MenuItem menuItem, int itemIndex);

        public event ItemSelectEvent OnItemSelect;

        protected virtual void ItemSelectedEvent(MenuItem menuItem, int itemIndex)
        {
            OnItemSelect?.Invoke(this, menuItem, itemIndex);
        }
        #endregion

        #region constants or readonlys
        public const float Width = 500f;

        #endregion

        #region private variables
        private static SizeF headerSize = new SizeF(500f, 110f);
        #endregion

        #region Public Variables
        public string MenuTitle;

        public string MenuSubtitle;

        public int MaxItemsOnScreen = 10;

        public List<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();

        public int Size => MenuItems.Count;

        public bool Visible { get; set; } = false;

        public bool LeftAligned = true;

        public PointF Position { get; private set; } = new PointF(0f, 0f);

        public float MenuItemsYOffset { get; private set; } = 0f;

        public string CounterPreText { get; set; }

        public int CurrentIndex { get; private set; } = 0;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="Menu"/>.
        /// </summary>
        /// <param name="name"></param>
        public Menu(string name) : this(name, null) { }

        /// <summary>
        /// Creates a new <see cref="Menu"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subtitle"></param>
        public Menu(string name, string subtitle)
        {
            MenuTitle = name;
            MenuSubtitle = subtitle;
        }
        #endregion

        #region public functions
        /// <summary>
        /// Adds a <see cref="MenuItem"/> to this <see cref="Menu"/>.
        /// </summary>
        /// <param name="item"></param>
        public void AddMenuItem(MenuItem item)
        {
            MenuItems.Add(item);
            item.Index = MenuItems.Count - 1;
            item.PositionOnScreen = item.Index;
            foreach (var menuItem in MenuItems)
            {
                menuItem.Index = MenuItems.IndexOf(menuItem);
            }
            item.ParentMenu = this;
        }

        /// <summary>
        /// Removes the specified <see cref="MenuItem"/> from this <see cref="Menu"/>.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveMenuItem(MenuItem item)
        {
            if (MenuItems.Contains(item))
            {
                MenuItems.Remove(item);
            }
            foreach (var menuItem in MenuItems)
            {
                menuItem.Index = MenuItems.IndexOf(menuItem);
            }
        }

        /// <summary>
        /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(int index)
        {
            if (index > -1 && MenuItems.Count - 1 >= index)
            {
                SelectItem(MenuItems[index]);
            }
        }

        /// <summary>
        /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(MenuItem item)
        {
            ItemSelectedEvent(item, item.Index);
        }
        #endregion

        #region internal functions
        /// <summary>
        /// Draws the menu title + subtitle, calls all Draw functions for all menu items and draws the description for the selected item.
        /// </summary>
        /// <returns></returns>
        internal async Task Draw()
        {
            if (Visible == false) await BaseScript.Delay(0);

            MenuItemsYOffset = 0f;

            #region header
            if (!string.IsNullOrEmpty(MenuTitle))
            {
                #region header background
                SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                float x = (Position.X + (headerSize.Width / 2f)) / Main.ScreenWidth;
                float y = (Position.Y + (headerSize.Height / 2f)) / Main.ScreenHeight;
                float width = headerSize.Width / Main.ScreenWidth;
                float height = headerSize.Height / Main.ScreenHeight;

                DrawSprite(Main._texture_dict, Main._header_texture, x, y, width, height, 0f, 255, 255, 255, 255);

                ResetScriptGfxAlign();
                #endregion

                #region Header Menu Title
                int font = 1;
                float size = 1.1f;

                SetScriptGfxAlign(76, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                BeginTextCommandDisplayText("STRING");
                SetTextFont(font);
                SetTextColour(255, 255, 255, 255);
                SetTextScale(size, size);
                SetTextJustification(0);
                AddTextComponentSubstringPlayerName(MenuTitle);
                if (LeftAligned)
                {
                    EndTextCommandDisplayText(((headerSize.Width / 2f) / Main.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f));
                }
                else
                {
                    EndTextCommandDisplayText(GetSafeZoneSize() - ((headerSize.Width / 2f) / Main.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f));
                }

                ResetScriptGfxAlign();

                MenuItemsYOffset = headerSize.Height;
                #endregion
            }
            #endregion

            #region Subtitle
            {
                #region subtitle background
                SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                float bgHeight = 38f;


                float x = (Position.X + (headerSize.Width / 2f)) / Main.ScreenWidth;
                float y = ((Position.Y + MenuItemsYOffset + (bgHeight / 2f)) / Main.ScreenHeight);
                float width = headerSize.Width / Main.ScreenWidth;
                float height = bgHeight / Main.ScreenHeight;

                DrawRect(x, y, width, height, 0, 0, 0, 250);
                ResetScriptGfxAlign();
                #endregion

                #region subtitle text
                if (!string.IsNullOrEmpty(MenuSubtitle))
                {
                    int font = 0;
                    float size = 0.34f;

                    SetScriptGfxAlign(76, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    BeginTextCommandDisplayText("STRING");
                    SetTextFont(font);
                    SetTextScale(size, size);
                    SetTextJustification(1);
                    AddTextComponentSubstringPlayerName("~HUD_COLOUR_HB_BLUE~" + MenuSubtitle.ToUpper());
                    if (LeftAligned)
                    {
                        EndTextCommandDisplayText(10f / Main.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / Main.ScreenHeight)));
                    }
                    else
                    {
                        EndTextCommandDisplayText(GetSafeZoneSize() - ((headerSize.Width - 10f) / Main.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f + (4f / Main.ScreenHeight)));
                    }

                    ResetScriptGfxAlign();
                }
                #endregion



                #region counterText
                string counterText = $"{CounterPreText ?? ""}{CurrentIndex + 1} / {Size}";
                if (CounterPreText != null || MaxItemsOnScreen < Size)
                {
                    //float x = (Position.X + (headerSize.Width / 2f)) / Main.ScreenWidth;
                    //float y = ((Position.Y + MenuItemsYOffset + 18f) / Main.ScreenHeight);
                    //float width = headerSize.Width / Main.ScreenWidth;
                    //float height = 36f / Main.ScreenHeight;

                    int font = 0;
                    float size = 0.34f;

                    SetScriptGfxAlign(76, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    BeginTextCommandDisplayText("STRING");
                    SetTextFont(font);
                    SetTextScale(size, size);
                    SetTextJustification(2);
                    AddTextComponentSubstringPlayerName("~HUD_COLOUR_HB_BLUE~" + counterText.ToUpper());
                    if (LeftAligned)
                    {
                        SetTextWrap(0f, (485f / Main.ScreenWidth));
                        EndTextCommandDisplayText(10f / Main.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / Main.ScreenHeight)));
                    }
                    else
                    {
                        SetTextWrap(0f, GetSafeZoneSize() - (10f / Main.ScreenWidth));
                        EndTextCommandDisplayText(0f, y - (GetTextScaleHeight(size, font) / 2f + (4f / Main.ScreenHeight)));
                    }

                    ResetScriptGfxAlign();
                }
                if (!string.IsNullOrEmpty(MenuSubtitle) || (CounterPreText != null || MaxItemsOnScreen < Size))
                {
                    MenuItemsYOffset += bgHeight - 1f;
                }

                #endregion
            }
            #endregion

            #region menuitems backgground
            if (MenuItems.Count > 0)
            {
                SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                float bgHeight = 38f * MenuItems.Count;


                float x = (Position.X + (headerSize.Width / 2f)) / Main.ScreenWidth;
                float y = ((Position.Y + MenuItemsYOffset + ((bgHeight + 1f) / 2f)) / Main.ScreenHeight);
                float width = headerSize.Width / Main.ScreenWidth;
                float height = (bgHeight + 1f) / Main.ScreenHeight;

                //DrawRect(x, y, width, height, 0, 0, 0, 150);
                DrawSprite(Main._texture_dict, "gradient_bgd", x, y, width, height, 0f, 255, 255, 255, 255);
                ResetScriptGfxAlign();
                MenuItemsYOffset += bgHeight - 1f;
            }
            #endregion

            #region menuItems
            if (MenuItems.Count > 0)
            {
                foreach (var item in MenuItems)
                {
                    item.Draw();
                }
            }
            #endregion




            /*
             *   Drawing description.
             *  
             */


            #region Description
            if (!string.IsNullOrEmpty(MenuItems[CurrentIndex].Description))
            {
                /*
                 *   Drawing background gradient.
                 *  
                 */
                #region background

                float descWidth = Width / Main.ScreenWidth;
                float descHeight = MathUtil.Clamp((10f + MenuItems[CurrentIndex].GetDescriptionHeight()), 45f, 600f) / Main.ScreenHeight;
                float descX = (Position.X + (Width / 2f)) / Main.ScreenWidth;
                float descY = (2f + MenuItemsYOffset + ((descHeight / 2f) * Main.ScreenHeight)) / Main.ScreenHeight + (28f / Main.ScreenHeight);

                SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                DrawRect(descX, descY - descHeight / 2f, descWidth, 4f / Main.ScreenHeight, 0, 0, 0, 200);
                DrawSprite(Main._texture_dict, "gradient_bgd", descX, descY, descWidth, descHeight, 0f, 255, 255, 255, 255);

                ResetScriptGfxAlign();
                #endregion

                /*
                 *   Drawing description text.
                 *  
                 */
                #region description text
                int font = 0;
                float size = 0.35f;

                SetScriptGfxAlign(76, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                BeginTextCommandDisplayText("CELL_EMAIL_BCON");
                SetTextFont(font);
                SetTextScale(size, size);
                SetTextJustification(1);
                string text = MenuItems[CurrentIndex].Description;
                string[] strings = CitizenFX.Core.UI.Screen.StringToArray(text);
                foreach (string s in strings)
                {
                    AddTextComponentSubstringPlayerName(s);
                }

                if (LeftAligned)
                {
                    SetTextWrap(0f, 500f / Main.ScreenWidth);
                    EndTextCommandDisplayText(15f / Main.ScreenWidth, descY - (descHeight / 2f) + (4f / Main.ScreenHeight));
                }
                else
                {
                    SetTextWrap(0f, GetSafeZoneSize() - (15f / Main.ScreenWidth));
                    EndTextCommandDisplayText((Main.ScreenWidth - 485f) / Main.ScreenWidth - (1f - GetSafeZoneSize()), descY - (descHeight / 2f) + (4f / Main.ScreenHeight));
                }

                ResetScriptGfxAlign();

                #endregion
            }

            #endregion

        }
        #endregion

    }
}
