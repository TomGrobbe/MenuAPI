using System;

using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared;
using static CitizenFX.FiveM.Client.Native;

namespace MenuAPI
{
    /// <summary>
    /// The keyboard menu controls, registered as FiveM key mappings so players can rebind them from their
    /// own settings menu. Controller input is not registered here, <see cref="MenuController"/> keeps
    /// polling that.
    /// </summary>
    internal static class MenuKeyBindings
    {
        // Command names are global across every resource, so they carry the resource name to stop two
        // resources that both ship MenuAPI from fighting over the same commands. Lowercased because the
        // hash the instructional buttons are looked up by is case sensitive, see BindingControl.
        private static readonly string Prefix = $"menuapi:{GetCurrentResourceName().ToLower()}:";

        private static readonly string Toggle = $"{Prefix}toggle";
        private static readonly string Up = $"{Prefix}up";
        private static readonly string Down = $"{Prefix}down";
        private static readonly string Left = $"{Prefix}left";
        private static readonly string Right = $"{Prefix}right";
        private static readonly string Select = $"{Prefix}select";
        private static readonly string Back = $"{Prefix}back";

        internal static bool UpHeld { get; private set; }
        internal static bool DownHeld { get; private set; }
        internal static bool LeftHeld { get; private set; }
        internal static bool RightHeld { get; private set; }

        private static bool togglePressed;
        private static bool selectReleased;
        private static bool backReleased;

        private static bool registered;

        private static readonly int SelectControl = BindingControl($"+{Select}");
        private static readonly int BackControl = BindingControl($"+{Back}");

        /// <summary>
        /// Registers every command and its key mapping. Deliberately called a tick after MenuAPI starts,
        /// so a resource has had the chance to set <see cref="MenuController.MenuToggleKeyDefault"/> first.
        /// </summary>
        internal static void Register()
        {
            if (registered)
            {
                return;
            }
            registered = true;

            string toggleKey = MenuController.MenuToggleKeyDefault;
            if (string.IsNullOrWhiteSpace(toggleKey))
            {
                toggleKey = "M";
            }

            SharedAPI.Commands.RegisterCommand(Toggle, false, new Action(() => togglePressed = true));
            RegisterKeyMapping(Toggle, "Open / close menu", "keyboard", toggleKey);

            RegisterHold(Up, "Menu up", "UP", (held) => UpHeld = held);
            RegisterHold(Down, "Menu down", "DOWN", (held) => DownHeld = held);
            RegisterHold(Left, "Menu left", "LEFT", (held) => LeftHeld = held);
            RegisterHold(Right, "Menu right", "RIGHT", (held) => RightHeld = held);

            // Select and back fire on release, matching what the polled controls used to do.
            RegisterHold(Select, "Menu select", "RETURN", (held) => { if (!held) selectReleased = true; });
            RegisterHold(Back, "Menu back", "BACK", (held) => { if (!held) backReleased = true; });
        }

        /// <summary>
        /// Registers a press/release pair. FiveM runs "+command" on press and "-command" on release, and
        /// only the "+" form goes into the key mapping.
        /// </summary>
        private static void RegisterHold(string command, string description, string defaultKey, Action<bool> setHeld)
        {
            SharedAPI.Commands.RegisterCommand($"+{command}", false, new Action(() => setHeld(true)));
            SharedAPI.Commands.RegisterCommand($"-{command}", false, new Action(() => setHeld(false)));
            RegisterKeyMapping($"+{command}", description, "keyboard", defaultKey);
        }

        /// <summary>
        /// A held direction normally clears itself: whatever FiveM missed, pressing the key again sends a
        /// fresh "+" and "-". Unbinding the key while holding it is the one case that never recovers,
        /// which is what this is for.
        /// </summary>
        internal static void ClearHeld()
        {
            UpHeld = false;
            DownHeld = false;
            LeftHeld = false;
            RightHeld = false;
        }

        internal static bool ConsumeToggle()
        {
            bool pressed = togglePressed;
            togglePressed = false;
            return pressed;
        }

        internal static bool ConsumeSelect()
        {
            bool released = selectReleased;
            selectReleased = false;
            return released;
        }

        internal static bool ConsumeBack()
        {
            bool released = backReleased;
            backReleased = false;
            return released;
        }

        internal static string GetSelectButton() =>
            GetControlInstructionalButton(0, IsUsingKeyboardAndMouse(2) ? SelectControl : (int)Control.FrontendAccept, true);

        internal static string GetBackButton() =>
            GetControlInstructionalButton(0, IsUsingKeyboardAndMouse(2) ? BackControl : (int)Control.FrontendCancel, true);

        /// <summary>
        /// The control id FiveM uses for a key mapping, so the game's own control functions resolve it to
        /// whatever the player bound the command to.
        /// </summary>
        // FiveM keys its bindings on joaat(command) with the top bit set, and its hooks on the game's
        // control functions all switch on that bit. int.MinValue is that top bit. The command has to be
        // the exact string it was registered with, '+' prefix and all.
        private static int BindingControl(string command) => API.HashSigned(command) | int.MinValue;
    }
}
