using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared.Data;

namespace MenuAPI;

/// <summary>
/// The moving glow GTA Online draws behind its pause menu banner, ported onto MenuAPI's header.
/// </summary>
/// <remarks>
/// This is the game's own <c>mp_menu_glare</c> scaleform, so there is no NUI involved. The movie
/// animates itself: it is handed a heading in degrees and eases its glare towards that angle on its
/// own frame loop, which is why nothing here interpolates anything.
/// </remarks>
internal static class HeaderGlare
{
    private const string ScaleformName = "mp_menu_glare";

    // The movie only ever eases towards a new angle, so feeding it every sub degree camera twitch
    // costs a scaleform call for a movement nobody can see.
    private const float RotationTolerance = 0.5f;

    private static int _handle = -1;
    private static float _direction;

    // mp_menu_glare lays itself out across the whole screen with the glare sitting in one corner, so
    // it is not drawn at header size. It is drawn near fullscreen and slid sideways until only the
    // glare part lands on the header. These two are the size of that near fullscreen draw.
    private const float MovieWidth = 1.07f;
    private const float MovieHeight = 1.13f;

    // Where the glare ends up inside the movie depends on the shape of the screen, so how far the
    // movie has to be slid past the header depends on it too. Measured by hand at 4:3, 25:16, 16:10
    // and 16:9, where the slide came out as a straight line against the aspect ratio squared, hitting
    // every one of those to well under a pixel.
    //
    // These are fitted to measurements rather than derived from anything, and only across 4:3 to
    // 16:9. A single reading just past 16:9 wanted noticeably less slide than the curve predicts, so
    // the glare very likely sits a little too far right on ultrawide. GlareTuning plus TestMenu's
    // header styling menu are what these numbers were measured with, if that ever needs redoing.
    private const float SlideBase = 0.227442f;
    private const float SlidePerAspectSquared = 0.061761f;

    // Shrinking the safe zone pushes the header down the screen, and the movie has to follow it.
    // Only measured at two safe zone sizes, so this one is roughly right rather than exact.
    private const float ReferenceSafeZone = 0.95f;
    private const float YAtReferenceSafeZone = 0.561f;
    private const float YPerSafeZone = 0.05f;

    /// <summary>Draws the glare for this frame. Silently does nothing until the movie has loaded.</summary>
    internal static void Draw(bool leftAligned)
    {
        if (!Native.HasScaleformMovieLoaded(_handle))
        {
            _handle = Native.RequestScaleformMovie(ScaleformName);

            if (!Native.HasScaleformMovieLoaded(_handle))
            {
                return;
            }
        }

        float heading = GlareTuning.PinHeading
            ? GlareTuning.PinnedHeading
            : Wrap(Native.GetFinalRenderedCamRot(2).Z);

        if (Math.Abs(_direction - heading) > RotationTolerance)
        {
            _direction = heading;

            Native.BeginScaleformMovieMethod(_handle, "SET_DATA_SLOT");
            Native.ScaleformMovieMethodAddParamFloat(_direction);
            Native.EndScaleformMovieMethod();
        }

        float aspect = MenuLayout.AspectRatio;

        // Anchoring to the header's own centre is what makes both alignments and the safe zone fall
        // out for free, since those anchors already account for them.
        float x = (leftAligned ? MenuLayout.RowCenterX : MenuLayout.RightHeaderCenterX)
            + SlideBase
            + (SlidePerAspectSquared * aspect * aspect);

        float y = YAtReferenceSafeZone - ((ReferenceSafeZone - MenuLayout.SafeZone) * YPerSafeZone);

        GlareTuning.DrawnX = x + GlareTuning.XOffset;
        GlareTuning.DrawnY = y + GlareTuning.YOffset;
        GlareTuning.DrawnWidth = MovieWidth + GlareTuning.WidthOffset;
        GlareTuning.DrawnHeight = MovieHeight + GlareTuning.HeightOffset;

        Native.DrawScaleformMovie(
            _handle,
            GlareTuning.DrawnX,
            GlareTuning.DrawnY,
            GlareTuning.DrawnWidth,
            GlareTuning.DrawnHeight,
            255, 255, 255, 255,
            0);
    }

    /// <summary>Releases the movie. Called when the last menu closes.</summary>
    internal static void Dispose()
    {
        if (Native.HasScaleformMovieLoaded(_handle))
        {
            Native.SetScaleformMovieAsNoLongerNeeded(ref _handle);
        }

        // The movie starts its fade in from scratch next time, so the angle it was last told about
        // means nothing any more.
        _direction = 0f;
    }

    /// <summary>Camera rotation comes back as -180 to 180, the movie wants 0 to 360.</summary>
    private static float Wrap(float degrees)
    {
        float wrapped = degrees % 360f;

        return wrapped < 0f ? wrapped + 360f : wrapped;
    }
}
