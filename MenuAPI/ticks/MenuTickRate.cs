namespace MenuAPI;

/// <summary>How long one of MenuAPI's internal ticks waits between iterations.</summary>
// The wait happens after the handler returns rather than on a timer, so a slow iteration delays the
// next instead of overlapping with it. That is the whole difference from API.SetInterval.
// Not a readonly struct: the Legacy IL verifier rejects the ldflda that member calls on readonly fields of one produce.
public struct MenuTickRate
{
    private readonly long _milliseconds;

    private MenuTickRate(long milliseconds) => _milliseconds = milliseconds;

    /// <summary>Once per frame.</summary>
    public static MenuTickRate PerFrame => default;

    public static MenuTickRate Every(long milliseconds) => new(milliseconds);

    public long Milliseconds => _milliseconds;

    public override string ToString() => _milliseconds <= 0 ? "per frame" : $"every {_milliseconds}ms";
}
