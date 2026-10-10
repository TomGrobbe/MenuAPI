namespace MenuAPI;

/// <summary>
/// A row that only labels the rows under it. Drawn centred, and does nothing when pressed.
/// </summary>
/// <remarks>
/// The cursor scrolls onto a separator like any other row, it just cannot act on it.
/// <see cref="MenuItem.LeftIcon"/>, <see cref="MenuItem.RightIcon"/> and <see cref="MenuItem.Label"/>
/// are never drawn.
/// </remarks>
public class SeparatorMenuItem : MenuItem
{
    /// <summary>Draws the text as <c>↓ Text ↓</c>. On by default.</summary>
    public bool ShowArrows
    {
        get => _showArrows;
        set => MenuNui.Change(ref _showArrows, value);
    }

    private bool _showArrows = true;

    /// <summary>
    /// Creates a <see cref="SeparatorMenuItem"/> with the down arrows around its text.
    /// </summary>
    /// <param name="text"></param>
    public SeparatorMenuItem(string text) : this(text, null, true) { }

    /// <summary>
    /// Creates a <see cref="SeparatorMenuItem"/> and says whether to draw the down arrows.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="showArrows"></param>
    public SeparatorMenuItem(string text, bool showArrows) : this(text, null, showArrows) { }

    /// <summary>
    /// Creates a <see cref="SeparatorMenuItem"/> with a description, shown while the cursor is on it.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="description"></param>
    public SeparatorMenuItem(string text, string? description) : this(text, description, true) { }

    /// <summary>
    /// Creates a <see cref="SeparatorMenuItem"/> with all options set.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="description"></param>
    /// <param name="showArrows"></param>
    public SeparatorMenuItem(string text, string? description, bool showArrows) : base(text, description)
    {
        ShowArrows = showArrows;

        // What MenuController's left and right handling checks. Select is turned away by type
        // instead, in Menu.SelectItem, so that it can be silent rather than an error.
        Enabled = false;
    }

    internal override void Select() { }

    internal override void GoLeft() { }

    internal override void GoRight() { }
}
