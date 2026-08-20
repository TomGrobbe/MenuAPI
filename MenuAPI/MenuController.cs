using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Client.Extensions;
using CitizenFX.FiveM.Shared.Data;
using CitizenFX.FiveM.Shared.Script;

namespace MenuAPI;

public class MenuController : IScript
{
    public static List<Menu> Menus { get; protected set; } = new List<Menu>();
    internal static HashSet<Menu> VisibleMenus { get; } = new HashSet<Menu>();
    public const string _texture_dict = "commonmenu";
    public const string _header_texture = "interaction_bgd";
    private static readonly List<string> menuTextureAssets = new()
    {
        "commonmenu",
        "commonmenutu",
        "mpleaderboard",
        "mphud",
        "mpshopsale",
        "mpinventory",
        "mprankbadge",
        "mpcarhud",
        "mpcarhud2",
        "shared"
    };

    // How often the controller toggle button is checked while no menu is open. The gesture is a 400ms
    // hold, so this is well inside it: once the button is actually down the hold is timed per frame.
    private const long ControllerPollIntervalMs = 100;

    // How often the cached screen values are re-read while a menu is open. The safe zone slider is in
    // the pause menu and a resolution change is not something a player does mid menu, so this only has
    // to be often enough that the menu has settled by the time they look at it again.
    private const long LayoutRefreshIntervalMs = 500;

    private static float AspectRatio => Native.GetScreenAspectRatio(false);
    public static float ScreenWidth => 1080 * AspectRatio;
    public static float ScreenHeight => 1080;
    public static bool DisableMenuButtons { get; set; } = false;

    // Control 360 is a control that is rarely used, no other scripts should be disabling
    // it randomly (I hope). It gets disabled when the console (F8) is opened, so we
    // can use that to guess if the console is open or not by checking if the control is enabled.
    private static bool IsF8ConsoleLikelyOpen => !Native.IsControlEnabled(0, 360);

    public static bool AreMenuButtonsEnabled =>
        IsAnyMenuOpen() &&
        !Native.IsPauseMenuActive() &&
        Native.IsScreenFadedIn() &&
        !Native.IsPlayerSwitchInProgress() &&
        !DisableMenuButtons &&
        !API.Players.Local.IsDead &&
        !IsF8ConsoleLikelyOpen;

    public static bool NavigateMenuUsingArrows { get; set; } = true;
    public static bool PreventExitingMenu { get; set; } = false;
    public static bool DisableBackButton { get; set; } = false;
    public static bool SetDrawOrder { get; set; } = true;

    #region Menu title styling defaults
    // What every menu falls back to when it has not been told otherwise, so a resource can set the
    // look of its whole menu tree in one place. The defaults here are the values MenuAPI has always
    // drawn with, so leaving them alone changes nothing.

    /// <summary>The font menu titles are drawn in. See <see cref="MenuFont"/>.</summary>
    public static int DefaultTitleFont { get; set; } = MenuFont.HouseScript;

    /// <summary>Where menu titles sit inside the header.</summary>
    public static Menu.TitleAlignmentOption DefaultTitleAlignment { get; set; } = Menu.TitleAlignmentOption.Center;

    /// <summary>Whether GTA Online's moving header glow is drawn over menu banners.</summary>
    public static bool DefaultShowHeaderGlare { get; set; } = false;
    #endregion

    private static bool _dontOpenAnyMenu = false;

    // Backed by a field rather than an auto property because the controller toggle tick is gated on
    // it, and a tick's condition is only re-run when something asks for it.
    public static bool DontOpenAnyMenu
    {
        get => _dontOpenAnyMenu;
        set
        {
            if (_dontOpenAnyMenu == value)
            {
                return;
            }

            _dontOpenAnyMenu = value;
            MenuTicks.Reevaluate();
        }
    }

    private static bool _enableMenuToggleKeyOnController = true;

    // Same as DontOpenAnyMenu: gates a tick, so a change has to re-run the conditions.
    public static bool EnableMenuToggleKeyOnController
    {
        get => _enableMenuToggleKeyOnController;
        set
        {
            if (_enableMenuToggleKeyOnController == value)
            {
                return;
            }

            _enableMenuToggleKeyOnController = value;
            MenuTicks.Reevaluate();
        }
    }

    /// <summary>
    /// The key the menu toggle is bound to for players who have never rebound it themselves. Must be
    /// set before the first tick, so from your resource's constructor. A FiveM keyboard input mapper
    /// parameter id, for example "M" or "F5".
    /// </summary>
    public static string MenuToggleKeyDefault { get; set; } = "M";

    internal static Dictionary<MenuItem, Menu> MenuButtons { get; private set; } = new Dictionary<MenuItem, Menu>();

    public static Menu? MainMenu { get; set; } = null;

    internal static int _scale = Native.RequestScaleformMovie("INSTRUCTIONAL_BUTTONS");

    // Whether the mouse button was pressed down while a menu was open, see IsMouseButtonUsed.
    private static bool mouseSelectArmed = false;
    private static bool mouseBackArmed = false;

