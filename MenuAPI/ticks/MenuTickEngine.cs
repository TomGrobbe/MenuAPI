namespace MenuAPI;

internal enum MenuTickLog
{
    Debug,
    Info,
    Warn,
    Error,
}

/// <summary>Every named loop MenuAPI runs, so each one can be named, gated and stopped.</summary>
// The runtime has no tick registration that awaits its handler. ScheduleRepeated takes an Action, so
// an async Task handler re-arms the timer at its first await and the next invocation starts while the
// previous is still suspended. That leaves driving the loop by hand, which MenuTickHandle does.
// Waiting and logging come in through the constructor rather than being called directly, so the
// engine matches vMenu's runtime agnostic version line for line and fixes carry across easily.
// Conditions are a bare Func<bool> so this stays free of everything else in MenuAPI.
internal sealed class MenuTickEngine(Func<long, Task> delay, Func<Task> yield, Action<MenuTickLog, string> write)
{
    private readonly List<MenuTickHandle> _registered = new();

    public IReadOnlyList<MenuTickHandle> Handles => _registered;

    /// <summary>Raised when a tick starts, stops, joins or leaves the engine.</summary>
    // A single Reevaluate pass raises it once per tick it flips, so a subscriber doing real work
    // should coalesce.
    public event Action? Changed;

    /// <param name="condition">
    /// Re-run by <see cref="Reevaluate"/>. When null the tick answers to <see cref="MenuTickHandle.Start"/>
    /// and <see cref="MenuTickHandle.Stop"/> instead.
    /// </param>
    /// <param name="autoStart">Ignored when <paramref name="condition"/> is set.</param>
    public MenuTickHandle Register(
        string name,
        Func<Task> handler,
        MenuTickRate rate = default,
        Func<bool>? condition = null,
        Action? onStarted = null,
        Action? onStopped = null,
        bool autoStart = true)
    {
        var handle = new MenuTickHandle(this, name, handler, rate, condition, autoStart)
        {
            OnStarted = onStarted,
            OnStopped = onStopped,
        };

        _registered.Add(handle);

        handle.Apply();

        NotifyChanged();

        return handle;
    }

    /// <summary>Wrapped once here rather than once per iteration.</summary>
    public MenuTickHandle Register(
        string name,
        Action handler,
        MenuTickRate rate = default,
        Func<bool>? condition = null,
        Action? onStarted = null,
        Action? onStopped = null,
        bool autoStart = true)
    {
        return Register(
            name,
            () =>
            {
                handler();

                return Task.CompletedTask;
            },
            rate,
            condition,
            onStarted,
            onStopped,
            autoStart);
    }

    /// <summary>Re-runs every condition.</summary>
    public void Reevaluate()
    {
        // Indexed, because a condition is caller code and one that registers or disposes a tick
        // would invalidate the enumerator mid pass.
        for (var i = 0; i < _registered.Count; i++)
        {
            _registered[i].Apply();
        }
    }

    public IEnumerable<string> Describe()
    {
        foreach (var handle in _registered)
        {
            yield return $"{handle.Name} ({handle.Rate}): {(handle.IsRunning ? "running" : "stopped")}";
        }
    }

    internal Task DelayAsync(long milliseconds) => delay(milliseconds);

    internal Task YieldAsync() => yield();

    internal void Log(MenuTickLog level, string message) => write(level, message);

    internal void Unregister(MenuTickHandle handle)
    {
        if (_registered.Remove(handle))
        {
            NotifyChanged();
        }
    }

    /// <summary>A throwing subscriber must not abort the state change that raised the event.</summary>
    internal void NotifyChanged()
    {
        try
        {
            Changed?.Invoke();
        }
        catch (Exception exception)
        {
            Log(MenuTickLog.Error, $"a change subscriber threw: {exception}");
        }
    }
}
