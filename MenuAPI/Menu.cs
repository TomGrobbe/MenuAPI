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
        public delegate void CheckboxItemChangeEvent(Menu parentMenu, MenuCheckboxItem menuItem, int itemIndex, bool checkedState);

        public event ItemSelectEvent OnItemSelect;
        public event CheckboxItemChangeEvent OnCheckboxChange;

        protected virtual void ItemSelectedEvent(MenuItem menuItem, int itemIndex)
        {
            OnItemSelect?.Invoke(this, menuItem, itemIndex);
        }

        protected virtual void CheckboxChangedEvent(MenuCheckboxItem menuItem, int itemIndex, bool _checked)
        {
            OnCheckboxChange?.Invoke(this, menuItem, itemIndex, _checked);
        }
        #endregion

        #region constants or readonlys
        public const float Width = 500f;

        #endregion

        #region private variables
        private static SizeF headerSize = new SizeF(500f, 110f);

        private int viewIndexOffset = 0;

        private List<MenuItem> VisibleMenuItems
        {
            get
            {
                var items = _MenuItems.GetRange(viewIndexOffset, Math.Min(MaxItemsOnScreen, Size - viewIndexOffset));
                return items;
            }
        }

        private List<MenuItem> _MenuItems { get; set; } = new List<MenuItem>();

        #endregion

        #region Public Variables
        public string MenuTitle { get; set; }

        public string MenuSubtitle { get; set; }

        public int MaxItemsOnScreen { get; set; } = 10;

        public int Size => _MenuItems.Count;

        public bool Visible { get; set; } = false;

        public bool LeftAligned => MenuController.MenuAlignment == MenuController.MenuAlignmentOption.Left;

        public PointF Position { get; private set; } = new PointF(0f, 0f);

        public float MenuItemsYOffset { get; private set; } = 0f;

        public string CounterPreText { get; set; }

        public Menu ParentMenu { get; internal set; } = null;

        public int CurrentIndex { get; internal set; } = 0;
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

        #region Public functions
        /// <summary>
        /// Adds a <see cref="MenuItem"/> to this <see cref="Menu"/>.
        /// </summary>
        /// <param name="item"></param>
        public void AddMenuItem(MenuItem item)
        {
            _MenuItems.Add(item);
            item.PositionOnScreen = item.Index;
            item.ParentMenu = this;
        }

        /// <summary>
        /// Returns the menu items in this menu.
        /// </summary>
        /// <returns></returns>
        public List<MenuItem> GetMenuItems()
        {
            return _MenuItems.ToList();
        }

        /// <summary>
        /// Removes the specified <see cref="MenuItem"/> from this <see cref="Menu"/>.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveMenuItem(MenuItem item)
        {
            if (_MenuItems.Contains(item))
            {
                _MenuItems.Remove(item);
            }
        }

        /// <summary>
        /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(int index)
        {
            if (index > -1 && _MenuItems.Count - 1 >= index)
            {
                SelectItem(_MenuItems[index]);
            }
        }

        /// <summary>
        /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(MenuItem item)
        {
            if (item != null && item.Enabled)
            {
                if (item is MenuCheckboxItem checkbox)
                {
                    checkbox.Checked = !checkbox.Checked;
                    CheckboxChangedEvent(checkbox, item.Index, checkbox.Checked);
                }
                else
                {
                    ItemSelectedEvent(item, item.Index);
                }
                PlaySoundFrontend(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                if (MenuController.MenuButtons.ContainsKey(item))
                {
                    var currentMenu = MenuController.GetCurrentMenu();
                    if (currentMenu != null)
                    {
                        currentMenu.Visible = false;
                    }
                    MenuController.MenuButtons[item].Visible = true;
                }
            }
            else if (item != null && !item.Enabled)
                PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }

        /// <summary>
        /// Returns to the parent menu. If there's no parent menu, then the current menu just closes.
        /// </summary>
        public void GoBack()
        {
            if (ParentMenu != null)
            {
                ParentMenu.Visible = true;
            }
            PlaySoundFrontend(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            Visible = false;
        }

        /// <summary>
        /// Closes the current menu.
        /// </summary>
        public void CloseMenu()
        {
            Visible = false;
            PlaySoundFrontend(-1, "BACK", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }

        /// <summary>
        /// Goes up one menu item if possible.
        /// </summary>
        public void GoUp()
        {
            if (Visible && Size > 1)
            {
                CurrentIndex--; if (CurrentIndex < 0)
                {
                    CurrentIndex = Size - 1;
                }
                if (!VisibleMenuItems.Contains(_MenuItems[CurrentIndex]))
                {
                    viewIndexOffset--;
                    if (viewIndexOffset < 0)
                    {
                        viewIndexOffset = Math.Max(Size - MaxItemsOnScreen, 0);
                    }
                }
                PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            }
        }

        /// <summary>
        /// Goes down one menu item if possible.
        /// </summary>
        public void GoDown()
        {
            if (Visible && Size > 1)
            {
                CurrentIndex++;
                if (CurrentIndex >= Size)
                {
                    CurrentIndex = 0;
                }
                if (!VisibleMenuItems.Contains(_MenuItems[CurrentIndex]))
                {
                    viewIndexOffset++;
                    if (CurrentIndex == 0)
                    {
                        viewIndexOffset = 0;
                    }
                }
                PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
            }
        }

        public void GoLeft() { }
        public void GoRight() { }

        #endregion

        #region internal task function
        /// <summary>
        /// Draws the menu title + subtitle, calls all Draw functions for all menu items and draws the description for the selected item.
        /// </summary>
        /// <returns></returns>
        internal async Task Draw()
        {
            if (!Game.IsPaused && IsScreenFadedIn() && !IsPlayerSwitchInProgress() && !Game.PlayerPed.IsDead)
            {
                // impossible to reach this code, but i don't like Visual studio warnings.
                if (CurrentIndex == -2) { await BaseScript.Delay(0); }

                MenuItemsYOffset = 0f;

                #region Draw Header
                if (!string.IsNullOrEmpty(MenuTitle))
                {
                    #region Draw Header Background
                    SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    float x = (Position.X + (headerSize.Width / 2f)) / MenuController.ScreenWidth;
                    float y = (Position.Y + (headerSize.Height / 2f)) / MenuController.ScreenHeight;
                    float width = headerSize.Width / MenuController.ScreenWidth;
                    float height = headerSize.Height / MenuController.ScreenHeight;

                    DrawSprite(MenuController._texture_dict, MenuController._header_texture, x, y, width, height, 0f, 255, 255, 255, 255);

                    ResetScriptGfxAlign();
                    #endregion

                    #region Draw Header Menu Title
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
                        EndTextCommandDisplayText(((headerSize.Width / 2f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f));
                    }
                    else
                    {
                        EndTextCommandDisplayText(GetSafeZoneSize() - ((headerSize.Width / 2f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f));
                    }

                    ResetScriptGfxAlign();

                    MenuItemsYOffset = headerSize.Height;
                    #endregion
                }
                #endregion

                #region Draw Subtitle
                {
                    #region draw subtitle background
                    SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    float bgHeight = 38f;


                    float x = (Position.X + (headerSize.Width / 2f)) / MenuController.ScreenWidth;
                    float y = ((Position.Y + MenuItemsYOffset + (bgHeight / 2f)) / MenuController.ScreenHeight);
                    float width = headerSize.Width / MenuController.ScreenWidth;
                    float height = bgHeight / MenuController.ScreenHeight;

                    DrawRect(x, y, width, height, 0, 0, 0, 250);
                    ResetScriptGfxAlign();
                    #endregion

                    #region draw subtitle text
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
                            EndTextCommandDisplayText(10f / MenuController.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)));
                        }
                        else
                        {
                            EndTextCommandDisplayText(GetSafeZoneSize() - ((headerSize.Width - 10f) / MenuController.ScreenWidth), y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)));
                        }

                        ResetScriptGfxAlign();
                    }
                    #endregion

                    #region draw counter + pre-counter text
                    string counterText = $"{CounterPreText ?? ""}{CurrentIndex + 1} / {Size}";
                    if (CounterPreText != null || MaxItemsOnScreen < Size)
                    {
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
                            SetTextWrap(0f, (485f / MenuController.ScreenWidth));
                            EndTextCommandDisplayText(10f / MenuController.ScreenWidth, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)));
                        }
                        else
                        {
                            SetTextWrap(0f, GetSafeZoneSize() - (10f / MenuController.ScreenWidth));
                            EndTextCommandDisplayText(0f, y - (GetTextScaleHeight(size, font) / 2f + (4f / MenuController.ScreenHeight)));
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

                #region Draw menu items background gradient
                if (Size > 0)
                {
                    SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    float bgHeight = 38f * MathUtil.Clamp(_MenuItems.Count, 0, MaxItemsOnScreen);


                    float x = (Position.X + (headerSize.Width / 2f)) / MenuController.ScreenWidth;
                    float y = ((Position.Y + MenuItemsYOffset + ((bgHeight + 1f) / 2f)) / MenuController.ScreenHeight);
                    float width = headerSize.Width / MenuController.ScreenWidth;
                    float height = (bgHeight + 1f) / MenuController.ScreenHeight;

                    DrawSprite(MenuController._texture_dict, "gradient_bgd", x, y, width, height, 0f, 255, 255, 255, 255);
                    ResetScriptGfxAlign();
                    MenuItemsYOffset += bgHeight - 1f;
                }
                #endregion

                #region Draw menu items that are visible in the current view.
                if (Size > 0)
                {
                    foreach (var item in VisibleMenuItems)
                    {
                        item.Draw(viewIndexOffset);
                    }
                }
                #endregion

                #region Up Down overflow Indicator
                float descriptionYOffset = 0f;
                if (Size > 0)
                {
                    if (Size > MaxItemsOnScreen)
                    {
                        #region background
                        float width = 500f / MenuController.ScreenWidth;
                        float height = 60f / MenuController.ScreenWidth;
                        float x = (Position.X + (Width / 2f)) / MenuController.ScreenWidth;
                        float y = MenuItemsYOffset / MenuController.ScreenHeight + (height / 2f) + (2f / MenuController.ScreenHeight);

                        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        DrawRect(x, y, width, height, 0, 0, 0, 200);
                        descriptionYOffset = height;
                        ResetScriptGfxAlign();
                        #endregion

                        #region up/down icons
                        SetScriptGfxAlign(76, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
                        float xMin = 0f;
                        float xMax = 500f / MenuController.ScreenWidth;
                        float xCenter = 250f / MenuController.ScreenWidth;
                        float yTop = y - (20f / MenuController.ScreenHeight);
                        float yBottom = y - (10f / MenuController.ScreenHeight);

                        BeginTextCommandDisplayText("STRING");
                        AddTextComponentSubstringPlayerName("↑");

                        SetTextFont(0);
                        SetTextScale(0.35f, 0.35f);
                        SetTextJustification(0);
                        if (LeftAligned)
                        {
                            SetTextWrap(xMin, xMax);
                            EndTextCommandDisplayText(xCenter, yTop);
                        }
                        else
                        {
                            xMin = GetSafeZoneSize() - ((Width - 10f) / MenuController.ScreenWidth);
                            xMax = GetSafeZoneSize() - (10f / MenuController.ScreenWidth);
                            xCenter = GetSafeZoneSize() - (250f / MenuController.ScreenWidth);
                            SetTextWrap(xMin, xMax);
                            EndTextCommandDisplayText(xCenter, yTop);
                        }

                        BeginTextCommandDisplayText("STRING");
                        AddTextComponentSubstringPlayerName("↓");

                        SetTextFont(0);
                        SetTextScale(0.35f, 0.35f);
                        SetTextJustification(0);
                        if (LeftAligned)
                        {
                            SetTextWrap(xMin, xMax);
                            EndTextCommandDisplayText(xCenter, yBottom);
                        }
                        else
                        {
                            SetTextWrap(xMin, xMax);
                            EndTextCommandDisplayText(xCenter, yBottom);
                        }

                        ResetScriptGfxAlign();
                        #endregion
                    }
                }

                #endregion

                #region Draw Description
                if (Size > 0)
                {
                    if (!string.IsNullOrEmpty(_MenuItems[CurrentIndex].Description))
                    {

                        #region description text
                        int font = 0;
                        float textSize = 0.35f;

                        float textMinX = 0f + (10f / MenuController.ScreenWidth);
                        float textMaxX = Width / MenuController.ScreenWidth - (10f / MenuController.ScreenWidth);
                        float textY = MenuItemsYOffset / MenuController.ScreenHeight + (16f / MenuController.ScreenHeight) + descriptionYOffset;

                        SetScriptGfxAlign(76, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        BeginTextCommandDisplayText("CELL_EMAIL_BCON");
                        SetTextFont(font);
                        SetTextScale(textSize, textSize);
                        SetTextJustification(1);
                        string text = _MenuItems[CurrentIndex].Description;
                        foreach (string s in CitizenFX.Core.UI.Screen.StringToArray(text))
                        {
                            AddTextComponentSubstringPlayerName(s);
                        }
                        float textHeight = GetTextScaleHeight(textSize, font);
                        if (LeftAligned)
                        {
                            SetTextWrap(textMinX, textMaxX);
                            EndTextCommandDisplayText(textMinX, textY);
                        }
                        else
                        {
                            textMinX = GetSafeZoneSize() - ((Width - 10f) / MenuController.ScreenWidth);
                            textMaxX = GetSafeZoneSize() - (10f / MenuController.ScreenWidth);
                            SetTextWrap(textMinX, textMaxX);
                            EndTextCommandDisplayText(textMinX, textY);
                        }

                        BeginTextCommandLineCount("CELL_EMAIL_BCON");
                        SetTextScale(textSize, textSize);
                        SetTextJustification(1);
                        SetTextFont(font);
                        int lineCount = 1;
                        foreach (string s in CitizenFX.Core.UI.Screen.StringToArray(text))
                        {
                            AddTextComponentSubstringPlayerName(s);
                        }
                        if (LeftAligned)
                        {
                            SetTextWrap(textMinX, textMaxX);
                            lineCount = GetTextScreenLineCount(textMinX, textY);
                        }
                        else
                        {
                            SetTextWrap(textMinX, textMaxX);
                            lineCount = GetTextScreenLineCount(textMinX, textY);
                        }

                        ResetScriptGfxAlign();

                        #endregion

                        #region background
                        float descWidth = Width / MenuController.ScreenWidth;
                        float descHeight = (textHeight + 0.005f) * lineCount + (8f / MenuController.ScreenHeight) + (1.5f / MenuController.ScreenHeight);
                        float descX = (Position.X + (Width / 2f)) / MenuController.ScreenWidth;
                        float descY = textY - (6f / MenuController.ScreenHeight) + (descHeight / 2f);

                        SetScriptGfxAlign(LeftAligned ? 76 : 82, 84);
                        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                        DrawRect(descX, descY - (descHeight / 2f) + (2f / MenuController.ScreenHeight), descWidth, 4f / MenuController.ScreenHeight, 0, 0, 0, 200);
                        DrawSprite(MenuController._texture_dict, "gradient_bgd", descX, descY, descWidth, descHeight, 0f, 255, 255, 255, 225);

                        ResetScriptGfxAlign();
                        #endregion
                    }
                }


                #endregion
            }
        }
        #endregion

    }
}