    private static MenuAlignmentOption _alignment = MenuAlignmentOption.Left;
    public static MenuAlignmentOption MenuAlignment
    {
        get
        {
            return _alignment;
        }
        set
        {
            if (AspectRatio < 1.888888888888889f)
            {
                // alignment can be whatever the resource wants it to be because this aspect ratio is supported.
                _alignment = value;
            }
            // right aligned menus are not supported for aspect ratios 17:9 or 21:9.
            else
            {
                // no matter what the new value would've been, the aspect ratio does not support right aligned menus,
                // so (re)set it to be left aligned.
                _alignment = MenuAlignmentOption.Left;

                // In case the value was being changed to be right aligned, notify the user properly.
                if (value == MenuAlignmentOption.Right)
                {
                    API.Log.Error($"[MenuAPI ({Native.GetCurrentResourceName()})] Right aligned menus are not supported for aspect ratios 17:9 or 21:9, left aligned will be used instead.");
                }
            }
        }
    }

    public enum MenuAlignmentOption
    {
        Left,
        Right
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public void Initialize()
    {
        MenuTicks.Initialize();

        RegisterKeyBindings();

        // Every one of these is gated on a menu actually being open, so with everything closed none
        // of them run at all. Stopping a tick ends its loop rather than idling it, which is the whole
        // point: no wasted native calls while the player is just driving around.

        // Registered before Menu.Draw so that when a menu opens, this one's onStarted has already
        // refreshed the screen values before the draw tick's first frame reads them.
        MenuTicks.Register("Menu.Layout", MenuLayout.Refresh, MenuTickRate.Every(LayoutRefreshIntervalMs), IsAnyMenuOpen,
            onStarted: MenuLayout.Refresh);

        MenuTicks.Register("Menu.Draw", ProcessMenus, MenuTickRate.PerFrame, IsAnyMenuOpen,
            onStopped: () =>
            {
                UnloadAssets();
                HeaderGlare.Dispose();
            });

        MenuTicks.Register("Menu.InstructionalButtons", DrawInstructionalButtons, MenuTickRate.PerFrame, IsAnyMenuOpen,
            onStopped: () =>
            {
                DisposeInstructionalButtonsScaleform();
                InstructionalButtonIcons.Clear();
            });

        MenuTicks.Register("Menu.Select", ProcessMainButtons, MenuTickRate.PerFrame, IsAnyMenuOpen,
            // Nothing drains input while every menu is closed, so a menu has to open from a clean
            // slate rather than acting on presses that arrived when there was nothing to act on.
            onStarted: () =>
            {
                MenuKeyBindings.ClearPending();
                MenuKeyBindings.ClearHeld();
            },
            onStopped: () =>
            {
                MenuKeyBindings.ClearHeld();
                // Disarming here is what stops a mouse button that was already down before the menu
                // opened from selecting or going back the moment it is released.
                mouseSelectArmed = false;
                mouseBackArmed = false;
            });

        // Separate from Menu.Select rather than merged: this one blocks inside its hold to repeat
        // loops until the key is released, and select has to stay responsive while that happens.
        MenuTicks.Register("Menu.Navigate", ProcessDirectionalButtons, MenuTickRate.PerFrame, IsAnyMenuOpen);

        MenuTicks.Register("Menu.OnscreenKeyboard", MenuButtonsDisableChecks, MenuTickRate.PerFrame, IsAnyMenuOpen);

        // The only always on tick. It has to notice the toggle key with everything closed, but it
        // reads a flag the key mapping sets rather than polling, so an idle frame costs no natives.
        MenuTicks.Register("Menu.Toggle", ProcessToggleMenuButton, MenuTickRate.PerFrame);

        // Polling is the only way to see a held controller button, so this one cannot be event
        // driven. The gesture is a 400ms hold, so checking ten times a second still opens the menu at
        // the same moment while costing a sixth of what a per frame check did.
        MenuTicks.Register("Menu.ToggleController", ProcessControllerToggle, MenuTickRate.Every(ControllerPollIntervalMs),
            condition: () => !IsAnyMenuOpen() && EnableMenuToggleKeyOnController && !DontOpenAnyMenu);
    }

    // Waits a frame before registering. Every IScript is constructed before the first tick runs, so
    // this is what lets a resource set MenuToggleKeyDefault without having to care whether its own
    // script or MenuAPI's was constructed first. Not a tick, so it is not worth a handle.
    static async void RegisterKeyBindings()
    {
        await API.Delay(0);
        MenuKeyBindings.Register();
    }
    /// <summary>
    /// This binds the <paramref name="childMenu"/> menu to the <paramref name="menuItem"/> and sets the menu's parent to <paramref name="parentMenu"/>.
    /// </summary>
    /// <param name="parentMenu"></param>
    /// <param name="childMenu"></param>
    /// <param name="menuItem"></param>
    public static void BindMenuItem(Menu parentMenu, Menu childMenu, MenuItem menuItem)
    {
        AddSubmenu(parentMenu, childMenu);

        // Pointing the row at something else leaves whatever it opened before unreachable through it,
        // so that one goes unless another row still opens it.
        if (MenuButtons.TryGetValue(menuItem, out var previous) && !ReferenceEquals(previous, childMenu))
        {
            Unbind(menuItem);
        }

        MenuButtons[menuItem] = childMenu;
    }

    /// <summary>
    /// This adds the <paramref name="menu"/> <see cref="Menu"/> to the <see cref="Menus"/> list.
    /// </summary>
    /// <param name="menu"></param>
    public static void AddMenu(Menu menu)
    {
        if (!Menus.Contains(menu))
        {
            Menus.Add(menu);
            // automatically set the first menu as the main menu if none is set yet, this can be changed at any time though.
            MainMenu ??= menu;
        }
    }

    /// <summary>
    /// Adds the <paramref name="child"/> <see cref="Menu"/> to the menus list and sets the menu's parent to <paramref name="parent"/>.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    public static void AddSubmenu(Menu parent, Menu child)
    {
        if (!Menus.Contains(child))
        {
            AddMenu(child);
        }

        child.ParentMenu = parent;
    }

