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
}
