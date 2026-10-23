namespace MenuAPI;

/// <summary>
/// A set of instructional buttons, the button hints in the bottom right of the screen, that belongs to
/// your resource instead of to a menu. Use it for anything that is not a menu, like a noclip, a camera
/// mode or a placement tool.
/// </summary>
/// <remarks>
/// <para>
/// A menu's own hints (<see cref="Menu.CustomInstructionalButtons"/>, key bindings with a button text)
/// only show while that menu is open. Global instructional buttons show whenever you call
/// <see cref="Show"/>, with or without a menu open, and stay until you call <see cref="Hide"/>.
/// </para>
/// <para>
/// Each set is drawn as its own row. The open menu's row is always the bottom one, and shown sets stack
/// above it in the order they were shown. When the menu closes they slide down into its place. At most
/// <see cref="MaxShown"/> sets are on screen at once, the menu's row does not count towards that.
/// </para>
/// <para>
/// Global means global to your resource: every resource has its own NUI page, so two resources never
/// share or limit each other's sets. Inside a set the first hint added is the one on the right, like
/// the game does it. Text given as a function is read again about 20 times a second, so it can change
/// by itself.
/// </para>
/// </remarks>
public sealed class GlobalInstructionalButtons
{
    /// <summary>How many sets can be on screen at the same time, not counting the open menu's own row.</summary>
    public const int MaxShown = 5;

    private const string Plus = "b_998";

    private static readonly List<GlobalInstructionalButtons> _shown = new();
    private static readonly object _lock = new();
    private static int _nextId;
    private static bool _changed;

    private readonly List<Hint> _hints = new();

    /// <summary>Creates an empty set that is not on screen yet. Add hints to it, then call <see cref="Show"/>.</summary>
    public GlobalInstructionalButtons()
    {
        Id = "row" + Interlocked.Increment(ref _nextId);
    }

    /// <summary>Every set of your resource that is on screen right now, from the bottom row up.</summary>
    public static IReadOnlyList<GlobalInstructionalButtons> Shown
    {
        get
        {
            lock (_lock)
            {
                return _shown.ToArray();
            }
        }
    }

    /// <summary>Whether this set is on screen right now.</summary>
    public bool IsShown
    {
        get
        {
            lock (_lock)
            {
                return _shown.Contains(this);
            }
        }
    }

    /// <summary>
    /// Whether the set hides while the HUD is hidden, for example by a script that hides it for a
    /// cutscene. True by default.
    /// </summary>
    public bool HideWithHud { get; set; } = true;

    internal string Id { get; }

    internal static bool AnyShown
    {
        get
        {
            lock (_lock)
            {
                return _shown.Count != 0;
            }
        }
    }

    /// <summary>
    /// Adds a hint for one or more game controls, drawn next to each other in the order given. Cast a
    /// <c>Control</c> to <c>int</c>, or pass the control of your own key mapping.
    /// </summary>
    /// <param name="text">The text next to the buttons.</param>
    /// <param name="controls">The controls whose buttons are drawn.</param>
    /// <returns>This set, so calls can be chained.</returns>
    public GlobalInstructionalButtons Add(string text, params int[] controls) => Add(() => text, controls);

    /// <summary>
    /// Adds a hint for one or more game controls, with text that is read again about 20 times a second.
    /// </summary>
    /// <param name="text">Returns the text next to the buttons.</param>
    /// <param name="controls">The controls whose buttons are drawn, in the order given.</param>
    /// <returns>This set, so calls can be chained.</returns>
    public GlobalInstructionalButtons Add(Func<string> text, params int[] controls) =>
        AddHint(new Hint(text, () => Join(controls, "%")));

    /// <summary>
    /// Adds a hint for buttons that are pressed together, drawn with a <c>+</c> between them in the
    /// order given.
    /// </summary>
    /// <param name="text">The text next to the buttons.</param>
    /// <param name="controls">The controls whose buttons are drawn.</param>
    /// <returns>This set, so calls can be chained.</returns>
    public GlobalInstructionalButtons AddCombination(string text, params int[] controls) => AddCombination(() => text, controls);

    /// <summary>
    /// Adds a hint for buttons that are pressed together, with text that is read again a few times a
    /// second.
    /// </summary>
    /// <param name="text">Returns the text next to the buttons.</param>
    /// <param name="controls">The controls whose buttons are drawn, in the order given.</param>
    /// <returns>This set, so calls can be chained.</returns>
    public GlobalInstructionalButtons AddCombination(Func<string> text, params int[] controls) =>
        AddHint(new Hint(text, () => Join(controls, "%" + Plus + "%")));

    /// <summary>
    /// Adds a hint for a key binding, which follows the player's own binding and switches between
    /// keyboard and controller by itself.
    /// </summary>
    /// <param name="text">The text next to the button.</param>
    /// <param name="binding">The key binding whose button is drawn.</param>
    /// <returns>This set, so calls can be chained.</returns>
    public GlobalInstructionalButtons Add(string text, MenuKeyBinding binding) => Add(() => text, binding);

