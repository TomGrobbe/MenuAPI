using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    /// <summary>
    /// Draws every registered <see cref="Menu"/> and handles all menu controls. It starts by itself when MenuAPI.dll is loaded, so you only use its static members.
    /// </summary>
    public class MenuController : BaseScript
    {
        /// <summary>
        /// Every menu registered with <see cref="AddMenu(Menu)"/> or <see cref="AddSubmenu(Menu, Menu)"/>.
        /// </summary>
        public static List<Menu> Menus { get; protected set; } = new List<Menu>();
        internal static HashSet<Menu> VisibleMenus { get; } = new HashSet<Menu>();
        /// <summary>
        /// The streamed texture dictionary used for the default menu sprites.
        /// </summary>
        public const string _texture_dict = "menu_textures";
        /// <summary>
        /// The texture name used for the default menu banner.
        /// </summary>
        public const string _header_texture = "translate_bg_1a";
        private static List<string> menuTextureAssets = new List<string>()
        {
            "menu_textures",
            "boot_flow",
            "generic_textures",
        };

        private static float AspectRatio => 16 / 9;
        /// <summary>
        /// The screen width that menu sizes are scaled against, in pixels. Always 1080 in the RedM build.
        /// </summary>
        public static float ScreenWidth => 1080 * AspectRatio;
        /// <summary>
        /// The screen height that menu sizes are scaled against, in pixels. Always 1080.
        /// </summary>
        public static float ScreenHeight => 1080;
        /// <summary>
        /// When true, all menu controls are ignored. The menu stays on screen but does not respond to input.
        /// </summary>
        public static bool DisableMenuButtons { get; set; } = false;
        /// <summary>
        /// Whether menu controls are processed right now. False when no menu is open, the screen is faded out, the pause menu is open, the player is dead, or <see cref="DisableMenuButtons"/> is true.
        /// </summary>
        public static bool AreMenuButtonsEnabled =>
            IsAnyMenuOpen() &&
            IsScreenFadedIn() &&
            !IsPauseMenuActive() &&
            !DisableMenuButtons &&
            !IsEntityDead(PlayerPedId());

        /// <summary>
        /// When true, pressing left on an item with no left and right behaviour goes back to the parent menu.
        /// </summary>
        public static bool NavigateMenuUsingArrows { get; set; } = true;
        /// <summary>
        /// Whether MenuAPI runs a garbage collection once a minute while menus are drawn. Turn this off if your resource manages that itself.
        /// </summary>
        public static bool EnableManualGCs { get; set; } = true;
        /// <summary>
        /// When true, open menus are closed and no menu is drawn, except menus with <see cref="Menu.IgnoreDontOpenMenus"/> set.
        /// </summary>
        public static bool DontOpenAnyMenu { get; set; } = false;
        /// <summary>
        /// When true, the back control can not close a menu that has no parent. Submenus can still go back.
        /// </summary>
        public static bool PreventExitingMenu { get; set; } = false;
        /// <summary>
        /// When true, the back control does nothing at all, not even in submenus.
        /// </summary>
        public static bool DisableBackButton { get; set; } = false;
        /// <summary>
        /// Whether MenuAPI sets the script graphics draw order while drawing. Turn this off if the menu layers wrongly with your own UI.
        /// </summary>
        public static bool SetDrawOrder { get; set; } = true;
        // RedM controls are hashes, not GTA's 0 to 402 indexes, so only 0 and -1 mean "no key".
        /// <summary>
        /// False when <see cref="MenuToggleKey"/> is 0 or -1, which turns the toggle key off.
        /// </summary>
        public static bool MenuToggleKeyIsValid => (uint)MenuToggleKey != 0 && (uint)MenuToggleKey != uint.MaxValue;
        /// <summary>
        /// The control that opens <see cref="MainMenu"/> when no menu is open. Defaults to <see cref="Control.PlayerMenu"/>.
        /// </summary>
        public static Control MenuToggleKey { get; set; }
            = Control.PlayerMenu;

        /// <summary>
        /// Has no effect in RedM, there is no separate controller handling for the toggle key.
        /// </summary>
        public static bool EnableMenuToggleKeyOnController { get; set; } = true;

        internal static Dictionary<MenuItem, Menu> MenuButtons { get; private set; } = new Dictionary<MenuItem, Menu>();

        /// <summary>
        /// The menu that the <see cref="MenuToggleKey"/> opens. Set to the first menu you register, but it can be changed at any time.
        /// </summary>
        public static Menu MainMenu { get; set; } = null;


        private static int ManualTimerForGC = GetGameTimer();


        /// <summary>
        /// Constructor
        /// </summary>
        public MenuController()
        {
            Tick += ProcessMenus;
            Tick += ProcessMainButtons;
            Tick += ProcessDirectionalButtons;
            Tick += ProcessToggleMenuButton;
            Tick += MenuButtonsDisableChecks;
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
            if (MenuButtons.ContainsKey(menuItem))
            {
                MenuButtons[menuItem] = childMenu;
            }
            else
            {
                MenuButtons.Add(menuItem, childMenu);
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
                if (MainMenu == null)
                {
                    MainMenu = menu;
                }
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
                AddMenu(child);
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
                await Delay(0);
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
        public static bool IsAnyMenuOpen() => VisibleMenus.Any();


        #region Process Menu Buttons
        /// <summary>
        /// Process the select &amp; go back/cancel buttons.
        /// </summary>
        /// <returns></returns>
        private async Task ProcessMainButtons()
        {
            if (!IsAnyMenuOpen())
            {
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
            HandlePreventExit();
            if (!currentMenu.Visible || !AreMenuButtonsEnabled)
            {
                return;
            }
            await HandleMainNavigationButtons(currentMenu);
        }

        private async Task HandleMainNavigationButtons(Menu currentMenu)
        {
            // Select / Enter
            if (
                IsDisabledControlJustReleased(0, (uint)Control.FrontendAccept) ||
                IsControlJustReleased(0, (uint)Control.FrontendAccept)
            )
            {
                if (currentMenu.Size > 0)
                {
                    currentMenu.SelectItem(currentMenu.CurrentIndex);
                }
            }
            // Cancel / Go Back
            else if (
                !DisableBackButton &&
                IsDisabledControlJustReleased(0, (uint)Control.FrontendCancel)
            )
            {
                // Wait for the next frame to make sure the "cinematic camera" button doesn't get "re-enabled" before the menu gets closed.
                await Delay(0);
                currentMenu.GoBack();
            }
            else if (
                PreventExitingMenu && !DisableBackButton &&
                IsDisabledControlJustReleased(0, (uint)Control.CellphoneCancel)
            )
            {
                // if there's a parent menu, allow going back to that, but don't allow a 'top-level' menu to be closed.
                if (currentMenu.ParentMenu != null)
                {
                    currentMenu.GoBack();
                }
                await Delay(0);
            }
        }

        private void HandlePreventExit()
        {
            if (PreventExitingMenu)
            {
                DisableControlAction(0, (uint)Control.FrontendPause, true);
                DisableControlAction(0, (uint)Control.FrontendPauseAlternate, true);
            }
        }

        /// <summary>
        /// Returns true when one of the 'up' controls is currently pressed, only if the button can be active according to some conditions.
        /// </summary>
        /// <returns></returns>
        private bool IsUpPressed()
        {
            if (!AreMenuButtonsEnabled)
            {
                return false;
            }
            if (
                IsControlPressed(0, (uint)Control.FrontendUp) ||
                IsDisabledControlPressed(0, (uint)Control.FrontendUp) ||
                IsControlPressed(0, (uint)Control.CellphoneScrollBackward) ||
                IsDisabledControlPressed(0, (uint)Control.CellphoneScrollBackward)
            )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true when one of the 'down' controls is currently pressed, only if the button can be active according to some conditions.
        /// </summary>
        /// <returns></returns>
        private bool IsDownPressed()
        {
            if (!AreMenuButtonsEnabled)
            {
                return false;
            }
            if (
                IsControlPressed(0, (uint)Control.FrontendDown) ||
                IsDisabledControlPressed(0, (uint)Control.FrontendDown) ||
                IsControlPressed(0, (uint)Control.CellphoneScrollForward) ||
                IsDisabledControlPressed(0, (uint)Control.CellphoneScrollForward)
            )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the menu toggle button to check if the menu should open or close.
        /// </summary>
        /// <returns></returns>
        private async Task ProcessToggleMenuButton()
        {
            if (!MenuToggleKeyIsValid)
            {
                await Delay(1_500);
                return;
            }

            ProcessToggleMenuButtonRedM();
            await Task.FromResult(0);
        }
        private void ProcessToggleMenuButtonRedM()
        {
            DisableControlAction(0, (uint)MenuToggleKey, true);
            if (
                !IsPauseMenuActive() &&
                IsScreenFadedIn() &&
                !IsAnyMenuOpen() &&
                !DisableMenuButtons &&
                !IsEntityDead(PlayerPedId()) &&
                IsDisabledControlJustReleased(0, (uint)MenuToggleKey)
            )
            {
                if (MainMenu != null)
                {
                    MainMenu.OpenMenu();
                }
                else
                {
                    Debug.WriteLine($"[ERROR] [{GetCurrentResourceName()}] [MenuAPI] MainMenu is null, so we can't open it! Make sure that MenuController.MainMenu is set to a valid Menu which is not null!");
                }
            }
        }

        /// <summary>
        /// Process left/right/up/down buttons (also holding down buttons will speed up after 3 iterations)
        /// </summary>
        /// <returns></returns>
        private async Task ProcessDirectionalButtons()
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
            else if (
                AreMenuButtonsEnabled && (
                    IsDisabledControlJustPressed(0, (uint)Control.FrontendLeft) ||
                    IsControlJustPressed(0, (uint)Control.FrontendLeft)
                )
            )
            {
                await HandleLeftNavigation(currentMenu);
            }

            // Check if the Go Right controls are pressed.
            else if (
                AreMenuButtonsEnabled && (
                    IsDisabledControlJustPressed(0, (uint)Control.FrontendRight) ||
                    IsControlJustPressed(0, (uint)Control.FrontendRight)
                )
            )
            {
                await HandleRightNavigation(currentMenu);
            }
        }

        private async Task HandleRightNavigation(Menu currentMenu)
        {
            var item = currentMenu.GetMenuItems()[currentMenu.CurrentIndex];
            if (item.Enabled)
            {
                currentMenu.GoRight();
                var time = GetGameTimer();
                var times = 0;
                var delay = 200;
                while (
                    GetCurrentMenu() != null &&
                    AreMenuButtonsEnabled && (
                        IsDisabledControlPressed(0, (uint)Control.FrontendRight) ||
                        IsControlPressed(0, (uint)Control.FrontendRight)
                    )
                )
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
                    await Delay(0);
                }
            }
        }

        private async Task HandleLeftNavigation(Menu currentMenu)
        {
            if (currentMenu.GetCurrentMenuItem() is MenuItem item && item.Enabled)
            {
                currentMenu.GoLeft();
                var time = GetGameTimer();
                var times = 0;
                var delay = 200;
                while (
                    GetCurrentMenu() != null &&
                    AreMenuButtonsEnabled && (
                        IsDisabledControlPressed(0, (uint)Control.FrontendLeft) ||
                        IsControlPressed(0, (uint)Control.FrontendLeft)
                    )
                )
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
                    await Delay(0);
                }
            }
        }

        private async Task HandleDownNavigation(Menu currentMenu)
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
                await Delay(0);
            }
        }


        private async Task HandleUpNavigation(Menu currentMenu)
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
                await Delay(0);
            }
        }

        private async Task MenuButtonsDisableChecks()
        {
            bool isInputVisible() => UpdateOnscreenKeyboard() == 0;
            if (isInputVisible())
            {
                bool buttonsState = DisableMenuButtons;
                while (isInputVisible())
                {
                    await Delay(0);
                    DisableMenuButtons = true;
                }
                int timer = GetGameTimer();
                while (GetGameTimer() - timer < 300)
                {
                    await Delay(0);
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
                return;
            var currMenu = GetCurrentMenu();
            if (currMenu == null)
                return;
            if (
                    IsEntityDead(PlayerPedId())
                )
            {
                // Close all menus when the player dies.
                CloseAllMenus();
            }
            DisableControlsRedM();
        }

        private static void DisableControlsRedM()
        {
            DisableControlAction(0, (uint)Control.Attack, true);
            DisableControlAction(0, (uint)Control.Attack2, true);
            DisableControlAction(0, (uint)Control.HorseAim, true);
            DisableControlAction(0, (uint)Control.HorseAttack, true);
            DisableControlAction(0, (uint)Control.HorseAttack2, true);
            DisableControlAction(0, (uint)Control.HorseMelee, true);
            DisableControlAction(0, (uint)Control.MeleeAttack, true);
            DisableControlAction(0, (uint)Control.MeleeBlock, true);
            DisableControlAction(0, (uint)Control.MeleeGrapple, true);
            DisableControlAction(0, (uint)Control.MeleeGrappleAttack, true);
            DisableControlAction(0, (uint)Control.MeleeGrappleBreakout, true);
            DisableControlAction(0, (uint)Control.MeleeGrappleChoke, true);
            DisableControlAction(0, (uint)Control.MeleeGrappleMountSwitch, true);
            DisableControlAction(0, (uint)Control.MeleeGrappleReversal, true);
            DisableControlAction(0, (uint)Control.MeleeGrappleStandSwitch, true);
            DisableControlAction(0, (uint)Control.MeleeHorseAttackPrimary, true);
            DisableControlAction(0, (uint)Control.MeleeHorseAttackSecondary, true);
            DisableControlAction(0, (uint)Control.MeleeModifier, true);
            DisableControlAction(0, (uint)Control.VehAttack, true);
            DisableControlAction(0, (uint)Control.VehAttack2, true);
            DisableControlAction(0, (uint)Control.VehBoatAttack, true);
            DisableControlAction(0, (uint)Control.VehBoatAttack2, true);
            DisableControlAction(0, (uint)Control.VehCarAttack, true);
            DisableControlAction(0, (uint)Control.VehCarAttack2, true);
            DisableControlAction(0, (uint)Control.VehDraftAttack, true);
            DisableControlAction(0, (uint)Control.VehDraftAttack2, true);
            DisableControlAction(0, (uint)Control.VehFlyAttack, true);
            DisableControlAction(0, (uint)Control.VehFlyAttack2, true);
            DisableControlAction(0, (uint)Control.VehPassengerAttack, true);
            if (IsInputDisabled(2))
            {
                DisableControlAction(0, (uint)Control.FrontendPauseAlternate, true);
            }
        }

        /// <summary>
        /// Draws all the menus that are visible on the screen.
        /// </summary>
        /// <returns></returns>
        private static async Task ProcessMenus()
        {
            if (!(
                Menus.Any() &&
                IsAnyMenuOpen() &&
                IsScreenFadedIn() &&
                !IsPauseMenuActive() &&
                !IsEntityDead(PlayerPedId())
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

        /// <summary>
        /// Prevent the UI prompts getting stuck on screen if this resource is ever to be restarted while someone has any menu's open.
        /// </summary>
        /// <param name="name"></param>
        [EventHandler("onResourceStop")]
        internal static void OnResourceStop(string name)
        {
            if (name == GetCurrentResourceName())
            {
                Debug.WriteLine($"^3[WARNING] [{name}] Closing all menus because this resource is being stopped, to prevent the UI Prompts getting stuck.^7");
                CloseAllMenus();
            }
        }
    }
}
