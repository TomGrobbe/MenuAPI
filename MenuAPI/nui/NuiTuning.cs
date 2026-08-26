namespace MenuAPI;

public static class NuiTuning
{
    private static float _textSize = DefaultTextSize;
    private static int _textBrightness = DefaultTextBrightness;
    private static TextWeightMode _textWeight = TextWeightMode.Default;

    public const float DefaultTextSize = 21f;
    public const int DefaultTextBrightness = 225;

    public enum TextWeightMode
    {
        Default,
        GeometricPrecision,
        Supersampled,
    }

    public static float TextSize
    {
        get => _textSize;
        set
        {
            _textSize = Math.Clamp(value, 8f, 48f);

            MenuNui.Invalidate();
        }
    }

    public static int TextBrightness
    {
        get => _textBrightness;
        set
        {
            _textBrightness = Math.Clamp(value, 0, 255);

            MenuNui.Invalidate();
        }
    }

    public static TextWeightMode TextWeight
    {
        get => _textWeight;
        set
        {
            _textWeight = value;

            MenuNui.Invalidate();
        }
    }

    public static void Reset()
    {
        _textSize = DefaultTextSize;
        _textBrightness = DefaultTextBrightness;
        _textWeight = TextWeightMode.Default;

        MenuNui.Invalidate();
    }

    public static string Describe() =>
        $"size {_textSize:0.##}px, brightness {_textBrightness}, weight {_textWeight}";

    internal static string WeightName => _textWeight switch
    {
        TextWeightMode.GeometricPrecision => "geometric",
        TextWeightMode.Supersampled => "supersampled",
        _ => "default",
    };
}
