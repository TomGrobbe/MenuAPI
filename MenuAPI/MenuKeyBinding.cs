namespace MenuAPI;

/// <summary>When a key binding's handler is called.</summary>
public enum MenuKeyPressType
{
    /// <summary>Once, the moment the key is let go.</summary>
    JUST_RELEASED,

    /// <summary>Once, the moment the key goes down.</summary>
    JUST_PRESSED,

    /// <summary>Every frame the key is not held.</summary>
    RELEASED,

    /// <summary>Every frame the key is held.</summary>
    PRESSED
}

/// <summary>A rebindable key. Every menu control is one, and so is every key added with <see cref="Menu.AddKeyBinding"/>.</summary>
public sealed class MenuKeyBinding
{
    internal const int Keyboard = 0;
    internal const int Controller = 1;
    internal const int Mouse = 2;
    internal const int AlternateKeyboard = 3;
    internal const int DeviceCount = 4;

    private readonly bool[] held = new bool[DeviceCount];
    private readonly bool[] armed = new bool[DeviceCount];

    private bool pressed;
    private bool released;

    internal MenuKeyBinding(string name, string command)
    {
        Name = name;
        Command = command;
        KeyboardControl = MenuInput.BindingControl($"+{command}");
        ControllerControl = MenuInput.BindingControl($"+{MenuInput.PadCommand(command)}");
    }

    /// <summary>The name it was registered under, without the resource prefix.</summary>
    public string Name { get; }

    /// <summary>The console command FiveM runs for it.</summary>
    public string Command { get; }

    /// <summary>Whether the key is held right now, on any device.</summary>
    public bool Held => Array.IndexOf(held, true) >= 0;

    /// <summary>Its instructional button icon, for the device in use. Empty when the game has none.</summary>
    public string Icon => MenuInput.IconFor(this);

    internal int KeyboardControl { get; }

    internal int ControllerControl { get; }

    internal string KeyboardDefault { get; set; } = string.Empty;

    internal string ControllerDefault { get; set; } = string.Empty;

    internal void Press(int device)
    {
        held[device] = true;
        pressed = true;
        armed[device] = true;
    }

    internal void Release(int device)
    {
        held[device] = false;

        if (!armed[device])
        {
            return;
        }

        armed[device] = false;
        released = true;
    }

    internal bool ConsumePress()
    {
        bool pending = pressed;
        pressed = false;
        return pending;
    }

    internal bool ConsumeRelease()
    {
        bool pending = released;
        released = false;
        return pending;
    }

    internal void ClearHeld() => Array.Clear(held);

    internal void ClearPending()
    {
        Array.Clear(armed);
        pressed = false;
        released = false;
    }
}
