namespace MenuAPI;

internal sealed partial class NuiJson
{
    internal bool Matches(string? value) => value is not null && _builder.Equals(value.AsSpan());
}
