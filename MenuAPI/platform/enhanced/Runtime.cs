using CitizenFX.FiveM.Shared;
using CitizenFX.FiveM.Shared.FuncRef;

namespace MenuAPI;

/// <summary>What MenuAPI needs from the scripting runtime, other than natives. FiveM Enhanced version.</summary>
internal static class Runtime
{
    internal static Task Delay(long ms) => API.Delay(ms);

    internal static Task Yield() => API.Yield();

    internal static int Hash(string text) => API.HashSigned(text);

    internal static void RegisterCommand(string name, Action handler) =>
        SharedAPI.Commands.RegisterCommand(name, false, handler);

    internal static void RegisterNuiCallback(string name, Action<object> handler)
    {
        Native.RegisterNuiCallbackType(name);

        SharedAPI.OnEvent($"__cfx_nui:{name}", new Action<object, FunctionReference>((body, callback) =>
        {
            try
            {
                callback?.CallVoid(new object[] { "ok" });
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
        internal static void Debug(string message) => API.Log.Debug(message);

        internal static void Info(string message) => API.Log.Info(message);

        internal static void Warn(string message) => API.Log.Warn(message);

        internal static void Error(string message) => API.Log.Error(message);
    }
}
