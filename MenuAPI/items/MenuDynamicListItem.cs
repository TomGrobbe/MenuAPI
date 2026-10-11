namespace MenuAPI;

/// <summary>
/// A list item without a fixed list. Left and right call <see cref="Callback"/>, and the string it returns becomes <see cref="CurrentItem"/>.
/// Changing the value raises <see cref="Menu.OnDynamicListItemCurrentItemChange"/>, pressing it raises <see cref="Menu.OnDynamicListItemSelect"/>.
/// </summary>
/// <param name="text">The text on the left side of the item.</param>
/// <param name="initialValue">The value shown before the user changes anything.</param>
/// <param name="callback">Returns the next value on every left or right press. Must not be null.</param>
/// <param name="description">Shown below the menu while the item is highlighted. Null for none.</param>
public class MenuDynamicListItem(string text, string? initialValue, MenuDynamicListItem.ChangeItemCallback callback, string? description) : MenuItem(text, description)
{
    /// <summary>
    /// Hides the left and right arrows while the item is not highlighted. False by default.
    /// </summary>
    public bool HideArrowsWhenNotSelected
    {
        get => _hideArrowsWhenNotSelected;
        set => MenuNui.Change(ref _hideArrowsWhenNotSelected, value);
    }

    /// <summary>
    /// The value being shown. Null shows <c>N/A</c>.
    /// </summary>
    public string? CurrentItem
    {
        get => _currentItem;
        set => MenuNui.Change(ref _currentItem, value);
    }

    private bool _hideArrowsWhenNotSelected = false;
    private string? _currentItem = initialValue;

    /// <summary>
    /// Runs on every left or right press. The returned string becomes the new <see cref="CurrentItem"/>.
    /// </summary>
    /// <param name="item">The item being changed. Its <see cref="CurrentItem"/> still holds the old value.</param>
    /// <param name="left">True for the left control, false for the right control.</param>
    /// <returns>The new value to show.</returns>
    public delegate string ChangeItemCallback(MenuDynamicListItem item, bool left);

    /// <summary>
    /// Called on left and right to get the next value. Must not be null.
    /// </summary>
    public ChangeItemCallback Callback { get; set; } = callback;

    /// <summary>
    /// Creates a <see cref="MenuDynamicListItem"/> without a description.
    /// </summary>
    /// <param name="text">The text on the left side of the item.</param>
    /// <param name="initialValue">The value shown before the user changes anything.</param>
    /// <param name="callback">Returns the next value on every left or right press. Must not be null.</param>
    public MenuDynamicListItem(string text, string? initialValue, ChangeItemCallback callback) : this(text, initialValue, callback, null) { }

    internal override void PrepareForDisplay()
    {
        if (HideArrowsWhenNotSelected && !Selected)
        {
            Label = CurrentItem ?? "~r~N/A";
        }
        else
        {
            Label = $"~s~← {CurrentItem ?? "~r~N/A~s~"} ~s~→";
        }
    }

    internal override void GoRight()
    {
        string? oldValue = CurrentItem;
        string newSelectedItem = Callback(this, false);
        CurrentItem = newSelectedItem;

        if (ParentMenu is Menu parent)
        {
            parent.DynamicListItemCurrentItemChanged(parent, this, oldValue, newSelectedItem);
        }

        Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    internal override void GoLeft()
    {
        string? oldValue = CurrentItem;
        string newSelectedItem = Callback(this, true);
        CurrentItem = newSelectedItem;

        if (ParentMenu is Menu parent)
        {
            parent.DynamicListItemCurrentItemChanged(parent, this, oldValue, newSelectedItem);
        }

        Native.PlaySoundFrontend(-1, "NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
    }

    internal override void Select()
    {
        if (ParentMenu is Menu parent)
        {
            parent.DynamicListItemSelectEvent(parent, this, CurrentItem);
        }
    }
}
