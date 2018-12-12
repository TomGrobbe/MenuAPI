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

        /// <summary>
        /// Constructor
        /// </summary>
        public Main()
        {
            var menu = new Menu("Vespura", "Main Menu") { Visible = true, CounterPreText = "test" };

            menu.AddMenuItem(new MenuItem("5", "This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. ") { Selected = false });
            menu.AddMenuItem(new MenuItem("5", "This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. ") { Selected = false });
            menu.AddMenuItem(new MenuItem("4", "This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. ") { Selected = false });
            menu.AddMenuItem(new MenuItem("3", "This is a test item with a long description test. This is a test item with a long description test. This is a test item with a long description test. ") { Selected = false });
            menu.AddMenuItem(new MenuItem("2", "This is a test item with a long description test. This is a test item with a long description test. ") { Selected = false });
            menu.AddMenuItem(new MenuItem("1", "This is a test item with a long description test.") { Selected = false });

            Menus.Add(menu);

            Tick += DrawMenus;
        }

        /// <summary>
        /// Loads the texture dict for the common menu sprites.
        /// </summary>
        /// <returns></returns>
        private async Task LoadTextures()
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
        private void UnloadTextures()
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
                UnloadTextures();
            }
            else
            {
                await LoadTextures();
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
