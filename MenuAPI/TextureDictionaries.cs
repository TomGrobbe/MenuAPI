namespace MenuAPI;

/// <summary>
/// Which texture dictionaries the menu currently holds, so the game only has to be asked about one
/// while it is still streaming in.
/// </summary>
// HasStreamedTextureDictLoaded is not a flag read, it hashes the name and goes looking through the
// streaming module for it, and the drawing code asked it about ten dictionaries twice per frame. A
// dictionary that has been asked for and not handed back stays in memory, that is what asking for it
// means, so once one is seen loaded it is remembered and the native is not called for it again. The
// remembering is dropped in Release, the only place that ever gives one back.
internal static class TextureDictionaries
{
    private static readonly HashSet<string> Loaded = new HashSet<string>();

    /// <summary>Whether <paramref name="dict"/> is loaded, asking the game only until it says yes once.</summary>
    internal static bool IsLoaded(string dict)
    {
        if (Loaded.Contains(dict))
        {
            return true;
        }

        if (!Native.HasStreamedTextureDictLoaded(dict))
        {
            return false;
        }

        Loaded.Add(dict);

        MenuNui.Invalidate();

        return true;
    }

    /// <summary>Asks for <paramref name="dict"/> if it is not loaded yet. True once it is.</summary>
    internal static bool Request(string dict)
    {
        if (IsLoaded(dict))
        {
            return true;
        }

        Native.RequestStreamedTextureDict(dict, false);
        return false;
    }

    /// <summary>Asks for every dictionary that is missing. True once they are all loaded.</summary>
    internal static bool RequestAll(List<string> dicts)
    {
        bool allLoaded = true;

        for (int i = 0; i < dicts.Count; i++)
        {
            allLoaded &= Request(dicts[i]);
        }

        return allLoaded;
    }

    /// <summary>Hands <paramref name="dict"/> back to the game and stops treating it as loaded.</summary>
    internal static void Release(string dict)
    {
        if (Loaded.Remove(dict))
        {
            Native.SetStreamedTextureDictAsNoLongerNeeded(dict);
        }
    }

    /// <summary>Hands back every dictionary in <paramref name="dicts"/> that is still held.</summary>
    internal static void ReleaseAll(List<string> dicts)
    {
        for (int i = 0; i < dicts.Count; i++)
        {
            Release(dicts[i]);
        }
    }
}
