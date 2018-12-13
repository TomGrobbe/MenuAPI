using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    public class MenuController : BaseScript
    {
        public static List<Menu> Menus { get; protected set; } = new List<Menu>();
        public const string _texture_dict = "commonmenu";
        public const string _header_texture = "interaction_bgd";

        public static float ScreenWidth { get { int width = 0, height = 0; GetScreenActiveResolution(ref width, ref height); return (float)width; } }
        public static float ScreenHeight { get { int width = 0, height = 0; GetScreenActiveResolution(ref width, ref height); return (float)height; } }
        public static bool DisableMenuButtons { get; set; } = false;
        public static bool AreMenuButtonsEnabled => Menus.Any((m) => m.Visible) && !Game.IsPaused && CitizenFX.Core.UI.Screen.Fading.IsFadedIn && !IsPlayerSwitchInProgress() && !DisableMenuButtons;

        public static bool EnableManualGCs = true;
        public static bool DontOpenAnyMenu = false;
        public static bool PreventExitingMenu = false;
        public static bool DisableMenuControls = false;

        internal static Dictionary<MenuItem, Menu> MenuButtons { get; private set; } = new Dictionary<MenuItem, Menu>();

        public static Menu MainMenu = null;

        public static MenuAlignmentOption MenuAlignment = MenuAlignmentOption.Left;
        public enum MenuAlignmentOption
        {
            Left,
            Right
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuController()
        {
            Tick += ProcessMenus;
            Tick += ProcessMainButtons;
            Tick += ProcessDirectionalButtons;
            Tick += ProcessToggleMenuButton;
        }

        public static void BindMenuItem(Menu parent, Menu child, MenuItem item)
        {
            AddSubmenu(parent, child);
            if (MenuButtons.ContainsKey(item))
            {
                MenuButtons[item] = child;
            }
            else
            {
                MenuButtons.Add(item, child);
            }
        }

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
            if (!HasStreamedTextureDictLoaded(_texture_dict))
            {
                RequestStreamedTextureDict(_texture_dict, false);
                while (!HasStreamedTextureDictLoaded(_texture_dict))
                {
                    await Delay(0);
                }
            }
        }

        /// <summary>
        /// Unloads the texture dict for the common menu sprites.
        /// </summary>
        private static void UnloadAssets()
        {
            if (HasStreamedTextureDictLoaded(_texture_dict))
            {
                SetStreamedTextureDictAsNoLongerNeeded(_texture_dict);
            }
        }

        /// <summary>
        /// Returns the currently opened menu.
        /// </summary>
        /// <returns></returns>
        public static Menu GetCurrentMenu()
        {
            if (Menus.Any((m) => m.Visible))
                return Menus.Find((m) => m.Visible);
            return null;
        }

        /// <summary>
        /// Returns true if any menu is currently open.
        /// </summary>
        /// <returns></returns>
        public static bool IsAnyMenuOpen() => Menus.Any((m) => m.Visible);

        private static int timer = GetGameTimer();

        #region Process Menu Buttons
        /// <summary>
        /// Process the select & go back/cancel buttons.
        /// </summary>
        /// <returns></returns>
        private async Task ProcessMainButtons()
        {
            if (IsAnyMenuOpen())
            {
                var currentMenu = GetCurrentMenu();
                if (currentMenu != null && !DontOpenAnyMenu)
                {
                    if (PreventExitingMenu)
                    {
                        Game.DisableControlThisFrame(0, Control.FrontendPause);
                        Game.DisableControlThisFrame(0, Control.FrontendPauseAlternate);
                    }

                    if (currentMenu.Visible && !DisableMenuControls)
                    {
                        // Select / Enter
                        if (Game.IsDisabledControlJustReleased(0, Control.FrontendAccept) || Game.IsControlJustReleased(0, Control.FrontendAccept))
                        {
                            if (currentMenu.Size > 0)
                            {
                                currentMenu.SelectItem(currentMenu.CurrentIndex);
                            }
                        }
                        // Cancel / Go Back
                        else if (Game.IsDisabledControlJustReleased(0, Control.PhoneCancel) && !PreventExitingMenu)
                        {
                            // Wait for the next frame to make sure the "cinematic camera" button doesn't get "re-enabled" before the menu gets closed.
                            await Delay(0);
                            currentMenu.GoBack();
                        }
                        else if (Game.IsDisabledControlJustReleased(0, Control.PhoneCancel) && PreventExitingMenu)
                        {
                            await Delay(0);
                            //Notify.Alert("You must save your ped first before exiting, or click the ~r~Exit Without Saving~s~ button.");
                        }
                    }
                }
                else
                {
                    await Delay(0);
                }
            }
        }

        /// <summary>
        /// Returns true when one of the 'up' controls is currently pressed, only if the button can be active according to some conditions.
        /// </summary>
        /// <returns></returns>
        private bool IsUpPressed()
        {
            // return false (not pressed) if the pause menu is active.
            if (Game.IsPaused)
            {
                return false;
            }

            // when the player is holding TAB, while not in a vehicle, and when the scrollwheel is being used, return false to prevent interferring with weapon selection.
            if (!Game.PlayerPed.IsInVehicle())
            {
                if (Game.IsControlPressed(0, Control.SelectWeapon))
                {
                    if (Game.IsControlPressed(0, Control.SelectNextWeapon) || Game.IsControlPressed(0, Control.SelectPrevWeapon))
                    {
                        return false;
                    }
                }
            }

            // return true if the scrollwheel up or the arrow up key is being used at this frame.
            if (Game.IsControlPressed(0, Control.FrontendUp) ||
                Game.IsDisabledControlPressed(0, Control.FrontendUp) ||
                Game.IsControlPressed(0, Control.PhoneScrollBackward) ||
                Game.IsDisabledControlPressed(0, Control.PhoneScrollBackward))
            {
                return true;
            }

            // return false if none of the conditions matched.
            return false;
        }

        /// <summary>
        /// Returns true when one of the 'down' controls is currently pressed, only if the button can be active according to some conditions.
        /// </summary>
        /// <returns></returns>
        private bool IsDownPressed()
        {
            // return false (not pressed) if the pause menu is active.
            if (Game.IsPaused)
            {
                return false;
            }

            // when the player is holding TAB, while not in a vehicle, and when the scrollwheel is being used, return false to prevent interferring with weapon selection.
            if (!Game.PlayerPed.IsInVehicle())
            {
                if (Game.IsControlPressed(0, Control.SelectWeapon))
                {
                    if (Game.IsControlPressed(0, Control.SelectNextWeapon) || Game.IsControlPressed(0, Control.SelectPrevWeapon))
                    {
                        return false;
                    }
                }
            }

            // return true if the scrollwheel down or the arrow down key is being used at this frame.
            if (Game.IsControlPressed(0, Control.FrontendDown) ||
                Game.IsDisabledControlPressed(0, Control.FrontendDown) ||
                Game.IsControlPressed(0, Control.PhoneScrollForward) ||
                Game.IsDisabledControlPressed(0, Control.PhoneScrollForward))
            {
                return true;
            }

            // return false if none of the conditions matched.
            return false;
        }

        /// <summary>
        /// Processes the menu toggle button to check if the menu should open or close.
        /// </summary>
        /// <returns></returns>
        private async Task ProcessToggleMenuButton()
        {

            if (IsAnyMenuOpen())
            {
                if (Game.CurrentInputMode == InputMode.MouseAndKeyboard)
                {
                    if ((Game.IsControlJustPressed(0, Control.InteractionMenu) || Game.IsDisabledControlJustPressed(0, Control.InteractionMenu)) && !Game.IsPaused && IsScreenFadedIn() && !IsPlayerSwitchInProgress() && !PreventExitingMenu)
                    {
                        var menu = GetCurrentMenu();
                        if (menu != null)
                        {
                            menu.CloseMenu();
                        }
                    }
                }
            }
            else
            {
                if (Game.CurrentInputMode == InputMode.GamePad)
                {
                    int tmpTimer = GetGameTimer();
                    while ((Game.IsControlPressed(0, Control.InteractionMenu) || Game.IsDisabledControlPressed(0, Control.InteractionMenu)) && !Game.IsPaused && IsScreenFadedIn() && !IsPlayerSwitchInProgress() && !PreventExitingMenu)
                    {
                        if (GetGameTimer() - tmpTimer > 400)
                        {
                            if (MainMenu != null)
                            {
                                MainMenu.Visible = true;
                            }
                            else
                            {
                                if (Menus.Count > 0)
                                {
                                    Menus[0].Visible = true;
                                }
                            }
                            break;
                        }
                        await Delay(0);
                    }
                }
                else
                {
                    if ((Game.IsControlJustPressed(0, Control.InteractionMenu) || Game.IsDisabledControlJustPressed(0, Control.InteractionMenu)) && !Game.IsPaused && IsScreenFadedIn() && !IsPlayerSwitchInProgress() && !PreventExitingMenu)
                    {
                        if (Menus.Count > 0)
                        {
                            if (MainMenu != null)
                            {
                                MainMenu.Visible = true;
                            }
                            else
                            {
                                Menus[0].Visible = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process left/right/up/down buttons (also holding down buttons will speed up after 3 iterations)
        /// </summary>
        /// <returns></returns>
        private async Task ProcessDirectionalButtons()
        {
            if (IsAnyMenuOpen())
            {
                // Get the currently open menu.
                var currentMenu = GetCurrentMenu();
                // If it exists.
                if (currentMenu != null && !DontOpenAnyMenu && currentMenu.Size > 0)
                {
                    if (currentMenu.Visible && !DisableMenuControls)
                    {
                        // Check if the Go Up controls are pressed.
                        if (IsUpPressed())
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

                                    // Update the currently selected item to the new one.
                                    currentMenu.GoUp();

                                    // Reset the time to the current game timer.
                                    time = GetGameTimer();
                                }

                                // Wait for the next game tick.
                                await Delay(0);
                            }
                        }

                        // Check if the Go Down controls are pressed.
                        else if (IsDownPressed())
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
                                    currentMenu.GoDown();

                                    time = GetGameTimer();
                                }
                                await Delay(0);
                            }
                        }

                        // Check if the Go Left controls are pressed.
                        else if (Game.IsDisabledControlJustPressed(0, Control.PhoneLeft) && !Game.IsControlPressed(0, Control.SelectWeapon))
                        {
                            var item = currentMenu.GetMenuItems()[currentMenu.CurrentIndex];
                            if (item.Enabled) //&& item is MenuItemButton)
                            {
                                currentMenu.GoLeft();
                                var time = GetGameTimer();
                                var times = 0;
                                var delay = 200;
                                while (Game.IsDisabledControlPressed(0, Control.PhoneLeft) && !Game.IsControlPressed(0, Control.SelectWeapon) && GetCurrentMenu() != null)
                                {
                                    currentMenu = GetCurrentMenu();
                                    if (GetGameTimer() - time > delay)
                                    {
                                        times++;
                                        if (times > 2)
                                        {
                                            delay = 150;
                                        }
                                        currentMenu.GoLeft();
                                        time = GetGameTimer();
                                    }
                                    await Delay(0);
                                }
                            }
                        }

                        // Check if the Go Right controls are pressed.
                        else if (Game.IsDisabledControlJustPressed(0, Control.PhoneRight) && !Game.IsControlPressed(0, Control.SelectWeapon))
                        {
                            var item = currentMenu.GetMenuItems()[currentMenu.CurrentIndex];
                            if (item.Enabled)
                            {
                                currentMenu.GoRight();
                                var time = GetGameTimer();
                                var times = 0;
                                var delay = 200;
                                while ((Game.IsDisabledControlPressed(0, Control.PhoneRight) || Game.IsControlPressed(0, Control.PhoneRight)) && !Game.IsControlPressed(0, Control.SelectWeapon) && GetCurrentMenu() != null)
                                {
                                    currentMenu = GetCurrentMenu();
                                    if (GetGameTimer() - time > delay)
                                    {
                                        times++;
                                        if (times > 2)
                                        {
                                            delay = 150;
                                        }
                                        currentMenu.GoRight();
                                        time = GetGameTimer();
                                    }
                                    await Delay(0);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                await Delay(0);
            }
        }
        #endregion

        /// <summary>
        /// Closes all menus.
        /// </summary>
        public static void CloseAllMenus()
        {
            Menus.ForEach((m) => m.Visible = false);
        }

        /// <summary>
        /// Disables the most important controls for when a menu is open.
        /// </summary>
        private static void DisableControls()
        {
            #region Disable Inputs when any menu is open.
            if (IsAnyMenuOpen())
            {
                // Close all menus when the player dies.
                if (Game.PlayerPed.IsDead)
                {
                    CloseAllMenus();
                }

                // Disable Gamepad/Controller Specific controls:
                if (Game.CurrentInputMode == InputMode.GamePad)
                {
                    Game.DisableControlThisFrame(0, Control.MultiplayerInfo);
                    // when in a vehicle.
                    if (Game.PlayerPed.IsInVehicle())
                    {
                        Game.DisableControlThisFrame(0, Control.VehicleHeadlight);
                        Game.DisableControlThisFrame(0, Control.VehicleDuck);
                    }
                }
                else // when not using a controller.
                {
                    Game.DisableControlThisFrame(0, Control.FrontendPauseAlternate); // disable the escape key opening the pause menu, pressing P still works.

                    // Disable the scrollwheel button changing weapons while the menu is open.
                    // Only if you press TAB (to show the weapon wheel) then it will allow you to change weapons.
                    if (!Game.IsControlPressed(0, Control.SelectWeapon))
                    {
                        Game.DisableControlThisFrame(24, Control.SelectNextWeapon);
                        Game.DisableControlThisFrame(24, Control.SelectPrevWeapon);
                    }
                }
                // Disable Shared Controls

                // Radio Inputs
                Game.DisableControlThisFrame(0, Control.RadioWheelLeftRight);
                Game.DisableControlThisFrame(0, Control.RadioWheelUpDown);
                Game.DisableControlThisFrame(0, Control.VehicleNextRadio);
                Game.DisableControlThisFrame(0, Control.VehicleRadioWheel);
                Game.DisableControlThisFrame(0, Control.VehiclePrevRadio);

                // Phone / Arrows Inputs
                Game.DisableControlThisFrame(0, Control.Phone);
                Game.DisableControlThisFrame(0, Control.PhoneCancel);
                Game.DisableControlThisFrame(0, Control.PhoneDown);
                Game.DisableControlThisFrame(0, Control.PhoneLeft);
                Game.DisableControlThisFrame(0, Control.PhoneRight);

                // Attack Controls
                Game.DisableControlThisFrame(0, Control.Attack);
                Game.DisableControlThisFrame(0, Control.Attack2);
                Game.DisableControlThisFrame(0, Control.MeleeAttack1);
                Game.DisableControlThisFrame(0, Control.MeleeAttack2);
                Game.DisableControlThisFrame(0, Control.MeleeAttackAlternate);
                Game.DisableControlThisFrame(0, Control.MeleeAttackHeavy);
                Game.DisableControlThisFrame(0, Control.MeleeAttackLight);
                Game.DisableControlThisFrame(0, Control.VehicleAttack);
                Game.DisableControlThisFrame(0, Control.VehicleAttack2);
                Game.DisableControlThisFrame(0, Control.VehicleFlyAttack);
                Game.DisableControlThisFrame(0, Control.VehiclePassengerAttack);
                Game.DisableControlThisFrame(0, Control.Aim);

                // When in a vehicle
                if (Game.PlayerPed.IsInVehicle())
                {
                    Game.DisableControlThisFrame(0, Control.VehicleSelectNextWeapon);
                    Game.DisableControlThisFrame(0, Control.VehicleSelectPrevWeapon);
                    Game.DisableControlThisFrame(0, Control.VehicleCinCam);
                }
            }
            #endregion
        }

        /// <summary>
        /// Draws all the menus that are visible on the screen.
        /// </summary>
        /// <returns></returns>
        private static async Task ProcessMenus()
        {
            if (Menus.Count > 0)
            {
                await LoadAssets();
            }
            else
            {
                UnloadAssets();
            }

            //Debug.WriteLine(Menus.Count.ToString())

            if (IsAnyMenuOpen())
            {
                DisableControls();
            }

            if (EnableManualGCs)
            {
                // once a minute
                if (GetGameTimer() - timer > 60000)
                {
                    GC.Collect();
                    timer = GetGameTimer();
                }
            }

            var menu = GetCurrentMenu();
            if (menu != null)
            {
                if (menu.Visible)
                    await menu.Draw();
            }
        }
    }
}
