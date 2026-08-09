using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared.Data;

using static CitizenFX.FiveM.Client.Native;

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
    // it is not drawn at banner size. It is drawn near fullscreen and slid until only the glare part
    // lands on the header, which is what these are. Found by eye, and they differ per alignment
    // because the movie is placed in plain screen coordinates rather than menu ones.
    private const float LeftAlignedX = 0.553f;
    private const float LeftAlignedY = 0.561f;
    private const float RightAlignedX = 1.243f;
    private const float RightAlignedY = 0.561f;
    private const float MovieWidth = 1.07f;
    private const float MovieHeight = 1.13f;

    /// <summary>Draws the glare for this frame. Silently does nothing until the movie has loaded.</summary>
    internal static void Draw(bool leftAligned)
    {
        if (!HasScaleformMovieLoaded(_handle))
        {
            _handle = RequestScaleformMovie(ScaleformName);

            if (!HasScaleformMovieLoaded(_handle))
            {
                return;
            }
        }

        float heading = Wrap(GetFinalRenderedCamRot(2).Z);

        if (Math.Abs(_direction - heading) > RotationTolerance)
        {
            _direction = heading;

            BeginScaleformMovieMethod(_handle, "SET_DATA_SLOT");
            ScaleformMovieMethodAddParamFloat(_direction);
            EndScaleformMovieMethod();
        }

        DrawScaleformMovie(
            _handle,
            leftAligned ? LeftAlignedX : RightAlignedX,
            leftAligned ? LeftAlignedY : RightAlignedY,
            MovieWidth,
            MovieHeight,
            255, 255, 255, 255,
            0);
    }

    /// <summary>Releases the movie. Called when the last menu closes.</summary>
    internal static void Dispose()
    {
        if (HasScaleformMovieLoaded(_handle))
        {
            SetScaleformMovieAsNoLongerNeeded(new Ref<int>(ref _handle));
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
