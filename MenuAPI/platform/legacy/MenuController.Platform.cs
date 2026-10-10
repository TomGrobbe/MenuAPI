using CitizenFX.Core;

namespace MenuAPI;

public partial class MenuController : BaseScript
{
    // Event handlers hang off a script instance on Legacy, and EventHandlers is protected.
    internal static MenuController? Instance { get; private set; }

    /// <summary>
    /// FiveM creates this itself when the resource starts, you never have to.
    /// </summary>
    public MenuController()
    {
        Instance = this;

        Tick += Start;
    }

    internal void AddEventHandler(string eventName, Delegate handler) => EventHandlers[eventName] += handler;

    // From the first tick rather than the constructor, so every loop MenuAPI starts runs on the
    // script scheduler, and every other script has had its constructor run first.
    private Task Start()
    {
        Tick -= Start;

        Initialize();

        return Task.FromResult(0);
    }
}
