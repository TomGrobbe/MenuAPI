using static CitizenFX.FiveM.Client.Native;

namespace MenuAPI;

public class MenuSliderItem(string name, string? description, int min, int max, int startPosition, bool showDivider) : MenuItem(name, description)
{
    public int Min { get; private set; } = min;
    public int Max { get; private set; } = max;
    public bool ShowDivider { get; set; } = showDivider;
    public int Position { get; set; } = startPosition;

    public Icon SliderLeftIcon { get; set; } = Icon.NONE;
    public Icon SliderRightIcon { get; set; } = Icon.NONE;

    public System.Drawing.Color BackgroundColor { get; set; } = System.Drawing.Color.FromArgb(255, 24, 93, 151);
    public System.Drawing.Color BarColor { get; set; } = System.Drawing.Color.FromArgb(255, 53, 165, 223);

    public MenuSliderItem(string name, int min, int max, int startPosition) : this(name, min, max, startPosition, false) { }
    public MenuSliderItem(string name, int min, int max, int startPosition, bool showDivider) : this(name, null, min, max, startPosition, showDivider) { }
    public MenuSliderItem(string name, string? description, int min, int max, int startPosition) : this(name, description, min, max, startPosition, false) { }

    /// <summary>
    /// Maps '<see cref="float"/> <paramref name="val"/>' to be a value between '<see cref="float"/> <paramref name="out_min"/>' and '<see cref="float"/> <paramref name="out_max"/>'.
    /// </summary>
    /// <param name="val"></param>
    /// <param name="in_min"></param>
    /// <param name="in_max"></param>
    /// <param name="out_min"></param>
    /// <param name="out_max"></param>
    /// <returns></returns>
    private static float Map(float val, float in_min, float in_max, float out_min, float out_max)
    {
        return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    internal override void Draw(int indexOffset)
    {
        RightIcon = SliderRightIcon;
        Label = null;

        base.Draw(indexOffset);

        if (ParentMenu is not Menu parent)
        {
            return;
        }

        if (Position > Max || Position < Min)
        {
            Position = (Max - Min) / 2;
        }

        int index = Index;
        bool selected = parent.CurrentIndex == index;

        float yOffset = RowYOffset(parent);

        float width = 150f / MenuLayout.ScreenWidth;
        float height = 10f / MenuLayout.ScreenHeight;
        float y = (((index - indexOffset) * RowHeight) + 20f + yOffset) / MenuLayout.ScreenHeight;
        float x = MenuLayout.MenuWidthN - (width / 2f) - (8f / MenuLayout.ScreenWidth);
        if (!parent.LeftAligned)
        {
            x = (width / 2f) - (8f / MenuLayout.ScreenWidth);
        }

        if (SliderLeftIcon != Icon.NONE && SliderRightIcon != Icon.NONE)
        {
            x -= 40f / MenuLayout.ScreenWidth;

            var leftColor = GetSpriteColour(SliderLeftIcon, selected);

            SetScriptGfxAlign(parent.LeftAligned ? 76 : 82, 84);
            SetScriptGfxAlignParams(0f, 0f, 0f, 0f);

            string textureDictionary = GetSpriteDictionary(SliderLeftIcon);

            if (parent.LeftAligned)
            {
                // left sprite left aligned.
                DrawSprite(textureDictionary, GetSpriteName(SliderLeftIcon, selected), x - (width / 2f + (4f / MenuLayout.ScreenWidth)) - (GetSpriteSize(SliderLeftIcon, true) / 2f), y, GetSpriteSize(SliderLeftIcon, true), GetSpriteSize(SliderLeftIcon, false), 0f, leftColor.R, leftColor.G, leftColor.B, 255, false, false);

                // right sprite is managed by the regular function in MenuItem that handles the right icon.
            }
            else
            {
                // left sprite right aligned.
                DrawSprite(textureDictionary, GetSpriteName(SliderLeftIcon, selected), x - (width + (4f / MenuLayout.ScreenWidth)) - GetSpriteSize(SliderLeftIcon, true) - (20f / MenuLayout.ScreenWidth), y, GetSpriteSize(SliderLeftIcon, true), GetSpriteSize(SliderLeftIcon, false), 0f, leftColor.R, leftColor.G, leftColor.B, 255, false, false);

                // right sprite is managed by the regular function in MenuItem that handles the right icon.
            }

            ResetScriptGfxAlign();
        }

        SetScriptGfxAlign(parent.LeftAligned ? 76 : 82, 84);
        SetScriptGfxAlignParams(0f, 0f, 0f, 0f);
        #region drawing background bar and foreground bar

        // background
        DrawRect(x, y, width, height, BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A, false);

        float xOffset = Map(
            (float)Position,
            (float)Min,
            (float)Max,
            -((width / 4f) * MenuLayout.ScreenWidth),
            (width / 4f) * MenuLayout.ScreenWidth
        );
        xOffset /= MenuLayout.ScreenWidth;

        // bar (foreground)
        if (!parent.LeftAligned)
        {
            DrawRect(x - (width / 2f) + xOffset, y, width / 2f, height, BarColor.R, BarColor.G, BarColor.B, BarColor.A, false);
        }
        else
        {
            DrawRect(x + xOffset, y, width / 2f, height, BarColor.R, BarColor.G, BarColor.B, BarColor.A, false);
        }

        #endregion

        #region drawing divider
        if (ShowDivider)
        {
            if (!parent.LeftAligned)
            {
                DrawRect(x - width + (4f / MenuLayout.ScreenWidth), y, 4f / MenuLayout.ScreenWidth, RowHeight / MenuLayout.ScreenHeight / 2f, 255, 255, 255, 255, false);
            }
            else
            {
                DrawRect(x + (2f / MenuLayout.ScreenWidth), y, 4f / MenuLayout.ScreenWidth, RowHeight / MenuLayout.ScreenHeight / 2f, 255, 255, 255, 255, false);
            }
        }
        #endregion
        ResetScriptGfxAlign();
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
            PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
        else
        {
            PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
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
            PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
        }
        else
        {
            PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
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