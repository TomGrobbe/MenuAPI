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
            MALE,
            LOCK_ARENA,
            ADVERSARY,
            BASE_JUMPING,
            BRIEFCASE,
            MISSION_STAR,
            DEATHMATCH,
            CASTLE,
            CROWN2,
            THREE_CROWNS,
            TROPHY,
            RACE_FLAG,
            RACE_FLAG_PLANE,
            RACE_FLAG_BICYCLE,
            RACE_FLAG_PERSON,
            RACE_FLAG_CAR,
            RACE_FLAG_BOAT_ANCHOR,
            ROCKSTAR,
            STUNT,
            STUNT_PREMIUM,
            RACE_FLAG_STUNT_JUMP,
            SHIELD,
            TEAM_DEATHMATCH,
            VEHICLE_DEATHMATCH,
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

        protected string GetSpriteDictionary(Icon icon)
        {
            switch (icon)
            {
                case Icon.MALE:
                case Icon.FEMALE:
                    return "mpleaderboard";
                case Icon.ADVERSARY:
                case Icon.BASE_JUMPING:
                case Icon.BRIEFCASE:
                case Icon.MISSION_STAR:
                case Icon.DEATHMATCH:
                case Icon.CASTLE:
                case Icon.CROWN2:
                case Icon.THREE_CROWNS:
                case Icon.TROPHY:
                case Icon.RACE_FLAG:
                case Icon.RACE_FLAG_PLANE:
                case Icon.RACE_FLAG_BICYCLE:
                case Icon.RACE_FLAG_PERSON:
                case Icon.RACE_FLAG_CAR:
                case Icon.RACE_FLAG_BOAT_ANCHOR:
                case Icon.ROCKSTAR:
                case Icon.STUNT:
                case Icon.STUNT_PREMIUM:
                case Icon.RACE_FLAG_STUNT_JUMP:
                case Icon.SHIELD:
                case Icon.TEAM_DEATHMATCH:
                case Icon.VEHICLE_DEATHMATCH:
                    return "commonmenutu";
                default:
                    return "commonmenu";
            }
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
                case Icon.LOCK_ARENA: return "shop_lock_arena";
                case Icon.ADVERSARY: return "adversary";
                case Icon.BASE_JUMPING: return "base_jumping";
                case Icon.BRIEFCASE: return "capture_the_flag";
                case Icon.MISSION_STAR: return "custom_mission";
                case Icon.DEATHMATCH: return "deathmatch";
                case Icon.CASTLE: return "gang_attack";
                case Icon.CROWN2: return "king_of_the_hill";
                case Icon.THREE_CROWNS: return "king_of_the_hill_teams";
                case Icon.TROPHY: return "last_team_standing";
                case Icon.RACE_FLAG: return "race";
                case Icon.RACE_FLAG_PLANE: return "race_air";
                case Icon.RACE_FLAG_BICYCLE: return "race_bicycle";
                case Icon.RACE_FLAG_PERSON: return "race_foot";
                case Icon.RACE_FLAG_CAR: return "race_land";
                case Icon.RACE_FLAG_BOAT_ANCHOR: return "race_water";
                case Icon.ROCKSTAR: return "rockstar";
                case Icon.STUNT: return "stunt";
                case Icon.STUNT_PREMIUM: return "stunt_premium";
                case Icon.RACE_FLAG_STUNT_JUMP: return "stunt_race";
                case Icon.SHIELD: return "survival";
                case Icon.TEAM_DEATHMATCH: return "team_deathmatch";
                case Icon.VEHICLE_DEATHMATCH: return "vehicle_deathmatch";
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
                case Icon.ADVERSARY:
                case Icon.BASE_JUMPING:
                case Icon.BRIEFCASE:
                case Icon.MISSION_STAR:
                case Icon.DEATHMATCH:
                case Icon.CASTLE:
                case Icon.CROWN2:
                case Icon.THREE_CROWNS:
                case Icon.TROPHY:
                case Icon.RACE_FLAG:
                case Icon.RACE_FLAG_PLANE:
                case Icon.RACE_FLAG_BICYCLE:
                case Icon.RACE_FLAG_PERSON:
                case Icon.RACE_FLAG_CAR:
                case Icon.RACE_FLAG_BOAT_ANCHOR:
                case Icon.ROCKSTAR:
                case Icon.STUNT:
                case Icon.STUNT_PREMIUM:
                case Icon.RACE_FLAG_STUNT_JUMP:
                case Icon.SHIELD:
                case Icon.TEAM_DEATHMATCH:
                case Icon.VEHICLE_DEATHMATCH:
                    return 30f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);

                case Icon.STAR:
                case Icon.LOCK_ARENA:
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
                case Icon.LOCK:
                case Icon.LOCK_ARENA:
                case Icon.ADVERSARY:
                case Icon.BASE_JUMPING:
                case Icon.BRIEFCASE:
                case Icon.MISSION_STAR:
                case Icon.DEATHMATCH:
                case Icon.CASTLE:
                case Icon.CROWN2:
                case Icon.THREE_CROWNS:
                case Icon.TROPHY:
                case Icon.RACE_FLAG:
                case Icon.RACE_FLAG_PLANE:
                case Icon.RACE_FLAG_BICYCLE:
                case Icon.RACE_FLAG_PERSON:
                case Icon.RACE_FLAG_CAR:
                case Icon.RACE_FLAG_BOAT_ANCHOR:
                case Icon.ROCKSTAR:
                case Icon.STUNT:
                case Icon.STUNT_PREMIUM:
                case Icon.RACE_FLAG_STUNT_JUMP:
                case Icon.SHIELD:
                case Icon.TEAM_DEATHMATCH:
                case Icon.VEHICLE_DEATHMATCH:
                    return selected ? (Enabled ? 0 : 50) : (Enabled ? 255 : 109);
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
                case Icon.LOCK_ARENA:
                case Icon.ADVERSARY:
                case Icon.BASE_JUMPING:
                case Icon.BRIEFCASE:
                case Icon.MISSION_STAR:
                case Icon.DEATHMATCH:
                case Icon.CASTLE:
                case Icon.CROWN2:
                case Icon.THREE_CROWNS:
                case Icon.TROPHY:
                case Icon.RACE_FLAG:
                case Icon.RACE_FLAG_PLANE:
                case Icon.RACE_FLAG_BICYCLE:
                case Icon.RACE_FLAG_PERSON:
                case Icon.RACE_FLAG_CAR:
                case Icon.RACE_FLAG_BOAT_ANCHOR:
                case Icon.ROCKSTAR:
                case Icon.STUNT:
                case Icon.STUNT_PREMIUM:
                case Icon.RACE_FLAG_STUNT_JUMP:
                case Icon.SHIELD:
                case Icon.TEAM_DEATHMATCH:
                case Icon.VEHICLE_DEATHMATCH:
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
                case Icon.LOCK_ARENA:
                case Icon.ADVERSARY:
                case Icon.BASE_JUMPING:
                case Icon.BRIEFCASE:
                case Icon.MISSION_STAR:
                case Icon.DEATHMATCH:
                case Icon.CASTLE:
                case Icon.CROWN2:
                case Icon.THREE_CROWNS:
                case Icon.TROPHY:
                case Icon.RACE_FLAG:
                case Icon.RACE_FLAG_PLANE:
                case Icon.RACE_FLAG_BICYCLE:
                case Icon.RACE_FLAG_PERSON:
                case Icon.RACE_FLAG_CAR:
                case Icon.RACE_FLAG_BOAT_ANCHOR:
                case Icon.ROCKSTAR:
                case Icon.STUNT:
                case Icon.STUNT_PREMIUM:
                case Icon.RACE_FLAG_STUNT_JUMP:
                case Icon.SHIELD:
                case Icon.TEAM_DEATHMATCH:
                case Icon.VEHICLE_DEATHMATCH:
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
                    string textureDictionary = GetSpriteDictionary(LeftIcon);
                    //string textureDictionary = "commonmenu";
                    //if (LeftIcon == Icon.MALE || LeftIcon == Icon.FEMALE)
                    //{
                    //    textureDictionary = "mpleaderboard";
                    //}

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
                    string textureDictionary = GetSpriteDictionary(RightIcon);
                    //string textureDictionary = "commonmenu";
                    //if (RightIcon == Icon.MALE || RightIcon == Icon.FEMALE)
                    //{
                    //    textureDictionary = "mpleaderboard";
                    //}

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
                //float textHeight = GetTextScaleHeight(textSize, font);
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