    /// <summary>
    /// Adds a hint for a key binding, with text that is read again about 20 times a second.
    /// </summary>
    /// <param name="text">Returns the text next to the button.</param>
    /// <param name="binding">The key binding whose button is drawn.</param>
    /// <returns>This set, so calls can be chained.</returns>
    public GlobalInstructionalButtons Add(Func<string> text, MenuKeyBinding binding)
    {
        if (binding is null)
        {
            throw new ArgumentNullException(nameof(binding));
        }

        return AddHint(new Hint(text, () => binding.Icon));
    }

    /// <summary>
    /// Adds a hint from a raw button string, the same kind <see cref="Menu.CustomInstructionalButtons"/>
    /// takes. It is read from right to left, like the game's own.
    /// </summary>
    /// <param name="buttons">The raw button string, for example from <c>GetControlInstructionalButton</c>.</param>
    /// <param name="text">The text next to the buttons.</param>
    /// <returns>This set, so calls can be chained.</returns>
    public GlobalInstructionalButtons AddRaw(string buttons, string text) => AddRaw(() => buttons, () => text);

    /// <summary>
    /// Adds a hint from a raw button string, with both read again about 20 times a second.
    /// </summary>
    /// <param name="buttons">Returns the raw button string.</param>
    /// <param name="text">Returns the text next to the buttons.</param>
    /// <returns>This set, so calls can be chained.</returns>
    public GlobalInstructionalButtons AddRaw(Func<string> buttons, Func<string> text) => AddHint(new Hint(text, buttons));

    /// <summary>Removes every hint. A shown set stays shown, so add the new hints straight after.</summary>
    public void Clear()
    {
        lock (_lock)
        {
            _hints.Clear();
            _changed = true;
        }
    }

    /// <summary>
    /// Puts the set on screen as a new row, above the rows that are already there. Does nothing when it
    /// already is on screen.
    /// </summary>
    /// <returns>False when <see cref="MaxShown"/> sets are already on screen, this one is then not shown.</returns>
    public bool Show()
    {
        lock (_lock)
        {
            if (_shown.Contains(this))
            {
                return true;
            }

            if (_shown.Count >= MaxShown)
            {
                Runtime.Log.Warn($"[MenuAPI ({Native.GetCurrentResourceName()})] Global instructional buttons were not shown, {MaxShown} sets are already on screen. Hide one first.");

                return false;
            }

            _shown.Add(this);
            _changed = true;
        }

        MenuTicks.Reevaluate();

        return true;
    }

    /// <summary>Takes the set off screen. The rows above it slide down to close the gap.</summary>
    public void Hide()
    {
        lock (_lock)
        {
            if (!_shown.Remove(this))
            {
                return;
            }

            _changed = true;
        }

        MenuTicks.Reevaluate();
    }

    /// <summary>
    /// Reads the text and buttons of every hint again straight away, on this frame, instead of up to 50 milliseconds
    /// later.
    /// </summary>
    public void Refresh()
    {
        lock (_lock)
        {
            _changed = true;
        }
    }

    internal static bool ConsumeChanged()
    {
        lock (_lock)
        {
            var changed = _changed;
            _changed = false;
            return changed;
        }
    }

    internal static void WriteShown(bool hudHidden)
    {
        GlobalInstructionalButtons[] rows;

        lock (_lock)
        {
            rows = _shown.ToArray();
            _changed = false;
        }

        foreach (var row in rows)
        {
            if (hudHidden && row.HideWithHud)
            {
                continue;
            }

            Hint[] hints;

            lock (_lock)
            {
                hints = row._hints.ToArray();
            }

            InstructionalButtonsNui.BeginRow(row.Id);

            foreach (var hint in hints)
            {
                InstructionalButtonsNui.Add(Read(hint.Buttons), Read(hint.Text));
            }

            InstructionalButtonsNui.EndRow();
        }
    }

    private GlobalInstructionalButtons AddHint(Hint hint)
    {
        if (hint.Text is null)
        {
            throw new ArgumentNullException("text");
        }

        lock (_lock)
        {
            _hints.Add(hint);
            _changed = true;
        }

        return this;
    }

    // A hint that throws should not take the whole bar down with it, or the menu's own row.
    private static string Read(Func<string> value)
    {
        try
        {
            return value() ?? string.Empty;
        }
        catch (Exception exception)
        {
            Runtime.Log.Error($"[MenuAPI ({Native.GetCurrentResourceName()})] An instructional button hint threw: {exception.Message}");

            return string.Empty;
        }
    }

    // Button strings are read right to left, so the controls go in back to front to be drawn in order.
    private static string Join(int[] controls, string separator)
    {
        if (controls is null || controls.Length == 0)
        {
            return string.Empty;
        }

        var icons = new string[controls.Length];

        for (var i = 0; i < controls.Length; i++)
        {
            icons[controls.Length - 1 - i] = InstructionalButtonIcons.For(controls[i]);
        }

        return string.Join(separator, icons);
    }

    private sealed class Hint(Func<string> text, Func<string> buttons)
    {
        internal Func<string> Text { get; } = text;

        internal Func<string> Buttons { get; } = buttons;
    }
}
