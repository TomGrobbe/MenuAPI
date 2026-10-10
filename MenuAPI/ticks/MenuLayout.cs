namespace MenuAPI;

/// <summary>
/// The screen values the NUI layout needs, read once instead of every frame.
/// </summary>
// Aspect ratio and safe zone only change when the player changes a setting, and the safe zone slider
// lives in the pause menu, which hides menus anyway.
internal static class MenuLayout
{
    private static bool _computed;

    internal static float ScreenWidth { get; private set; }

    internal static float SafeZone { get; private set; }

    /// <summary>Reads the screen values back from the game.</summary>
    internal static void Refresh()
    {
        var previousWidth = ScreenWidth;
        var previousSafeZone = SafeZone;

        ScreenWidth = 1080f * Native.GetScreenAspectRatio(false);
        SafeZone = Native.GetSafeZoneSize();

        if (ScreenWidth != previousWidth || SafeZone != previousSafeZone)
        {
            MenuNui.Invalidate();
        }

        _computed = true;
    }

    /// <summary>
    /// Works the values out if nothing has yet, so the very first frame never reads an empty cache
    /// no matter what order things start in.
    /// </summary>
    internal static void EnsureComputed()
    {
        if (!_computed)
        {
            Refresh();
        }
    }
}
