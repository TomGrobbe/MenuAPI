namespace MenuAPI;

/// <summary>
/// The game's built in font ids, for use with <see cref="Menu.MenuTitleFont"/> and
/// <see cref="MenuController.DefaultTitleFont"/>.
/// </summary>
/// <remarks>
/// Only these ids are styled by the menu. Any other id, including a font registered at runtime through
/// <c>RegisterFontId</c>, falls back to the item font at item size.
/// </remarks>
public static class MenuFont
{
    /// <summary>The game's normal text font.</summary>
    public const int ChaletLondon = 0;

    /// <summary>The handwritten look GTA uses for menu banners. MenuAPI's default.</summary>
    public const int HouseScript = 1;

    /// <summary>A monospaced font.</summary>
    public const int Monospace = 2;

    /// <summary>A narrow version of the game's normal text font.</summary>
    public const int ChaletComprimeCologne = 4;

    /// <summary>The Grand Theft Auto logo font.</summary>
    public const int Pricedown = 7;
}
