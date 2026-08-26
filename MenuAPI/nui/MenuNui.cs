using System.Globalization;

using CitizenFX.FiveM.Client;

namespace MenuAPI;

internal static class MenuNui
{
    private const string MessageType = "menuapi";

    private const string HideMessage = "{\"type\":\"menuapi\",\"visible\":false}";

    private const float RotationTolerance = 0.5f;
    private const int FreemodeHudColour = 116;
    private const int PauseBackgroundHudColour = 117;
    private const int WhiteHudColour = 1;
    private const int PaletteSize = 64;

    private static string? _sent;
    private static readonly NuiJson _snapshot = new();
    private static readonly NuiJson _glare = new();
    private static bool _dirty = true;
    private static readonly List<string> _pendingTextures = new();
    private static float _heading = float.NaN;
    private static string? _freemode;
    private static string? _theme;
    private static string? _accent;
    private static MenuListItem.ColorPanelType? _palette;

    internal static void Invalidate() => _dirty = true;

    internal static void Change<T>(ref T field, T value)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;

        _dirty = true;
    }

    internal static void RequestPendingTextures()
    {
        for (var i = _pendingTextures.Count - 1; i >= 0; i--)
        {
            if (TextureDictionaries.Request(_pendingTextures[i]))
            {
                _pendingTextures.RemoveAt(i);
            }
        }
    }

    internal static void SendChanges(Menu menu)
    {
        if (_dirty)
        {
            SendSnapshot(menu);
        }

        if (menu.ResolvedShowHeaderGlare)
        {
            SendHeading();
        }
    }

    internal static void SendSnapshot(Menu menu)
    {
        _dirty = false;

        Build(menu);

        if (_snapshot.Matches(_sent))
        {
            return;
        }

        _sent = _snapshot.ToString();

        Native.SendNuiMessage(_sent);
    }

    internal static void Hide()
    {
        if (_sent == HideMessage)
        {
            return;
        }

        _sent = HideMessage;

        _dirty = true;

        _heading = float.NaN;

        _palette = null;

        _pendingTextures.Clear();

        Native.SendNuiMessage(HideMessage);
    }

    private static void SendHeading()
    {
        var heading = Wrap(Native.GetFinalRenderedCamRot(2).Z);

        if (!float.IsNaN(_heading) && Math.Abs(_heading - heading) <= RotationTolerance)
        {
            return;
        }

        _heading = heading;

        var json = _glare
            .Reset()
            .Object()
            .Prop("type", "menuapi:glare")
            .Prop("heading", heading)
            .EndObject()
            .ToString();

        Native.SendNuiMessage(json);
    }

    private static void Build(Menu menu)
    {
        _pendingTextures.Clear();

        var json = _snapshot
            .Reset()
            .Object()
            .Prop("type", MessageType)
            .Prop("visible", true)
            .Prop("align", menu.LeftAligned ? "left" : "right");

        WriteOrigin(json, menu);

        json.Prop("panelBackground", PanelBackground())
            .Prop("panelAccent", PanelAccent());

        json.Object("text")
            .Prop("size", NuiTuning.TextSize)
            .Prop("brightness", NuiTuning.TextBrightness)
            .Prop("weight", NuiTuning.WeightName)
            .EndObject();

        WriteHeader(json, menu);
        WriteSubtitle(json, menu);
        WriteRows(json, menu);

        json.Prop("overflow", menu.Size > menu.MaxItemsOnScreen);

        json.Prop("description", menu.GetCurrentMenuItem()?.Description);

        WritePanel(json, menu);
        WriteStats(json, menu);

        json.EndObject();
    }

    private static void WriteOrigin(NuiJson json, Menu menu)
    {
        var inset = (1f - MenuLayout.SafeZone) / 2f;

        var x = menu.LeftAligned
            ? inset
            : 1f - inset - (Menu.Width / MenuLayout.ScreenWidth);

        json.Object("origin")
            .Prop("x", x)
            .Prop("y", inset)
            .EndObject();
    }

    private static void WriteHeader(NuiJson json, Menu menu)
    {
        if (string.IsNullOrEmpty(menu.MenuTitle))
        {
            json.Null("header");

            return;
        }

        json.Object("header")
            .Prop("title", menu.MenuTitle)
            .Prop("font", menu.ResolvedTitleFont)
            .Prop("titleAlign", menu.ResolvedTitleAlignment switch
            {
                Menu.TitleAlignmentOption.Left => "left",
                Menu.TitleAlignmentOption.Right => "right",
                _ => "center",
            })
            .Prop("glare", menu.ResolvedShowHeaderGlare);

        var custom = !string.IsNullOrEmpty(menu.HeaderTexture.Key) && !string.IsNullOrEmpty(menu.HeaderTexture.Value);
        var dictionary = custom ? menu.HeaderTexture.Key : MenuController._texture_dict;

        if (TextureReady(dictionary))
        {
            json.Object("texture")
                .Prop("dict", dictionary)
                .Prop("name", custom ? menu.HeaderTexture.Value : MenuController._header_texture)
                .EndObject();
        }
        else
        {
            json.Null("texture");
        }

        json.EndObject();
    }

    private static void WriteSubtitle(NuiJson json, Menu menu)
    {
        var counter = !string.IsNullOrEmpty(menu.CounterPreText) || menu.MaxItemsOnScreen < menu.Size
            ? (menu.CounterPreText ?? "") + (menu.CurrentIndex + 1) + " / " + menu.Size
            : null;

        json.Object("subtitle")
            .Prop("text", menu.MenuSubtitle)
            .Prop("counter", counter)
            .Prop("colour", Freemode())
            .Prop("freemode", !(menu.MenuSubtitle ?? "").Contains('~')
                && !(menu.CounterPreText ?? "").Contains('~')
                && !string.IsNullOrEmpty(menu.MenuTitle))
            .EndObject();
    }

    private static void WriteRows(NuiJson json, Menu menu)
    {
        json.Array("rows");

        var visible = menu.VisibleMenuItems;

        for (var i = 0; i < visible.Count; i++)
        {
            var item = visible[i];

            item.PrepareForDisplay();

            var selected = menu.CurrentIndex == menu.ViewIndexOffset + i;

            json.Object()
                .Prop("kind", item switch
                {
                    MenuCheckboxItem => "checkbox",
                    MenuSliderItem => "slider",
                    SeparatorMenuItem => "separator",
                    _ => "item",
                })
                .Prop("text", item.Text)
                .Prop("label", item.Label)
                .Prop("enabled", item.Enabled)
                .Prop("selected", selected);

            WriteIcon(json, "leftIcon", item, item.LeftIcon, selected);
            WriteIcon(json, "rightIcon", item, item.RightIcon, selected);

            switch (item)
            {
                case MenuCheckboxItem checkbox when TextureReady(MenuController._texture_dict):
                    json.Object("checkbox")
                        .Prop("dict", MenuController._texture_dict)
                        .Prop("name", checkbox.GetSpriteName(selected))
                        .Prop("size", MenuCheckboxItem.SpriteSizePx)
                        .Prop("shade", checkbox.Enabled ? 255 : 109)
                        .EndObject();

                    break;

                case MenuSliderItem slider:
                    json.Object("slider")
                        .Prop("min", slider.Min)
                        .Prop("max", slider.Max)
                        .Prop("position", slider.Position)
                        .Prop("divider", slider.ShowDivider)
                        .Prop("background", Hex(slider.BackgroundColor))
                        .Prop("bar", Hex(slider.BarColor));

                    WriteIcon(json, "sliderLeftIcon", slider, slider.SliderLeftIcon, selected);

                    json.EndObject();

                    break;

                case SeparatorMenuItem separator:
                    json.Prop("arrows", separator.ShowArrows);

                    break;
            }

            json.EndObject();
        }

        json.EndArray();
    }

    private static bool TextureReady(string dict)
    {
        if (TextureDictionaries.Request(dict))
        {
            return true;
        }

        if (!_pendingTextures.Contains(dict))
        {
            _pendingTextures.Add(dict);
        }

        return false;
    }

    private static void WriteIcon(NuiJson json, string name, MenuItem item, MenuItem.Icon icon, bool selected)
    {
        if (icon == MenuItem.Icon.NONE)
        {
            json.Null(name);

            return;
        }

        var dictionary = item.GetSpriteDictionary(icon);

        if (!TextureReady(dictionary))
        {
            json.Null(name);

            return;
        }

        var colour = item.GetSpriteColour(icon, selected);

        json.Object(name)
            .Prop("dict", dictionary)
            .Prop("name", item.GetSpriteName(icon, selected))
            .Prop("size", MenuItem.GetSpriteSizePx(icon))
            .Prop("r", colour.R)
            .Prop("g", colour.G)
            .Prop("b", colour.B)
            .EndObject();
    }

    private static void WritePanel(NuiJson json, Menu menu)
    {
        if (menu.GetCurrentMenuItem() is not MenuListItem item
            || (!item.ShowColorPanel && !item.ShowOpacityPanel))
        {
            json.Null("panel");

            return;
        }

        json.Object("panel")
            .Prop("colours", item.ShowColorPanel)
            .Prop("index", item.ListIndex)
            .Prop("title", "Opacity");

        if (item.ShowOpacityPanel)
        {
            json.Prop("opacity", item.ResolvedOpacityPercent);
        }
        else
        {
            json.Null("opacity");
        }

        json.Prop("name", item.ShowColorPanel ? ColourName(item.ListIndex + 1, item.ItemsCount) : null);

        json.EndObject();

        if (item.ShowColorPanel)
        {
            SendPalette(item.ColorPanelColorType);
        }
    }

    private static void SendPalette(MenuListItem.ColorPanelType type)
    {
        if (_palette == type)
        {
            return;
        }

        _palette = type;

        var json = new NuiJson()
            .Object()
            .Prop("type", "menuapi:palette")
            .Array("colours");

        for (var i = 0; i < PaletteSize; i++)
        {
            int r;
            int g;
            int b;

            if (type == MenuListItem.ColorPanelType.Hair)
            {
                Native.GetHairRgbColor(i, out r, out g, out b);
            }
            else
            {
                Native.GetMakeupRgbColor(i, out r, out g, out b);
            }

            json.Array().Value(r).Value(g).Value(b).EndArray();
        }

        Native.SendNuiMessage(json.EndArray().EndObject().ToString());
    }

    private static string ColourName(int position, int count)
    {
        var template = Native.GetLabelText("FACE_COLOUR");

        if (string.IsNullOrEmpty(template) || template == "NULL")
        {
            return position + " / " + count;
        }

        return Substitute(Substitute(template, position), count);
    }

    private static string Substitute(string text, int value)
    {
        var at = text.IndexOf("~1~", StringComparison.Ordinal);

        return at < 0 ? text : text[..at] + value + text[(at + 3)..];
    }

    private static void WriteStats(NuiJson json, Menu menu)
    {
        if (menu.GetCurrentMenuItem() is MenuListItem { ShowColorPanel: true } or MenuListItem { ShowOpacityPanel: true })
        {
            json.Null("stats");

            return;
        }

        if (!menu.ShowWeaponStatsPanel && !menu.ShowVehicleStatsPanel)
        {
            json.Null("stats");

            return;
        }

        var weapon = menu.ShowWeaponStatsPanel;
        var values = weapon ? menu.WeaponStats : menu.VehicleStats;
        var upgrades = weapon ? menu.WeaponComponentStats : menu.VehicleUpgradeStats;

        json.Array("stats");

        for (var i = 0; i < 4; i++)
        {
            json.Object()
                .Prop("label", Native.GetLabelText(menu.StatLabelKey(i)))
                .Prop("value", values[i])
                .Prop("upgrade", upgrades[i])
                .EndObject();
        }

        json.EndArray();
    }

    private static string Freemode()
    {
        if (_freemode is null)
        {
            Native.GetHudColour(FreemodeHudColour, out var r, out var g, out var b, out _);

            _freemode = $"rgb({r} {g} {b})";
        }

        return _freemode;
    }

    private static string PanelBackground()
    {
        if (_theme is null)
        {
            Native.GetHudColour(PauseBackgroundHudColour, out var r, out var g, out var b, out var a);

            var alpha = (a / 255f).ToString("0.###", CultureInfo.InvariantCulture);

            _theme = "rgb(" + r + " " + g + " " + b + " / " + alpha + ")";
        }

        return _theme;
    }

    private static string PanelAccent()
    {
        if (_accent is null)
        {
            Native.GetHudColour(WhiteHudColour, out var r, out var g, out var b, out _);

            // Comma separated: it goes into rgba(var(--accent), 0.3).
            _accent = r + ", " + g + ", " + b;
        }

        return _accent;
    }

    private static string Hex(System.Drawing.Color colour) =>
        "#" + colour.R.ToString("x2") + colour.G.ToString("x2") + colour.B.ToString("x2");

    private static float Wrap(float degrees)
    {
        var wrapped = degrees % 360f;

        return wrapped < 0f ? wrapped + 360f : wrapped;
    }
}
