using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Client.Extensions;
using CitizenFX.FiveM.Shared.Script;

using static CitizenFX.FiveM.Client.Native;

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

    private static float AspectRatio => GetScreenAspectRatio(false);
    public static float ScreenWidth => 1080 * AspectRatio;
    public static float ScreenHeight => 1080;
    public static bool DisableMenuButtons { get; set; } = false;

    // Control 360 is a control that is rarely used, no other scripts should be disabling
    // it randomly (I hope). It gets disabled when the console (F8) is opened, so we
    // can use that to guess if the console is open or not by checking if the control is enabled.
    private static bool IsF8ConsoleLikelyOpen => !IsControlEnabled(0, 360);

    public static bool AreMenuButtonsEnabled =>
        IsAnyMenuOpen() &&
        !IsPauseMenuActive() &&
        IsScreenFadedIn() &&
        !IsPlayerSwitchInProgress() &&
        !DisableMenuButtons &&
        !API.Players.Local.IsDead &&
        !IsF8ConsoleLikelyOpen;

    public static bool NavigateMenuUsingArrows { get; set; } = true;
    public static bool EnableManualGCs { get; set; } = true;
    public static bool DontOpenAnyMenu { get; set; } = false;
    public static bool PreventExitingMenu { get; set; } = false;
    public static bool DisableBackButton { get; set; } = false;
    public static bool SetDrawOrder { get; set; } = true;

    public static bool EnableMenuToggleKeyOnController { get; set; } = true;

    /// <summary>
    /// The key the menu toggle is bound to for players who have never rebound it themselves. Must be
    /// set before the first tick, so from your resource's constructor. A FiveM keyboard input mapper
    /// parameter id, for example "M" or "F5".
    /// </summary>
    public static string MenuToggleKeyDefault { get; set; } = "M";

    internal static Dictionary<MenuItem, Menu> MenuButtons { get; private set; } = new Dictionary<MenuItem, Menu>();

    public static Menu MainMenu { get; set; } = null;

    internal static int _scale = RequestScaleformMovie("INSTRUCTIONAL_BUTTONS");

    private static int ManualTimerForGC = GetGameTimer();

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
                    API.Log.Error($"[MenuAPI ({GetCurrentResourceName()})] Right aligned menus are not supported for aspect ratios 17:9 or 21:9, left aligned will be used instead.");
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
        LoopRegisterKeyBindings();
        LoopProcessMenus();
        LoopDrawInstructionalButtons();
        LoopProcessMainButtons();
        LoopProcessDirectionalButtons();
        LoopProcessToggleMenuButton();
        LoopMenuButtonsDisableChecks();
    }

    // Waits a frame before registering. Every IScript is constructed before the first tick runs, so
    // this is what lets a resource set MenuToggleKeyDefault without having to care whether its own
    // script or MenuAPI's was constructed first.
    static async void LoopRegisterKeyBindings()
    {
        await API.Delay(0);
        MenuKeyBindings.Register();
    }
    static async void LoopProcessMenus()
    {
        while (true)
        {
            await ProcessMenus();
            await API.Yield();
        }
    }
    static async void LoopDrawInstructionalButtons()
    {
        while (true)
        {
            await DrawInstructionalButtons();
            await API.Yield();
        }
    }
    static async void LoopProcessMainButtons()
    {
        while (true)
        {
            await ProcessMainButtons();
            await API.Yield();
        }
    }
    static async void LoopProcessDirectionalButtons()
    {
        while (true)
        {
            await ProcessDirectionalButtons();
            await API.Yield();
        }
    }
    static async void LoopProcessToggleMenuButton()
    {
        while (true)
        {
            await ProcessToggleMenuButton();
            await API.Yield();
        }
    }
    static async void LoopMenuButtonsDisableChecks()
    {
        while (true)
        {
            await MenuButtonsDisableChecks();
            await API.Yield();
        }
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
        if (!MenuButtons.TryAdd(menuItem, childMenu))
        {
            MenuButtons[menuItem] = childMenu;
        }
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

    /// <summary>
    /// Loads the texture dict for the common menu sprites.
    /// </summary>
    /// <returns></returns>
    private static async Task LoadAssets()
    {
        menuTextureAssets.ForEach(asset =>
        {
            if (!HasStreamedTextureDictLoaded(asset))
            {
                RequestStreamedTextureDict(asset, false);
            }
        });
        while (menuTextureAssets.Any(asset => { return !HasStreamedTextureDictLoaded(asset); }))
        {
            await API.Delay(0);
        }
    }

    /// <summary>
    /// Unloads the texture dict for the common menu sprites.
    /// </summary>
    private static void UnloadAssets()
    {
        menuTextureAssets.ForEach(asset =>
        {
            if (!string.IsNullOrEmpty(asset))
            {
                if (HasStreamedTextureDictLoaded(asset))
                {
                    SetStreamedTextureDictAsNoLongerNeeded(asset);
                }
            }
        });
    }

    /// <summary>
    /// Returns the currently opened menu.
    /// </summary>
    /// <returns></returns>
    public static Menu GetCurrentMenu()
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

        if (!IsAnyMenuOpen())
        {
            MenuKeyBindings.ClearHeld();
            // Disarming here is what stops a mouse button that was already down before the menu
            // opened from selecting or going back the moment it is released.
            mouseSelectArmed = false;
            mouseBackArmed = false;
            return;
        }
        if (IsPauseMenuActive())
        {
            return;
        }
        var currentMenu = GetCurrentMenu();
        if (currentMenu == null || DontOpenAnyMenu)
        {
            return;
        }
        DisableControlAction(0, (int)Control.MultiplayerInfo, false);
        HandlePreventExit();
        if (!currentMenu.Visible || !AreMenuButtonsEnabled)
        {
            return;
        }
        await HandleMainNavigationButtons(currentMenu, selectPressed, backPressed);
    }

    private static async Task HandleMainNavigationButtons(Menu currentMenu, bool selectPressed, bool backPressed)
    {
        bool onController = !IsUsingKeyboardAndMouse(2);

        // On keyboard the mouse buttons are the only polled part left, everything else comes from the
        // key mappings. The controller controls are gated so a keyboard press is not counted twice.
        bool select = selectPressed || (!onController && IsMouseButtonUsed(Control.VehicleMouseControlOverride, ref mouseSelectArmed));
        bool back = backPressed || (!onController && IsMouseButtonUsed(Control.Aim, ref mouseBackArmed));

        if (onController)
        {
            select = select ||
                IsDisabledControlJustReleased(0, (int)Control.FrontendAccept) ||
                IsControlJustReleased(0, (int)Control.FrontendAccept);

            back = back ||
                IsDisabledControlJustReleased(0, (int)Control.PhoneCancel) ||
                IsControlJustReleased(0, (int)Control.PhoneCancel);
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
        if (IsDisabledControlJustPressed(0, (int)control) || IsControlJustPressed(0, (int)control))
        {
            armed = true;
        }

        if (!armed || !(IsDisabledControlJustReleased(0, (int)control) || IsControlJustReleased(0, (int)control)))
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
            DisableControlAction(0, (int)Control.FrontendPause, false);
            DisableControlAction(0, (int)Control.FrontendPauseAlternate, false);
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
        if (!IsControlPressed(0, (int)Control.SelectWeapon))
        {
            return false;
        }
        return IsControlPressed(0, (int)Control.SelectNextWeapon) || IsControlPressed(0, (int)Control.SelectPrevWeapon);
    }

    /// <summary>
    /// Returns true when one of the 'up' controls is currently pressed, only if the button can be active according to some conditions.
    /// </summary>
    /// <returns></returns>
    private static bool IsUpPressed()
    {
        if (!AreMenuButtonsEnabled)
        {
            return false;
        }
        if (MenuKeyBindings.UpHeld)
        {
            return true;
        }
        if (!IsUsingWeaponWheel() && (
            IsControlPressed(0, (int)Control.PhoneScrollBackward) ||
            IsDisabledControlPressed(0, (int)Control.PhoneScrollBackward)))
        {
            return true;
        }
        // Only on a controller, otherwise a keyboard press would count twice: once through the key
        // mapping and once here.
        return !IsUsingKeyboardAndMouse(2) && (
            IsControlPressed(0, (int)Control.FrontendUp) ||
            IsDisabledControlPressed(0, (int)Control.FrontendUp));
    }

    /// <summary>
    /// Returns true when one of the 'down' controls is currently pressed, only if the button can be active according to some conditions.
    /// </summary>
    /// <returns></returns>
    private static bool IsDownPressed()
    {
        if (!AreMenuButtonsEnabled)
        {
            return false;
        }
        if (MenuKeyBindings.DownHeld)
        {
            return true;
        }
        if (!IsUsingWeaponWheel() && (
            IsControlPressed(0, (int)Control.PhoneScrollForward) ||
            IsDisabledControlPressed(0, (int)Control.PhoneScrollForward)))
        {
            return true;
        }
        return !IsUsingKeyboardAndMouse(2) && (
            IsControlPressed(0, (int)Control.FrontendDown) ||
            IsDisabledControlPressed(0, (int)Control.FrontendDown));
    }

    /// <summary>
    /// Returns true when one of the 'left' controls is currently pressed, only if the button can be active according to some conditions.
    /// </summary>
    /// <returns></returns>
    private static bool IsLeftPressed()
    {
        if (!AreMenuButtonsEnabled)
        {
            return false;
        }
        if (MenuKeyBindings.LeftHeld)
        {
            return true;
        }
        return !IsUsingKeyboardAndMouse(2) && (
            IsControlPressed(0, (int)Control.PhoneLeft) ||
            IsDisabledControlPressed(0, (int)Control.PhoneLeft));
    }

    /// <summary>
    /// Returns true when one of the 'right' controls is currently pressed, only if the button can be active according to some conditions.
    /// </summary>
    /// <returns></returns>
    private static bool IsRightPressed()
    {
        if (!AreMenuButtonsEnabled)
        {
            return false;
        }
        if (MenuKeyBindings.RightHeld)
        {
            return true;
        }
        return !IsUsingKeyboardAndMouse(2) && (
            IsControlPressed(0, (int)Control.PhoneRight) ||
            IsDisabledControlPressed(0, (int)Control.PhoneRight));
    }

    /// <summary>
    /// Processes the menu toggle button to check if the menu should open or close.
    /// </summary>
    /// <returns></returns>
    private static async Task ProcessToggleMenuButton()
    {
        // Drained every frame, so a press from while the menu could not open does not open it later.
        bool togglePressed = MenuKeyBindings.ConsumeToggle();

        if (IsPauseMenuActive() || IsPauseMenuRestarting() || !IsScreenFadedIn() || IsPlayerSwitchInProgress() || API.Players.Local.IsDead || DisableMenuButtons)
        {
            return;
        }

        if (IsAnyMenuOpen())
        {
            if (togglePressed && !PreventExitingMenu)
            {
                GetCurrentMenu()?.CloseMenu();
            }
            return;
        }

        if (DontOpenAnyMenu)
        {
            return;
        }

        if (togglePressed)
        {
            OpenMainMenu();
            return;
        }

        if (!IsUsingKeyboardAndMouse(2) && EnableMenuToggleKeyOnController)
        {
            await HandleMenuToggleKeyForController();
        }
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
        // Return if the buttons are not currently enabled.
        if (!AreMenuButtonsEnabled)
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
        if (IsUpPressed())
        {
            await HandleUpNavigation(currentMenu);
        }
        else if (IsDownPressed())
        {
            await HandleDownNavigation(currentMenu);
        }

        // Check if the Go Left controls are pressed.
        else if (IsLeftPressed())
        {
            await HandleLeftNavigation(currentMenu);
        }

        // Check if the Go Right controls are pressed.
        else if (IsRightPressed())
        {
            await HandleRightNavigation(currentMenu);
        }
    }

    private static async Task HandleRightNavigation(Menu currentMenu)
    {
        if (currentMenu.GetCurrentMenuItem() is MenuItem item && item.Enabled)
        {
            currentMenu.GoRight();
            var time = GetGameTimer();
            var times = 0;
            var delay = 200;
            while (IsRightPressed() && GetCurrentMenu() != null)
            {
                currentMenu = GetCurrentMenu();
                if (GetGameTimer() - time > delay)
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
                    currentMenu.GoRight();
                    time = GetGameTimer();
                }
                await API.Delay(0);
            }
        }
    }

    private static async Task HandleLeftNavigation(Menu currentMenu)
    {
        if (currentMenu.GetCurrentMenuItem() is MenuItem item && item.Enabled)
        {
            currentMenu.GoLeft();
            var time = GetGameTimer();
            var times = 0;
            var delay = 200;
            while (IsLeftPressed() && GetCurrentMenu() != null)
            {
                currentMenu = GetCurrentMenu();
                if (GetGameTimer() - time > delay)
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
                    currentMenu.GoLeft();
                    time = GetGameTimer();
                }
                await API.Delay(0);
            }
        }
    }

    private static async Task HandleDownNavigation(Menu currentMenu)
    {
        currentMenu.GoDown();

        var time = GetGameTimer();
        var times = 0;
        var delay = 200;
        while (IsDownPressed() && GetCurrentMenu() != null)
        {
            currentMenu = GetCurrentMenu();
            if (GetGameTimer() - time > delay)
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

                time = GetGameTimer();
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
        int tmpTimer = GetGameTimer();
        while ((IsControlPressed(0, (int)Control.InteractionMenu) || IsDisabledControlPressed(0, (int)Control.InteractionMenu)) && !IsPauseMenuActive() && IsScreenFadedIn() && !API.Players.Local.IsDead && !IsPlayerSwitchInProgress() && !DontOpenAnyMenu)
        {
            if (GetGameTimer() - tmpTimer > 400)
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
        var time = GetGameTimer();
        var times = 0;
        var delay = 200;

        // Do the following as long as the controls are being pressed.
        while (IsUpPressed() && IsAnyMenuOpen() && GetCurrentMenu() != null)
        {
            // Update the current menu.
            currentMenu = GetCurrentMenu();

            // Check if the game time has changed by "delay" amount.
            if (GetGameTimer() - time > delay)
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
                time = GetGameTimer();
            }

            // Wait for the next game tick.
            await API.Delay(0);
        }
    }

    private static async Task MenuButtonsDisableChecks()
    {
        static bool isInputVisible() => UpdateOnscreenKeyboard() == 0;
        if (isInputVisible())
        {
            bool buttonsState = DisableMenuButtons;
            while (isInputVisible())
            {
                await API.Delay(0);
                DisableMenuButtons = true;
            }
            int timer = GetGameTimer();
            while (GetGameTimer() - timer < 300)
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
        DisableControlAction(0, (int)Control.InteractionMenu, false);

        // When in a vehicle
        if (API.Players.Local.Ped.IsPedInAnyVehicle())
        {
            DisableControlAction(0, (int)Control.VehicleSelectNextWeapon, false);
            DisableControlAction(0, (int)Control.VehicleSelectPrevWeapon, false);
            DisableControlAction(0, (int)Control.VehicleCinCam, false);
        }
    }

    /// <summary>
    /// Disable required game controls when the menu is open.
    /// </summary>
    /// <param name="currMenu"></param>
    private static void DisableGenericControls(Menu currMenu)
    {
        // Disable Gamepad/Controller Specific controls:
        if (!IsUsingKeyboardAndMouse(2))
        {
            DisableControlAction(0, (int)Control.MultiplayerInfo, false);
            // when in a vehicle.
            if (API.Players.Local.Ped.IsPedInAnyVehicle())
            {
                DisableControlAction(0, (int)Control.VehicleHeadlight, false);
                DisableControlAction(0, (int)Control.VehicleDuck, false);

                // toggles boost in some dlc vehicles, hence it's disabled for controllers only (pressing select in the menu would trigger this).
                DisableControlAction(0, (int)Control.VehicleFlyTransform, false);
            }
        }
        else // when not using a controller.
        {
            DisableControlAction(0, (int)Control.FrontendPauseAlternate, false); // disable the escape key opening the pause menu, pressing P still works.

            // Disable the scrollwheel button changing weapons while the menu is open.
            // Only if you press TAB (to show the weapon wheel) then it will allow you to change weapons.
            if (!IsControlPressed(0, (int)Control.SelectWeapon))
            {
                DisableControlAction(24, (int)Control.SelectNextWeapon, false);
                DisableControlAction(24, (int)Control.SelectPrevWeapon, false);
            }
        }
        var currentItem = currMenu.GetCurrentMenuItem();
        if (currentItem != null)
        {
            if (currentItem is MenuSliderItem || currentItem is MenuListItem || currentItem is MenuDynamicListItem)
            {
                // Controller only. Disabling it on keyboard would break the TAB + scrollwheel weapon
                // wheel check, because a disabled control never reads as pressed.
                if (!IsUsingKeyboardAndMouse(2))
                {
                    DisableControlAction(0, (int)Control.SelectWeapon, false);
                }
            }
        }
    }

    /// <summary>
    /// Disable conflicting Attack related game controls when the menu is open.
    /// </summary>
    private static void DisableAttackControls()
    {
        DisableControlAction(0, (int)Control.Attack, false);
        DisableControlAction(0, (int)Control.Attack2, false);
        DisableControlAction(0, (int)Control.MeleeAttack1, false);
        DisableControlAction(0, (int)Control.MeleeAttack2, false);
        DisableControlAction(0, (int)Control.MeleeAttackAlternate, false);
        DisableControlAction(0, (int)Control.MeleeAttackHeavy, false);
        DisableControlAction(0, (int)Control.MeleeAttackLight, false);
        DisableControlAction(0, (int)Control.VehicleAttack, false);
        DisableControlAction(0, (int)Control.VehicleAttack2, false);
        DisableControlAction(0, (int)Control.VehicleFlyAttack, false);
        DisableControlAction(0, (int)Control.VehiclePassengerAttack, false);
        DisableControlAction(0, (int)Control.Aim, false);
        // fires vehicle specific weapons when using right click on the mouse sometimes.
        DisableControlAction(0, (int)Control.VehicleAim, false);

        // Scroll wheel for changing weapons
        DisableControlAction(0, 16, false);
        DisableControlAction(0, 17, false);
    }

    /// <summary>
    /// Disable conflicting Phone/Navigation related game controls when the menu is open.
    /// </summary>
    private static void DisablePhoneAndArrowKeysInputs()
    {
        DisableControlAction(0, (int)Control.Phone, false);
        DisableControlAction(0, (int)Control.PhoneCancel, false);
        DisableControlAction(0, (int)Control.PhoneDown, false);
        DisableControlAction(0, (int)Control.PhoneLeft, false);
        DisableControlAction(0, (int)Control.PhoneRight, false);
    }

    /// <summary>
    /// Disable conflicting Radio related game controls when the menu is open.
    /// </summary>
    private static void DisableRadioInputs()
    {
        DisableControlAction(0, (int)Control.RadioWheelLeftRight, false);
        DisableControlAction(0, (int)Control.RadioWheelUpDown, false);
        DisableControlAction(0, (int)Control.VehicleNextRadio, false);
        DisableControlAction(0, (int)Control.VehicleRadioWheel, false);
        DisableControlAction(0, (int)Control.VehiclePrevRadio, false);
    }

    /// <summary>
    /// Draws all the menus that are visible on the screen.
    /// </summary>
    /// <returns></returns>
    private static async Task ProcessMenus()
    {
        if (!(
            Menus.Count != 0 &&
            IsAnyMenuOpen() &&
            IsScreenFadedIn() &&
            !IsPauseMenuActive() &&
            !API.Players.Local.IsDead
            && !IsPlayerSwitchInProgress()
            )
        )
        {
            UnloadAssets();
            return;
        }
        await LoadAssets();
        DisableControls();
        await DrawMenus();
        PerformGC();
    }

    private static void PerformGC()
    {
        if (EnableManualGCs)
        {
            // once a minute
            if (GetGameTimer() - ManualTimerForGC > 60000)
            {
                GC.Collect();
                ManualTimerForGC = GetGameTimer();
            }
        }
    }

    private static async Task DrawMenus()
    {
        Menu menu = GetCurrentMenu();
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
        if (
            IsPlayerSwitchInProgress() ||
            API.Players.Local.IsDead ||
            !IsScreenFadedIn() ||
            IsPlayerSwitchInProgress() ||
            IsWarningMessageActive() ||
            UpdateOnscreenKeyboard() == 0
        )
        {
            DisposeInstructionalButtonsScaleform();
            return;
        }
        Menu menu = GetCurrentMenu();
        if (menu == null || !menu.Visible || !menu.EnableInstructionalButtons)
        {
            DisposeInstructionalButtonsScaleform();
            return;
        }
        if (!HasScaleformMovieLoaded(_scale))
        {
            _scale = RequestScaleformMovie("INSTRUCTIONAL_BUTTONS");
        }
        while (!HasScaleformMovieLoaded(_scale))
        {
            await API.Delay(0);
        }

        DrawScaleformMovieFullscreen(_scale, 255, 255, 255, 0, 0);

        BeginScaleformMovieMethod(_scale, "CLEAR_ALL");
        EndScaleformMovieMethod();


        int slot = 0;

        if (menu.ShowSelectInstructionalButton)
        {
            SetInstructionalButtonSlot(slot++, MenuKeyBindings.GetSelectButton(), menu.SelectButtonText);
        }
        if (menu.ShowBackInstructionalButton)
        {
            SetInstructionalButtonSlot(slot++, MenuKeyBindings.GetBackButton(), menu.BackButtonText);
        }

        for (int i = 0; i < menu.InstructionalButtons.Count; i++)
        {
            KeyValuePair<Control, string> button = menu.InstructionalButtons.ElementAt(i);
            SetInstructionalButtonSlot(slot++, GetControlInstructionalButton(0, (int)button.Key, true), button.Value);
        }

        for (int i = 0; i < menu.CustomInstructionalButtons.Count; i++)
        {
            Menu.InstructionalButton button = menu.CustomInstructionalButtons[i];
            SetInstructionalButtonSlot(slot++, button.controlString, button.instructionText);
        }

        BeginScaleformMovieMethod(_scale, "DRAW_INSTRUCTIONAL_BUTTONS");
        ScaleformMovieMethodAddParamInt(0);
        EndScaleformMovieMethod();

        DrawScaleformMovieFullscreen(_scale, 255, 255, 255, 255, 0);
    }

    private static void SetInstructionalButtonSlot(int slot, string buttonString, string text)
    {
        BeginScaleformMovieMethod(_scale, "SET_DATA_SLOT");
        ScaleformMovieMethodAddParamInt(slot);
        PushScaleformMovieMethodParameterString(buttonString);
        PushScaleformMovieMethodParameterString(text);
        EndScaleformMovieMethod();
    }

    private static void DisposeInstructionalButtonsScaleform()
    {
        if (HasScaleformMovieLoaded(_scale))
        {
            SetScaleformMovieAsNoLongerNeeded(out _scale);
        }
    }
}