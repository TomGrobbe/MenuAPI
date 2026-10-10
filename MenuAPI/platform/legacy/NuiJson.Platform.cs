namespace MenuAPI;

internal sealed partial class NuiJson
{
    // No span overload of StringBuilder.Equals on .NET Framework. Compared by hand so a frame that
    // sends nothing new still allocates nothing.
    internal bool Matches(string? value)
    {
        if (value is null || value.Length != _builder.Length)
        {
            return false;
        }

        for (var i = 0; i < value.Length; i++)
        {
            if (_builder[i] != value[i])
            {
                return false;
            }
        }

        return true;
    }
}
