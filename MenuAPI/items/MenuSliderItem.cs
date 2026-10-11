namespace MenuAPI;

/// <summary>
/// An item with a slider on the right. Left and right move it one step, it does not wrap around.
/// Moving it raises <see cref="Menu.OnSliderPositionChange"/>, pressing it raises <see cref="Menu.OnSliderItemSelect"/>.
/// </summary>
/// <param name="name">The text on the left side of the item.</param>
/// <param name="description">Shown below the menu while the item is highlighted. Null for none.</param>
/// <param name="min">The lowest position.</param>
/// <param name="max">The highest position.</param>
/// <param name="startPosition">The starting position, between <paramref name="min"/> and <paramref name="max"/>.</param>
/// <param name="showDivider">Draws a divider line in the middle of the slider.</param>
public class MenuSliderItem(string name, string? description, int min, int max, int startPosition, bool showDivider) : MenuItem(name, description)
{
    /// <summary>
    /// The lowest position. Can only be set through a constructor.
    /// </summary>
    public int Min { get; private set; } = min;
    /// <summary>
    /// The highest position. Can only be set through a constructor.
    /// </summary>
    public int Max { get; private set; } = max;

    /// <summary>
    /// Draws a divider line in the middle of the slider.
    /// </summary>
    public bool ShowDivider
    {
        get => _showDivider;
        set => MenuNui.Change(ref _showDivider, value);
    }

    /// <summary>
    /// The current position. A position outside <see cref="Min"/> to <see cref="Max"/> is reset to <c>(Max - Min) / 2</c> when the item is drawn.
    /// </summary>
    public int Position
    {
        get => _position;
        set => MenuNui.Change(ref _position, value);
    }

    /// <summary>
    /// The icon on the left side of the slider. Defaults to <see cref="MenuItem.Icon.NONE"/>.
    /// </summary>
    public Icon SliderLeftIcon
    {
        get => _sliderLeftIcon;
        set => MenuNui.Change(ref _sliderLeftIcon, value);
    }

    /// <summary>
    /// The icon on the right side of the slider. Defaults to <see cref="MenuItem.Icon.NONE"/>.
    /// </summary>
    public Icon SliderRightIcon
    {
        get => _sliderRightIcon;
        set => MenuNui.Change(ref _sliderRightIcon, value);
    }

    /// <summary>
    /// The color of the slider background. Defaults to dark blue (24, 93, 151).
    /// </summary>
    public System.Drawing.Color BackgroundColor
    {
        get => _backgroundColor;
        set => MenuNui.Change(ref _backgroundColor, value);
    }

    /// <summary>
    /// The color of the filled bar. Defaults to light blue (53, 165, 223).
    /// </summary>
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

    /// <summary>
    /// Creates a <see cref="MenuSliderItem"/> without a description or divider.
    /// </summary>
    /// <param name="name">The text on the left side of the item.</param>
    /// <param name="min">The lowest position.</param>
    /// <param name="max">The highest position.</param>
    /// <param name="startPosition">The starting position, between <paramref name="min"/> and <paramref name="max"/>.</param>
    public MenuSliderItem(string name, int min, int max, int startPosition) : this(name, min, max, startPosition, false) { }
    /// <summary>
    /// Creates a <see cref="MenuSliderItem"/> without a description.
    /// </summary>
    /// <param name="name">The text on the left side of the item.</param>
    /// <param name="min">The lowest position.</param>
    /// <param name="max">The highest position.</param>
    /// <param name="startPosition">The starting position, between <paramref name="min"/> and <paramref name="max"/>.</param>
    /// <param name="showDivider">Draws a divider line in the middle of the slider.</param>
    public MenuSliderItem(string name, int min, int max, int startPosition, bool showDivider) : this(name, null, min, max, startPosition, showDivider) { }
    /// <summary>
    /// Creates a <see cref="MenuSliderItem"/> without a divider.
    /// </summary>
    /// <param name="name">The text on the left side of the item.</param>
    /// <param name="description">Shown below the menu while the item is highlighted. Null for none.</param>
    /// <param name="min">The lowest position.</param>
    /// <param name="max">The highest position.</param>
    /// <param name="startPosition">The starting position, between <paramref name="min"/> and <paramref name="max"/>.</param>
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