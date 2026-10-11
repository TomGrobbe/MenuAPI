namespace MenuAPI;

/// <summary>
/// A plain button in a <see cref="Menu"/>. Every other item type inherits from it.
/// Pressing it raises <see cref="Menu.OnItemSelect"/> on its menu.
/// </summary>
public class MenuItem
{
    /// <summary>
    /// The icons (game sprites) an item can show, for example in <see cref="LeftIcon"/> and <see cref="RightIcon"/>.
    /// </summary>
    public enum Icon
    {
        /// <summary>No icon.</summary>
        NONE,
        /// <summary>A padlock.</summary>
        LOCK,
        /// <summary>A star, the shop's "new" marker.</summary>
        STAR,
        /// <summary>A warning triangle.</summary>
        WARNING,
        /// <summary>A crown.</summary>
        CROWN,
        /// <summary>A bronze medal.</summary>
        MEDAL_BRONZE,
        /// <summary>A gold medal.</summary>
        MEDAL_GOLD,
        /// <summary>A silver medal.</summary>
        MEDAL_SILVER,
        /// <summary>A stack of cash.</summary>
        CASH,
        /// <summary>A bag of cocaine.</summary>
        COKE,
        /// <summary>A bag of heroin.</summary>
        HEROIN,
        /// <summary>A bag of meth.</summary>
        METH,
        /// <summary>A bag of weed.</summary>
        WEED,
        /// <summary>Bullets, the Ammu-Nation shop icon.</summary>
        AMMO,
        /// <summary>A body armor vest.</summary>
        ARMOR,
        /// <summary>The barber shop icon.</summary>
        BARBER,
        /// <summary>A shirt, the clothing shop icon.</summary>
        CLOTHING,
        /// <summary>Franklin's character icon.</summary>
        FRANKLIN,
        /// <summary>A motorbike.</summary>
        BIKE,
        /// <summary>A car.</summary>
        CAR,
        /// <summary>A gun.</summary>
        GUN,
        /// <summary>A heart.</summary>
        HEALTH_HEART,
        /// <summary>A makeup brush.</summary>
        MAKEUP_BRUSH,
        /// <summary>A mask.</summary>
        MASK,
        /// <summary>Michael's character icon.</summary>
        MICHAEL,
        /// <summary>The tattoo parlor icon.</summary>
        TATTOO,
        /// <summary>A tick mark.</summary>
        TICK,
        /// <summary>Trevor's character icon.</summary>
        TREVOR,
        /// <summary>The female symbol.</summary>
        FEMALE,
        /// <summary>The male symbol.</summary>
        MALE,
        /// <summary>The Arena War style padlock.</summary>
        LOCK_ARENA,
        /// <summary>The adversary mode job icon.</summary>
        ADVERSARY,
        /// <summary>The parachuting job icon.</summary>
        BASE_JUMPING,
        /// <summary>A briefcase, the capture job icon.</summary>
        BRIEFCASE,
        /// <summary>A star, the custom mission job icon.</summary>
        MISSION_STAR,
        /// <summary>The deathmatch job icon.</summary>
        DEATHMATCH,
        /// <summary>A castle, the gang attack job icon.</summary>
        CASTLE,
        /// <summary>A trophy, the last team standing job icon.</summary>
        TROPHY,
        /// <summary>A checkered race flag.</summary>
        RACE_FLAG,
        /// <summary>A race flag with a plane, the air race icon.</summary>
        RACE_FLAG_PLANE,
        /// <summary>A race flag with a bicycle.</summary>
        RACE_FLAG_BICYCLE,
        /// <summary>A race flag with a person, the foot race icon.</summary>
        RACE_FLAG_PERSON,
        /// <summary>A race flag with a car, the land race icon.</summary>
        RACE_FLAG_CAR,
        /// <summary>A race flag with an anchor, the sea race icon.</summary>
        RACE_FLAG_BOAT_ANCHOR,
        /// <summary>The Rockstar logo.</summary>
        ROCKSTAR,
        /// <summary>The stunt job icon.</summary>
        STUNT,
        /// <summary>The premium stunt job icon.</summary>
        STUNT_PREMIUM,
        /// <summary>A race flag with a stunt ramp, the stunt race icon.</summary>
        RACE_FLAG_STUNT_JUMP,
        /// <summary>A shield, the survival job icon.</summary>
        SHIELD,
        /// <summary>The team deathmatch job icon.</summary>
        TEAM_DEATHMATCH,
        /// <summary>The vehicle deathmatch job icon.</summary>
        VEHICLE_DEATHMATCH,
        /// <summary>The ammo pickup HUD icon.</summary>
        MP_AMMO_PICKUP,
        /// <summary>The ammo HUD icon.</summary>
        MP_AMMO,
        /// <summary>The cash HUD icon.</summary>
        MP_CASH,
        /// <summary>The RP HUD icon.</summary>
        MP_RP,
        /// <summary>The spectating icon.</summary>
        MP_SPECTATING,
        /// <summary>A sale tag.</summary>
        SALE,
        /// <summary>A white globe.</summary>
        GLOBE_WHITE,
        /// <summary>A red globe.</summary>
        GLOBE_RED,
        /// <summary>A blue globe.</summary>
        GLOBE_BLUE,
        /// <summary>A yellow globe.</summary>
        GLOBE_YELLOW,
        /// <summary>A green globe.</summary>
        GLOBE_GREEN,
        /// <summary>An orange globe.</summary>
        GLOBE_ORANGE,
        /// <summary>The arm wrestling inventory icon.</summary>
        INV_ARM_WRESTLING,
        /// <summary>The parachuting inventory icon.</summary>
        INV_BASEJUMP,
        /// <summary>The custom mission inventory icon.</summary>
        INV_MISSION,
        /// <summary>The darts inventory icon.</summary>
        INV_DARTS,
        /// <summary>The deathmatch inventory icon.</summary>
        INV_DEATHMATCH,
        /// <summary>The drug trafficking inventory icon.</summary>
        INV_DRUG,
        /// <summary>A castle, the gang attack inventory icon.</summary>
        INV_CASTLE,
        /// <summary>The golf inventory icon.</summary>
        INV_GOLF,
        /// <summary>A motorbike inventory icon.</summary>
        INV_BIKE,
        /// <summary>A boat inventory icon.</summary>
        INV_BOAT,
        /// <summary>An anchor inventory icon.</summary>
        INV_ANCHOR,
        /// <summary>A car inventory icon.</summary>
        INV_CAR,
        /// <summary>A dollar inventory icon.</summary>
        INV_DOLLAR,
        /// <summary>A cocaine inventory icon.</summary>
        INV_COKE,
        /// <summary>A key inventory icon.</summary>
        INV_KEY,
        /// <summary>A data inventory icon.</summary>
        INV_DATA,
        /// <summary>A helicopter inventory icon.</summary>
        INV_HELI,
        /// <summary>A heroin inventory icon.</summary>
        INV_HEORIN,
        /// <summary>A keycard inventory icon.</summary>
        INV_KEYCARD,
        /// <summary>A meth inventory icon.</summary>
        INV_METH,
        /// <summary>A briefcase inventory icon.</summary>
        INV_BRIEFCASE,
        /// <summary>A chain link inventory icon.</summary>
        INV_LINK,
        /// <summary>A person inventory icon.</summary>
        INV_PERSON,
        /// <summary>A plane inventory icon.</summary>
        INV_PLANE,
        /// <summary>A second plane inventory icon.</summary>
        INV_PLANE2,
        /// <summary>A question mark inventory icon.</summary>
        INV_QUESTIONMARK,
        /// <summary>A remote control inventory icon.</summary>
        INV_REMOTE,
        /// <summary>A safe inventory icon.</summary>
        INV_SAFE,
        /// <summary>A steering wheel inventory icon.</summary>
        INV_STEER_WHEEL,
        /// <summary>A weapon inventory icon.</summary>
        INV_WEAPON,
        /// <summary>A weed inventory icon.</summary>
        INV_WEED,
        /// <summary>The air race inventory icon.</summary>
        INV_RACE_FLAG_PLANE,
        /// <summary>The bike race inventory icon.</summary>
        INV_RACE_FLAG_BICYCLE,
        /// <summary>The boat race inventory icon.</summary>
        INV_RACE_FLAG_BOAT_ANCHOR,
        /// <summary>The foot race inventory icon.</summary>
        INV_RACE_FLAG_PERSON,
        /// <summary>The land race inventory icon.</summary>
        INV_RACE_FLAG_CAR,
        /// <summary>A helmet, the off road race inventory icon.</summary>
        INV_RACE_FLAG_HELMET,
        /// <summary>The shooting range inventory icon.</summary>
        INV_SHOOTING_RANGE,
        /// <summary>The survival inventory icon.</summary>
        INV_SURVIVAL,
        /// <summary>The team deathmatch inventory icon.</summary>
        INV_TEAM_DEATHMATCH,
        /// <summary>The tennis inventory icon.</summary>
        INV_TENNIS,
        /// <summary>The vehicle deathmatch inventory icon.</summary>
        INV_VEHICLE_DEATHMATCH,
        /// <summary>A muted speaker.</summary>
        AUDIO_MUTE,
        /// <summary>A speaker without sound waves.</summary>
        AUDIO_INACTIVE,
        /// <summary>A speaker with one sound wave.</summary>
        AUDIO_VOL1,
        /// <summary>A speaker with two sound waves.</summary>
        AUDIO_VOL2,
        /// <summary>A speaker with three sound waves.</summary>
        AUDIO_VOL3,
        /// <summary>The flag of the United States.</summary>
        COUNTRY_USA,
        /// <summary>The flag of the United Kingdom.</summary>
        COUNTRY_UK,
        /// <summary>The flag of Sweden.</summary>
        COUNTRY_SWEDEN,
        /// <summary>The flag of South Korea.</summary>
        COUNTRY_KOREA,
        /// <summary>The flag of Japan.</summary>
        COUNTRY_JAPAN,
        /// <summary>The flag of Italy.</summary>
        COUNTRY_ITALY,
        /// <summary>The flag of Germany.</summary>
        COUNTRY_GERMANY,
        /// <summary>The flag of France.</summary>
        COUNTRY_FRANCE,
        /// <summary>The Albany vehicle brand logo.</summary>
        BRAND_ALBANY,
        /// <summary>The Annis vehicle brand logo.</summary>
        BRAND_ANNIS,
        /// <summary>The Banshee logo.</summary>
        BRAND_BANSHEE,
        /// <summary>The Benefactor vehicle brand logo.</summary>
        BRAND_BENEFACTOR,
        /// <summary>The BF vehicle brand logo.</summary>
        BRAND_BF,
        /// <summary>The Bollokan vehicle brand logo.</summary>
        BRAND_BOLLOKAN,
        /// <summary>The Bravado vehicle brand logo.</summary>
        BRAND_BRAVADO,
        /// <summary>The Brute vehicle brand logo.</summary>
        BRAND_BRUTE,
        /// <summary>The Buckingham vehicle brand logo.</summary>
        BRAND_BUCKINGHAM,
        /// <summary>The Canis vehicle brand logo.</summary>
        BRAND_CANIS,
        /// <summary>The Chariot vehicle brand logo.</summary>
        BRAND_CHARIOT,
        /// <summary>The Cheval vehicle brand logo.</summary>
        BRAND_CHEVAL,
        /// <summary>The Classique vehicle brand logo.</summary>
        BRAND_CLASSIQUE,
        /// <summary>The Coil vehicle brand logo.</summary>
        BRAND_COIL,
        /// <summary>The Declasse vehicle brand logo.</summary>
        BRAND_DECLASSE,
        /// <summary>The Dewbauchee vehicle brand logo.</summary>
        BRAND_DEWBAUCHEE,
        /// <summary>The Dilettante vehicle brand logo.</summary>
        BRAND_DILETTANTE,
        /// <summary>The Dinka vehicle brand logo.</summary>
        BRAND_DINKA,
        /// <summary>The Dundreary vehicle brand logo.</summary>
        BRAND_DUNDREARY,
        /// <summary>The Emporer vehicle brand logo.</summary>
        BRAND_EMPORER,
        /// <summary>The Enus vehicle brand logo.</summary>
        BRAND_ENUS,
        /// <summary>The Fathom vehicle brand logo.</summary>
        BRAND_FATHOM,
        /// <summary>The Galivanter vehicle brand logo.</summary>
        BRAND_GALIVANTER,
        /// <summary>The Grotti vehicle brand logo.</summary>
        BRAND_GROTTI,
        /// <summary>The second style of the Grotti vehicle brand logo.</summary>
        BRAND_GROTTI2,
        /// <summary>The Hijak vehicle brand logo.</summary>
        BRAND_HIJAK,
        /// <summary>The HVY vehicle brand logo.</summary>
        BRAND_HVY,
        /// <summary>The Imponte vehicle brand logo.</summary>
        BRAND_IMPONTE,
        /// <summary>The Invetero vehicle brand logo.</summary>
        BRAND_INVETERO,
        /// <summary>The Jacksheepe vehicle brand logo.</summary>
        BRAND_JACKSHEEPE,
        /// <summary>The LCC vehicle brand logo.</summary>
        BRAND_LCC,
        /// <summary>The JoBuilt vehicle brand logo.</summary>
        BRAND_JOBUILT,
        /// <summary>The Karin vehicle brand logo.</summary>
        BRAND_KARIN,
        /// <summary>The Lampadati vehicle brand logo.</summary>
        BRAND_LAMPADATI,
        /// <summary>The Maibatsu vehicle brand logo.</summary>
        BRAND_MAIBATSU,
        /// <summary>The Mammoth vehicle brand logo.</summary>
        BRAND_MAMMOTH,
        /// <summary>The MTL vehicle brand logo.</summary>
        BRAND_MTL,
        /// <summary>The Nagasaki vehicle brand logo.</summary>
        BRAND_NAGASAKI,
        /// <summary>The Obey vehicle brand logo.</summary>
        BRAND_OBEY,
        /// <summary>The Ocelot vehicle brand logo.</summary>
        BRAND_OCELOT,
        /// <summary>The Overflod vehicle brand logo.</summary>
        BRAND_OVERFLOD,
        /// <summary>The Ped vehicle brand logo.</summary>
        BRAND_PED,
        /// <summary>The Pegassi vehicle brand logo.</summary>
        BRAND_PEGASSI,
        /// <summary>The Pfister vehicle brand logo.</summary>
        BRAND_PFISTER,
        /// <summary>The Principe vehicle brand logo.</summary>
        BRAND_PRINCIPE,
        /// <summary>The Progen vehicle brand logo.</summary>
        BRAND_PROGEN,
        /// <summary>The second style of the Progen vehicle brand logo.</summary>
        BRAND_PROGEN2,
        /// <summary>The Rune vehicle brand logo.</summary>
        BRAND_RUNE,
        /// <summary>The Schyster vehicle brand logo.</summary>
        BRAND_SCHYSTER,
        /// <summary>The Shitzu vehicle brand logo.</summary>
        BRAND_SHITZU,
        /// <summary>The Speedophile vehicle brand logo.</summary>
        BRAND_SPEEDOPHILE,
        /// <summary>The Stanley vehicle brand logo.</summary>
        BRAND_STANLEY,
        /// <summary>The Truffade vehicle brand logo.</summary>
        BRAND_TRUFFADE,
        /// <summary>The Ubermacht vehicle brand logo.</summary>
        BRAND_UBERMACHT,
        /// <summary>The Vapid vehicle brand logo.</summary>
        BRAND_VAPID,
        /// <summary>The Vulcar vehicle brand logo.</summary>
        BRAND_VULCAR,
        /// <summary>The Weeny vehicle brand logo.</summary>
        BRAND_WEENY,
        /// <summary>The Western vehicle brand logo.</summary>
        BRAND_WESTERN,
        /// <summary>The Western Motorcycle vehicle brand logo.</summary>
        BRAND_WESTERNMOTORCYCLE,
        /// <summary>The Willard vehicle brand logo.</summary>
        BRAND_WILLARD,
        /// <summary>The Zirconium vehicle brand logo.</summary>
        BRAND_ZIRCONIUM,
        /// <summary>An info icon, a letter i in a circle.</summary>
        INFO
    }
    /// <summary>
    /// The text on the left side of the item. Supports the game's color codes like <c>~r~</c>.
    /// </summary>
    public string Text
    {
        get => _text;
        set => MenuNui.Change(ref _text, value);
    }

