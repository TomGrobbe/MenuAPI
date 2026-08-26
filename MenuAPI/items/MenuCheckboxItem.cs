using CitizenFX.FiveM.Client;

namespace MenuAPI;

/// <summary>
/// Creates a new <see cref="MenuCheckboxItem"/> with all parameters set.
/// </summary>
/// <param name="text"></param>
/// <param name="description"></param>
/// <param name="_checked"></param>
public class MenuCheckboxItem(string text, string? description, bool _checked) : MenuItem(text, description)
{
    public bool Checked
    {
        get => _isChecked;
        set => MenuNui.Change(ref _isChecked, value);
    }

    public CheckboxStyle Style
    {
        get => _style;
        set => MenuNui.Change(ref _style, value);
    }

    private bool _isChecked = _checked;
    private CheckboxStyle _style = CheckboxStyle.Tick;
    public enum CheckboxStyle
    {
        Cross,
        Tick
    }

    /// <summary>
    /// Creates a basic <see cref="MenuCheckboxItem"/>.
    /// </summary>
    /// <param name="text"></param>
    public MenuCheckboxItem(string text) : this(text, null) { }
    /// <summary>
    /// Creates a basic <see cref="MenuCheckboxItem"/> and sets the checked state to <param name="_checked"></param>'s state.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="_checked"></param>
    public MenuCheckboxItem(string text, bool _checked) : this(text, null, _checked) { }
    /// <summary>
    /// Creates a basic <see cref="MenuCheckboxItem"/> and adds an item description.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="description"></param>
    public MenuCheckboxItem(string text, string? description) : this(text, description, false) { }

    private int GetSpriteColour()
    {
        return Enabled ? 255 : 109;
    }

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

    private static float GetSpriteX(Menu parent)
    {
        bool leftSide = false;
        bool leftAligned = parent.LeftAligned;
        if (leftSide)
        {
            if (leftAligned)
            {
                return 20f / MenuLayout.ScreenWidth;
            }
            else
            {
                return MenuLayout.RightWideIconX;
            }
        }
        else
        {
            if (leftAligned)
            {
                return (Width - 20f) / MenuLayout.ScreenWidth;
            }
            else
            {
                return MenuLayout.RightIconX;
            }
        }
    }

    internal override void PrepareForDisplay()
    {
        RightIcon = Icon.NONE;
        Label = null;
    }

    internal override void Draw(int offset)
    {
        base.Draw(offset);

        if (ParentMenu is not Menu parent)
        {
            return;
        }

        Native.SetScriptGfxAlign(76, 84);
        Native.SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

        int index = Index;
        bool selected = parent.CurrentIndex == index;

        float yOffset = RowYOffset(parent);
        string name = GetSpriteName(selected);

        float spriteY = (((index - offset) * RowHeight) + 20f + yOffset) / MenuLayout.ScreenHeight;
        float spriteX = GetSpriteX(parent);
        float spriteHeight = SpriteSizePx / MenuLayout.ScreenHeight;
        float spriteWidth = SpriteSizePx / MenuLayout.ScreenWidth;
        int color = GetSpriteColour();
        Native.DrawSprite("commonmenu", name, spriteX, spriteY, spriteWidth, spriteHeight, 0f, color, color, color, 255, false, false);
        Native.ResetScriptGfxAlign();
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