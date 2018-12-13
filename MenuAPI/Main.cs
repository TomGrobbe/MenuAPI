using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace MenuAPI
{
    public class Main : BaseScript
    {
        public static List<Menu> Menus = new List<Menu>();
        public const string _texture_dict = "commonmenu";
        public const string _header_texture = "interaction_bgd";

        public static float ScreenWidth { get { int width = 0, height = 0; GetScreenActiveResolution(ref width, ref height); return (float)width; } }
        public static float ScreenHeight { get { int width = 0, height = 0; GetScreenActiveResolution(ref width, ref height); return (float)height; } }
        public static bool DisableMenuButtons { get; set; } = false;
        public static bool AreMenuButtonsEnabled => Menus.Any((m) => m.Visible) && !Game.IsPaused && CitizenFX.Core.UI.Screen.Fading.IsFadedIn && !IsPlayerSwitchInProgress() && !DisableMenuButtons;

        /// <summary>
        /// Constructor
        /// </summary>
        public Main()
        {
            Menu menu = new Menu("Vespura", "Main Menu") { Visible = true/*, CounterPreText = "items "*/ };

            //menu.AddMenuItem(new MenuItem("Test Item 5", "This is a test item with a long description test.This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. ") { Selected = false });
            menu.AddMenuItem(new MenuItem("Test Item #1", "Description (1). abc abc abc abc"));
            menu.AddMenuItem(new MenuItem("Test Item #2", "Description (2). abc abc abc abc abc abc abc abc abc abc abc abc"));
            menu.AddMenuItem(new MenuItem("Test Item #3", "Description (3). abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc"));
            menu.AddMenuItem(new MenuItem("Test Item #4", "Description (4). abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc"));
            menu.AddMenuItem(new MenuItem("Test Item #5", "Description (5). abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc abc"));
            menu.AddMenuItem(new MenuItem("Test Item #6") { LeftIcon = MenuItem.Icon.LOCK });
            menu.AddMenuItem(new MenuItem("Test Item #7") { LeftIcon = MenuItem.Icon.LOCK, Label = "right text" });
            menu.AddMenuItem(new MenuItem("Test Item #8") { LeftIcon = MenuItem.Icon.LOCK, Label = "right text →→→" });
            menu.AddMenuItem(new MenuItem("Test Item #9") { LeftIcon = MenuItem.Icon.LOCK, Label = "→→→" });
            menu.AddMenuItem(new MenuItem("Test Item #10") { Label = "right text" });
            menu.AddMenuItem(new MenuItem("Test Item #11") { Label = "right text →→→" });
            menu.AddMenuItem(new MenuItem("Test Item #12") { Label = "→→→" });
            menu.AddMenuItem(new MenuItem("Test Item #13"));
            menu.AddMenuItem(new MenuItem("Test Item #14"));
            menu.AddMenuItem(new MenuItem("Test Item #15"));
            menu.AddMenuItem(new MenuItem("Test Item #16"));


            Menus.Add(menu);

            Tick += DrawMenus;
        }

        /// <summary>
        /// Loads the texture dict for the common menu sprites.
        /// </summary>
        /// <returns></returns>
        private async Task LoadAssets()
        {
            if (!HasStreamedTextureDictLoaded(_texture_dict))
            {
                RequestStreamedTextureDict(_texture_dict, false);
                while (!HasStreamedTextureDictLoaded(_texture_dict))
                {
                    Debug.WriteLine("-loading-");
                    await Delay(0);
                }
            }
        }

        /// <summary>
        /// Unloads the texture dict for the common menu sprites.
        /// </summary>
        private void UnloadAssets()
        {
            if (HasStreamedTextureDictLoaded(_texture_dict))
            {
                SetStreamedTextureDictAsNoLongerNeeded(_texture_dict);
            }
        }

        /// <summary>
        /// Draws all the menus that are visible on the screen.
        /// </summary>
        /// <returns></returns>
        private async Task DrawMenus()
        {
            if (Menus.Count == 0)
            {
                UnloadAssets();
            }
            else
            {
                await LoadAssets();
            }

            if (Menus.Any((m) => m.Visible))
            {
                Game.DisableControlThisFrame(0, Control.FrontendUp);
                Game.DisableControlThisFrame(0, Control.FrontendDown);
                Game.DisableControlThisFrame(0, Control.FrontendAccept);
                if (Game.IsDisabledControlJustPressed(0, Control.FrontendAccept) && AreMenuButtonsEnabled)
                {
                    foreach (Menu m in Menus)
                    {
                        m.LeftAligned = !m.LeftAligned;
                        PlaySoundFrontend(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
                if (Game.IsDisabledControlJustPressed(0, Control.FrontendUp) && AreMenuButtonsEnabled)
                {
                    if (Menus.Any((m) => m.Visible))
                    {

                        Menus.ForEach((m) =>
                        {
                            m.GoUp();
                        });
                        PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }
                if (Game.IsDisabledControlJustPressed(0, Control.FrontendDown) && AreMenuButtonsEnabled)
                {
                    if (Menus.Any((m) => m.Visible))
                    {
                        Menus.ForEach((m) =>
                        {
                            m.GoDown();
                        });
                        PlaySoundFrontend(-1, "NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET", false);
                    }
                }

                foreach (Menu menu in Menus)
                {
                    if (menu.Visible)
                    {
                        await menu.Draw();
                    }
                }
            }



        }
    }
}
