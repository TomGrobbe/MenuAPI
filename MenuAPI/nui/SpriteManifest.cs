namespace MenuAPI;

/// <summary>
/// Every game sprite the menu is able to draw, as dictionary and texture name pairs.
/// </summary>
// Built by asking the item classes themselves rather than by keeping a second hand written list, so
// a new icon added to the enum is preloaded without anyone having to remember this file exists.
internal static class SpriteManifest
{
    internal readonly record struct Sprite(string Dict, string Name);

    private static List<Sprite>? _sprites;
    private static List<string>? _dictionaries;

    /// <summary>Every dictionary and texture name pair, with duplicates removed.</summary>
    internal static List<Sprite> Sprites => _sprites ??= Build();

    /// <summary>Every dictionary the sprites live in, with duplicates removed.</summary>
    internal static List<string> Dictionaries => _dictionaries ??= CollectDictionaries();

    internal static string Key(string dict, string name) => dict + "/" + name;

    internal static string Key(Sprite sprite) => Key(sprite.Dict, sprite.Name);

    private static List<Sprite> Build()
    {
        var seen = new HashSet<string>();
        var sprites = new List<Sprite>();

        void Add(string dict, string name)
        {
            if (string.IsNullOrEmpty(dict) || string.IsNullOrEmpty(name))
            {
                return;
            }

            if (seen.Add(Key(dict, name)))
            {
                sprites.Add(new Sprite(dict, name));
            }
        }

        Add(MenuController._texture_dict, MenuController._header_texture);

        // The sprite lookups are instance methods, so throwaway items are the only way to reach them.
        var item = new MenuItem("");

        foreach (var icon in Enum.GetValues<MenuItem.Icon>())
        {
            if (icon == MenuItem.Icon.NONE)
            {
                continue;
            }

            var dict = item.GetSpriteDictionary(icon);

            Add(dict, item.GetSpriteName(icon, false));
            Add(dict, item.GetSpriteName(icon, true));
        }

        var checkbox = new MenuCheckboxItem("");

        foreach (var style in Enum.GetValues<MenuCheckboxItem.CheckboxStyle>())
        {
            checkbox.Style = style;

            for (var ticked = 0; ticked < 2; ticked++)
            {
                checkbox.Checked = ticked == 1;

                Add(MenuController._texture_dict, checkbox.GetSpriteName(false));
                Add(MenuController._texture_dict, checkbox.GetSpriteName(true));
            }
        }

        return sprites;
    }

    private static List<string> CollectDictionaries()
    {
        var seen = new HashSet<string>();
        var dictionaries = new List<string>();

        foreach (var sprite in Sprites)
        {
            if (seen.Add(sprite.Dict))
            {
                dictionaries.Add(sprite.Dict);
            }
        }

        return dictionaries;
    }
}
