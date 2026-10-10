using CitizenFX.Core;

using Cfx = CitizenFX.Core.Native.API;

namespace MenuAPI;

/// <summary>What MenuAPI needs from the scripting runtime, other than natives. FiveM Legacy version.</summary>
internal static class Runtime
{
    internal static Task Delay(long ms) => BaseScript.Delay((int)ms);

    internal static Task Yield() => BaseScript.Delay(0);

    internal static int Hash(string text) => Cfx.GetHashKey(text);

    internal static void RegisterCommand(string name, Action handler) =>
        Cfx.RegisterCommand(name, new Action<int, List<object>, string>((_, _, _) => handler()), false);

    internal static void RegisterNuiCallback(string name, Action<object> handler)
    {
        Cfx.RegisterNuiCallbackType(name);

        MenuController.Instance!.AddEventHandler($"__cfx_nui:{name}", new Action<IDictionary<string, object>, CallbackDelegate>((body, callback) =>
        {
            try
            {
                callback?.Invoke("ok");
            }
            catch
            {
                // Replying is a courtesy, losing it must not stop the handler.
            }

            handler(body);
        }));
    }

    internal static class Log
    {
        internal static void Debug(string message) => CitizenFX.Core.Debug.WriteLine($"[debug] {message}");

        internal static void Info(string message) => CitizenFX.Core.Debug.WriteLine(message);

        internal static void Warn(string message) => CitizenFX.Core.Debug.WriteLine($"^3{message}^7");

        internal static void Error(string message) => CitizenFX.Core.Debug.WriteLine($"^1{message}^7");
    }
}
