using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared;

namespace MenuAPI;

/// <summary>
/// Every per frame loop MenuAPI runs. Registration is internal on purpose: this schedules MenuAPI's
/// own work and is not a general purpose tick registry for other resources to add to. Reading it is
/// public, so a resource can show what MenuAPI is doing in its own debug tooling.
/// </summary>
public static class MenuTicks
{
    private static readonly MenuTickEngine Engine = new(ms => API.Delay(ms), () => API.Yield(), Write);

    private static bool _initialized;

    private static bool _reevaluatePending;

    /// <summary>Every registered tick, running or not.</summary>
    public static IReadOnlyList<MenuTickHandle> Handles => Engine.Handles;

    /// <summary>Raised when a tick starts or stops.</summary>
    // Reevaluate raises this once per tick it flips, so a subscriber doing real work (redrawing a
    // panel, sending a NUI message) should set a dirty flag and act once, not act per event.
    public static event Action? Changed
    {
        add => Engine.Changed += value;
        remove => Engine.Changed -= value;
    }

    /// <summary>One "{name} ({rate}): running|stopped" line per tick.</summary>
    public static IEnumerable<string> Describe() => Engine.Describe();

    /// <summary>Writes every tick and its state to the console.</summary>
    public static void Dump()
    {
        API.Log.Info($"[MenuAPI] {Engine.Handles.Count} ticks registered:");

        foreach (var line in Engine.Describe())
        {
            API.Log.Info("[MenuAPI]   " + line);
        }
    }

    internal static void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;

        // Carries the resource name, like the key bindings do, so two resources that both ship
        // MenuAPI do not fight over one command name.
        SharedAPI.Commands.RegisterCommand($"{MenuKeyBindings.Prefix}ticks", false, new Action(Dump));

        Engine.Register("Menu.Scheduler", Flush, MenuTickRate.PerFrame);
    }

    /// <summary>Asks for every condition to be re-checked on the next frame.</summary>
    // Deliberately not immediate. Opening a submenu closes the parent and opens the child in one go,
    // so acting on each half separately would see "no menu open" for an instant and tear down the
    // instructional buttons scaleform and the menu textures, only to rebuild them a moment later.
    // Waiting a frame lets a close and an open that belong together cancel each other out.
    internal static void Reevaluate() => _reevaluatePending = true;

    private static void Flush()
    {
        FrameState.Invalidate();

        if (!_reevaluatePending)
        {
            return;
        }

        _reevaluatePending = false;

        Engine.Reevaluate();
    }

    internal static MenuTickHandle Register(
        string name,
        Func<Task> handler,
        MenuTickRate rate = default,
        Func<bool>? condition = null,
        Action? onStarted = null,
        Action? onStopped = null,
        bool autoStart = true) =>
        Engine.Register(name, handler, rate, condition, onStarted, onStopped, autoStart);

    internal static MenuTickHandle Register(
        string name,
        Action handler,
        MenuTickRate rate = default,
        Func<bool>? condition = null,
        Action? onStarted = null,
        Action? onStopped = null,
        bool autoStart = true) =>
        Engine.Register(name, handler, rate, condition, onStarted, onStopped, autoStart);

    private static void Write(MenuTickLog level, string message)
    {
        switch (level)
        {
            case MenuTickLog.Error:
                API.Log.Error($"[MenuAPI] {message}");
                break;
            case MenuTickLog.Warn:
                API.Log.Warn($"[MenuAPI] {message}");
                break;
            case MenuTickLog.Info:
                API.Log.Info($"[MenuAPI] {message}");
                break;
            default:
                API.Log.Debug($"[MenuAPI] {message}");
                break;
        }
    }
}
