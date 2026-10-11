namespace MenuAPI;

/// <summary>
/// Tunes how menu text looks on the NUI page, and switches between registered themes. Every change
/// shows on the next frame.
/// </summary>
public static class NuiTuning
{
    private static float _textSize = DefaultTextSize;
    private static int _textBrightness = DefaultTextBrightness;
    private static TextWeightMode _textWeight = TextWeightMode.Default;

    private static readonly Dictionary<string, string> _themes = new(StringComparer.OrdinalIgnoreCase);
    private static string? _theme;

    /// <summary>The <see cref="TextSize"/> menus start with, and go back to on <see cref="Reset"/>.</summary>
    public const float DefaultTextSize = 21f;
    /// <summary>The <see cref="TextBrightness"/> menus start with, and go back to on <see cref="Reset"/>.</summary>
    public const int DefaultTextBrightness = 225;

    /// <summary>How the page renders menu text, which changes how sharp and heavy it looks.</summary>
    public enum TextWeightMode
    {
        /// <summary>The browser's normal text rendering.</summary>
        Default,
        /// <summary>The browser's <c>geometricPrecision</c> text rendering, which keeps letter shapes exact.</summary>
        GeometricPrecision,
        /// <summary>Draws the menu at double size and scales it down, for smoother text. Costs more to draw.</summary>
        Supersampled,
    }

    /// <summary>Menu text size in pixels, clamped between 8 and 48.</summary>
    public static float TextSize
    {
        get => _textSize;
        set
        {
            _textSize = MenuMath.Clamp(value, 8f, 48f);

            MenuNui.Invalidate();
        }
    }

    /// <summary>How bright menu text is, from 0 for black to 255 for white.</summary>
    public static int TextBrightness
    {
        get => _textBrightness;
        set
        {
            _textBrightness = MenuMath.Clamp(value, 0, 255);

            MenuNui.Invalidate();
        }
    }

    /// <summary>How menu text is rendered. See <see cref="TextWeightMode"/>.</summary>
    public static TextWeightMode TextWeight
    {
        get => _textWeight;
        set
        {
            _textWeight = value;

            MenuNui.Invalidate();
        }
    }

    /// <summary>The applied theme, or null for the default look.</summary>
    public static string? Theme => _theme;

    /// <summary>Every registered theme name.</summary>
    public static IReadOnlyCollection<string> Themes => _themes.Keys.ToList();

    /// <summary>
    /// Registers a stylesheet under a name. The path is relative to your NUI page. Registering the
    /// same name again repoints it.
    /// </summary>
    public static void RegisterTheme(string name, string path)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A theme name cannot be empty.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException($"The path for theme '{name}' cannot be empty.", nameof(path));
        }

        if (path.TrimStart().StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"The path for theme '{name}' must point at a stylesheet.", nameof(path));
        }

        _themes[name] = path;

        if (string.Equals(_theme, name, StringComparison.OrdinalIgnoreCase))
        {
            MenuNui.Invalidate();
        }
    }

    /// <summary>Removes a registered theme. Clears the applied theme if it was the one removed.</summary>
    public static bool UnregisterTheme(string name)
    {
        if (string.IsNullOrEmpty(name) || !_themes.Remove(name))
        {
            return false;
        }

        if (string.Equals(_theme, name, StringComparison.OrdinalIgnoreCase))
        {
            _theme = null;
        }

        MenuNui.Invalidate();

        return true;
    }

    /// <summary>Applies a registered theme, or null for the default look. Throws if unregistered.</summary>
    public static void SetTheme(string? name)
    {
        if (name is null)
        {
            if (_theme is null)
            {
                return;
            }

            _theme = null;

            MenuNui.Invalidate();

            return;
        }

        if (!_themes.TryGetValue(name, out _))
        {
            throw new ArgumentException($"No theme named '{name}' is registered. Call RegisterTheme first.", nameof(name));
        }

        if (string.Equals(_theme, name, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        _theme = name;

        MenuNui.Invalidate();
    }

    internal static string? ThemeUrl =>
        _theme is not null && _themes.TryGetValue(_theme, out var path) ? path : null;

    /// <summary>
    /// Puts text size, brightness and weight back to their defaults and clears the applied theme.
    /// Registered themes are kept.
    /// </summary>
    public static void Reset()
    {
        _textSize = DefaultTextSize;
        _textBrightness = DefaultTextBrightness;
        _textWeight = TextWeightMode.Default;
        _theme = null;

        MenuNui.Invalidate();
    }

    /// <summary>The current text size, brightness and weight as one readable line, for debug output.</summary>
    public static string Describe() =>
        $"size {_textSize:0.##}px, brightness {_textBrightness}, weight {_textWeight}";

    internal static string WeightName => _textWeight switch
    {
        TextWeightMode.GeometricPrecision => "geometric",
        TextWeightMode.Supersampled => "supersampled",
        _ => "default",
    };
}
