namespace MenuAPI;

/// <summary>
/// How menus used to be drawn. Menus are always drawn with NUI now, so this is no longer used.
/// </summary>
[Obsolete("Menus are always drawn with NUI. This enum will be removed.")]
public enum MenuRenderMode
{
    Native,
    Nui,
}
