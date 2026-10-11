namespace MenuAPI;

/// <summary>
/// How menus used to be drawn. Menus are always drawn with NUI now, so this is no longer used.
/// </summary>
[Obsolete("Menus are always drawn with NUI. This enum will be removed.")]
public enum MenuRenderMode
{
    /// <summary>The old native drawing. No longer used.</summary>
    Native,
    /// <summary>Drawing on the NUI page, which is how menus are always drawn now.</summary>
    Nui,
}
