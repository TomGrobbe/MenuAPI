namespace MenuAPI;

/// <summary>
/// An item with a checkbox on the right. Pressing select or right toggles it and raises <see cref="Menu.OnCheckboxChange"/>.
/// </summary>
/// <param name="text">The text on the left side of the item.</param>
/// <param name="description">Shown below the menu while the item is highlighted. Null for none.</param>
/// <param name="_checked">Whether the checkbox starts checked.</param>
public class MenuCheckboxItem(string text, string? description, bool _checked) : MenuItem(text, description)
{
    /// <summary>
    /// Whether the checkbox is checked. Already updated when <see cref="Menu.OnCheckboxChange"/> fires.
    /// </summary>
    public bool Checked
    {
        get => _isChecked;
        set => MenuNui.Change(ref _isChecked, value);
    }

    /// <summary>
    /// What the checkbox shows when checked. Defaults to <see cref="CheckboxStyle.Tick"/>.
    /// </summary>
    public CheckboxStyle Style
    {
        get => _style;
        set => MenuNui.Change(ref _style, value);
    }

    private bool _isChecked = _checked;
    private CheckboxStyle _style = CheckboxStyle.Tick;
    /// <summary>
    /// What a checked checkbox shows.
    /// </summary>
    public enum CheckboxStyle
    {
        /// <summary>A cross.</summary>
        Cross,
        /// <summary>A tick mark.</summary>
        Tick
    }

    /// <summary>
    /// Creates an unchecked <see cref="MenuCheckboxItem"/> without a description.
    /// </summary>
    /// <param name="text">The text on the left side of the item.</param>
    public MenuCheckboxItem(string text) : this(text, null) { }
    /// <summary>
    /// Creates a <see cref="MenuCheckboxItem"/> without a description and sets the checked state to <paramref name="_checked"/>.
    /// </summary>
    /// <param name="text">The text on the left side of the item.</param>
    /// <param name="_checked">Whether the checkbox starts checked.</param>
    public MenuCheckboxItem(string text, bool _checked) : this(text, null, _checked) { }
    /// <summary>
    /// Creates an unchecked <see cref="MenuCheckboxItem"/> with an item description.
    /// </summary>
    /// <param name="text">The text on the left side of the item.</param>
    /// <param name="description">Shown below the menu while the item is highlighted. Null for none.</param>
    public MenuCheckboxItem(string text, string? description) : this(text, description, false) { }

    internal const float SpriteSizePx = 45f;

    internal string GetSpriteName(bool selected)
    {
        if (Checked)
        {
            if (Style == CheckboxStyle.Tick)
            {
                if (selected)
                {
                    return "shop_box_tickb";
                }
                return "shop_box_tick";
            }
            else
            {
                if (selected)
                {
                    return "shop_box_crossb";
                }
                return "shop_box_cross";
            }
        }
        else
        {
            if (selected)
            {
                return "shop_box_blankb";
            }
            return "shop_box_blank";
        }
    }

    internal override void PrepareForDisplay()
    {
        RightIcon = Icon.NONE;
        Label = null;
    }

    internal override void GoRight()
    {
        ParentMenu?.SelectItem(this);
    }

    internal override void Select()
    {
        Checked = !Checked;
        ParentMenu?.CheckboxChangedEvent(this, Index, Checked);
    }
}