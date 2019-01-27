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
            CROWN,
            MEDAL_BRONZE,
            MEDAL_GOLD,
            MEDAL_SILVER,
            CASH,
            COKE,
            HEROIN,
            METH,
            WEED,
            AMMO,
            ARMOR,
            BARBER,
            CLOTHING,
            FRANKLIN,
            BIKE,
            CAR,
            GUN,
            HEALTH_HEART,
            MAKEUP_BRUSH,
            MASK,
            MICHAEL,
            TATTOO,
            TICK,
            TREVOR,
            FEMALE,
            MALE
        }

        public string Text { get; set; }
        public string Label { get; set; }
        public Icon LeftIcon { get; set; }
        public Icon RightIcon { get; set; }
        public bool Enabled { get; set; } = true;
        public string Description { get; set; }
        public int Index { get { if (ParentMenu != null) return ParentMenu.GetMenuItems().IndexOf(this); return -1; } } //{ get; internal set; }
        public bool Selected { get { if (ParentMenu != null) { return ParentMenu.CurrentIndex == Index; } return false; } }
        public Menu ParentMenu { get; set; }
        public int PositionOnScreen { get; internal set; }
        protected const float Width = Menu.Width;
        protected const float RowHeight = 38f;
        
        // Allows you to attach data to a menu item if you want to identify the menu item without having to put identification info in the visible text or description.
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

        protected string GetSpriteName(Icon icon, bool selected)
        {
            switch (icon)
            {
                case Icon.AMMO: return selected ? "shop_ammo_icon_b" : "shop_ammo_icon_a";
                case Icon.ARMOR: return selected ? "shop_armour_icon_b" : "shop_armour_icon_a";
                case Icon.BARBER: return selected ? "shop_barber_icon_b" : "shop_barber_icon_a";
                case Icon.BIKE: return selected ? "shop_garage_bike_icon_b" : "shop_garage_bike_icon_a";
                case Icon.CAR: return selected ? "shop_garage_icon_b" : "shop_garage_icon_a";
                case Icon.CASH: return "mp_specitem_cash";
                case Icon.CLOTHING: return selected ? "shop_clothing_icon_b" : "shop_clothing_icon_a";
                case Icon.COKE: return "mp_specitem_coke";
                case Icon.CROWN: return "mp_hostcrown";
                case Icon.FRANKLIN: return selected ? "shop_franklin_icon_b" : "shop_franklin_icon_a";
                case Icon.GUN: return selected ? "shop_gunclub_icon_b" : "shop_gunclub_icon_a";
                case Icon.HEALTH_HEART: return selected ? "shop_health_icon_b" : "shop_health_icon_a";
                case Icon.HEROIN: return "mp_specitem_heroin";
                case Icon.LOCK: return "shop_lock";
                case Icon.MAKEUP_BRUSH: return selected ? "shop_makeup_icon_b" : "shop_makeup_icon_a";
                case Icon.MASK: return selected ? "shop_mask_icon_b" : "shop_mask_icon_a";
                case Icon.MEDAL_BRONZE: return "mp_medal_bronze";
                case Icon.MEDAL_GOLD: return "mp_medal_gold";
                case Icon.MEDAL_SILVER: return "mp_medal_silver";
                case Icon.METH: return "mp_specitem_meth";
                case Icon.MICHAEL: return selected ? "shop_michael_icon_b" : "shop_michael_icon_a";
                case Icon.STAR: return "shop_new_star";
                case Icon.TATTOO: return selected ? "shop_tattoos_icon_b" : "shop_tattoos_icon_a";
                case Icon.TICK: return "shop_tick_icon";
                case Icon.TREVOR: return selected ? "shop_trevor_icon_b" : "shop_trevor_icon_a";
                case Icon.WARNING: return "mp_alerttriangle";
                case Icon.WEED: return "mp_specitem_weed";
                case Icon.MALE: return "leaderboard_male_icon";
                case Icon.FEMALE: return "leaderboard_female_icon";
                default:
                    break;
            }
            return "";
        }

        protected float GetSpriteSize(Icon icon, bool width)
        {
            switch (icon)
            {
                case Icon.CASH:
                case Icon.COKE:
                case Icon.CROWN:
                case Icon.HEROIN:
                case Icon.METH:
                case Icon.WEED:
                    return 30f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);

                case Icon.STAR:
                    return 52f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);
                case Icon.MEDAL_SILVER:
                    return 22f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);
                default:
                    return 38f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);
            }
        }

        protected int GetSpriteColour(Icon icon, bool selected)
        {
            switch (icon)
            {
                case Icon.CROWN:
                case Icon.TICK:
                case Icon.MALE:
                case Icon.FEMALE:
                //return selected ? 0 : 255;
                case Icon.LOCK: return selected ? (Enabled ? 0 : 50) : (Enabled ? 255 : 109);
                default:
                    return 255;
            }
        }

        protected float GetSpriteX(Icon icon, bool leftAligned, bool leftSide)
        {
            switch (icon)
            {
                case Icon.AMMO:
                case Icon.ARMOR:
                case Icon.BARBER:
                case Icon.BIKE:
                case Icon.CAR:
                case Icon.CASH:
                case Icon.CLOTHING:
                case Icon.COKE:
                case Icon.CROWN:
                case Icon.FRANKLIN:
                case Icon.GUN:
                case Icon.HEALTH_HEART:
                case Icon.HEROIN:
                case Icon.LOCK:
                case Icon.MAKEUP_BRUSH:
                case Icon.MASK:
                case Icon.MEDAL_BRONZE:
                case Icon.MEDAL_GOLD:
                case Icon.MEDAL_SILVER:
                case Icon.METH:
                case Icon.MICHAEL:
                case Icon.STAR:
                case Icon.TATTOO:
                case Icon.TICK:
                case Icon.TREVOR:
                case Icon.WARNING:
                case Icon.WEED:
                case Icon.FEMALE:
                case Icon.MALE:
                    return leftSide ? (leftAligned ? (20f / MenuController.ScreenWidth) : GetSafeZoneSize() - ((Width - 20f) / MenuController.ScreenWidth)) : (leftAligned ? (Width - 20f) / MenuController.ScreenWidth : (GetSafeZoneSize() - (20f / MenuController.ScreenWidth)));
                default:
                    break;
            }
            return 0f;
        }

        protected float GetSpriteY(Icon icon)
        {
            switch (icon)
            {
                case Icon.AMMO:
                case Icon.ARMOR:
                case Icon.BARBER:
                case Icon.BIKE:
                case Icon.CAR:
                case Icon.CASH:
                case Icon.CLOTHING:
                case Icon.COKE:
                case Icon.CROWN:
                case Icon.FRANKLIN:
                case Icon.GUN:
                case Icon.HEALTH_HEART:
                case Icon.HEROIN:
                case Icon.LOCK:
                case Icon.MAKEUP_BRUSH:
                case Icon.MASK:
                case Icon.MEDAL_BRONZE:
                case Icon.MEDAL_GOLD:
                case Icon.MEDAL_SILVER:
                case Icon.METH:
                case Icon.MICHAEL:
                case Icon.STAR:
                case Icon.TATTOO:
                case Icon.TICK:
                case Icon.TREVOR:
                case Icon.WARNING:
                case Icon.WEED:
                case Icon.MALE:
                case Icon.FEMALE:
                    break;
                default:
                    break;
            }
            return 0f;
        }


        /// <summary>
        /// Draws the item on the screen.
        /// </summary>
        internal virtual void Draw(int indexOffset)
        {
            if (ParentMenu != null)
            {
                float yOffset = ParentMenu.MenuItemsYOffset + 1f - (RowHeight * MathUtil.Clamp(ParentMenu.Size, 0, ParentMenu.MaxItemsOnScreen));

                #region Background Rect
                SetScriptGfxAlign(ParentMenu.LeftAligned ? 76 : 82, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                float x = (ParentMenu.Position.X + (Width / 2f)) / MenuController.ScreenWidth;
                float y = (ParentMenu.Position.Y + ((Index - indexOffset) * RowHeight) + (20f) + yOffset) / MenuController.ScreenHeight;
                float width = Width / MenuController.ScreenWidth;
                float height = (RowHeight) / MenuController.ScreenHeight;

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

                    string name = GetSpriteName(LeftIcon, Selected);
                    float spriteY = y;// GetSpriteY(LeftIcon);
                    float spriteX = GetSpriteX(LeftIcon, ParentMenu.LeftAligned, true);
                    float spriteHeight = GetSpriteSize(LeftIcon, false);
                    float spriteWidth = GetSpriteSize(LeftIcon, true);
                    int spriteColor = GetSpriteColour(LeftIcon, Selected);
                    string textureDictionary = "commonmenu";
                    if (LeftIcon == Icon.MALE || LeftIcon == Icon.FEMALE)
                    {
                        textureDictionary = "mpleaderboard";
                    }

                    DrawSprite(textureDictionary, name, spriteX, spriteY, spriteWidth, spriteHeight, 0f, spriteColor, spriteColor, spriteColor, 255);
                    ResetScriptGfxAlign();
                }
                #endregion

                float rightTextIconOffset = 0f;
                #region Right Icon
                if (RightIcon != Icon.NONE)
                {
                    rightTextIconOffset = 25f;

                    SetScriptGfxAlign(76, 84);
                    SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

                    string name = GetSpriteName(RightIcon, Selected);
                    float spriteY = y;// GetSpriteY(RightIcon);
                    float spriteX = GetSpriteX(RightIcon, ParentMenu.LeftAligned, false);
                    float spriteHeight = GetSpriteSize(RightIcon, false);
                    float spriteWidth = GetSpriteSize(RightIcon, true);
                    int spriteColor = GetSpriteColour(RightIcon, Selected);
                    string textureDictionary = "commonmenu";
                    if (RightIcon == Icon.MALE || RightIcon == Icon.FEMALE)
                    {
                        textureDictionary = "mpleaderboard";
                    }

                    DrawSprite(textureDictionary, name, spriteX, spriteY, spriteWidth, spriteHeight, 0f, spriteColor, spriteColor, spriteColor, 255);
                    ResetScriptGfxAlign();
                }
                #endregion

                #region Text
                int font = 0;
                float textSize = (14f * 27f) / MenuController.ScreenHeight;
                //float textSize = 0.34f;

                SetScriptGfxAlign(76, 84);
                SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
                SetTextFont(font);
                SetTextScale(textSize, textSize);
                SetTextJustification(1);
                BeginTextCommandDisplayText("STRING");
                AddTextComponentSubstringPlayerName(Text ?? "N/A");
                int textColor = Selected ? (Enabled ? 0 : 50) : (Enabled ? 255 : 109);
                if (Selected || !Enabled)
                {
                    SetTextColour(textColor, textColor, textColor, 255);
                }
                //selected ? (Enabled ? 0 : 50) : (Enabled ? 255 : 109);
                //if (Selected)
                //{
                //    if (Enabled)
                //        SetTextColour(textColor, textColor, textColor, 255);
                //    else
                //        SetTextColour(textColor, textColor, textColor, 255);
                //}
                //else
                //{
                //    if (!Enabled)
                //        SetTextColour(textColor, textColor, textColor, 255);
                //}
                float textMinX = (textXOffset / MenuController.ScreenWidth) + (10f / MenuController.ScreenWidth);
                float textMaxX = (Width - 10f) / MenuController.ScreenWidth;
                float textHeight = GetTextScaleHeight(textSize, font);
                float textY = y - ((30f / 2f) / MenuController.ScreenHeight);
                if (ParentMenu.LeftAligned)
                {
                    SetTextWrap(textMinX, textMaxX);
                    EndTextCommandDisplayText(textMinX, textY);
                }
                else
                {
                    textMinX = (textXOffset / MenuController.ScreenWidth) + GetSafeZoneSize() - ((Width - 10f) / MenuController.ScreenWidth);
                    textMaxX = GetSafeZoneSize() - (10f / MenuController.ScreenWidth);
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
                    if (Selected || !Enabled)
                    {
                        SetTextColour(textColor, textColor, textColor, 255);
                    }
                    //if (Selected)
                    //{
                    //    SetTextColour(0, 0, 0, 255);
                    //}
                    if (ParentMenu.LeftAligned)
                    {
                        SetTextWrap(0f, ((490f - rightTextIconOffset) / MenuController.ScreenWidth));
                        EndTextCommandDisplayText((10f + rightTextIconOffset) / MenuController.ScreenWidth, textY);
                    }
                    else
                    {
                        SetTextWrap(0f, GetSafeZoneSize() - ((10f + rightTextIconOffset) / MenuController.ScreenWidth));
                        EndTextCommandDisplayText(0f, textY);
                    }

                    ResetScriptGfxAlign();
                }
                #endregion



            }
        }

    }
}
