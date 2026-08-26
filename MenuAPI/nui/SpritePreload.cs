using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared;
using CitizenFX.FiveM.Shared.FuncRef;

namespace MenuAPI;

/// <summary>
/// Copies every menu sprite into the NUI page's own memory once, at resource start, so drawing a
/// menu never has to ask the game for a texture again.
/// </summary>
// The page reaches game textures over https://nui-img/{dict}/{name}, which only answers while the
// dictionary happens to be streamed in. A menu that opens before streaming has caught up gets a 404
// per icon, and a background image that 404'd is never retried by the browser, so those icons stay
// missing for as long as the menu is open. Instead every dictionary is held loaded at start, the
// page is asked to copy the bytes of every sprite into a Blob it keeps forever, and only then are
// the dictionaries handed back. From that point on the icons are local data and cannot 404.
internal static class SpritePreload
{
    private const string CallbackName = "menuapiSprites";

    private const int MaxRounds = 3;

    private const int RetryDelayMs = 500;

    private const int DictionaryWaitFrames = 600;

    private static readonly HashSet<string> _cached = new();

    private static List<SpriteManifest.Sprite> _round = new();

    private static bool _registered;
    private static bool _running;
    private static int _rounds;

    internal static bool Cached => _cached.Count != 0;

    internal static bool IsCached(string dict, string name) => _cached.Contains(SpriteManifest.Key(dict, name));

    internal static void Initialize()
    {
        if (_registered)
        {
            return;
        }

        _registered = true;

        Native.RegisterNuiCallbackType(CallbackName);

        SharedAPI.OnEvent($"__cfx_nui:{CallbackName}", new Action<object, FunctionReference>(OnReport));
    }

    private static void OnReport(object body, FunctionReference callback)
    {
        try
        {
            callback?.CallVoid(new object[] { "ok" });
        }
        catch
        {
            // Replying is a courtesy, losing it must not stop the preload.
        }

        switch (Text(body, "stage"))
        {
            case "ready":
                Begin();
                break;

            case "done":
                Complete(body);
                break;
        }
    }

    private static async void Begin()
    {
        if (_running)
        {
            return;
        }

        _running = true;
        _rounds = 0;

        _cached.Clear();

        await HoldDictionaries();

        Send(SpriteManifest.Sprites);
    }

    private static void Complete(object body)
    {
        if (!_running)
        {
            return;
        }

        var missing = Missing(body);

        foreach (var sprite in _round)
        {
            if (!missing.Contains(SpriteManifest.Key(sprite)))
            {
                _cached.Add(SpriteManifest.Key(sprite));
            }
        }

        var copied = Text(body, "mode") != "live";

        if (!copied)
        {
            _cached.Clear();
        }

        if (copied && missing.Count > 0 && _rounds < MaxRounds)
        {
            Retry(missing);

            return;
        }

        Finish(copied, missing);
    }

    private static async void Retry(HashSet<string> missing)
    {
        var again = new List<SpriteManifest.Sprite>();

        foreach (var sprite in _round)
        {
            if (missing.Contains(SpriteManifest.Key(sprite)))
            {
                again.Add(sprite);
            }
        }

        TextureDictionaries.ReleaseAll(SpriteManifest.Dictionaries);

        await HoldDictionaries();
        await API.Delay(RetryDelayMs);

        Send(again);
    }

    private static void Finish(bool copied, HashSet<string> missing)
    {
        _running = false;

        if (copied)
        {
            if (!MenuController.IsAnyMenuOpen())
            {
                TextureDictionaries.ReleaseAll(SpriteManifest.Dictionaries);
            }

            API.Log.Info($"[MenuAPI] NUI holds {_cached.Count} of {SpriteManifest.Sprites.Count} menu sprites, the game no longer has to.");
        }
        else
        {
            API.Log.Warn("[MenuAPI] NUI could not copy the menu sprites, they stay on the game's texture dictionaries.");
        }

        if (missing.Count > 0)
        {
            API.Log.Warn($"[MenuAPI] {missing.Count} menu sprites never arrived: {string.Join(", ", missing)}");
        }

        MenuNui.Invalidate();
    }

    private static async Task HoldDictionaries()
    {
        var frames = 0;

        while (!TextureDictionaries.RequestAll(SpriteManifest.Dictionaries) && frames < DictionaryWaitFrames)
        {
            frames++;

            await API.Delay(0);
        }
    }

    private static void Send(List<SpriteManifest.Sprite> sprites)
    {
        _round = sprites;
        _rounds++;

        var json = new NuiJson()
            .Object()
            .Prop("type", "menuapi:preload")
            .Prop("round", _rounds)
            .Array("sprites");

        foreach (var sprite in sprites)
        {
            json.Object()
                .Prop("dict", sprite.Dict)
                .Prop("name", sprite.Name)
                .EndObject();
        }

        Native.SendNuiMessage(json.EndArray().EndObject().ToString());
    }

    private static HashSet<string> Missing(object body)
    {
        var missing = new HashSet<string>();
        var text = Text(body, "missing");

        if (text.Length == 0)
        {
            return missing;
        }

        foreach (var key in text.Split(','))
        {
            if (key.Length > 0)
            {
                missing.Add(key);
            }
        }

        return missing;
    }

    private static string Text(object? body, string key)
    {
        switch (body)
        {
            case IDictionary<string, object> typed:
                return typed.TryGetValue(key, out var value) ? value?.ToString() ?? "" : "";

            case System.Collections.IDictionary loose:
                foreach (System.Collections.DictionaryEntry entry in loose)
                {
                    if (entry.Key?.ToString() == key)
                    {
                        return entry.Value?.ToString() ?? "";
                    }
                }

                return "";

            case string json:
                return FromJson(json, key);

            default:
                return "";
        }
    }

    private static string FromJson(string json, string key)
    {
        var at = json.IndexOf("\"" + key + "\"", StringComparison.Ordinal);

        if (at < 0)
        {
            return "";
        }

        at = json.IndexOf(':', at);

        if (at < 0)
        {
            return "";
        }

        at++;

        while (at < json.Length && json[at] == ' ')
        {
            at++;
        }

        if (at < json.Length && json[at] == '"')
        {
            var end = json.IndexOf('"', at + 1);

            return end < 0 ? "" : json[(at + 1)..end];
        }

        var stop = at;

        while (stop < json.Length && json[stop] != ',' && json[stop] != '}')
        {
            stop++;
        }

        return json[at..stop].Trim();
    }
}
