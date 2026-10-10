namespace MenuAPI;

public class MenuSliderItem(string name, string? description, int min, int max, int startPosition, bool showDivider) : MenuItem(name, description)
{
    public int Min { get; private set; } = min;
    public int Max { get; private set; } = max;

    public bool ShowDivider
    {
        get => _showDivider;
        set => MenuNui.Change(ref _showDivider, value);
    }

    public int Position
    {
        get => _position;
        set => MenuNui.Change(ref _position, value);
    }

    public Icon SliderLeftIcon
    {
        get => _sliderLeftIcon;
        set => MenuNui.Change(ref _sliderLeftIcon, value);
    }

    public Icon SliderRightIcon
    {
        get => _sliderRightIcon;
        set => MenuNui.Change(ref _sliderRightIcon, value);
    }

    public System.Drawing.Color BackgroundColor
    {
        get => _backgroundColor;
        set => MenuNui.Change(ref _backgroundColor, value);
    }

    public System.Drawing.Color BarColor
    {
        get => _barColor;
        set => MenuNui.Change(ref _barColor, value);
    }

    private bool _showDivider = showDivider;
    private int _position = startPosition;
    private Icon _sliderLeftIcon = Icon.NONE;
    private Icon _sliderRightIcon = Icon.NONE;
    private System.Drawing.Color _backgroundColor = System.Drawing.Color.FromArgb(255, 24, 93, 151);
    private System.Drawing.Color _barColor = System.Drawing.Color.FromArgb(255, 53, 165, 223);

    public MenuSliderItem(string name, int min, int max, int startPosition) : this(name, min, max, startPosition, false) { }
    public MenuSliderItem(string name, int min, int max, int startPosition, bool showDivider) : this(name, null, min, max, startPosition, showDivider) { }
    public MenuSliderItem(string name, string? description, int min, int max, int startPosition) : this(name, description, min, max, startPosition, false) { }

    internal override void PrepareForDisplay()
    {
        RightIcon = SliderRightIcon;
        Label = null;

        if (Position > Max || Position < Min)
        {
            Position = (Max - Min) / 2;
        }
    }

    internal override void GoRight()
    {
        if (Position < Max)
        {
            Position++;
            if (ParentMenu is Menu parent)
            {
                parent.SliderItemChangedEvent(parent, this, Position - 1, Position, Index);
            }
            Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
        else
        {
            Native.PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
    }

    internal override void GoLeft()
    {
        if (Position > Min)
        {
            Position--;
            if (ParentMenu is Menu parent)
            {
                parent.SliderItemChangedEvent(parent, this, Position + 1, Position, Index);
            }
            Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
        else
        {
            Native.PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
    }

    internal override void Select()
    {
        if (ParentMenu is Menu parent)
        {
            parent.SliderSelectedEvent(parent, this, Position, Index);
        }
    }
}