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
            MP_AMMO_PICKUP,
            MP_AMMO,
            MP_CASH,
            MP_RP,
            MP_SPECTATING,
            SALE,
            GLOBE_WHITE,
            GLOBE_RED,
            GLOBE_BLUE,
            GLOBE_YELLOW,
            GLOBE_GREEN,
            GLOBE_ORANGE,
            INV_ARM_WRESTLING,
            INV_BASEJUMP,
            INV_MISSION,
            INV_DARTS,
            INV_DEATHMATCH,
            INV_DRUG,
            INV_CASTLE,
            INV_GOLF,
            INV_BIKE,
            INV_BOAT,
            INV_ANCHOR,
            INV_CAR,
            INV_DOLLAR,
            INV_COKE,
            INV_KEY,
            INV_DATA,
            INV_HELI,
            INV_HEORIN,
            INV_KEYCARD,
            INV_METH,
            INV_BRIEFCASE,
            INV_LINK,
            INV_PERSON,
            INV_PLANE,
            INV_PLANE2,
            INV_QUESTIONMARK,
            INV_REMOTE,
            INV_SAFE,
            INV_STEER_WHEEL,
            INV_WEAPON,
            INV_WEED,
            INV_RACE_FLAG_PLANE,
            INV_RACE_FLAG_BICYCLE,
            INV_RACE_FLAG_BOAT_ANCHOR,
            INV_RACE_FLAG_PERSON,
            INV_RACE_FLAG_CAR,
            INV_RACE_FLAG_HELMET,
            INV_SHOOTING_RANGE,
            INV_SURVIVAL,
            INV_TEAM_DEATHMATCH,
            INV_TENNIS,
            INV_VEHICLE_DEATHMATCH,
            AUDIO_MUTE,
            AUDIO_INACTIVE,
            AUDIO_VOL1,
            AUDIO_VOL2,
            AUDIO_VOL3,
            COUNTRY_USA,
            COUNTRY_UK,
            COUNTRY_SWEDEN,
            COUNTRY_KOREA,
            COUNTRY_JAPAN,
            COUNTRY_ITALY,
            COUNTRY_GERMANY,
            COUNTRY_FRANCE,
            BRAND_ALBANY,
            BRAND_ANNIS,
            BRAND_BANSHEE,
            BRAND_BENEFACTOR,
            BRAND_BF,
            BRAND_BOLLOKAN,
            BRAND_BRAVADO,
            BRAND_BRUTE,
            BRAND_BUCKINGHAM,
            BRAND_CANIS,
            BRAND_CHARIOT,
            BRAND_CHEVAL,
            BRAND_CLASSIQUE,
            BRAND_COIL,
            BRAND_DECLASSE,
            BRAND_DEWBAUCHEE,
            BRAND_DILETTANTE,
            BRAND_DINKA,
            BRAND_DUNDREARY,
            BRAND_EMPORER,
            BRAND_ENUS,
            BRAND_FATHOM,
            BRAND_GALIVANTER,
            BRAND_GROTTI,
            BRAND_GROTTI2,
            BRAND_HIJAK,
            BRAND_HVY,
            BRAND_IMPONTE,
            BRAND_INVETERO,
            BRAND_JACKSHEEPE,
            BRAND_LCC,
            BRAND_JOBUILT,
            BRAND_KARIN,
            BRAND_LAMPADATI,
            BRAND_MAIBATSU,
            BRAND_MAMMOTH,
            BRAND_MTL,
            BRAND_NAGASAKI,
            BRAND_OBEY,
            BRAND_OCELOT,
            BRAND_OVERFLOD,
            BRAND_PED,
            BRAND_PEGASSI,
            BRAND_PFISTER,
            BRAND_PRINCIPE,
            BRAND_PROGEN,
            BRAND_PROGEN2,
            BRAND_RUNE,
            BRAND_SCHYSTER,
            BRAND_SHITZU,
            BRAND_SPEEDOPHILE,
            BRAND_STANLEY,
            BRAND_TRUFFADE,
            BRAND_UBERMACHT,
            BRAND_VAPID,
            BRAND_VULCAR,
            BRAND_WEENY,
            BRAND_WESTERN,
            BRAND_WESTERNMOTORCYCLE,
            BRAND_WILLARD,
            BRAND_ZIRCONIUM,
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
                case Icon.AUDIO_MUTE:
                case Icon.AUDIO_INACTIVE:
                case Icon.AUDIO_VOL1:
                case Icon.AUDIO_VOL2:
                case Icon.AUDIO_VOL3:
                    return "mpleaderboard";
                case Icon.INV_ARM_WRESTLING:
                case Icon.INV_BASEJUMP:
                case Icon.INV_MISSION:
                case Icon.INV_DARTS:
                case Icon.INV_DEATHMATCH:
                case Icon.INV_DRUG:
                case Icon.INV_CASTLE:
                case Icon.INV_GOLF:
                case Icon.INV_BIKE:
                case Icon.INV_BOAT:
                case Icon.INV_ANCHOR:
                case Icon.INV_CAR:
                case Icon.INV_DOLLAR:
                case Icon.INV_COKE:
                case Icon.INV_KEY:
                case Icon.INV_DATA:
                case Icon.INV_HELI:
                case Icon.INV_HEORIN:
                case Icon.INV_KEYCARD:
                case Icon.INV_METH:
                case Icon.INV_BRIEFCASE:
                case Icon.INV_LINK:
                case Icon.INV_PERSON:
                case Icon.INV_PLANE:
                case Icon.INV_PLANE2:
                case Icon.INV_QUESTIONMARK:
                case Icon.INV_REMOTE:
                case Icon.INV_SAFE:
                case Icon.INV_STEER_WHEEL:
                case Icon.INV_WEAPON:
                case Icon.INV_WEED:
                case Icon.INV_RACE_FLAG_PLANE:
                case Icon.INV_RACE_FLAG_BICYCLE:
                case Icon.INV_RACE_FLAG_BOAT_ANCHOR:
                case Icon.INV_RACE_FLAG_PERSON:
                case Icon.INV_RACE_FLAG_CAR:
                case Icon.INV_RACE_FLAG_HELMET:
                case Icon.INV_SHOOTING_RANGE:
                case Icon.INV_SURVIVAL:
                case Icon.INV_TEAM_DEATHMATCH:
                case Icon.INV_TENNIS:
                case Icon.INV_VEHICLE_DEATHMATCH:
                    return "mpinventory";
                case Icon.ADVERSARY:
                case Icon.BASE_JUMPING:
                case Icon.BRIEFCASE:
                case Icon.MISSION_STAR:
                case Icon.DEATHMATCH:
                case Icon.CASTLE:
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
                case Icon.MP_AMMO_PICKUP:
                case Icon.MP_AMMO:
                case Icon.MP_CASH:
                case Icon.MP_RP:
                case Icon.MP_SPECTATING:
                    return "mphud";
                case Icon.SALE:
                    return "mpshopsale";
                case Icon.GLOBE_WHITE:
                case Icon.GLOBE_RED:
                case Icon.GLOBE_BLUE:
                case Icon.GLOBE_YELLOW:
                case Icon.GLOBE_GREEN:
                case Icon.GLOBE_ORANGE:
                    return "mprankbadge";
                case Icon.COUNTRY_USA:
                case Icon.COUNTRY_UK:
                case Icon.COUNTRY_SWEDEN:
                case Icon.COUNTRY_KOREA:
                case Icon.COUNTRY_JAPAN:
                case Icon.COUNTRY_ITALY:
                case Icon.COUNTRY_GERMANY:
                case Icon.COUNTRY_FRANCE:
                case Icon.BRAND_ALBANY:
                case Icon.BRAND_ANNIS:
                case Icon.BRAND_BANSHEE:
                case Icon.BRAND_BENEFACTOR:
                case Icon.BRAND_BF:
                case Icon.BRAND_BOLLOKAN:
                case Icon.BRAND_BRAVADO:
                case Icon.BRAND_BRUTE:
                case Icon.BRAND_BUCKINGHAM:
                case Icon.BRAND_CANIS:
                case Icon.BRAND_CHARIOT:
                case Icon.BRAND_CHEVAL:
                case Icon.BRAND_CLASSIQUE:
                case Icon.BRAND_COIL:
                case Icon.BRAND_DECLASSE:
                case Icon.BRAND_DEWBAUCHEE:
                case Icon.BRAND_DILETTANTE:
                case Icon.BRAND_DINKA:
                case Icon.BRAND_DUNDREARY:
                case Icon.BRAND_EMPORER:
                case Icon.BRAND_ENUS:
                case Icon.BRAND_FATHOM:
                case Icon.BRAND_GALIVANTER:
                case Icon.BRAND_GROTTI:
                case Icon.BRAND_HIJAK:
                case Icon.BRAND_HVY:
                case Icon.BRAND_IMPONTE:
                case Icon.BRAND_INVETERO:
                case Icon.BRAND_JACKSHEEPE:
                case Icon.BRAND_JOBUILT:
                case Icon.BRAND_KARIN:
                case Icon.BRAND_LAMPADATI:
                case Icon.BRAND_MAIBATSU:
                case Icon.BRAND_MAMMOTH:
                case Icon.BRAND_MTL:
                case Icon.BRAND_NAGASAKI:
                case Icon.BRAND_OBEY:
                case Icon.BRAND_OCELOT:
                case Icon.BRAND_OVERFLOD:
                case Icon.BRAND_PED:
                case Icon.BRAND_PEGASSI:
                case Icon.BRAND_PFISTER:
                case Icon.BRAND_PRINCIPE:
                case Icon.BRAND_PROGEN:
                case Icon.BRAND_SCHYSTER:
                case Icon.BRAND_SHITZU:
                case Icon.BRAND_SPEEDOPHILE:
                case Icon.BRAND_STANLEY:
                case Icon.BRAND_TRUFFADE:
                case Icon.BRAND_UBERMACHT:
                case Icon.BRAND_VAPID:
                case Icon.BRAND_VULCAR:
                case Icon.BRAND_WEENY:
                case Icon.BRAND_WESTERN:
                case Icon.BRAND_WESTERNMOTORCYCLE:
                case Icon.BRAND_WILLARD:
                case Icon.BRAND_ZIRCONIUM:
                    return "mpcarhud";
                case Icon.BRAND_GROTTI2:
                case Icon.BRAND_LCC:
                case Icon.BRAND_PROGEN2:
                case Icon.BRAND_RUNE:
                    return "mpcarhud2";
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
                case Icon.MP_AMMO_PICKUP: return "ammo_pickup";
                case Icon.MP_AMMO: return "mp_anim_ammo";
                case Icon.MP_CASH: return "mp_anim_cash";
                case Icon.MP_RP: return "mp_anim_rp";
                case Icon.MP_SPECTATING: return "spectating";
                case Icon.SALE: return "saleicon";
                case Icon.GLOBE_WHITE:
                case Icon.GLOBE_RED:
                case Icon.GLOBE_BLUE:
                case Icon.GLOBE_YELLOW:
                case Icon.GLOBE_GREEN:
                case Icon.GLOBE_ORANGE:
                    return "globe";
                case Icon.INV_ARM_WRESTLING: return "arm_wrestling";
                case Icon.INV_BASEJUMP: return "basejump";
                case Icon.INV_MISSION: return "custom_mission";
                case Icon.INV_DARTS: return "darts";
                case Icon.INV_DEATHMATCH: return "deathmatch";
                case Icon.INV_DRUG: return "drug_trafficking";
                case Icon.INV_CASTLE: return "gang_attack";
                case Icon.INV_GOLF: return "golf";
                case Icon.INV_BIKE: return "mp_specitem_bike";
                case Icon.INV_BOAT: return "mp_specitem_boat";
                case Icon.INV_ANCHOR: return "mp_specitem_boatpickup";
                case Icon.INV_CAR: return "mp_specitem_car";
                case Icon.INV_DOLLAR: return "mp_specitem_cash";
                case Icon.INV_COKE: return "mp_specitem_coke";
                case Icon.INV_KEY: return "mp_specitem_cuffkeys";
                case Icon.INV_DATA: return "mp_specitem_data";
                case Icon.INV_HELI: return "mp_specitem_heli";
                case Icon.INV_HEORIN: return "mp_specitem_heroin";
                case Icon.INV_KEYCARD: return "mp_specitem_keycard";
                case Icon.INV_METH: return "mp_specitem_meth";
                case Icon.INV_BRIEFCASE: return "mp_specitem_package";
                case Icon.INV_LINK: return "mp_specitem_partnericon";
                case Icon.INV_PERSON: return "mp_specitem_ped";
                case Icon.INV_PLANE: return "mp_specitem_plane";
                case Icon.INV_PLANE2: return "mp_specitem_plane2";
                case Icon.INV_QUESTIONMARK: return "mp_specitem_randomobject";
                case Icon.INV_REMOTE: return "mp_specitem_remote";
                case Icon.INV_SAFE: return "mp_specitem_safe";
                case Icon.INV_STEER_WHEEL: return "mp_specitem_steer_wheel";
                case Icon.INV_WEAPON: return "mp_specitem_weapons";
                case Icon.INV_WEED: return "mp_specitem_weed";
                case Icon.INV_RACE_FLAG_PLANE: return "race_air";
                case Icon.INV_RACE_FLAG_BICYCLE: return "race_bike";
                case Icon.INV_RACE_FLAG_BOAT_ANCHOR: return "race_boat";
                case Icon.INV_RACE_FLAG_PERSON: return "race_foot";
                case Icon.INV_RACE_FLAG_CAR: return "race_land";
                case Icon.INV_RACE_FLAG_HELMET: return "race_offroad";
                case Icon.INV_SHOOTING_RANGE: return "shooting_range";
                case Icon.INV_SURVIVAL: return "survival";
                case Icon.INV_TEAM_DEATHMATCH: return "team_deathmatch";
                case Icon.INV_TENNIS: return "tennis";
                case Icon.INV_VEHICLE_DEATHMATCH: return "vehicle_deathmatch";
                case Icon.AUDIO_MUTE: return "leaderboard_audio_mute";
                case Icon.AUDIO_INACTIVE: return "leaderboard_audio_inactive";
                case Icon.AUDIO_VOL1: return "leaderboard_audio_1";
                case Icon.AUDIO_VOL2: return "leaderboard_audio_2";
                case Icon.AUDIO_VOL3: return "leaderboard_audio_3";
                case Icon.COUNTRY_USA: return "vehicle_card_icons_flag_usa";
                case Icon.COUNTRY_UK: return "vehicle_card_icons_flag_uk";
                case Icon.COUNTRY_SWEDEN: return "vehicle_card_icons_flag_sweden";
                case Icon.COUNTRY_KOREA: return "vehicle_card_icons_flag_korea";
                case Icon.COUNTRY_JAPAN: return "vehicle_card_icons_flag_japan";
                case Icon.COUNTRY_ITALY: return "vehicle_card_icons_flag_italy";
                case Icon.COUNTRY_GERMANY: return "vehicle_card_icons_flag_germany";
                case Icon.COUNTRY_FRANCE: return "vehicle_card_icons_flag_france";
                case Icon.BRAND_ALBANY: return "albany";
                case Icon.BRAND_ANNIS: return "annis";
                case Icon.BRAND_BANSHEE: return "banshee";
                case Icon.BRAND_BENEFACTOR: return "benefactor";
                case Icon.BRAND_BF: return "bf";
                case Icon.BRAND_BOLLOKAN: return "bollokan";
                case Icon.BRAND_BRAVADO: return "bravado";
                case Icon.BRAND_BRUTE: return "brute";
                case Icon.BRAND_BUCKINGHAM: return "buckingham";
                case Icon.BRAND_CANIS: return "canis";
                case Icon.BRAND_CHARIOT: return "chariot";
                case Icon.BRAND_CHEVAL: return "cheval";
                case Icon.BRAND_CLASSIQUE: return "classique";
                case Icon.BRAND_COIL: return "coil";
                case Icon.BRAND_DECLASSE: return "declasse";
                case Icon.BRAND_DEWBAUCHEE: return "dewbauchee";
                case Icon.BRAND_DILETTANTE: return "dilettante";
                case Icon.BRAND_DINKA: return "dinka";
                case Icon.BRAND_DUNDREARY: return "dundreary";
                case Icon.BRAND_EMPORER: return "emporer";
                case Icon.BRAND_ENUS: return "enus";
                case Icon.BRAND_FATHOM: return "fathom";
                case Icon.BRAND_GALIVANTER: return "galivanter";
                case Icon.BRAND_GROTTI: return "grotti";
                case Icon.BRAND_HIJAK: return "hijak";
                case Icon.BRAND_HVY: return "hvy";
                case Icon.BRAND_IMPONTE: return "imponte";
                case Icon.BRAND_INVETERO: return "invetero";
                case Icon.BRAND_JACKSHEEPE: return "jacksheepe";
                case Icon.BRAND_JOBUILT: return "jobuilt";
                case Icon.BRAND_KARIN: return "karin";
                case Icon.BRAND_LAMPADATI: return "lampadati";
                case Icon.BRAND_MAIBATSU: return "maibatsu";
                case Icon.BRAND_MAMMOTH: return "mammoth";
                case Icon.BRAND_MTL: return "mtl";
                case Icon.BRAND_NAGASAKI: return "nagasaki";
                case Icon.BRAND_OBEY: return "obey";
                case Icon.BRAND_OCELOT: return "ocelot";
                case Icon.BRAND_OVERFLOD: return "overflod";
                case Icon.BRAND_PED: return "ped";
                case Icon.BRAND_PEGASSI: return "pegassi";
                case Icon.BRAND_PFISTER: return "pfister";
                case Icon.BRAND_PRINCIPE: return "principe";
                case Icon.BRAND_PROGEN: return "progen";
                case Icon.BRAND_SCHYSTER: return "schyster";
                case Icon.BRAND_SHITZU: return "shitzu";
                case Icon.BRAND_SPEEDOPHILE: return "speedophile";
                case Icon.BRAND_STANLEY: return "stanley";
                case Icon.BRAND_TRUFFADE: return "truffade";
                case Icon.BRAND_UBERMACHT: return "ubermacht";
                case Icon.BRAND_VAPID: return "vapid";
                case Icon.BRAND_VULCAR: return "vulcar";
                case Icon.BRAND_WEENY: return "weeny";
                case Icon.BRAND_WESTERN: return "western";
                case Icon.BRAND_WESTERNMOTORCYCLE: return "westernmotorcycle";
                case Icon.BRAND_WILLARD: return "willard";
                case Icon.BRAND_ZIRCONIUM: return "zirconium";
                case Icon.BRAND_GROTTI2: return "grotti_2";
                case Icon.BRAND_LCC: return "lcc";
                case Icon.BRAND_PROGEN2: return "progen";
                case Icon.BRAND_RUNE: return "rune";
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
                case Icon.AUDIO_MUTE:
                case Icon.AUDIO_INACTIVE:
                case Icon.AUDIO_VOL1:
                case Icon.AUDIO_VOL2:
                case Icon.AUDIO_VOL3:
                case Icon.BRAND_ALBANY:
                case Icon.BRAND_ANNIS:
                case Icon.BRAND_BANSHEE:
                case Icon.BRAND_BENEFACTOR:
                case Icon.BRAND_BF:
                case Icon.BRAND_BOLLOKAN:
                case Icon.BRAND_BRAVADO:
                case Icon.BRAND_BRUTE:
                case Icon.BRAND_BUCKINGHAM:
                case Icon.BRAND_CANIS:
                case Icon.BRAND_CHARIOT:
                case Icon.BRAND_CHEVAL:
                case Icon.BRAND_CLASSIQUE:
                case Icon.BRAND_COIL:
                case Icon.BRAND_DECLASSE:
                case Icon.BRAND_DEWBAUCHEE:
                case Icon.BRAND_DILETTANTE:
                case Icon.BRAND_DINKA:
                case Icon.BRAND_DUNDREARY:
                case Icon.BRAND_EMPORER:
                case Icon.BRAND_ENUS:
                case Icon.BRAND_FATHOM:
                case Icon.BRAND_GALIVANTER:
                case Icon.BRAND_GROTTI:
                case Icon.BRAND_HIJAK:
                case Icon.BRAND_HVY:
                case Icon.BRAND_IMPONTE:
                case Icon.BRAND_INVETERO:
                case Icon.BRAND_JACKSHEEPE:
                case Icon.BRAND_JOBUILT:
                case Icon.BRAND_KARIN:
                case Icon.BRAND_LAMPADATI:
                case Icon.BRAND_MAIBATSU:
                case Icon.BRAND_MAMMOTH:
                case Icon.BRAND_MTL:
                case Icon.BRAND_NAGASAKI:
                case Icon.BRAND_OBEY:
                case Icon.BRAND_OCELOT:
                case Icon.BRAND_OVERFLOD:
                case Icon.BRAND_PED:
                case Icon.BRAND_PEGASSI:
                case Icon.BRAND_PFISTER:
                case Icon.BRAND_PRINCIPE:
                case Icon.BRAND_PROGEN:
                case Icon.BRAND_SCHYSTER:
                case Icon.BRAND_SHITZU:
                case Icon.BRAND_SPEEDOPHILE:
                case Icon.BRAND_STANLEY:
                case Icon.BRAND_TRUFFADE:
                case Icon.BRAND_UBERMACHT:
                case Icon.BRAND_VAPID:
                case Icon.BRAND_VULCAR:
                case Icon.BRAND_WEENY:
                case Icon.BRAND_WESTERN:
                case Icon.BRAND_WESTERNMOTORCYCLE:
                case Icon.BRAND_WILLARD:
                case Icon.BRAND_ZIRCONIUM:
                case Icon.BRAND_GROTTI2:
                case Icon.BRAND_LCC:
                case Icon.BRAND_PROGEN2:
                case Icon.BRAND_RUNE:
                case Icon.COUNTRY_USA:
                case Icon.COUNTRY_UK:
                case Icon.COUNTRY_SWEDEN:
                case Icon.COUNTRY_KOREA:
                case Icon.COUNTRY_JAPAN:
                case Icon.COUNTRY_ITALY:
                case Icon.COUNTRY_GERMANY:
                case Icon.COUNTRY_FRANCE:
                    return 30f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);
                case Icon.STAR:
                case Icon.LOCK_ARENA:
                    return 52f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);
                case Icon.MEDAL_SILVER:
                case Icon.MP_AMMO_PICKUP:
                case Icon.MP_AMMO:
                case Icon.MP_CASH:
                case Icon.MP_RP:
                case Icon.GLOBE_WHITE:
                case Icon.GLOBE_BLUE:
                case Icon.GLOBE_GREEN:
                case Icon.GLOBE_ORANGE:
                case Icon.GLOBE_RED:
                case Icon.GLOBE_YELLOW:
                case Icon.INV_ARM_WRESTLING:
                case Icon.INV_BASEJUMP:
                case Icon.INV_MISSION:
                case Icon.INV_DARTS:
                case Icon.INV_DEATHMATCH:
                case Icon.INV_DRUG:
                case Icon.INV_CASTLE:
                case Icon.INV_GOLF:
                case Icon.INV_BIKE:
                case Icon.INV_BOAT:
                case Icon.INV_ANCHOR:
                case Icon.INV_CAR:
                case Icon.INV_DOLLAR:
                case Icon.INV_COKE:
                case Icon.INV_KEY:
                case Icon.INV_DATA:
                case Icon.INV_HELI:
                case Icon.INV_HEORIN:
                case Icon.INV_KEYCARD:
                case Icon.INV_METH:
                case Icon.INV_BRIEFCASE:
                case Icon.INV_LINK:
                case Icon.INV_PERSON:
                case Icon.INV_PLANE:
                case Icon.INV_PLANE2:
                case Icon.INV_QUESTIONMARK:
                case Icon.INV_REMOTE:
                case Icon.INV_SAFE:
                case Icon.INV_STEER_WHEEL:
                case Icon.INV_WEAPON:
                case Icon.INV_WEED:
                case Icon.INV_RACE_FLAG_PLANE:
                case Icon.INV_RACE_FLAG_BICYCLE:
                case Icon.INV_RACE_FLAG_BOAT_ANCHOR:
                case Icon.INV_RACE_FLAG_PERSON:
                case Icon.INV_RACE_FLAG_CAR:
                case Icon.INV_RACE_FLAG_HELMET:
                case Icon.INV_SHOOTING_RANGE:
                case Icon.INV_SURVIVAL:
                case Icon.INV_TEAM_DEATHMATCH:
                case Icon.INV_TENNIS:
                case Icon.INV_VEHICLE_DEATHMATCH:
                    return 22f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);
                default:
                    return 38f / (width ? MenuController.ScreenWidth : MenuController.ScreenHeight);
            }
        }

        protected int[] GetSpriteColour(Icon icon, bool selected)
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
                case Icon.MP_SPECTATING:
                case Icon.GLOBE_WHITE:
                case Icon.AUDIO_MUTE:
                case Icon.AUDIO_INACTIVE:
                case Icon.AUDIO_VOL1:
                case Icon.AUDIO_VOL2:
                case Icon.AUDIO_VOL3:
                case Icon.BRAND_ALBANY:
                case Icon.BRAND_ANNIS:
                case Icon.BRAND_BANSHEE:
                case Icon.BRAND_BENEFACTOR:
                case Icon.BRAND_BF:
                case Icon.BRAND_BOLLOKAN:
                case Icon.BRAND_BRAVADO:
                case Icon.BRAND_BRUTE:
                case Icon.BRAND_BUCKINGHAM:
                case Icon.BRAND_CANIS:
                case Icon.BRAND_CHARIOT:
                case Icon.BRAND_CHEVAL:
                case Icon.BRAND_CLASSIQUE:
                case Icon.BRAND_COIL:
                case Icon.BRAND_DECLASSE:
                case Icon.BRAND_DEWBAUCHEE:
                case Icon.BRAND_DILETTANTE:
                case Icon.BRAND_DINKA:
                case Icon.BRAND_DUNDREARY:
                case Icon.BRAND_EMPORER:
                case Icon.BRAND_ENUS:
                case Icon.BRAND_FATHOM:
                case Icon.BRAND_GALIVANTER:
                case Icon.BRAND_GROTTI:
                case Icon.BRAND_HIJAK:
                case Icon.BRAND_HVY:
                case Icon.BRAND_IMPONTE:
                case Icon.BRAND_INVETERO:
                case Icon.BRAND_JACKSHEEPE:
                case Icon.BRAND_JOBUILT:
                case Icon.BRAND_KARIN:
                case Icon.BRAND_LAMPADATI:
                case Icon.BRAND_MAIBATSU:
                case Icon.BRAND_MAMMOTH:
                case Icon.BRAND_MTL:
                case Icon.BRAND_NAGASAKI:
                case Icon.BRAND_OBEY:
                case Icon.BRAND_OCELOT:
                case Icon.BRAND_OVERFLOD:
                case Icon.BRAND_PED:
                case Icon.BRAND_PEGASSI:
                case Icon.BRAND_PFISTER:
                case Icon.BRAND_PRINCIPE:
                case Icon.BRAND_PROGEN:
                case Icon.BRAND_SCHYSTER:
                case Icon.BRAND_SHITZU:
                case Icon.BRAND_SPEEDOPHILE:
                case Icon.BRAND_STANLEY:
                case Icon.BRAND_TRUFFADE:
                case Icon.BRAND_UBERMACHT:
                case Icon.BRAND_VAPID:
                case Icon.BRAND_VULCAR:
                case Icon.BRAND_WEENY:
                case Icon.BRAND_WESTERN:
                case Icon.BRAND_WESTERNMOTORCYCLE:
                case Icon.BRAND_WILLARD:
                case Icon.BRAND_ZIRCONIUM:
                case Icon.BRAND_GROTTI2:
                case Icon.BRAND_LCC:
                case Icon.BRAND_PROGEN2:
                case Icon.BRAND_RUNE:
                    return selected ? (Enabled ? new int[3] { 0, 0, 0 } : new int[3] { 50, 50, 50 }) : (Enabled ? new int[3] { 255, 255, 255 } : new int[3] { 109, 109, 109 });
                case Icon.GLOBE_BLUE:
                    return Enabled ? new int[3] { 10, 103, 166 } : new int[3] { 11, 62, 117 };
                case Icon.GLOBE_GREEN:
                    return Enabled ? new int[3] { 10, 166, 85 } : new int[3] { 5, 71, 22 };
                case Icon.GLOBE_ORANGE:
                    return Enabled ? new int[3] { 232, 145, 14 } : new int[3] { 133, 77, 12 };
                case Icon.GLOBE_RED:
                    return Enabled ? new int[3] { 207, 43, 31 } : new int[3] { 110, 7, 7 };
                case Icon.GLOBE_YELLOW:
                    return Enabled ? new int[3] { 232, 207, 14 } : new int[3] { 131, 133, 12 };
                default:
                    return Enabled ? new int[3] { 255, 255, 255 } : new int[3] { 109, 109, 109 };
            }
        }

        protected float GetSpriteX(Icon icon, bool leftAligned, bool leftSide)
        {
            if (icon == Icon.NONE)
            {
                return 0f;
            }
            return leftSide ? (leftAligned ? (20f / MenuController.ScreenWidth) : GetSafeZoneSize() - ((Width - 20f) / MenuController.ScreenWidth)) : (leftAligned ? (Width - 20f) / MenuController.ScreenWidth : (GetSafeZoneSize() - (20f / MenuController.ScreenWidth)));
        }

        protected float GetSpriteY(Icon icon)
        {
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
                    int[] spriteColor = GetSpriteColour(LeftIcon, Selected);
                    string textureDictionary = GetSpriteDictionary(LeftIcon);

                    DrawSprite(textureDictionary, name, spriteX, spriteY, spriteWidth, spriteHeight, 0f, spriteColor[0], spriteColor[1], spriteColor[2], 255);
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
                    int[] spriteColor = GetSpriteColour(RightIcon, Selected);
                    string textureDictionary = GetSpriteDictionary(RightIcon);

                    DrawSprite(textureDictionary, name, spriteX, spriteY, spriteWidth, spriteHeight, 0f, spriteColor[0], spriteColor[1], spriteColor[2], 255);
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