    #region Removing menus

    /// <summary>Whether any row anywhere opens a menu. Lets a menu with none skip the unbind pass.</summary>
    internal static bool HasBoundMenus => MenuButtons.Count > 0;

    /// <summary>
    /// Takes <paramref name="menu"/> out of MenuAPI, along with every menu that could only be reached
    /// through one of its rows.
    /// </summary>
    /// <remarks>
    /// Everything MenuAPI held is let go: the menu leaves <see cref="Menus"/>, its rows are unbound and
    /// detached from it, and its event subscribers are dropped. What the calling resource still holds is
    /// then the only reference left, so letting go of that collects it.
    /// </remarks>
    public static void RemoveMenu(Menu menu)
    {
        if (menu is not null)
        {
            RemoveMenu(menu, []);
        }
    }

    /// <summary>Drops every menu. For a resource shutting down, or rebuilding from scratch.</summary>
    public static void RemoveAllMenus()
    {
        // Over a copy, because removing walks the list it is taking menus out of.
        foreach (var menu in Menus.ToArray())
        {
            RemoveMenu(menu);
        }

        // A menu that was never added could not have been walked above, so its rows may still be in
        // the bound table.
        Menus.Clear();
        VisibleMenus.Clear();
        MenuButtons.Clear();
        MainMenu = null;

        MenuTicks.Reevaluate();
    }

    internal static void RemoveMenu(Menu menu, HashSet<Menu> removed)
    {
        // Added before anything cascades, so a menu bound in a loop back to this one stops here.
        if (!removed.Add(menu))
        {
            return;
        }

        var parent = menu.ParentMenu;
        var wasOpen = menu.Visible;

        if (wasOpen)
        {
            // Through CloseMenu, so subscribers hear about it while they are still attached.
            menu.CloseMenu();
        }

        Menus.Remove(menu);

        // Clears the rows, which unbinds whatever they opened and cascades back into here.
        menu.Detach(removed);

        foreach (var other in Menus)
        {
            if (ReferenceEquals(other.ParentMenu, menu))
            {
                other.ParentMenu = null;
            }
        }

        if (ReferenceEquals(MainMenu, menu))
        {
            MainMenu = Menus.Count > 0 ? Menus[0] : null;
        }

        // The player was looking at it, so put them somewhere rather than nowhere.
        if (wasOpen && parent is not null && !removed.Contains(parent) && Menus.Contains(parent))
        {
            parent.OpenMenu();
        }
    }

    /// <summary>
    /// Forgets the menu <paramref name="item"/> opened, and removes that menu when no other row opens it.
    /// </summary>
    internal static void Unbind(MenuItem item, HashSet<Menu>? removed = null)
    {
        if (!MenuButtons.Remove(item, out var child))
        {
            return;
        }

        // Checked after the entry is gone, so a row bound twice to the same menu keeps it.
        if (StillBound(child) || ReferenceEquals(MainMenu, child))
        {
            return;
        }

        RemoveMenu(child, removed ?? []);
    }