    /// <summary>
    /// The text on the right side of the item. Null shows nothing.
    /// List, checkbox and slider items overwrite it, so it only works on plain items.
    /// </summary>
    public string? Label
    {
        get => _label;
        set => MenuNui.Change(ref _label, value);
    }

    /// <summary>
    /// The icon on the left side of the item. Defaults to <see cref="Icon.NONE"/>.
    /// </summary>
    public Icon LeftIcon
    {
        get => _leftIcon;
        set => MenuNui.Change(ref _leftIcon, value);
    }

    /// <summary>
    /// The icon on the right side of the item. Defaults to <see cref="Icon.NONE"/>.
    /// Checkbox and slider items overwrite it.
    /// </summary>
    public Icon RightIcon
    {
        get => _rightIcon;
        set => MenuNui.Change(ref _rightIcon, value);
    }

    /// <summary>
    /// When false, the item is greyed out and cannot be used. True by default.
    /// </summary>
    public bool Enabled
    {
        get => _enabled;
        set => MenuNui.Change(ref _enabled, value);
    }

    /// <summary>
    /// Shown below the menu while the item is highlighted. Null shows no description.
    /// </summary>
    public string? Description
    {
        get => _description;
        set => MenuNui.Change(ref _description, value);
    }

    private string _text = "";
    private string? _label;
    private Icon _leftIcon;
    private Icon _rightIcon;
    private bool _enabled = true;
    private string? _description;
    /// <summary>
    /// The position of the item in its menu, or -1 when it is not in a menu.
    /// </summary>
    public int Index => ParentMenu?.IndexOf(this) ?? -1;
    /// <summary>
    /// Whether the item is the highlighted one in its menu. Always false when it is not in a menu.
    /// </summary>
    public bool Selected { get { if (ParentMenu != null) { return ParentMenu.CurrentIndex == Index; } return false; } }
    /// <summary>
    /// The menu this item was added to, or null. Set by <see cref="Menu.AddMenuItem(MenuItem)"/>, mainly used internally.
    /// </summary>
    public Menu? ParentMenu { get; set; }
    /// <summary>
    /// Left over from the old native drawing. Nothing reads it any more, so it does nothing.
    /// </summary>
    [Obsolete("Menus are always drawn with NUI. This property does nothing and will be removed.")]
    public int PositionOnScreen { get; internal set; }

