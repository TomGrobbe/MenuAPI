using CitizenFX.FiveM.Client;

namespace MenuAPI;

/// <summary>
/// Everything about the screen that the drawing code needs, worked out once instead of per draw call.
/// </summary>
// Aspect ratio, screen size and safe zone only change when the player changes a setting, and the safe
// zone slider lives in the pause menu, which stops menus drawing anyway. Asking the game for them at
// each of the hundred-odd places that needed them was most of the native calls in a drawn frame.
//
// Only the values that cost a native call, or that combine one with a division that is repeated all
// over the drawing code, are kept here. A plain divide is a few nanoseconds, so the one-off ones are
// left where they are rather than turned into a wall of named fields.
internal static class MenuLayout
{
    // MenuItem.RowHeight is protected and Menu's header constants are private, so the two this needs
    // are repeated rather than reached for. They are compile time constants on both sides.
    private const float RowHeight = 38f;
    private const float HeaderHeight = 110f;

    private static bool _computed;

    internal static float AspectRatio { get; private set; }

    internal static float ScreenWidth { get; private set; }

    internal static float ScreenHeight { get; private set; }

    internal static float SafeZone { get; private set; }

    /// <summary>A full menu width, as a fraction of the screen.</summary>
    internal static float MenuWidthN { get; private set; }

    internal static float HeaderHeightN { get; private set; }

    internal static float RowHeightN { get; private set; }

    /// <summary>The size every menu item's text is drawn at.</summary>
    internal static float ItemTextSize { get; private set; }

    /// <summary>Centre of a full width row. Constant because menus are always drawn at x 0.</summary>
    internal static float RowCenterX { get; private set; }

    // The right aligned anchors. Each of these used to be a GetSafeZoneSize call plus a division, at
    // several sites, every frame.
    internal static float RightTextMinX { get; private set; }

    internal static float RightTextMaxX { get; private set; }

    internal static float RightIconX { get; private set; }

    internal static float RightWideIconX { get; private set; }

    internal static float RightHeaderCenterX { get; private set; }

    internal static float RightHeaderTextX { get; private set; }

    internal static float RightDescriptionCenterX { get; private set; }

    /// <summary>Reads the screen values back from the game and works the rest out from them.</summary>
    internal static void Refresh()
    {
        AspectRatio = Native.GetScreenAspectRatio(false);
        ScreenWidth = 1080f * AspectRatio;
        ScreenHeight = 1080f;
        SafeZone = Native.GetSafeZoneSize();

        MenuWidthN = Menu.Width / ScreenWidth;
        HeaderHeightN = HeaderHeight / ScreenHeight;
        RowHeightN = RowHeight / ScreenHeight;
        ItemTextSize = (14f * 27f) / ScreenHeight;
        RowCenterX = (Menu.Width / 2f) / ScreenWidth;

        RightTextMinX = SafeZone - ((Menu.Width - 10f) / ScreenWidth);
        RightTextMaxX = SafeZone - (10f / ScreenWidth);
        RightIconX = SafeZone - (20f / ScreenWidth);
        RightWideIconX = SafeZone - ((Menu.Width - 20f) / ScreenWidth);
        RightHeaderCenterX = SafeZone - ((Menu.Width / 2f) / ScreenWidth);
        RightHeaderTextX = SafeZone - ((Menu.Width - 10f) / ScreenWidth);
        RightDescriptionCenterX = SafeZone - (250f / ScreenWidth);

        _computed = true;
    }

    /// <summary>
    /// Works the values out if nothing has yet, so the very first drawn frame never reads an empty
    /// cache no matter what order things start in.
    /// </summary>
    internal static void EnsureComputed()
    {
        if (!_computed)
        {
            Refresh();
        }
    }
}
