namespace MenuAPI;

/// <summary>
/// Debug nudges applied on top of whatever <see cref="HeaderGlare"/> works out for itself, for
/// checking the glare on a screen shape it has never been measured at. Every offset starts at zero,
/// so an offset still sitting at zero when the glare looks right means the placement was already
/// correct on its own, and an offset you had to move is the correction that screen needs.
/// </summary>
/// <remarks>
/// TestMenu's "Header styling" menu drives all of this and can print the numbers to the console.
/// Nothing here affects a normal resource unless it deliberately sets one of these.
/// </remarks>
public static class GlareTuning
{
    /// <summary>
    /// Holds the glare still instead of letting it sweep with the camera. Two screens cannot be
    /// compared by eye while it is moving, because the camera is never pointing the same way twice.
    /// </summary>
    public static bool PinHeading { get; set; } = false;

    public static float PinnedHeading { get; set; } = 90f;

    public static float XOffset { get; set; }
    public static float YOffset { get; set; }
    public static float WidthOffset { get; set; }
    public static float HeightOffset { get; set; }

    /// <summary>What actually went to the scaleform on the most recently drawn frame.</summary>
    public static float DrawnX { get; internal set; }

    public static float DrawnY { get; internal set; }

    public static float DrawnWidth { get; internal set; }

    public static float DrawnHeight { get; internal set; }
}