    /// <summary>
    /// Your own data to identify the item by, so you do not have to put it in the visible text or description.
    /// Null by default.
    /// </summary>
    public dynamic? ItemData { get; set; }

    /// <summary>
    /// Creates a new <see cref="MenuItem"/> without a description.
    /// </summary>
    /// <param name="text">The text on the left side of the item.</param>
    public MenuItem(string text) : this(text, null) { }

    /// <summary>
    /// Creates a new <see cref="MenuItem"/>.
    /// </summary>
    /// <param name="text">The text on the left side of the item.</param>
    /// <param name="description">Shown below the menu while the item is highlighted. Null for none.</param>
    public MenuItem(string text, string? description)
    {
        Text = text;
        Description = description;
    }

    /// <summary>
    /// Get the sprite dictionary name for the given icon.
    /// </summary>
    /// <param name="icon">The icon to look up.</param>
    /// <returns>The texture dictionary that holds the icon's sprite.</returns>
    protected internal string GetSpriteDictionary(Icon icon)
    {
        return icon switch
        {
            Icon.MALE or Icon.FEMALE or Icon.AUDIO_MUTE or Icon.AUDIO_INACTIVE or Icon.AUDIO_VOL1 or Icon.AUDIO_VOL2 or Icon.AUDIO_VOL3 => "mpleaderboard",
            Icon.INV_ARM_WRESTLING or Icon.INV_BASEJUMP or Icon.INV_MISSION or Icon.INV_DARTS or Icon.INV_DEATHMATCH or Icon.INV_DRUG or Icon.INV_CASTLE or Icon.INV_GOLF or Icon.INV_BIKE or Icon.INV_BOAT or Icon.INV_ANCHOR or Icon.INV_CAR or Icon.INV_DOLLAR or Icon.INV_COKE or Icon.INV_KEY or Icon.INV_DATA or Icon.INV_HELI or Icon.INV_HEORIN or Icon.INV_KEYCARD or Icon.INV_METH or Icon.INV_BRIEFCASE or Icon.INV_LINK or Icon.INV_PERSON or Icon.INV_PLANE or Icon.INV_PLANE2 or Icon.INV_QUESTIONMARK or Icon.INV_REMOTE or Icon.INV_SAFE or Icon.INV_STEER_WHEEL or Icon.INV_WEAPON or Icon.INV_WEED or Icon.INV_RACE_FLAG_PLANE or Icon.INV_RACE_FLAG_BICYCLE or Icon.INV_RACE_FLAG_BOAT_ANCHOR or Icon.INV_RACE_FLAG_PERSON or Icon.INV_RACE_FLAG_CAR or Icon.INV_RACE_FLAG_HELMET or Icon.INV_SHOOTING_RANGE or Icon.INV_SURVIVAL or Icon.INV_TEAM_DEATHMATCH or Icon.INV_TENNIS or Icon.INV_VEHICLE_DEATHMATCH => "mpinventory",
            Icon.ADVERSARY or Icon.BASE_JUMPING or Icon.BRIEFCASE or Icon.MISSION_STAR or Icon.DEATHMATCH or Icon.CASTLE or Icon.TROPHY or Icon.RACE_FLAG or Icon.RACE_FLAG_PLANE or Icon.RACE_FLAG_BICYCLE or Icon.RACE_FLAG_PERSON or Icon.RACE_FLAG_CAR or Icon.RACE_FLAG_BOAT_ANCHOR or Icon.ROCKSTAR or Icon.STUNT or Icon.STUNT_PREMIUM or Icon.RACE_FLAG_STUNT_JUMP or Icon.SHIELD or Icon.TEAM_DEATHMATCH or Icon.VEHICLE_DEATHMATCH => "commonmenutu",
            Icon.MP_AMMO_PICKUP or Icon.MP_AMMO or Icon.MP_CASH or Icon.MP_RP or Icon.MP_SPECTATING => "mphud",
            Icon.SALE => "mpshopsale",
            Icon.GLOBE_WHITE or Icon.GLOBE_RED or Icon.GLOBE_BLUE or Icon.GLOBE_YELLOW or Icon.GLOBE_GREEN or Icon.GLOBE_ORANGE => "mprankbadge",
            Icon.COUNTRY_USA or Icon.COUNTRY_UK or Icon.COUNTRY_SWEDEN or Icon.COUNTRY_KOREA or Icon.COUNTRY_JAPAN or Icon.COUNTRY_ITALY or Icon.COUNTRY_GERMANY or Icon.COUNTRY_FRANCE or Icon.BRAND_ALBANY or Icon.BRAND_ANNIS or Icon.BRAND_BANSHEE or Icon.BRAND_BENEFACTOR or Icon.BRAND_BF or Icon.BRAND_BOLLOKAN or Icon.BRAND_BRAVADO or Icon.BRAND_BRUTE or Icon.BRAND_BUCKINGHAM or Icon.BRAND_CANIS or Icon.BRAND_CHARIOT or Icon.BRAND_CHEVAL or Icon.BRAND_CLASSIQUE or Icon.BRAND_COIL or Icon.BRAND_DECLASSE or Icon.BRAND_DEWBAUCHEE or Icon.BRAND_DILETTANTE or Icon.BRAND_DINKA or Icon.BRAND_DUNDREARY or Icon.BRAND_EMPORER or Icon.BRAND_ENUS or Icon.BRAND_FATHOM or Icon.BRAND_GALIVANTER or Icon.BRAND_GROTTI or Icon.BRAND_HIJAK or Icon.BRAND_HVY or Icon.BRAND_IMPONTE or Icon.BRAND_INVETERO or Icon.BRAND_JACKSHEEPE or Icon.BRAND_JOBUILT or Icon.BRAND_KARIN or Icon.BRAND_LAMPADATI or Icon.BRAND_MAIBATSU or Icon.BRAND_MAMMOTH or Icon.BRAND_MTL or Icon.BRAND_NAGASAKI or Icon.BRAND_OBEY or Icon.BRAND_OCELOT or Icon.BRAND_OVERFLOD or Icon.BRAND_PED or Icon.BRAND_PEGASSI or Icon.BRAND_PFISTER or Icon.BRAND_PRINCIPE or Icon.BRAND_PROGEN or Icon.BRAND_SCHYSTER or Icon.BRAND_SHITZU or Icon.BRAND_SPEEDOPHILE or Icon.BRAND_STANLEY or Icon.BRAND_TRUFFADE or Icon.BRAND_UBERMACHT or Icon.BRAND_VAPID or Icon.BRAND_VULCAR or Icon.BRAND_WEENY or Icon.BRAND_WESTERN or Icon.BRAND_WESTERNMOTORCYCLE or Icon.BRAND_WILLARD or Icon.BRAND_ZIRCONIUM => "mpcarhud",
            Icon.BRAND_GROTTI2 or Icon.BRAND_LCC or Icon.BRAND_PROGEN2 or Icon.BRAND_RUNE => "mpcarhud2",
            Icon.INFO => "shared",
            _ => "commonmenu",
        };
    }