    private static bool StillBound(Menu menu)
    {
        foreach (var bound in MenuButtons.Values)
        {
            if (ReferenceEquals(bound, menu))
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    /// <summary>
    /// Loads the texture dict for the common menu sprites.
    /// </summary>
    /// <returns></returns>
    /// <returns>Whether it had to wait for streaming, so callers know time has passed.</returns>
    private static async Task<bool> LoadAssets()
    {
        var waited = false;

        while (!TextureDictionaries.RequestAll(menuTextureAssets))
        {
            // The menu closing already ran UnloadAssets, so waiting out the rest of the stream would
            // leave dicts requested that nothing is going to release.
            if (!IsAnyMenuOpen())
            {
                return true;
            }

            waited = true;

            await API.Delay(0);
        }

        return waited;
    }

    /// <summary>
    /// Unloads the texture dict for the common menu sprites.
    /// </summary>
    private static void UnloadAssets()
    {
        TextureDictionaries.ReleaseAll(menuTextureAssets);
    }

    /// <summary>
    /// Returns the currently opened menu.
    /// </summary>
    /// <returns></returns>
    public static Menu? GetCurrentMenu()
    {
        if (IsAnyMenuOpen())
        {
            return VisibleMenus.FirstOrDefault();
        }
        return null;
    }

    /// <summary>
    /// Returns true if any menu is currently open.
    /// </summary>
    /// <returns></returns>
    public static bool IsAnyMenuOpen() => VisibleMenus.Count != 0;


    #region Process Menu Buttons
    /// <summary>
    /// Process the select & go back/cancel buttons.
    /// </summary>
    /// <returns></returns>
    private static async Task ProcessMainButtons()
    {
        // Always drained, so a press that arrived while the menu could not act on it is dropped
        // instead of firing later.
        bool selectPressed = MenuKeyBindings.ConsumeSelect();
        bool backPressed = MenuKeyBindings.ConsumeBack();

        if (Native.IsPauseMenuActive())
        {
            return;
        }
        var currentMenu = GetCurrentMenu();
        if (currentMenu == null || DontOpenAnyMenu)
        {
            return;
        }
        Native.DisableControlAction(0, (int)Control.MultiplayerInfo, false);
        HandlePreventExit();
        if (!currentMenu.Visible || !AreMenuButtonsEnabled)
        {
            return;
        }
        await HandleMainNavigationButtons(currentMenu, selectPressed, backPressed);
    }

    private static async Task HandleMainNavigationButtons(Menu currentMenu, bool selectPressed, bool backPressed)
    {
        bool onController = !Native.IsUsingKeyboardAndMouse(2);

        // On keyboard the mouse buttons are the only polled part left, everything else comes from the
        // key mappings. The controller controls are gated so a keyboard press is not counted twice.
        bool select = selectPressed || (!onController && IsMouseButtonUsed(Control.VehicleMouseControlOverride, ref mouseSelectArmed));
        bool back = backPressed || (!onController && IsMouseButtonUsed(Control.Aim, ref mouseBackArmed));

        if (onController)
        {
            select = select ||
                Native.IsDisabledControlJustReleased(0, (int)Control.FrontendAccept) ||
                Native.IsControlJustReleased(0, (int)Control.FrontendAccept);

            back = back ||
                Native.IsDisabledControlJustReleased(0, (int)Control.PhoneCancel) ||
                Native.IsControlJustReleased(0, (int)Control.PhoneCancel);
        }

        // Select / Enter
        if (select)
        {
            if (currentMenu.Size > 0)
            {
                currentMenu.SelectItem(currentMenu.CurrentIndex);
            }
        }
        // Cancel / Go Back
        else if (back && !DisableBackButton)
        {
            // Wait for the next frame to make sure the "cinematic camera" button doesn't get "re-enabled" before the menu gets closed.
            await API.Delay(0);

            // A submenu can always go back to its parent, a top level menu can only be closed when
            // the resource allows it.
            if (currentMenu.ParentMenu != null || !PreventExitingMenu)
            {
                currentMenu.GoBack();
            }
        }
    }

    /// <summary>
    /// Acts on a mouse button release, but only when the press that goes with it also happened while
    /// the menu was open. Without that, aiming and then opening the menu would go back as soon as the
    /// right mouse button is let go.
    /// </summary>
    private static bool IsMouseButtonUsed(Control control, ref bool armed)
    {
        if (Native.IsDisabledControlJustPressed(0, (int)control) || Native.IsControlJustPressed(0, (int)control))
        {
            armed = true;
        }

        if (!armed || !(Native.IsDisabledControlJustReleased(0, (int)control) || Native.IsControlJustReleased(0, (int)control)))
        {
            return false;
        }

        armed = false;
        return true;
    }

    private static void HandlePreventExit()
    {
        if (PreventExitingMenu)
        {
            Native.DisableControlAction(0, (int)Control.FrontendPause, false);
            Native.DisableControlAction(0, (int)Control.FrontendPauseAlternate, false);
        }
    }

    /// <summary>
    /// Returns true when the scrollwheel should be ignored because the player is picking a weapon
    /// with it (holding TAB, on foot).
    /// </summary>
    private static bool IsUsingWeaponWheel()
    {
        if (API.Players.Local.Ped.IsPedInAnyVehicle())
        {
            return false;
        }
        if (!Native.IsControlPressed(0, (int)Control.SelectWeapon))
        {
            return false;
        }
        return Native.IsControlPressed(0, (int)Control.SelectNextWeapon) || Native.IsControlPressed(0, (int)Control.SelectPrevWeapon);
    }

    /// <summary>
    /// Returns true when one of the 'up' controls is currently pressed, only if the button can be active according to some conditions.
    /// </summary>
    /// <returns></returns>
    private static bool IsUpPressed(bool buttonsEnabled)
    {
        if (!buttonsEnabled)
        {
            return false;
        }
        if (MenuKeyBindings.UpHeld)
        {
            return true;
        }
        if (!IsUsingWeaponWheel() && (
            Native.IsControlPressed(0, (int)Control.PhoneScrollBackward) ||
            Native.IsDisabledControlPressed(0, (int)Control.PhoneScrollBackward)))
        {
            return true;
        }
        // Only on a controller, otherwise a keyboard press would count twice: once through the key
        // mapping and once here.
        return !Native.IsUsingKeyboardAndMouse(2) && (
            Native.IsControlPressed(0, (int)Control.FrontendUp) ||
            Native.IsDisabledControlPressed(0, (int)Control.FrontendUp));
    }

    /// <summary>
    /// Returns true when one of the 'down' controls is currently pressed, only if the button can be active according to some conditions.
    /// </summary>
    /// <returns></returns>
    private static bool IsDownPressed(bool buttonsEnabled)
    {
        if (!buttonsEnabled)
        {
            return false;
        }
        if (MenuKeyBindings.DownHeld)
        {
            return true;
        }
        if (!IsUsingWeaponWheel() && (
            Native.IsControlPressed(0, (int)Control.PhoneScrollForward) ||
            Native.IsDisabledControlPressed(0, (int)Control.PhoneScrollForward)))
        {
            return true;
        }
        return !Native.IsUsingKeyboardAndMouse(2) && (
            Native.IsControlPressed(0, (int)Control.FrontendDown) ||
            Native.IsDisabledControlPressed(0, (int)Control.FrontendDown));
    }

    /// <summary>
    /// Returns true when one of the 'left' controls is currently pressed, only if the button can be active according to some conditions.
    /// </summary>
    /// <returns></returns>
    private static bool IsLeftPressed(bool buttonsEnabled)
    {
        if (!buttonsEnabled)
        {
            return false;
        }
        if (MenuKeyBindings.LeftHeld)
        {
            return true;
        }
        return !Native.IsUsingKeyboardAndMouse(2) && (
            Native.IsControlPressed(0, (int)Control.PhoneLeft) ||
            Native.IsDisabledControlPressed(0, (int)Control.PhoneLeft));
    }

    /// <summary>
    /// Returns true when one of the 'right' controls is currently pressed, only if the button can be active according to some conditions.
    /// </summary>
    /// <returns></returns>
    private static bool IsRightPressed(bool buttonsEnabled)
    {
        if (!buttonsEnabled)
        {
            return false;
        }
        if (MenuKeyBindings.RightHeld)
        {
            return true;
        }
        return !Native.IsUsingKeyboardAndMouse(2) && (
            Native.IsControlPressed(0, (int)Control.PhoneRight) ||
            Native.IsDisabledControlPressed(0, (int)Control.PhoneRight));
    }

    /// <summary>
    /// Processes the menu toggle button to check if the menu should open or close.
    /// </summary>
    /// <returns></returns>
    private static void ProcessToggleMenuButton()
    {
        // Drained every frame, so a press from while the menu could not open does not open it later.
        // Checked before anything else so an idle frame costs nothing: with no press the game state
        // below could not have changed the outcome anyway.
        if (!MenuKeyBindings.ConsumeToggle())
        {
            return;
        }

        if (Native.IsPauseMenuActive() || Native.IsPauseMenuRestarting() || !Native.IsScreenFadedIn() || Native.IsPlayerSwitchInProgress() || API.Players.Local.IsDead || DisableMenuButtons)
        {
            return;
        }

        if (IsAnyMenuOpen())
        {
            if (!PreventExitingMenu)
            {
                GetCurrentMenu()?.CloseMenu();
            }
            return;
        }

        if (DontOpenAnyMenu)
        {
            return;
        }

        OpenMainMenu();
    }

    /// <summary>
    /// The controller half of the toggle. Only registered while every menu is closed, so it does not
    /// need to check for that itself.
    /// </summary>
    private static async Task ProcessControllerToggle()
    {
        if (Native.IsUsingKeyboardAndMouse(2))
        {
            return;
        }

        if (Native.IsPauseMenuActive() || Native.IsPauseMenuRestarting() || !Native.IsScreenFadedIn() || Native.IsPlayerSwitchInProgress() || API.Players.Local.IsDead || DisableMenuButtons)
        {
            return;
        }

        await HandleMenuToggleKeyForController();
    }

    /// <summary>
    /// Opens <see cref="MainMenu"/>, or the first registered menu when no main menu is set.
    /// </summary>
    private static void OpenMainMenu()
    {
        if (MainMenu != null)
        {
            MainMenu.OpenMenu();
        }
        else if (Menus.Count > 0)
        {
            Menus[0].OpenMenu();
        }
    }

    /// <summary>
    /// Process left/right/up/down buttons (also holding down buttons will speed up after 3 iterations)
    /// </summary>
    /// <returns></returns>
    private static async Task ProcessDirectionalButtons()
    {
        // Read once and handed to whichever check runs. Each of those used to work it out again, so
        // a frame with nothing pressed evaluated the same seven conditions five times over.
        var buttonsEnabled = AreMenuButtonsEnabled;

        if (!buttonsEnabled)
        {
            return;
        }

        // Get the currently open menu.
        var currentMenu = GetCurrentMenu();
        // If it exists.
        if (currentMenu == null || DontOpenAnyMenu || currentMenu.Size < 1 || !currentMenu.Visible)
        {
            return;
        }
        if (IsUpPressed(buttonsEnabled))
        {
            await HandleUpNavigation(currentMenu);
        }
        else if (IsDownPressed(buttonsEnabled))
        {
            await HandleDownNavigation(currentMenu);
        }

        // Check if the Go Left controls are pressed.
        else if (IsLeftPressed(buttonsEnabled))
        {
            await HandleLeftNavigation(currentMenu);
        }

        // Check if the Go Right controls are pressed.
        else if (IsRightPressed(buttonsEnabled))
        {
            await HandleRightNavigation(currentMenu);
        }
    }

    /// <summary>
    /// Paging is deliberately slower than changing a value, and never accelerates. Held down, the
    /// accelerating repeat below would run through hundreds of pages before the player let go.
    /// </summary>
    private const int PageRepeatFirstDelay = 400;
    private const int PageRepeatDelay = 250;

    private static async Task HandleRightNavigation(Menu currentMenu)
    {
        var item = currentMenu.GetCurrentMenuItem();

        // A page move belongs to the menu, not to the row the cursor happens to be on, so a locked
        // row does not stop it.
        var paging = currentMenu.IsPageNavigation(item);

        if (item is null || (!item.Enabled && !paging))
        {
            return;
        }

        currentMenu.GoRight();
        var time = Native.GetGameTimer();
        var times = 0;
        var delay = paging ? PageRepeatFirstDelay : 200;
        while (IsRightPressed(AreMenuButtonsEnabled))
        {
            // Re-read rather than trust the captured menu: this loop awaits every frame, so the
            // menu can be closed from anywhere while it is suspended.
            if (GetCurrentMenu() is not Menu openMenu)
            {
                break;
            }
            currentMenu = openMenu;
            if (Native.GetGameTimer() - time > delay)
            {
                times++;
                if (paging)
                {
                    delay = PageRepeatDelay;
                }
                else
                {
                    if (times > 2)
                    {
                        delay = 150;
                    }
                    if (times > 5)
                    {
                        delay = 100;
                    }
                    if (times > 25)
                    {
                        delay = 50;
                    }
                    if (times > 60)
                    {
                        delay = 25;
                    }
                }
                currentMenu.GoRight();
                time = Native.GetGameTimer();
            }
            await API.Delay(0);
        }
    }

    private static async Task HandleLeftNavigation(Menu currentMenu)
    {
        var item = currentMenu.GetCurrentMenuItem();

        var paging = currentMenu.IsPageNavigation(item);

        if (item is null || (!item.Enabled && !paging))
        {
            return;
        }

        currentMenu.GoLeft();
        var time = Native.GetGameTimer();
        var times = 0;
        var delay = paging ? PageRepeatFirstDelay : 200;
        while (IsLeftPressed(AreMenuButtonsEnabled))
        {
            // Re-read rather than trust the captured menu: this loop awaits every frame, so the
            // menu can be closed from anywhere while it is suspended.
            if (GetCurrentMenu() is not Menu openMenu)
            {
                break;
            }
            currentMenu = openMenu;
            if (Native.GetGameTimer() - time > delay)
            {
                times++;
                if (paging)
                {
                    delay = PageRepeatDelay;
                }
                else
                {
                    if (times > 2)
                    {
                        delay = 150;
                    }
                    if (times > 5)
                    {
                        delay = 100;
                    }
                    if (times > 25)
                    {
                        delay = 50;
                    }
                    if (times > 60)
                    {
                        delay = 25;
                    }
                }
                currentMenu.GoLeft();
                time = Native.GetGameTimer();
            }
            await API.Delay(0);
        }
    }

    private static async Task HandleDownNavigation(Menu currentMenu)
    {
        currentMenu.GoDown();

        var time = Native.GetGameTimer();
        var times = 0;
        var delay = 200;
        while (IsDownPressed(AreMenuButtonsEnabled))
        {
            // Re-read rather than trust the captured menu: this loop awaits every frame, so the menu
            // can be closed from anywhere while it is suspended.
            if (GetCurrentMenu() is not Menu openMenu)
            {
                break;
            }
            currentMenu = openMenu;
            if (Native.GetGameTimer() - time > delay)
            {
                times++;
                if (times > 2)
                {
                    delay = 150;
                }
                if (times > 5)
                {
                    delay = 100;
                }
                if (times > 25)
                {
                    delay = 50;
                }
                if (times > 60)
                {
                    delay = 25;
                }

                currentMenu.GoDown();

                time = Native.GetGameTimer();
            }
            await API.Delay(0);
        }
    }

    /// <summary>
    /// The controller toggle is deliberately not a key mapping: it stays the back/select button, held
    /// for 400ms, and can not be rebound.
    /// </summary>
    private static async Task HandleMenuToggleKeyForController()
    {
        int tmpTimer = Native.GetGameTimer();
        while ((Native.IsControlPressed(0, (int)Control.InteractionMenu) || Native.IsDisabledControlPressed(0, (int)Control.InteractionMenu)) && !Native.IsPauseMenuActive() && Native.IsScreenFadedIn() && !API.Players.Local.IsDead && !Native.IsPlayerSwitchInProgress() && !DontOpenAnyMenu)
        {
            if (Native.GetGameTimer() - tmpTimer > 400)
            {
                OpenMainMenu();
                break;
            }
            await API.Delay(0);
        }
    }

    private static async Task HandleUpNavigation(Menu currentMenu)
    {
        // Update the currently selected item to the new one.
        currentMenu.GoUp();

        // Get the current game time.
        var time = Native.GetGameTimer();
        var times = 0;
        var delay = 200;

        // Do the following as long as the controls are being pressed.
        while (IsUpPressed(AreMenuButtonsEnabled))
        {
            // Re-read rather than trust the captured menu: this loop awaits every frame, so the menu
            // can be closed from anywhere while it is suspended.
            if (GetCurrentMenu() is not Menu openMenu)
            {
                break;
            }
            currentMenu = openMenu;

            // Check if the game time has changed by "delay" amount.
            if (Native.GetGameTimer() - time > delay)
            {
                // Increment the "changed indexes" counter
                times++;

                // If the controls are still being held down after moving 3 indexes, reduce the delay between index changes.
                if (times > 2)
                {
                    delay = 150;
                }
                if (times > 5)
                {
                    delay = 100;
                }
                if (times > 25)
                {
                    delay = 50;
                }
                if (times > 60)
                {
                    delay = 25;
                }

                // Update the currently selected item to the new one.
                currentMenu.GoUp();

                // Reset the time to the current game timer.
                time = Native.GetGameTimer();
            }

            // Wait for the next game tick.
            await API.Delay(0);
        }
    }

    private static async Task MenuButtonsDisableChecks()
    {
        static bool isInputVisible() => Native.UpdateOnscreenKeyboard() == 0;
        if (isInputVisible())
        {
            bool buttonsState = DisableMenuButtons;
            while (isInputVisible())
            {
                await API.Delay(0);
                DisableMenuButtons = true;
            }
            int timer = Native.GetGameTimer();
            while (Native.GetGameTimer() - timer < 300)
            {
                await API.Delay(0);
                DisableMenuButtons = true;
            }
            DisableMenuButtons = buttonsState;
        }
    }
    #endregion

    /// <summary>
    /// Closes all menus.
    /// </summary>
    public static void CloseAllMenus()
    {
        Menus.ForEach((m) => { if (m.Visible) { m.CloseMenu(); } });
    }

    /// <summary>
    /// Disables the most important controls for when a menu is open.
    /// </summary>
    private static void DisableControls()
    {
        if (!IsAnyMenuOpen())
        {
            return;
        }

        var currMenu = GetCurrentMenu();

        if (currMenu == null)
        {
            return;
        }

        if (API.Players.Local.IsDead)
        {
            // Close all menus when the player dies.
            CloseAllMenus();
            return;
        }

        DisableGenericControls(currMenu);
        DisableRadioInputs();
        DisablePhoneAndArrowKeysInputs();
        DisableAttackControls();

        // Both the default 'M' toggle key and the controller toggle button sit on this control, so
        // the game must not react to it while a menu is open.
        Native.DisableControlAction(0, (int)Control.InteractionMenu, false);

        // When in a vehicle
        if (API.Players.Local.Ped.IsPedInAnyVehicle())
        {
            Native.DisableControlAction(0, (int)Control.VehicleSelectNextWeapon, false);
            Native.DisableControlAction(0, (int)Control.VehicleSelectPrevWeapon, false);
            Native.DisableControlAction(0, (int)Control.VehicleCinCam, false);
        }
    }

    /// <summary>
    /// Disable required game controls when the menu is open.
    /// </summary>
    /// <param name="currMenu"></param>
    private static void DisableGenericControls(Menu currMenu)
    {
        // Disable Gamepad/Controller Specific controls:
        if (!Native.IsUsingKeyboardAndMouse(2))
        {
            Native.DisableControlAction(0, (int)Control.MultiplayerInfo, false);
            // when in a vehicle.
            if (API.Players.Local.Ped.IsPedInAnyVehicle())
            {
                Native.DisableControlAction(0, (int)Control.VehicleHeadlight, false);
                Native.DisableControlAction(0, (int)Control.VehicleDuck, false);

                // toggles boost in some dlc vehicles, hence it's disabled for controllers only (pressing select in the menu would trigger this).
                Native.DisableControlAction(0, (int)Control.VehicleFlyTransform, false);
            }
        }
        else // when not using a controller.
        {
            Native.DisableControlAction(0, (int)Control.FrontendPauseAlternate, false); // disable the escape key opening the pause menu, pressing P still works.

            // Disable the scrollwheel button changing weapons while the menu is open.
            // Only if you press TAB (to show the weapon wheel) then it will allow you to change weapons.
            if (!Native.IsControlPressed(0, (int)Control.SelectWeapon))
            {
                Native.DisableControlAction(24, (int)Control.SelectNextWeapon, false);
                Native.DisableControlAction(24, (int)Control.SelectPrevWeapon, false);
            }
        }
        var currentItem = currMenu.GetCurrentMenuItem();
        if (currentItem != null)
        {
            if (currentItem is MenuSliderItem || currentItem is MenuListItem || currentItem is MenuDynamicListItem)
            {
                // Controller only. Disabling it on keyboard would break the TAB + scrollwheel weapon
                // wheel check, because a disabled control never reads as pressed.
                if (!Native.IsUsingKeyboardAndMouse(2))
                {
                    Native.DisableControlAction(0, (int)Control.SelectWeapon, false);
                }
            }
        }
    }

    /// <summary>
    /// Disable conflicting Attack related game controls when the menu is open.
    /// </summary>
    private static void DisableAttackControls()
    {
        Native.DisableControlAction(0, (int)Control.Attack, false);
        Native.DisableControlAction(0, (int)Control.Attack2, false);
        Native.DisableControlAction(0, (int)Control.MeleeAttack1, false);
        Native.DisableControlAction(0, (int)Control.MeleeAttack2, false);
        Native.DisableControlAction(0, (int)Control.MeleeAttackAlternate, false);
        Native.DisableControlAction(0, (int)Control.MeleeAttackHeavy, false);
        Native.DisableControlAction(0, (int)Control.MeleeAttackLight, false);
        Native.DisableControlAction(0, (int)Control.VehicleAttack, false);
        Native.DisableControlAction(0, (int)Control.VehicleAttack2, false);
        Native.DisableControlAction(0, (int)Control.VehicleFlyAttack, false);
        Native.DisableControlAction(0, (int)Control.VehiclePassengerAttack, false);
        Native.DisableControlAction(0, (int)Control.Aim, false);
        // fires vehicle specific weapons when using right click on the mouse sometimes.
        Native.DisableControlAction(0, (int)Control.VehicleAim, false);

        // Scroll wheel for changing weapons
        Native.DisableControlAction(0, 16, false);
        Native.DisableControlAction(0, 17, false);
    }

    /// <summary>
    /// Disable conflicting Phone/Navigation related game controls when the menu is open.
    /// </summary>
    private static void DisablePhoneAndArrowKeysInputs()
    {
        Native.DisableControlAction(0, (int)Control.Phone, false);
        Native.DisableControlAction(0, (int)Control.PhoneCancel, false);
        Native.DisableControlAction(0, (int)Control.PhoneDown, false);
        Native.DisableControlAction(0, (int)Control.PhoneLeft, false);
        Native.DisableControlAction(0, (int)Control.PhoneRight, false);
    }

    /// <summary>
    /// Disable conflicting Radio related game controls when the menu is open.
    /// </summary>
    private static void DisableRadioInputs()
    {
        Native.DisableControlAction(0, (int)Control.RadioWheelLeftRight, false);
        Native.DisableControlAction(0, (int)Control.RadioWheelUpDown, false);
        Native.DisableControlAction(0, (int)Control.VehicleNextRadio, false);
        Native.DisableControlAction(0, (int)Control.VehicleRadioWheel, false);
        Native.DisableControlAction(0, (int)Control.VehiclePrevRadio, false);
    }

    /// <summary>
    /// Draws all the menus that are visible on the screen.
    /// </summary>
    /// <returns></returns>
    private static async Task ProcessMenus()
    {
        // Cheap insurance: Menu.Layout's onStarted normally fills this in before the first frame
        // here, but a resource that draws through some other path should never read an empty cache.
        MenuLayout.EnsureComputed();

        // Whether a menu is open is the tick's own condition, so it is not checked again here. What
        // is left changes every frame with no event to react to, which is why it stays inline: the
        // tick keeps running through a pause menu and simply draws nothing. Menu.Draw trusts this
        // check rather than repeating it.
        if (!CanDraw())
        {
            return;
        }

        // Only re-checked when the textures actually had to stream in, which waits frames and gives
        // the player time to open the pause menu. Normally they are already loaded and this is free.
        if (await LoadAssets() && !CanDraw())
        {
            return;
        }

        DisableControls();
        await DrawMenus();
    }

    /// <summary>The game states that stop a menu being drawn, none of which announce a change.</summary>
    private static bool CanDraw() =>
        Native.IsScreenFadedIn() &&
        !Native.IsPauseMenuActive() &&
        !API.Players.Local.IsDead &&
        !Native.IsPlayerSwitchInProgress();

    private static async Task DrawMenus()
    {
        Menu? menu = GetCurrentMenu();
        if (menu == null)
        {
            return;
        }
        if (DontOpenAnyMenu)
        {
            if (menu.Visible && !menu.IgnoreDontOpenMenus)
            {
                menu.CloseMenu();
            }
        }
        else if (menu.Visible)
        {
            await menu.Draw();
        }
    }

    internal static async Task DrawInstructionalButtons()
    {
        // Whether a menu is open is the tick's own condition. What is left is volatile game state
        // that changes every frame, so it stays inline.
        if (
            Native.IsPlayerSwitchInProgress() ||
            API.Players.Local.IsDead ||
            !Native.IsScreenFadedIn() ||
            Native.IsWarningMessageActive() ||
            Native.UpdateOnscreenKeyboard() == 0
        )
        {
            DisposeInstructionalButtonsScaleform();
            return;
        }
        Menu? menu = GetCurrentMenu();
        if (menu == null || !menu.Visible || !menu.EnableInstructionalButtons)
        {
            DisposeInstructionalButtonsScaleform();
            return;
        }
        if (!Native.HasScaleformMovieLoaded(_scale))
        {
            _scale = Native.RequestScaleformMovie("INSTRUCTIONAL_BUTTONS");
        }
        while (!Native.HasScaleformMovieLoaded(_scale))
        {
            await API.Delay(0);
        }

        Native.DrawScaleformMovieFullscreen(_scale, 255, 255, 255, 0, 0);

        Native.BeginScaleformMovieMethod(_scale, "CLEAR_ALL");
        Native.EndScaleformMovieMethod();

        // Once here rather than at each icon below, and the only place that has to run every frame.
        InstructionalButtonIcons.Refresh();

        int slot = 0;

        if (menu.ShowSelectInstructionalButton)
        {
            SetInstructionalButtonSlot(slot++, MenuKeyBindings.GetSelectButton(), menu.SelectButtonText);
        }
        if (menu.ShowBackInstructionalButton)
        {
            SetInstructionalButtonSlot(slot++, MenuKeyBindings.GetBackButton(), menu.BackButtonText);
        }

        // Only worth a hint when there is more than one page to move between.
        if (menu.Paginated && menu.ShowPageInstructionalButtons && menu.PageCount > 1)
        {
            SetInstructionalButtonSlot(slot++, MenuKeyBindings.GetLeftButton(), menu.PreviousPageButtonText);
            SetInstructionalButtonSlot(slot++, MenuKeyBindings.GetRightButton(), menu.NextPageButtonText);
        }

        // Enumerated rather than indexed: ElementAt on a dictionary walks it from the start every
        // time, so indexing it in a loop re-walked the whole thing once per button, every frame.
        foreach (KeyValuePair<Control, string> button in menu.InstructionalButtons)
        {
            SetInstructionalButtonSlot(slot++, InstructionalButtonIcons.For((int)button.Key), button.Value);
        }

        for (int i = 0; i < menu.CustomInstructionalButtons.Count; i++)
        {
            Menu.InstructionalButton button = menu.CustomInstructionalButtons[i];
            SetInstructionalButtonSlot(slot++, button.controlString, button.instructionText);
        }

        Native.BeginScaleformMovieMethod(_scale, "DRAW_INSTRUCTIONAL_BUTTONS");
        Native.ScaleformMovieMethodAddParamInt(0);
        Native.EndScaleformMovieMethod();

        Native.DrawScaleformMovieFullscreen(_scale, 255, 255, 255, 255, 0);
    }

    private static void SetInstructionalButtonSlot(int slot, string buttonString, string text)
    {
        Native.BeginScaleformMovieMethod(_scale, "SET_DATA_SLOT");
        Native.ScaleformMovieMethodAddParamInt(slot);
        Native.PushScaleformMovieMethodParameterString(buttonString);
        Native.PushScaleformMovieMethodParameterString(text);
        Native.EndScaleformMovieMethod();
    }

    private static void DisposeInstructionalButtonsScaleform()
    {
        if (Native.HasScaleformMovieLoaded(_scale))
        {
            Native.SetScaleformMovieAsNoLongerNeeded(ref _scale);
        }
    }
}