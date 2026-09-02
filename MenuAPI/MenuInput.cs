using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared;

namespace MenuAPI;

internal static class MenuInput
{
    internal static readonly string Prefix = $"menuapi:{Native.GetCurrentResourceName().ToLower()}:";

    private const int ControllerToggleHoldMs = 400;

    private const int MaxScrollSteps = 10;

    private static readonly string ToggleCommand = $"{Prefix}toggle";

    private static readonly int UnboundControl = BindingControl($"+{Prefix}unbound");

    private static readonly Dictionary<string, Control> DefaultIcons = new()
    {
        ["UP"] = Control.FrontendUp,
        ["DOWN"] = Control.FrontendDown,
        ["LEFT"] = Control.PhoneLeft,
        ["RIGHT"] = Control.PhoneRight,
        ["RETURN"] = Control.FrontendAccept,
        ["BACK"] = Control.FrontendCancel,
        ["LUP_INDEX"] = Control.FrontendUp,
        ["LDOWN_INDEX"] = Control.FrontendDown,
        ["LLEFT_INDEX"] = Control.PhoneLeft,
        ["LRIGHT_INDEX"] = Control.PhoneRight,
        ["RDOWN_INDEX"] = Control.FrontendAccept,
        ["RRIGHT_INDEX"] = Control.FrontendCancel,
        ["RUP_INDEX"] = Control.FrontendRup,
        ["RLEFT_INDEX"] = Control.FrontendRleft,
        ["L1_INDEX"] = Control.FrontendLb,
        ["R1_INDEX"] = Control.FrontendRb,
        ["L2_INDEX"] = Control.FrontendLt,
        ["R2_INDEX"] = Control.FrontendRt,
        ["L3_INDEX"] = Control.FrontendLs,
        ["R3_INDEX"] = Control.FrontendRs,
        ["SELECT_INDEX"] = Control.FrontendSelect,
        ["START_INDEX"] = Control.FrontendPause,
    };

    private static readonly MenuKeyBinding Up = new("up", $"{Prefix}up");
    private static readonly MenuKeyBinding Down = new("down", $"{Prefix}down");
    private static readonly MenuKeyBinding Left = new("left", $"{Prefix}left");
    private static readonly MenuKeyBinding Right = new("right", $"{Prefix}right");
    private static readonly MenuKeyBinding Select = new("select", $"{Prefix}select");
    private static readonly MenuKeyBinding Back = new("back", $"{Prefix}back");

    private static readonly MenuKeyBinding[] Own = [Up, Down, Left, Right, Select, Back];

    private static readonly Dictionary<string, MenuKeyBinding> Custom = new();

    internal static bool UpHeld => Up.Held;
    internal static bool DownHeld => Down.Held;
    internal static bool LeftHeld => Left.Held;
    internal static bool RightHeld => Right.Held;

    private static bool togglePressed;

    private static int controllerToggleSince = -1;
    private static bool controllerToggleFired;

    private static int scrollUpSteps;
    private static int scrollDownSteps;

    private static bool registered;

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

        RegisterToggle(toggleKey);

        RegisterHold(Up, "Menu up", "UP", "LUP_INDEX");
        RegisterHold(Down, "Menu down", "DOWN", "LDOWN_INDEX");
        RegisterHold(Left, "Menu left", "LEFT", "LLEFT_INDEX");
        RegisterHold(Right, "Menu right", "RIGHT", "LRIGHT_INDEX");
        RegisterHold(Select, "Menu select", "RETURN", "RDOWN_INDEX");
        RegisterHold(Back, "Menu back", "BACK", "RRIGHT_INDEX");

        RegisterMouseHold(Select, "Menu select", "MOUSE_LEFT");
        RegisterMouseHold(Back, "Menu back", "MOUSE_RIGHT");

        RegisterAlternate(Back, "Menu back", "ESCAPE");

        RegisterScroll(Up, "Menu up", "IOM_WHEEL_UP", AddScrollUp);
        RegisterScroll(Down, "Menu down", "IOM_WHEEL_DOWN", AddScrollDown);