    /// <summary>
    /// Get the sprite name for the given icon depending on the selected state of the item.
    /// </summary>
    /// <param name="icon">The icon to look up.</param>
    /// <param name="selected">Whether the item is highlighted. Some icons have a dark version for the highlighted row.</param>
    /// <returns>The sprite name inside the icon's dictionary, or an empty string for <see cref="Icon.NONE"/>.</returns>
    protected internal string GetSpriteName(Icon icon, bool selected)
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
            case Icon.INFO: return "info_icon_32";
            default:
                break;
        }
        return "";
    }

    /// <summary>
    /// Get the size the given icon is drawn at.
    /// </summary>
    /// <param name="icon">The icon to look up.</param>
    /// <returns>The width and height of the icon in pixels.</returns>
    protected internal static float GetSpriteSizePx(Icon icon)
    {
        return icon switch
        {
            Icon.CASH or Icon.COKE or Icon.CROWN or Icon.HEROIN or Icon.METH or Icon.WEED or Icon.ADVERSARY or Icon.BASE_JUMPING or Icon.BRIEFCASE or Icon.MISSION_STAR or Icon.DEATHMATCH or Icon.CASTLE or Icon.TROPHY or Icon.RACE_FLAG or Icon.RACE_FLAG_PLANE or Icon.RACE_FLAG_BICYCLE or Icon.RACE_FLAG_PERSON or Icon.RACE_FLAG_CAR or Icon.RACE_FLAG_BOAT_ANCHOR or Icon.ROCKSTAR or Icon.STUNT or Icon.STUNT_PREMIUM or Icon.RACE_FLAG_STUNT_JUMP or Icon.SHIELD or Icon.TEAM_DEATHMATCH or Icon.VEHICLE_DEATHMATCH or Icon.AUDIO_MUTE or Icon.AUDIO_INACTIVE or Icon.AUDIO_VOL1 or Icon.AUDIO_VOL2 or Icon.AUDIO_VOL3 or Icon.BRAND_ALBANY or Icon.BRAND_ANNIS or Icon.BRAND_BANSHEE or Icon.BRAND_BENEFACTOR or Icon.BRAND_BF or Icon.BRAND_BOLLOKAN or Icon.BRAND_BRAVADO or Icon.BRAND_BRUTE or Icon.BRAND_BUCKINGHAM or Icon.BRAND_CANIS or Icon.BRAND_CHARIOT or Icon.BRAND_CHEVAL or Icon.BRAND_CLASSIQUE or Icon.BRAND_COIL or Icon.BRAND_DECLASSE or Icon.BRAND_DEWBAUCHEE or Icon.BRAND_DILETTANTE or Icon.BRAND_DINKA or Icon.BRAND_DUNDREARY or Icon.BRAND_EMPORER or Icon.BRAND_ENUS or Icon.BRAND_FATHOM or Icon.BRAND_GALIVANTER or Icon.BRAND_GROTTI or Icon.BRAND_HIJAK or Icon.BRAND_HVY or Icon.BRAND_IMPONTE or Icon.BRAND_INVETERO or Icon.BRAND_JACKSHEEPE or Icon.BRAND_JOBUILT or Icon.BRAND_KARIN or Icon.BRAND_LAMPADATI or Icon.BRAND_MAIBATSU or Icon.BRAND_MAMMOTH or Icon.BRAND_MTL or Icon.BRAND_NAGASAKI or Icon.BRAND_OBEY or Icon.BRAND_OCELOT or Icon.BRAND_OVERFLOD or Icon.BRAND_PED or Icon.BRAND_PEGASSI or Icon.BRAND_PFISTER or Icon.BRAND_PRINCIPE or Icon.BRAND_PROGEN or Icon.BRAND_SCHYSTER or Icon.BRAND_SHITZU or Icon.BRAND_SPEEDOPHILE or Icon.BRAND_STANLEY or Icon.BRAND_TRUFFADE or Icon.BRAND_UBERMACHT or Icon.BRAND_VAPID or Icon.BRAND_VULCAR or Icon.BRAND_WEENY or Icon.BRAND_WESTERN or Icon.BRAND_WESTERNMOTORCYCLE or Icon.BRAND_WILLARD or Icon.BRAND_ZIRCONIUM or Icon.BRAND_GROTTI2 or Icon.BRAND_LCC or Icon.BRAND_PROGEN2 or Icon.BRAND_RUNE or Icon.COUNTRY_USA or Icon.COUNTRY_UK or Icon.COUNTRY_SWEDEN or Icon.COUNTRY_KOREA or Icon.COUNTRY_JAPAN or Icon.COUNTRY_ITALY or Icon.COUNTRY_GERMANY or Icon.COUNTRY_FRANCE => 30f,
            Icon.STAR or Icon.LOCK_ARENA => 52f,
            Icon.MEDAL_SILVER or Icon.MP_AMMO_PICKUP or Icon.MP_AMMO or Icon.MP_CASH or Icon.MP_RP or Icon.GLOBE_WHITE or Icon.GLOBE_BLUE or Icon.GLOBE_GREEN or Icon.GLOBE_ORANGE or Icon.GLOBE_RED or Icon.GLOBE_YELLOW or Icon.INV_ARM_WRESTLING or Icon.INV_BASEJUMP or Icon.INV_MISSION or Icon.INV_DARTS or Icon.INV_DEATHMATCH or Icon.INV_DRUG or Icon.INV_CASTLE or Icon.INV_GOLF or Icon.INV_BIKE or Icon.INV_BOAT or Icon.INV_ANCHOR or Icon.INV_CAR or Icon.INV_DOLLAR or Icon.INV_COKE or Icon.INV_KEY or Icon.INV_DATA or Icon.INV_HELI or Icon.INV_HEORIN or Icon.INV_KEYCARD or Icon.INV_METH or Icon.INV_BRIEFCASE or Icon.INV_LINK or Icon.INV_PERSON or Icon.INV_PLANE or Icon.INV_PLANE2 or Icon.INV_QUESTIONMARK or Icon.INV_REMOTE or Icon.INV_SAFE or Icon.INV_STEER_WHEEL or Icon.INV_WEAPON or Icon.INV_WEED or Icon.INV_RACE_FLAG_PLANE or Icon.INV_RACE_FLAG_BICYCLE or Icon.INV_RACE_FLAG_BOAT_ANCHOR or Icon.INV_RACE_FLAG_PERSON or Icon.INV_RACE_FLAG_CAR or Icon.INV_RACE_FLAG_HELMET or Icon.INV_SHOOTING_RANGE or Icon.INV_SURVIVAL or Icon.INV_TEAM_DEATHMATCH or Icon.INV_TENNIS or Icon.INV_VEHICLE_DEATHMATCH => 22f,
            _ => 38f,
        };
    }

    /// <summary>
    /// Get the sprite color for the provided icon depending on the current state of the item (its <see cref="Enabled"/> and selected values).
    /// </summary>
    /// <param name="icon">The icon to look up.</param>
    /// <param name="selected">Whether the item is highlighted.</param>
    /// <returns>The color to tint the icon with.</returns>
    protected internal System.Drawing.Color GetSpriteColour(Icon icon, bool selected)
    {
        return icon switch
        {
            Icon.CROWN or Icon.TICK or Icon.MALE or Icon.FEMALE or Icon.LOCK or Icon.LOCK_ARENA or Icon.ADVERSARY or Icon.BASE_JUMPING or Icon.BRIEFCASE or Icon.MISSION_STAR or Icon.DEATHMATCH or Icon.CASTLE or Icon.TROPHY or Icon.RACE_FLAG or Icon.RACE_FLAG_PLANE or Icon.RACE_FLAG_BICYCLE or Icon.RACE_FLAG_PERSON or Icon.RACE_FLAG_CAR or Icon.RACE_FLAG_BOAT_ANCHOR or Icon.ROCKSTAR or Icon.STUNT or Icon.STUNT_PREMIUM or Icon.RACE_FLAG_STUNT_JUMP or Icon.SHIELD or Icon.TEAM_DEATHMATCH or Icon.VEHICLE_DEATHMATCH or Icon.MP_SPECTATING or Icon.GLOBE_WHITE or Icon.AUDIO_MUTE or Icon.AUDIO_INACTIVE or Icon.AUDIO_VOL1 or Icon.AUDIO_VOL2 or Icon.AUDIO_VOL3 or Icon.BRAND_ALBANY or Icon.BRAND_ANNIS or Icon.BRAND_BANSHEE or Icon.BRAND_BENEFACTOR or Icon.BRAND_BF or Icon.BRAND_BOLLOKAN or Icon.BRAND_BRAVADO or Icon.BRAND_BRUTE or Icon.BRAND_BUCKINGHAM or Icon.BRAND_CANIS or Icon.BRAND_CHARIOT or Icon.BRAND_CHEVAL or Icon.BRAND_CLASSIQUE or Icon.BRAND_COIL or Icon.BRAND_DECLASSE or Icon.BRAND_DEWBAUCHEE or Icon.BRAND_DILETTANTE or Icon.BRAND_DINKA or Icon.BRAND_DUNDREARY or Icon.BRAND_EMPORER or Icon.BRAND_ENUS or Icon.BRAND_FATHOM or Icon.BRAND_GALIVANTER or Icon.BRAND_GROTTI or Icon.BRAND_HIJAK or Icon.BRAND_HVY or Icon.BRAND_IMPONTE or Icon.BRAND_INVETERO or Icon.BRAND_JACKSHEEPE or Icon.BRAND_JOBUILT or Icon.BRAND_KARIN or Icon.BRAND_LAMPADATI or Icon.BRAND_MAIBATSU or Icon.BRAND_MAMMOTH or Icon.BRAND_MTL or Icon.BRAND_NAGASAKI or Icon.BRAND_OBEY or Icon.BRAND_OCELOT or Icon.BRAND_OVERFLOD or Icon.BRAND_PED or Icon.BRAND_PEGASSI or Icon.BRAND_PFISTER or Icon.BRAND_PRINCIPE or Icon.BRAND_PROGEN or Icon.BRAND_SCHYSTER or Icon.BRAND_SHITZU or Icon.BRAND_SPEEDOPHILE or Icon.BRAND_STANLEY or Icon.BRAND_TRUFFADE or Icon.BRAND_UBERMACHT or Icon.BRAND_VAPID or Icon.BRAND_VULCAR or Icon.BRAND_WEENY or Icon.BRAND_WESTERN or Icon.BRAND_WESTERNMOTORCYCLE or Icon.BRAND_WILLARD or Icon.BRAND_ZIRCONIUM or Icon.BRAND_GROTTI2 or Icon.BRAND_LCC or Icon.BRAND_PROGEN2 or Icon.BRAND_RUNE => selected ? (Enabled ? System.Drawing.Color.FromArgb(0, 0, 0) : System.Drawing.Color.FromArgb(50, 50, 50)) : (Enabled ? System.Drawing.Color.FromArgb(255, 255, 255) : System.Drawing.Color.FromArgb(109, 109, 109)),
            Icon.GLOBE_BLUE => Enabled ? System.Drawing.Color.FromArgb(10, 103, 166) : System.Drawing.Color.FromArgb(11, 62, 117),
            Icon.GLOBE_GREEN => Enabled ? System.Drawing.Color.FromArgb(10, 166, 85) : System.Drawing.Color.FromArgb(5, 71, 22),
            Icon.GLOBE_ORANGE => Enabled ? System.Drawing.Color.FromArgb(232, 145, 14) : System.Drawing.Color.FromArgb(133, 77, 12),
            Icon.GLOBE_RED => Enabled ? System.Drawing.Color.FromArgb(207, 43, 31) : System.Drawing.Color.FromArgb(110, 7, 7),
            Icon.GLOBE_YELLOW => Enabled ? System.Drawing.Color.FromArgb(232, 207, 14) : System.Drawing.Color.FromArgb(131, 133, 12),
            _ => Enabled ? System.Drawing.Color.FromArgb(255, 255, 255) : System.Drawing.Color.FromArgb(109, 109, 109),
        };
    }

    /// <summary>
    /// Handles menu navigation to the right for items that support it, otherwise the item will be selected.
    /// </summary>
    internal virtual void GoRight()
    {
        if (Enabled)
        {
            ParentMenu?.SelectItem(this);
        }
    }

    /// <summary>
    /// Handles menu navigation to the left for items that support it, otherwise the menu will navigate to the parent menu.
    /// </summary>
    internal virtual void GoLeft()
    {
        if (MenuController.NavigateMenuUsingArrows && !MenuController.DisableBackButton && !(MenuController.PreventExitingMenu && ParentMenu == null))
        {
            ParentMenu?.GoBack();
        }
    }

    /// <summary>
    /// Handles item selection.
    /// </summary>
    internal virtual void Select()
    {
        ParentMenu?.ItemSelectedEvent(this, Index);
    }

    internal virtual void PrepareForDisplay() { }
}