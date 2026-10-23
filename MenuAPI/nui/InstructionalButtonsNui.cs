namespace MenuAPI;

internal static class InstructionalButtonsNui
{
    private const string MessageType = "menuapi:buttons";

    private const string HideMessage = "{\"type\":\"menuapi:buttons\",\"visible\":false}";

    private static readonly NuiJson _json = new();
    private static string? _sent;

    internal static void Begin()
    {
        _json.Reset()
            .Object()
            .Prop("type", MessageType)
            .Prop("visible", true)
            .Prop("inset", (1f - MenuLayout.SafeZone) / 2f)
            .Prop("theme", NuiTuning.ThemeUrl)
            .Array("buttons");
    }

    internal static void Add(string? buttons, string? text)
    {
        _json.Object()
            .Prop("buttons", buttons ?? "")
            .Prop("text", text ?? "")
            .EndObject();
    }

    // Built every few hundred milliseconds but only sent when something changed, the page keeps what
    // it was given until told otherwise.
    internal static void Send()
    {
        _json.EndArray().EndObject();

        if (_json.Matches(_sent))
        {
            return;
        }

        _sent = _json.ToString();

        Native.SendNuiMessage(_sent);
    }

    internal static void Hide()
    {
        if (_sent is null || _sent == HideMessage)
        {
            return;
        }

        _sent = HideMessage;

        Native.SendNuiMessage(HideMessage);
    }
}
