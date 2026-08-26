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
    private const int TextOnBackground = 255;

    private const int TextOnHighlight = 0;

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

    private string DisplayText => ShowArrows ? $"↓ {Text ?? ""} ↓" : (Text ?? "");

    // base.Draw would add the icons and the label, and inset the text for an icon that is not there.
    // The row geometry is reproduced instead so the text lands on the same baseline as every other row.
    internal override void Draw(int indexOffset)
    {
        if (ParentMenu is not Menu parent)
        {
            return;
        }

        int index = Index;
        bool selected = parent.CurrentIndex == index;

        int font = 0;
        float textSize = MenuLayout.ItemTextSize;
        int textColor = selected ? TextOnHighlight : TextOnBackground;

        float y = (((index - indexOffset) * RowHeight) + 20f + RowYOffset(parent)) / MenuLayout.ScreenHeight;
        float textY = y - ((30f / 2f) / MenuLayout.ScreenHeight);

        if (selected)
        {
            Native.SetScriptGfxAlign(parent.LeftAligned ? 76 : 82, 84);
            Native.SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
            Native.DrawRect(MenuLayout.RowCenterX, y, MenuLayout.MenuWidthN, MenuLayout.RowHeightN, 255, 255, 255, 225, false);
            Native.ResetScriptGfxAlign();
        }

        Native.SetScriptGfxAlign(76, 84);
        Native.SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

        Native.BeginTextCommandDisplayText("STRING");
        Native.AddTextComponentSubstringPlayerName(DisplayText);

        Native.SetTextFont(font);
        Native.SetTextScale(textSize, textSize);
        Native.SetTextJustification(0);
        Native.SetTextColour(textColor, textColor, textColor, 255);

        // Centred text is placed by the wrap box, so the box is the row and x is that box's centre.
        if (parent.LeftAligned)
        {
            Native.SetTextWrap(0f, Width / MenuLayout.ScreenWidth);
            Native.EndTextCommandDisplayText(MenuLayout.RowCenterX, textY, 0);
        }
        else
        {
            Native.SetTextWrap(MenuLayout.RightTextMinX, MenuLayout.RightTextMaxX);
            Native.EndTextCommandDisplayText(MenuLayout.RightHeaderCenterX, textY, 0);
        }

        Native.ResetScriptGfxAlign();
    }

    internal override void Select() { }

    internal override void GoLeft() { }

    internal override void GoRight() { }
}