        SharedAPI.Commands.RegisterCommand($"{Prefix}buttons", false, new Action(DumpButtons));
    }

    internal static MenuKeyBinding RegisterCustom(string name, string description, string keyboardKey, string? controllerButton)
    {
        if (Custom.TryGetValue(name, out MenuKeyBinding? existing))
        {
            return existing;
        }

        MenuKeyBinding binding = new(name, $"{Prefix}{name}");

        Custom[name] = binding;

        binding.KeyboardDefault = keyboardKey;
        RegisterHoldCommands(binding, binding.Command, MenuKeyBinding.Keyboard);
        Native.RegisterKeyMapping($"+{binding.Command}", description, "keyboard", keyboardKey);

        if (string.IsNullOrWhiteSpace(controllerButton))
        {
            return binding;
        }

        binding.ControllerDefault = controllerButton;

        string pad = PadCommand(binding.Command);
        RegisterHoldCommands(binding, pad, MenuKeyBinding.Controller);
        Native.RegisterKeyMapping($"+{pad}", $"{description} (controller)", "PAD_ANALOGBUTTON", controllerButton);

        return binding;
    }

    private static void RegisterToggle(string keyboardKey)
    {
        SharedAPI.Commands.RegisterCommand(ToggleCommand, false, new Action(() => togglePressed = true));
        Native.RegisterKeyMapping(ToggleCommand, "Open / close menu", "keyboard", keyboardKey);

        string pad = PadCommand(ToggleCommand);
        SharedAPI.Commands.RegisterCommand($"+{pad}", false, new Action(() =>
        {
            controllerToggleSince = Native.GetGameTimer();
            controllerToggleFired = false;
        }));
        SharedAPI.Commands.RegisterCommand($"-{pad}", false, new Action(() => controllerToggleSince = -1));
        Native.RegisterKeyMapping($"+{pad}", "Open / close menu (controller)", "PAD_ANALOGBUTTON", "SELECT_INDEX");
    }

    private static void RegisterHold(MenuKeyBinding binding, string description, string keyboardKey, string controllerButton)
    {
        binding.KeyboardDefault = keyboardKey;
        binding.ControllerDefault = controllerButton;

        RegisterHoldCommands(binding, binding.Command, MenuKeyBinding.Keyboard);
        Native.RegisterKeyMapping($"+{binding.Command}", description, "keyboard", keyboardKey);

        string pad = PadCommand(binding.Command);
        RegisterHoldCommands(binding, pad, MenuKeyBinding.Controller);
        Native.RegisterKeyMapping($"+{pad}", $"{description} (controller)", "PAD_ANALOGBUTTON", controllerButton);
    }

    private static void RegisterAlternate(MenuKeyBinding binding, string description, string keyboardKey)
    {
        string alternate = $"{binding.Command}:alt";

        RegisterHoldCommands(binding, alternate, MenuKeyBinding.AlternateKeyboard);
        Native.RegisterKeyMapping($"+{alternate}", $"{description} (alternate)", "keyboard", keyboardKey);
    }

    private static void RegisterMouseHold(MenuKeyBinding binding, string description, string mouseButton)
    {
        string mouse = MouseCommand(binding.Command);

        RegisterHoldCommands(binding, mouse, MenuKeyBinding.Mouse);
        Native.RegisterKeyMapping($"+{mouse}", $"{description} (mouse)", "MOUSE_BUTTON", mouseButton);
    }

    private static void RegisterScroll(MenuKeyBinding binding, string description, string wheelDirection, Action onScroll)
    {
        string mouse = MouseCommand(binding.Command);

        SharedAPI.Commands.RegisterCommand(mouse, false, onScroll);
        Native.RegisterKeyMapping(mouse, $"{description} (mouse wheel)", "MOUSE_WHEEL", wheelDirection);
    }

    private static void RegisterHoldCommands(MenuKeyBinding binding, string command, int device)
    {
        SharedAPI.Commands.RegisterCommand($"+{command}", false, new Action(() => binding.Press(device)));
        SharedAPI.Commands.RegisterCommand($"-{command}", false, new Action(() => binding.Release(device)));
    }

    internal static string PadCommand(string command) => $"{command}:pad";

    private static string MouseCommand(string command) => $"{command}:mouse";

    internal static void ClearHeld()
    {
        foreach (MenuKeyBinding binding in Own)
        {
            binding.ClearHeld();
        }

        foreach (MenuKeyBinding binding in Custom.Values)
        {
            binding.ClearHeld();
        }
    }

    /// <summary>
    /// Drops the pending press and release on every custom binding <paramref name="menu"/> is not
    /// about to read this frame.
    /// </summary>
    internal static void DrainUnhandled(Menu? menu)
    {
        foreach (MenuKeyBinding binding in Custom.Values)
        {
            if (menu is not null && menu.Handles(binding))
            {
                continue;
            }

            binding.ClearPending();
        }
    }

    internal static void ClearPending()
    {
        foreach (MenuKeyBinding binding in Own)
        {
            binding.ClearPending();
        }

        foreach (MenuKeyBinding binding in Custom.Values)
        {
            binding.ClearPending();
        }

        scrollUpSteps = 0;
        scrollDownSteps = 0;
    }

    internal static void PollControllerToggleHold(bool enabled)
    {
        if (controllerToggleSince < 0 || controllerToggleFired || !enabled)
        {
            return;
        }

        if (Native.GetGameTimer() - controllerToggleSince < ControllerToggleHoldMs)
        {
            return;
        }

        controllerToggleFired = true;
        togglePressed = true;
    }

    internal static bool ConsumeToggle()
    {
        bool pressed = togglePressed;
        togglePressed = false;
        return pressed;
    }

    internal static bool ConsumeSelect() => Select.ConsumeRelease();

    internal static bool ConsumeBack() => Back.ConsumeRelease();

    internal static int ConsumeScrollUp() => Consume(ref scrollUpSteps);

    internal static int ConsumeScrollDown() => Consume(ref scrollDownSteps);

    private static int Consume(ref int steps)
    {
        int pending = steps;
        steps = 0;
        return pending;
    }

    private static void AddScrollUp() => AddScroll(ref scrollUpSteps);

    private static void AddScrollDown() => AddScroll(ref scrollDownSteps);

    private static void AddScroll(ref int steps)
    {
        if (IsUsingWeaponWheel())
        {
            return;
        }

        steps = Math.Min(steps + 1, MaxScrollSteps);
    }

    private static bool IsUsingWeaponWheel()
    {
        if (FrameState.IsInVehicle)
        {
            return false;
        }
        if (!Native.IsControlPressed(0, (int)Control.SelectWeapon))
        {
            return false;
        }
        return Native.IsControlPressed(0, (int)Control.SelectNextWeapon) || Native.IsControlPressed(0, (int)Control.SelectPrevWeapon);
    }

    internal static string GetSelectButton() => IconFor(Select);

    internal static string GetBackButton() => IconFor(Back);

    internal static string GetLeftButton() => IconFor(Left);

    internal static string GetRightButton() => IconFor(Right);

    internal static string IconFor(MenuKeyBinding binding)
    {
        bool keyboard = InstructionalButtonIcons.UsingKeyboard;

        string icon = InstructionalButtonIcons.For(keyboard ? binding.KeyboardControl : binding.ControllerControl);

        return IsDrawable(icon) ? icon : DefaultIcon(keyboard ? binding.KeyboardDefault : binding.ControllerDefault);
    }

    private static string DefaultIcon(string parameter) =>
        DefaultIcons.TryGetValue(parameter, out Control control) ? InstructionalButtonIcons.For((int)control) : string.Empty;

    private static bool IsDrawable(string icon) =>
        !string.IsNullOrEmpty(icon) && icon != InstructionalButtonIcons.For(UnboundControl);

    internal static void DumpButtons()
    {
        InstructionalButtonIcons.Clear();

        string device = Native.IsUsingKeyboardAndMouse(2) ? "keyboard" : "a controller";

        API.Log.Info($"[MenuAPI] instructional button icons, on {device}:");
        API.Log.Info($"[MenuAPI]   a binding with no icon looks like \"{InstructionalButtonIcons.For(UnboundControl)}\"");

        foreach (MenuKeyBinding binding in Own.Concat(Custom.Values))
        {
            string keyboard = InstructionalButtonIcons.For(binding.KeyboardControl);
            string controller = InstructionalButtonIcons.For(binding.ControllerControl);

            API.Log.Info($"[MenuAPI]   {binding.Command}: keyboard \"{keyboard}\", controller \"{controller}\", " +
                $"defaults {binding.KeyboardDefault} \"{DefaultIcon(binding.KeyboardDefault)}\" and " +
                $"{binding.ControllerDefault} \"{DefaultIcon(binding.ControllerDefault)}\"");
        }
    }

    internal static int BindingControl(string command) => API.HashSigned(command) | int.MinValue;
}
