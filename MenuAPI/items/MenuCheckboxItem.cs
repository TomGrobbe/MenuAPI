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
    public bool Checked { get; set; } = _checked;
    public CheckboxStyle Style { get; set; } = CheckboxStyle.Tick;
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

    private string GetSpriteName(bool selected)
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

    internal override void Draw(int offset)
    {
        RightIcon = Icon.NONE;
        Label = null;
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
        float spriteHeight = 45f / MenuLayout.ScreenHeight;
        float spriteWidth = 45f / MenuLayout.ScreenWidth;
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