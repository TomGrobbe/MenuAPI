using CitizenFX.FiveM.Client;

namespace MenuAPI;

/// <summary>
/// One of MenuAPI's internal ticks. Read only from the outside: only MenuAPI itself can register,
/// start or stop one, but anything can see what is running through <see cref="MenuTicks.Handles"/>.
/// </summary>
// Stopping ends the loop rather than idling it, so a tick that is gated off costs nothing, not even
// a per frame branch. That is why gating belongs here instead of inside handlers.
public sealed class MenuTickHandle
{
    // A per frame tick would write sixty error lines a second while broken. Five is enough to prove
    // it was not a one off.
    private const int MaxFailures = 5;

    private readonly MenuTickEngine _engine;
    private readonly Func<Task> _handler;
    private readonly MenuTickRate _rate;
    private readonly Func<bool>? _condition;

    /// <summary>The loop's exit condition: the state <see cref="Apply"/> committed to, not the state it wants.</summary>
    private bool _running;

    /// <summary>Whether a <see cref="Drive"/> call is live, including while suspended at an await.</summary>
    private bool _driverInFlight;

    /// <summary>Only consulted when there is no condition.</summary>
    private bool _manuallyStarted;

    private int _failures;
    private bool _disposed;

    internal MenuTickHandle(
        MenuTickEngine engine,
        string name,
        Func<Task> handler,
        MenuTickRate rate,
        Func<bool>? condition,
        bool autoStart)
    {
        _engine = engine;
        Name = name;
        _handler = handler;
        _rate = rate;
        _condition = condition;
        _manuallyStarted = autoStart;
    }

    public string Name { get; }

    public MenuTickRate Rate => _rate;

    public bool IsRunning => _running;

    /// <summary>Runs when the tick starts, for setup that must not happen per iteration.</summary>
    internal Action? OnStarted { get; init; }

    /// <summary>Runs when the tick stops, for the teardown that pairs with <see cref="OnStarted"/>.</summary>
    internal Action? OnStopped { get; init; }

    internal void Start()
    {
        _manuallyStarted = true;

        Apply();
    }

    internal void Stop()
    {
        _manuallyStarted = false;

        Apply();
    }

    /// <summary>Re-runs the condition.</summary>
    // This re-arms a tick stopped by MaxFailures, so a permanently broken handler costs another five
    // log lines every time it is called.
    internal void Reevaluate() => Apply();

    // Not IDisposable on purpose: a public Dispose, even an explicit interface one, would let any
    // resource kill MenuAPI's draw loop through a cast.
    internal void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        Stop();

        _engine.Unregister(this);
    }

    internal void Apply()
    {
        var shouldRun = !_disposed && EvaluateCondition();
        var alreadyInDesiredState = shouldRun == _running;

        if (alreadyInDesiredState)
        {
            return;
        }

        _running = shouldRun;

        _engine.NotifyChanged();

        if (!shouldRun)
        {
            Notify(OnStopped);

            return;
        }

        _failures = 0;

        Notify(OnStarted);

        // A restart inside one frame leaves the previous driver suspended mid await, and a second
        // here is the overlap this type exists to prevent.
        if (!_driverInFlight)
        {
            Drive();
        }
    }

    // Fails closed: a throwing condition must not leave a tick stuck on, and a registry wide
    // re-evaluation must not abort partway through.
    private bool EvaluateCondition()
    {
        try
        {
            return _condition is null ? _manuallyStarted : _condition();
        }
        catch (Exception exception)
        {
            _engine.Log(MenuTickLog.Error, $"{Name} condition threw and is being treated as off: {exception}");

            return false;
        }
    }

    private async void Drive()
    {
        _driverInFlight = true;

        try
        {
            // So a tick body always runs from the tick pump. What starts a tick is usually a
            // callback, and a draw loop firing its first frame inside a menu event handler surprises.
            await _engine.YieldAsync();

            while (_running)
            {
                try
                {
                    Native.ProfilerEnterScope($"MenuAPI.Enhanced.{Name}");

                    try
                    {
                        await _handler();
                    }
                    finally
                    {
                        Native.ProfilerExitScope();
                    }

                    _failures = 0;
                }
                catch (Exception exception)
                {
                    _engine.Log(MenuTickLog.Error, $"{Name} threw: {exception}");

                    if (++_failures >= MaxFailures)
                    {
                        _engine.Log(MenuTickLog.Error, $"{Name} stopped after {MaxFailures} consecutive failures.");

                        _running = false;

                        _engine.NotifyChanged();

                        Notify(OnStopped);

                        break;
                    }
                }

                await _engine.DelayAsync(_rate.Milliseconds);
            }
        }
        finally
        {
            _driverInFlight = false;
        }
    }

    // The lifecycle callbacks are the teardown path, so one throwing must not leave a scaleform
    // loaded or a texture dict streamed in forever.
    private void Notify(Action? callback)
    {
        try
        {
            callback?.Invoke();
        }
        catch (Exception exception)
        {
            _engine.Log(MenuTickLog.Error, $"{Name} lifecycle callback threw: {exception}");
        }
    }
}
