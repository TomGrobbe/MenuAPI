namespace MenuAPI;

/// <summary>
/// The game's built in font ids, for use with <see cref="Menu.MenuTitleFont"/> and
/// <see cref="MenuController.DefaultTitleFont"/>.
/// </summary>
/// <remarks>
/// These are plain ints rather than an enum so a font registered at runtime through
/// <c>RegisterFontId</c> can be used just as easily. Ids not listed here are either duplicates of
/// the ones that are, or language specific fallbacks that resolve to one of them.
/// </remarks>
public static class MenuFont
{
    public const int ChaletLondon = 0;

    /// <summary>The handwritten look GTA uses for menu banners. MenuAPI's default.</summary>
    public const int HouseScript = 1;

    public const int Monospace = 2;

    public const int ChaletComprimeCologne = 4;

    /// <summary>The Grand Theft Auto logo font.</summary>
    public const int Pricedown = 7;

    // Each font disagrees about how big a given text scale is and about where its baseline sits, so
    // one shared size and one shared nudge would leave half of them looking wrong. Both tables were
    // picked by eye against the standard banner. A font neither table knows about, which means one
    // registered at runtime, gets a middle of the road value to start from.
    //
    // Internal on purpose. These are what makes a title sit correctly on its banner, not a taste
    // setting, so there is nothing to gain from letting a resource move a title off its own header.

    /// <summary>The size a title in <paramref name="font"/> is drawn at.</summary>
    internal static float DefaultSizeFor(int font) => font switch
    {
        ChaletLondon => 0.875f,
        HouseScript => 1.05f,
        Monospace => 1.075f,
        ChaletComprimeCologne => 0.975f,
        Pricedown => 1.05f,
        _ => 1f
    };

    /// <summary>
    /// How far a title in <paramref name="font"/> is nudged vertically, in pixels at 1080p, to sit
    /// centred on the banner. Positive is down.
    /// </summary>
    internal static float DefaultOffsetYFor(int font) => font switch
    {
        ChaletLondon => -4f,
        HouseScript => 0f,
        Monospace => -4f,
        ChaletComprimeCologne => 0f,
        Pricedown => 3f,
        _ => 0f
    };
}
