using CitizenFX.FiveM.Client;

namespace MenuAPI;

/// <summary>
/// The button icon strings the instructional buttons bar is built from, asked of the game once per
/// control instead of once per control per frame.
/// </summary>
// GetControlInstructionalButton looks up whatever the player has that control bound to and builds the
// icon string for it, and half a dozen of them went out every frame for answers that hardly ever
// change. What does change them is the player switching between keyboard and controller, or rebinding
// a key, so both of those throw the kept icons away: the input device is checked each frame, and a
// rebind can only have happened behind the pause menu, or while every menu was closed.
internal static class InstructionalButtonIcons
{
    private static readonly Dictionary<int, string> Icons = new Dictionary<int, string>();

    private static bool refreshed;
    private static bool pauseMenuWasActive;

    /// <summary>Whether the player is on keyboard and mouse rather than on a controller.</summary>
    internal static bool UsingKeyboard { get; private set; }

    /// <summary>
    /// Reads the input device back from the game and drops every icon if anything that decides what an
    /// icon looks like has changed. Meant to be called once at the top of a frame, so nothing below it
    /// has to ask again.
    /// </summary>
    internal static void Refresh()
    {
        bool usingKeyboard = Native.IsUsingKeyboardAndMouse(2);

        // The key mapping settings live in the pause menu, so a rebind is always followed by the pause
        // menu closing. Catching that edge is what keeps a rebound key's icon from staying wrong for as
        // long as the menu is open.
        bool pauseMenuActive = Native.IsPauseMenuActive();
        bool leftPauseMenu = pauseMenuWasActive && !pauseMenuActive;
        pauseMenuWasActive = pauseMenuActive;

        if (refreshed && usingKeyboard == UsingKeyboard && !leftPauseMenu)
        {
            return;
        }

        UsingKeyboard = usingKeyboard;
        refreshed = true;
        Icons.Clear();
    }

    /// <summary>The icon string for <paramref name="control"/>, asked of the game only the first time.</summary>
    internal static string For(int control)
    {
        if (!refreshed)
        {
            Refresh();
        }

        if (!Icons.TryGetValue(control, out string? icon))
        {
            icon = Native.GetControlInstructionalButton(0, control, true);
            Icons[control] = icon;
        }

        return icon;
    }

    /// <summary>Forgets everything, so the next frame that needs an icon asks the game again.</summary>
    internal static void Clear()
    {
        Icons.Clear();
        refreshed = false;
        pauseMenuWasActive = false;
    }
}
