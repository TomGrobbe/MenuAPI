using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using static CitizenFX.Core.Native.Function;
using static CitizenFX.Core.Native.Hash;

namespace MenuAPI
{
    /// <summary>
    /// A single menu with a banner, a subtitle, a list of <see cref="MenuItem"/>s and its own instructional buttons.
    /// Register it with <see cref="MenuController.AddMenu(Menu)"/> before opening it.
    /// </summary>
    public class Menu
    {
        #region Setting up events

        #region delegates
        /// <summary>
        /// Triggered when a <see cref="MenuItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this event occurred.</param>
        /// <param name="menuItem">The <see cref="MenuItem"/> that was selected.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuItem"/>.</param>
        public delegate void ItemSelectEvent(Menu menu, MenuItem menuItem, int itemIndex);

        /// <summary>
        /// Triggered when a <see cref="MenuCheckboxItem"/> was toggled.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this event occurred.</param>
        /// <param name="menuItem">The <see cref="MenuCheckboxItem"/> that was toggled.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuCheckboxItem"/>.</param>
        /// <param name="newCheckedState">The new <see cref="MenuCheckboxItem.Checked"/> state of this <see cref="MenuCheckboxItem"/>.</param>
        public delegate void CheckboxItemChangeEvent(Menu menu, MenuCheckboxItem menuItem, int itemIndex, bool newCheckedState);

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnListItemSelect"/> event occurred.</param>
        /// <param name="listItem">The <see cref="MenuListItem"/> that was selected.</param>
        /// <param name="selectedIndex">The <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of the <see cref="MenuListItem"/> in the <see cref="Menu"/>.</param>
        public delegate void ListItemSelectedEvent(Menu menu, MenuListItem listItem, int selectedIndex, int itemIndex);

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/>'s index was changed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnListIndexChange"/> event occurred.</param>
        /// <param name="listItem">The <see cref="MenuListItem"/> that was changed.</param>
        /// <param name="oldSelectionIndex">The old <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="newSelectionIndex">The new <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of the <see cref="MenuListItem"/> in the <see cref="Menu"/>.</param>
        public delegate void ListItemIndexChangedEvent(Menu menu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex);

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is closed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> that was closed.</param>
        public delegate void MenuClosedEvent(Menu menu);

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is opened.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> that has been opened.</param>
        public delegate void MenuOpenedEvent(Menu menu);

        /// <summary>
        /// Triggered when the <see cref="CurrentIndex"/> changes.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnIndexChange"/></param> event occurred.
        /// <param name="oldItem">The old <see cref="MenuItem"/> that was previously selected.</param>
        /// <param name="newItem">The new <see cref="MenuItem"/> that is now selected.</param>
        /// <param name="oldIndex">The old <see cref="MenuItem.Index"/> of this item.</param>
        /// <param name="newIndex">The new <see cref="MenuItem.Index"/> of this item.</param>
        public delegate void IndexChangedEvent(Menu menu, MenuItem oldItem, MenuItem newItem, int oldIndex, int newIndex);


        /// <summary>
        /// Triggered when a <see cref="MenuDynamicListItem"/>'s value was changed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemCurrentItemChange"/> event occurred.</param>
        /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was changed.</param>
        /// <param name="oldValue">The old <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/>.</param>
        /// <param name="newValue">The new <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/>.</param>
        public delegate void DynamicListItemCurrentItemChangedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string oldValue, string newValue);

        /// <summary>
        /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemSelect"/> event occurred.</param>
        /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was selected.</param>
        /// <param name="currentItem">The <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/> in the <see cref="Menu"/>.</param>
        public delegate void DynamicListItemSelectedEvent(Menu menu, MenuDynamicListItem dynamicListItem, string currentItem);
        #endregion

        #region events
        /// <summary>
        /// Triggered when a <see cref="MenuItem"/> is selected.
        /// Parameters: <see cref="Menu"/> parentMenu, <see cref="MenuItem"/> menuItem, <see cref="int"/> itemIndex.
        /// </summary>
        public event ItemSelectEvent OnItemSelect;

        /// <summary>
        /// Triggered when a <see cref="MenuCheckboxItem"/> was toggled.
        /// Parameters: <see cref="Menu"/> parentMenu, <see cref="MenuCheckboxItem"/> menuItem, <see cref="int"/> itemIndex, <see cref="bool"/> newCheckedState.
        /// </summary>
        public event CheckboxItemChangeEvent OnCheckboxChange;

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/> is selected.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> listItem, <see cref="MenuListItem.ListIndex"/> selectedIndex, <see cref="int"/> itemIndex.
        /// </summary>
        public event ListItemSelectedEvent OnListItemSelect;

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/>'s index was changed.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> listItem, <see cref="MenuListItem.ListIndex"/> oldSelectionIndex, <see cref="int"/> newSelectionIndex, <see cref="int"/> itemIndex.
        /// </summary>
        public event ListItemIndexChangedEvent OnListIndexChange;

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is closed.
        /// Parameters: <see cref="Menu"/> closedMenu.
        /// </summary>
        public event MenuClosedEvent OnMenuClose;

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is opened.
        /// Parameters: <see cref="Menu"/> openedMenu.
        /// </summary>
        public event MenuOpenedEvent OnMenuOpen;

        /// <summary>
        /// Triggered when the <see cref="CurrentIndex"/> changes.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuItem"/> oldSelectedItem, <see cref="MenuItem"/> newSelectedItem, <see cref="int"/> oldIndex, <see cref="int"/> newIndex.
        /// </summary>
        public event IndexChangedEvent OnIndexChange;

        /// <summary>
        /// Triggered when a <see cref="MenuDynamicListItem"/>'s value was changed.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuListItem"/> dynamicListItem, <see cref="MenuDynamicListItem.CurrentItem"/> oldValue, <see cref="MenuDynamicListItem.CurrentItem"/> newValue.
        /// </summary>
        public event DynamicListItemCurrentItemChangedEvent OnDynamicListItemCurrentItemChange;

        /// <summary>
        /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
        /// Parameters: <see cref="Menu"/> menu, <see cref="MenuDynamicListItem"/> dynamicListItem, <see cref="MenuDynamicListItem.CurrentItem"/> itemValue.
        /// </summary>
        public event DynamicListItemSelectedEvent OnDynamicListItemSelect;
        #endregion

        #region virtual voids
        /// <summary>
        /// Triggered when a <see cref="MenuItem"/> is selected.
        /// </summary>
        /// <param name="menuItem">The <see cref="MenuItem"/> that was selected.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuItem"/>.</param>
        internal virtual void ItemSelectedEvent(MenuItem menuItem, int itemIndex)
        {
            OnItemSelect?.Invoke(this, menuItem, itemIndex);
        }

        /// <summary>
        /// Triggered when a <see cref="MenuCheckboxItem"/> was toggled.
        /// </summary>
        /// <param name="menuItem">The <see cref="MenuCheckboxItem"/> that was toggled.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of this <see cref="MenuCheckboxItem"/>.</param>
        /// <param name="_checked">The new <see cref="MenuCheckboxItem.Checked"/> state of this <see cref="MenuCheckboxItem"/>.</param>
        internal virtual void CheckboxChangedEvent(MenuCheckboxItem menuItem, int itemIndex, bool _checked)
        {
            OnCheckboxChange?.Invoke(this, menuItem, itemIndex, _checked);
        }

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnListItemSelect"/> event occurred.</param>
        /// <param name="listItem">The <see cref="MenuListItem"/> that was selected.</param>
        /// <param name="selectedIndex">The <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of the <see cref="MenuListItem"/> in the <see cref="Menu"/>.</param>
        internal virtual void ListItemSelectEvent(Menu menu, MenuListItem listItem, int selectedIndex, int itemIndex)
        {
            OnListItemSelect?.Invoke(menu, listItem, selectedIndex, itemIndex);
        }

        /// <summary>
        /// Triggered when a <see cref="MenuListItem"/>'s index was changed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnListIndexChange"/> event occurred.</param>
        /// <param name="listItem">The <see cref="MenuListItem"/> that was changed.</param>
        /// <param name="oldSelectionIndex">The old <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="newSelectionIndex">The new <see cref="MenuListItem.ListIndex"/> of the <see cref="MenuListItem"/>.</param>
        /// <param name="itemIndex">The <see cref="MenuItem.Index"/> of the <see cref="MenuListItem"/> in the <see cref="Menu"/>.</param>
        internal virtual void ListItemIndexChangeEvent(Menu menu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)
        {
            OnListIndexChange?.Invoke(menu, listItem, oldSelectionIndex, newSelectionIndex, itemIndex);
        }

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is closed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> that was closed.</param>
        internal virtual void MenuCloseEvent(Menu menu)
        {
            OnMenuClose?.Invoke(menu);
        }

        /// <summary>
        /// Triggered when a <see cref="Menu"/> is opened.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> that has been opened.</param>
        internal virtual void MenuOpenEvent(Menu menu)
        {
            OnMenuOpen?.Invoke(menu);
        }

        /// <summary>
        /// Triggered when the <see cref="CurrentIndex"/> changes.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnIndexChange"/> event occurred.</param>
        /// <param name="oldItem">The old <see cref="MenuItem"/> that was previously selected.</param>
        /// <param name="newItem">The new <see cref="MenuItem"/> that is now selected.</param>
        /// <param name="oldIndex">The old <see cref="MenuItem.Index"/> of this item.</param>
        /// <param name="newIndex">The new <see cref="MenuItem.Index"/> of this item.</param>
        internal virtual void IndexChangeEvent(Menu menu, MenuItem oldItem, MenuItem newItem, int oldIndex, int newIndex)
        {
            OnIndexChange?.Invoke(menu, oldItem, newItem, oldIndex, newIndex);
        }


        /// <summary>
        /// Triggered when a <see cref="MenuDynamicListItem"/>'s value was changed.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemCurrentItemChange"/> event occurred.</param>
        /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was changed.</param>
        /// <param name="oldValue">The old <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/>.</param>
        /// <param name="newValue">The new <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/>.</param>
        internal virtual void DynamicListItemCurrentItemChanged(Menu menu, MenuDynamicListItem dynamicListItem, string oldValue, string newValue)
        {
            OnDynamicListItemCurrentItemChange?.Invoke(menu, dynamicListItem, oldValue, newValue);
        }

        /// <summary>
        /// Triggered when a <see cref="MenuDynamicListItem"/> is selected.
        /// </summary>
        /// <param name="menu">The <see cref="Menu"/> in which this <see cref="OnDynamicListItemSelect"/> event occurred.</param>
        /// <param name="dynamicListItem">The <see cref="MenuDynamicListItem"/> that was selected.</param>
        /// <param name="currentItem">The <see cref="MenuDynamicListItem.CurrentItem"/> of the <see cref="MenuDynamicListItem"/> in the <see cref="Menu"/>.</param>
        internal virtual void DynamicListItemSelectEvent(Menu menu, MenuDynamicListItem dynamicListItem, string currentItem)
        {
            OnDynamicListItemSelect?.Invoke(menu, dynamicListItem, currentItem);
        }
        #endregion

        #endregion

        #region constants or readonlys
        /// <summary>
        /// The width of a menu, in pixels on a 1080p screen.
        /// </summary>
        public const float Width = 300F;
        #endregion

        #region private variables
        private static KeyValuePair<float, float> headerSize = new KeyValuePair<float, float>(Width, 110f);

        private int index = 0;

        private bool visible = false;

        /// <summary>
        /// The index of the first item that is visible on screen.
        /// </summary>
        public int ViewIndexOffset { get; private set; } = 0;

        private List<MenuItem> VisibleMenuItems
        {
            get
            {
                // Create a duplicate list, just in case the original list is modified while we're looping through it.
                if (filterActive)
                {
                    var items = FilterItems.ToList().GetRange(ViewIndexOffset, Math.Min(MaxItemsOnScreen, Size - ViewIndexOffset));
                    return items;
                }
                else
                {
                    var items = GetMenuItems().ToList().GetRange(ViewIndexOffset, Math.Min(MaxItemsOnScreen, Size - ViewIndexOffset));
                    return items;
                }
            }
        }

        private List<MenuItem> FilterItems { get; set; } = new List<MenuItem>();
        private List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        #endregion

        #region Public Variables
        /// <summary>
        /// The text on the menu banner. When this is null or empty, no banner is drawn.
        /// </summary>
        public string MenuTitle { get; set; }

        /// <summary>
        /// The text in the subtitle bar below the banner. Null or empty hides the subtitle text.
        /// </summary>
        public string MenuSubtitle { get; set; }

        /// <summary>
        /// A custom banner image, with the texture dictionary as the key and the texture name as the value.
        /// The RedM build does not read this yet, the default banner is always drawn.
        /// </summary>
        public KeyValuePair<string, string> HeaderTexture { get; set; } = new KeyValuePair<string, string>();

        /// <summary>
        /// When true, this menu stays open while <see cref="MenuController.DontOpenAnyMenu"/> is true.
        /// </summary>
        public bool IgnoreDontOpenMenus { get; set; } = false;

        /// <summary>
        /// How many items are visible on screen at a time. Defaults to 10, use <see cref="SetMaxItemsOnScreen(int)"/> to change it.
        /// </summary>
        public int MaxItemsOnScreen { get; internal set; } = 10;

        /// <summary>
        /// The number of items in this menu. While a filter is active, only the items that passed the filter are counted.
        /// </summary>
        public int Size => filterActive ? FilterItems.Count : MenuItems.Count;

        /// <summary>
        /// Whether this menu is currently drawn. Prefer <see cref="OpenMenu"/> and <see cref="CloseMenu"/>, which also trigger the open and close events and toggle the UI prompts.
        /// </summary>
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                if (value)
                {
                    MenuController.VisibleMenus.Add(this);
                }
                else
                {
                    MenuController.VisibleMenus.Remove(this);
                }
                visible = value;
            }
        }

        /// <summary>
        /// The x and y position of the top left corner of the menu, in pixels on a 1080p screen.
        /// </summary>
        public KeyValuePair<float, float> Position { get; private set; } = new KeyValuePair<float, float>(0f, 20f);

        /// <summary>
        /// The y offset in pixels of the first menu item, measured from the top of the menu. Recalculated every frame while the menu is drawn.
        /// </summary>
        public float MenuItemsYOffset { get; private set; } = 0f;

        /// <summary>
        /// Text meant to go in front of the item counter. The RedM build does not draw it yet.
        /// </summary>
        public string CounterPreText { get; set; }

        /// <summary>
        /// The menu that the back control returns to, or null if this menu has no parent. Set by <see cref="MenuController.AddSubmenu(Menu, Menu)"/>.
        /// </summary>
        public Menu ParentMenu { get; internal set; } = null;

        /// <summary>
        /// The index of the highlighted item. Use <see cref="RefreshIndex(int)"/> to change it.
        /// </summary>
        public int CurrentIndex { get { return index; } internal set { index = MathUtil.Clamp(value, 0, Math.Max(0, Size - 1)); } }

        /// <summary>
        /// Whether the <see cref="InstructionalButtons"/> are shown as UI prompts while this menu is open.
        /// </summary>
        public bool EnableInstructionalButtons { get; set; } = true;

        /// <summary>
        /// Should contain 4 floats.
        /// </summary>
        public float[] WeaponStats { get; private set; } = new float[4] { 0f, 0f, 0f, 0f };
        /// <summary>
        /// Should contain 4 floats.
        /// </summary>
        public float[] WeaponComponentStats { get; private set; } = new float[4] { 0f, 0f, 0f, 0f };
        /// <summary>
        /// Should contain 4 floats.
        /// </summary>
        public float[] VehicleStats { get; private set; } = new float[4] { 0f, 0f, 0f, 0f };
        /// <summary>
        /// Should contain 4 floats.
        /// </summary>
        public float[] VehicleUpgradeStats { get; private set; } = new float[4] { 0f, 0f, 0f, 0f };

        /// <summary>
        /// Has no effect in RedM, the weapon stats panel is never drawn.
        /// </summary>
        public bool ShowWeaponStatsPanel { get; set; } = false;
        /// <summary>
        /// Has no effect in RedM, the vehicle stats panel is never drawn.
        /// </summary>
        public bool ShowVehicleStatsPanel { get; set; } = false;

        private readonly string[] weaponStatNames = new string[4] { "PM_DAMAGE", "PM_FIRERATE", "PM_ACCURACY", "PM_RANGE" };
        private readonly string[] vehicleStatNames = new string[4] { "FMMC_VEHST_0", "FMMC_VEHST_1", "FMMC_VEHST_2", "FMMC_VEHST_3" };

        private bool filterActive = false;


        /// <summary>
        /// The UI prompts shown while this menu is open. Defaults to select, back and up / down.
        /// Call <see cref="InstructionalButton.Dispose"/> on a prompt you remove, or it can get stuck on screen.
        /// </summary>
        public List<InstructionalButton> InstructionalButtons = new List<InstructionalButton>() {
            new InstructionalButton(
                new Control[1]
                {
                    Control.FrontendAccept
                },
                GetLabelText("INPUT_FRONTEND_SELECT")
            ),
            new InstructionalButton(
                new Control[1]
                {
                    Control.FrontendCancel
                },
                "Back"
            ),
            new InstructionalButton(
                new Control[2]
                {
                    Control.FrontendUp,
                    Control.FrontendDown
                },
                "Up / Down"
            )
        };

        /// <summary>
        /// A native UI prompt that shows one or more controls with a line of text next to them.
        /// </summary>
        public class InstructionalButton
        {
            //private long text;
            private string textString;
            private Control[] controls;
            private int promptHandle;

            /// <summary>
            /// Creates a new prompt for one or more controls.
            /// </summary>
            /// <param name="controls">The controls this prompt listens for. Pass more than one to show a combination.</param>
            /// <param name="text">The text shown next to the prompt.</param>
            public InstructionalButton(Control[] controls, string text)
            {
                this.controls = controls;
                textString = text;
                promptHandle = 0;
            }

            /// <summary>
            /// Prepare the instructional button before it is displayed.
            /// </summary>
            public void Prepare()
            {
                if (IsPrepared)
                {
                    return;
                }

                // 0x04F97DE45A519419 / UipromptRegisterBegin (new name, not yet in API)
                promptHandle = PromptRegisterBegin();

                // 0xFA925AC00EB830B9 / CreateVarString (Has incorrect parameter types in API)
                long _text = Call<long>((CitizenFX.Core.Native.Hash)0xFA925AC00EB830B9, 10, "LITERAL_STRING", textString);

                // 0x5DD02A8318420DD7 / UipromptSetText (Has incorrect parameter types in API)
                Call<long>((CitizenFX.Core.Native.Hash)0x5DD02A8318420DD7, promptHandle, _text);
                foreach (var c in controls)
                {
                    // 0xB5352B7494A08258 / UipromptSetControlAction (Has incorrect parameter types in API)
                    Call((CitizenFX.Core.Native.Hash)0xB5352B7494A08258, this.promptHandle, c);
                }
                PromptRegisterEnd(promptHandle); // UipromptRegisterEnd (new name, not yet in API)
                SetEnabled(false, false);
            }

            /// <summary>
            /// Check if it is ready to be displayed.
            /// </summary>
            /// <returns></returns>
            public bool IsPrepared => PromptIsValid(promptHandle);

            /// <summary>
            /// Enables or disables the prompt on screen. You must prepare the prompt first using <see cref="InstructionalButton.Prepare"/>.
            /// </summary>
            /// <param name="visible"></param>
            /// <param name="enabled"></param>
            public void SetEnabled(bool visible, bool enabled)
            {
                PromptSetVisible(promptHandle, visible ? 1 : 0);
                PromptSetEnabled(promptHandle, enabled ? 1 : 0);
            }

            /// <summary>
            /// Disposes the prompt. Requires you to call <see cref="InstructionalButton.Prepare"/> again before you can use it again.
            /// </summary>
            public void Dispose()
            {
                if (IsPrepared)
                {
                    SetEnabled(false, false);
                    PromptDelete(promptHandle);
                }
                promptHandle = 0;
            }

            //public long GetTextHandle() => text;
            /// <summary>
            /// Returns the text of this prompt.
            /// </summary>
            public string GetTextString() => textString;
            /// <summary>
            /// Returns the controls this prompt listens for.
            /// </summary>
            public Control[] GetControls() => controls;
        }

        /// <summary>
        /// How a <see cref="ButtonPressHandler"/> checks its control.
        /// </summary>
        public enum ControlPressCheckType
        {
            /// <summary>
            /// Triggers once, on the frame the control is released.
            /// </summary>
            JUST_RELEASED,
            /// <summary>
            /// Triggers once, on the frame the control is pressed down.
            /// </summary>
            JUST_PRESSED,
            /// <summary>
            /// Triggers every frame while the control is not held down.
            /// </summary>
            RELEASED,
            /// <summary>
            /// Triggers every frame while the control is held down.
            /// </summary>
            PRESSED
        }

        /// <summary>
        /// Runs your own function when a control is pressed while this menu is open. Add it to <see cref="ButtonPressHandlers"/>.
        /// </summary>
        public struct ButtonPressHandler
        {
            // The control to listen for.
            internal Control control;
            // The type. 
            internal ControlPressCheckType pressType;
            // The function to call when the control is triggered.
            internal Action<Menu, Control> function;
            // Whether or not the control needs to be disabled if the menu is visible.
            internal bool disableControl;

            /// <summary>
            /// Creates a new button press handler.
            /// </summary>
            /// <param name="control">The control to listen for.</param>
            /// <param name="pressType">How the control is checked.</param>
            /// <param name="function">The function to run. It receives the menu and the control that triggered it.</param>
            /// <param name="disableControl">Whether the control is disabled for the game while this menu is open, so only your function reacts to it.</param>
            public ButtonPressHandler(Control control, ControlPressCheckType pressType, Action<Menu, Control> function, bool disableControl)
            {
                this.control = control;
                this.pressType = pressType;
                this.function = function;
                this.disableControl = disableControl;
            }
        }
        /// <summary>
        /// Custom control handlers that run while this menu is open and <see cref="MenuController.DisableMenuButtons"/> is false.
        /// </summary>
        public List<ButtonPressHandler> ButtonPressHandlers = new List<ButtonPressHandler>();

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="Menu"/>.
        /// </summary>
        /// <param name="name"></param>
        public Menu(string name) : this(name, null) { }

        /// <summary>
        /// Creates a new <see cref="Menu"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subtitle"></param>
        public Menu(string name, string subtitle)
        {
            MenuTitle = name;
            MenuSubtitle = subtitle;
        }
        #endregion

        #region Public functions
        /// <summary>
        /// Sets the max amount of visible items on screen at a time.
        /// Min = 3, max = 10.
        /// </summary>
        /// <param name="max">A value between 3 and 10 (inclusive).</param>
        public void SetMaxItemsOnScreen(int max)
        {
            MaxItemsOnScreen = MathUtil.Clamp(max, 3, 10);
        }

        /// <summary>
        /// Resets the index to 0
        /// </summary>
        public void RefreshIndex() => RefreshIndex(0, 0);
        /// <summary>
        /// Highlights the item at <paramref name="index"/> and scrolls the menu so that item is visible.
        /// </summary>
        /// <param name="index">The index of the item to highlight.</param>
        public void RefreshIndex(int index) => RefreshIndex(index, index > MaxItemsOnScreen ? index - MaxItemsOnScreen : 0);
        /// <summary>
        /// Highlights the item at <paramref name="index"/> and makes the item at <paramref name="viewOffset"/> the first one on screen.
        /// </summary>
        /// <param name="index">The index of the item to highlight.</param>
        /// <param name="viewOffset">The index of the item drawn at the top of the menu.</param>
        public void RefreshIndex(int index, int viewOffset) { CurrentIndex = index; ViewIndexOffset = viewOffset; }

        /// <summary>
        /// Returns the menu items in this menu.
        /// </summary>
        /// <returns></returns>
        public List<MenuItem> GetMenuItems()
        {
            return filterActive ? FilterItems.ToList() : MenuItems.ToList();
        }

        /// <summary>
        /// Gets the currently selected (highlighted) menu item.
        /// </summary>
        /// <returns>Retuns the currently selected menu item. Or null if there are no menu items, or the current menu index is out of range.</returns>
        public MenuItem GetCurrentMenuItem()
        {
            return GetMenuItems().ElementAtOrDefault(CurrentIndex);
        }

        /// <summary>
        /// Removes all menu items.
        /// </summary>
        public void ClearMenuItems()
        {
            CurrentIndex = 0;
            ViewIndexOffset = 0;
            MenuItems.Clear();
            FilterItems.Clear();
        }

        /// <summary>
        /// Removes all menu items.
        /// </summary>
        public void ClearMenuItems(bool dontResetIndex)
        {
            if (!dontResetIndex)
            {
                CurrentIndex = 0;
                ViewIndexOffset = 0;
            }
            MenuItems.Clear();
            FilterItems.Clear();
        }

        /// <summary>
        /// Adds a <see cref="MenuItem"/> to this <see cref="Menu"/>.
        /// </summary>
        /// <param name="item"></param>
        public void AddMenuItem(MenuItem item)
        {
            MenuItems.Add(item);
            item.PositionOnScreen = item.Index;
            item.ParentMenu = this;
        }

        /// <summary>
        /// Removes the item at that index.
        /// </summary>
        /// <param name="itemIndex"></param>
        public void RemoveMenuItem(int itemIndex)
        {
            if (CurrentIndex >= itemIndex)
            {
                if (Size > CurrentIndex)
                {
                    CurrentIndex--;
                }
                else
                {
                    CurrentIndex = 0;
                }
            }
            if (itemIndex < Size && itemIndex > -1)
            {
                RemoveMenuItem(MenuItems[itemIndex]);
            }
        }

        /// <summary>
        /// Removes the specified <see cref="MenuItem"/> from this <see cref="Menu"/>.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveMenuItem(MenuItem item)
        {
            if (!MenuItems.Contains(item))
            {
                return;
            }
            if (CurrentIndex >= item.Index)
            {
                if (Size > CurrentIndex)
                {
                    CurrentIndex--;
                }
                else
                {
                    CurrentIndex = 0;
                }
            }
            MenuItems.Remove(item);
        }

        /// <summary>
        /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
        /// </summary>
        /// <param name="index"></param>
        public void SelectItem(int index)
        {
            if (!filterActive)
            {
                if (index > -1 && MenuItems.Count - 1 >= index)
                {
                    SelectItem(MenuItems[index]);
                }
            }
            else
            {
                if (index > -1 && FilterItems.Count - 1 >= index)
                {
                    SelectItem(FilterItems[index]);
                }
            }
        }

        /// <summary>
        /// Triggers the <see cref="ItemSelectedEvent(MenuItem, int)"/> event function.
        /// </summary>
        /// <param name="item"></param>
        public void SelectItem(MenuItem item)
        {
            if (item == null)
            {
                return;
            }
            if (!item.Enabled)
            {
                // Has invalid parameter types in API
                Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_ERROR", "HUD_SHOP_SOUNDSET", 1);
            }
            else
            {
                // Has invalid parameter types in API.
                Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "SELECT", "HUD_SHOP_SOUNDSET", 1);
                item.Select();
                if (MenuController.MenuButtons.ContainsKey(item))
                {
                    // this updates the parent menu.
                    MenuController.AddSubmenu(MenuController.GetCurrentMenu(), MenuController.MenuButtons[item]);
                    MenuController.GetCurrentMenu().CloseMenu();
                    MenuController.MenuButtons[item].OpenMenu();
                }
            }
        }

        /// <summary>
        /// Returns to the parent menu. If there's no parent menu, then the current menu just closes.
        /// </summary>
        public void GoBack()
        {
            // Has invalid parameter types in API
            Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "Back", "HUD_SHOP_SOUNDSET", 1);
            CloseMenu();
            if (ParentMenu != null)
            {
                ParentMenu.OpenMenu();
            }
        }

        /// <summary>
        /// Closes the menu. Also triggers the <see cref="OnMenuClose"/> event.
        /// </summary>
        public void CloseMenu()
        {
            Visible = false;
            MenuCloseEvent(this);
            foreach (var v in InstructionalButtons)
            {
                v.SetEnabled(false, false);
                //v.Dispose();
            }
        }

        /// <summary>
        /// Opens the menu and triggers the <see cref="OnMenuOpen"/> event.
        /// </summary>
        public void OpenMenu()
        {
            Visible = true;
            MenuOpenEvent(this);
            if (EnableInstructionalButtons)
            {
                foreach (var v in InstructionalButtons)
                {
                    v.Prepare();
                    v.SetEnabled(true, true);
                }
            }
        }

        /// <summary>
        /// Goes up one menu item if possible.
        /// </summary>
        public void GoUp()
        {
            if (!Visible || Size < 2)
            {
                return;
            }
            MenuItem oldItem;

            if (filterActive)
            {
                oldItem = FilterItems[CurrentIndex];
            }
            else
            {
                oldItem = MenuItems[CurrentIndex];
            }

            if (CurrentIndex == 0)
            {
                CurrentIndex = Size - 1;
            }
            else
            {
                CurrentIndex--;
            }

            var currItem = GetCurrentMenuItem();

            if (currItem == null || !VisibleMenuItems.Contains(currItem))
            {
                ViewIndexOffset--;
                if (ViewIndexOffset < 0)
                {
                    ViewIndexOffset = Math.Max(Size - MaxItemsOnScreen, 0);
                }
            }

            IndexChangeEvent(this, oldItem, currItem, oldItem.Index, CurrentIndex);
            // Has invalid parameter types in API
            Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_UP", "HUD_SHOP_SOUNDSET", 1);
        }

        /// <summary>
        /// Goes down one menu item if possible.
        /// </summary>
        public void GoDown()
        {
            if (!Visible || Size < 2)
            {
                return;
            }

            MenuItem oldItem;

            if (filterActive)
            {
                oldItem = FilterItems[CurrentIndex];
            }
            else
            {
                oldItem = MenuItems[CurrentIndex];
            }

            if (CurrentIndex > 0 && CurrentIndex >= Size - 1)
            {
                CurrentIndex = 0;
            }
            else
            {
                CurrentIndex++;
            }

            var currItem = GetCurrentMenuItem();
            if (currItem == null || !VisibleMenuItems.Contains(currItem))
            {
                ViewIndexOffset++;
                if (CurrentIndex == 0)
                {
                    ViewIndexOffset = 0;
                }
            }
            IndexChangeEvent(this, oldItem, currItem, oldItem.Index, CurrentIndex);
            // Has invalid parameter types in API.
            Call((CitizenFX.Core.Native.Hash)0xCE5D0FFE83939AF1, -1, "NAV_DOWN", "HUD_SHOP_SOUNDSET", 1);
        }

        /// <summary>
        /// If the item is a <see cref="MenuListItem"/> or a <see cref="MenuDynamicListItem"/> then it'll go left if possible.
        /// </summary>
        public void GoLeft()
        {
            if (!MenuController.AreMenuButtonsEnabled)
            {
                return;
            }
            var item = GetCurrentMenuItem();
            if (item != null)
            {
                item.GoLeft();
            }
            // If the item is not any of the above, return to parent menu.
            else if (MenuController.NavigateMenuUsingArrows && !MenuController.DisableBackButton && !(MenuController.PreventExitingMenu && ParentMenu == null))
            {
                GoBack();
            }
        }

        /// <summary>
        /// If the item is a <see cref="MenuListItem"/> or a <see cref="MenuDynamicListItem"/> then it'll go right if possible.
        /// </summary>
        public void GoRight()
        {
            if (!MenuController.AreMenuButtonsEnabled)
            {
                return;
            }
            var item = GetCurrentMenuItem();
            if (item != null)
            {
                item.GoRight();
            }
        }

        /// <summary>
        /// Sorts the menu items using the provided compare function.
        /// </summary>
        /// <param name="compare"></param>
        public void SortMenuItems(Comparison<MenuItem> compare)
        {
            if (filterActive)
            {
                filterActive = false;
                FilterItems.Clear();
            }
            MenuItems.Sort(compare);
        }

        /// <summary>
        /// Filters menu items using the provided filter function.
        /// </summary>
        /// <param name="predicate"></param>
        public void FilterMenuItems(Func<MenuItem, bool> predicate)
        {
            if (filterActive)
            {
                ResetFilter();
            }
            RefreshIndex(0, 0);
            ViewIndexOffset = 0;
            FilterItems = MenuItems.Where(i => predicate.Invoke(i)).ToList();
            filterActive = true;
        }

        /// <summary>
        /// Clears the current menu items filter for this menu.
        /// </summary>
        public void ResetFilter()
        {
            RefreshIndex(0, 0);
            filterActive = false;
            FilterItems.Clear();
        }

        #endregion

        #region internal/private task functions
        /// <summary>
        /// Processes any custom button press handlers for this menu.
        /// </summary>
        private void ProcessButtonPressHandlers()
        {
            if (ButtonPressHandlers.Any())
            {
                if (!MenuController.DisableMenuButtons)
                {
                    foreach (ButtonPressHandler handler in ButtonPressHandlers)
                    {
                        if (handler.disableControl)
                        {
                            DisableControlAction(0, (uint)handler.control, true);
                        }

                        switch (handler.pressType)
                        {
                            case ControlPressCheckType.JUST_PRESSED:
                                if (IsControlJustPressed(0, (uint)handler.control) || IsDisabledControlJustPressed(0, (uint)handler.control))
                                    handler.function.Invoke(this, handler.control);
                                break;
                            case ControlPressCheckType.JUST_RELEASED:
                                if (IsControlJustReleased(0, (uint)handler.control) || IsDisabledControlJustReleased(0, (uint)handler.control))
                                    handler.function.Invoke(this, handler.control);
                                break;
                            case ControlPressCheckType.PRESSED:
                                if (IsControlPressed(0, (uint)handler.control) || IsDisabledControlPressed(0, (uint)handler.control))
                                    handler.function.Invoke(this, handler.control);
                                break;
                            case ControlPressCheckType.RELEASED:
                                if (!IsControlPressed(0, (uint)handler.control) && !IsDisabledControlPressed(0, (uint)handler.control))
                                    handler.function.Invoke(this, handler.control);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws the menu header.
        /// </summary>
        /// <param name="menuItemsOffset"></param>
        /// <returns>The new menuItemsOffset value</returns>
        private async Task<float> DrawHeader(float menuItemsOffset)
        {
            if (!string.IsNullOrEmpty(MenuTitle))
            {
                #region Draw Header Background
                if (MenuController.SetDrawOrder)
                    SetScriptGfxDrawOrder(2);
                float x = (Position.Key + (headerSize.Key / 2f)) / MenuController.ScreenWidth;
                float y = (Position.Value + (headerSize.Value / 2f)) / MenuController.ScreenHeight;
                float width = headerSize.Key / MenuController.ScreenWidth;
                float height = headerSize.Value / MenuController.ScreenHeight;
                DrawSprite(MenuController._texture_dict, MenuController._header_texture, x, y, width, height, 0f, 181, 17, 18, 255, false);
                if (MenuController.SetDrawOrder)
                    SetScriptGfxDrawOrder(1);
                #endregion

                #region Draw Header Menu Title
                SetTextCentre(true);
                float size = (45f * 27f) / MenuController.ScreenHeight;
                SetTextScale(size, size);

                if (MenuController.SetDrawOrder)
                    SetScriptGfxDrawOrder(3);
                int font = 10;
                Call((CitizenFX.Core.Native.Hash)0xADA9255D, font);
                long _text = Call<long>(_CREATE_VAR_STRING, 10, "LITERAL_STRING", MenuTitle ?? "N/A");
                float textX = (headerSize.Key / 2f) / MenuController.ScreenWidth;
                float textY = y - (45f / MenuController.ScreenHeight);
                DisplayText(_text, textX, textY);
                if (MenuController.SetDrawOrder)
                    SetScriptGfxDrawOrder(1);
                menuItemsOffset = headerSize.Value;
                #endregion
            }
            else
            {
                menuItemsOffset = 40f;
            }
            await Task.FromResult(0);
            return menuItemsOffset;
        }

        /// <summary>
        /// Draws the menu subtitle.
        /// </summary>
        /// <param name="menuItemsOffset"></param>
        /// <returns>The new menuItemsOffset value</returns>
        private float DrawSubtitle(float menuItemsOffset)
        {
            float bgHeight = 38f;
            float x = (Position.Key + (headerSize.Key / 2f)) / MenuController.ScreenWidth;
            float y = ((Position.Value + menuItemsOffset + (bgHeight / 2f)) / MenuController.ScreenHeight);
            float width = headerSize.Key / MenuController.ScreenWidth;
            float height = bgHeight / MenuController.ScreenHeight;
            if (!string.IsNullOrEmpty(MenuSubtitle))
            {
                if (MenuController.SetDrawOrder)
                    SetScriptGfxDrawOrder(3);
                float size = (14f * 27f) / MenuController.ScreenHeight;
                SetTextScale(size, size);
                SetTextCentre(true);
                int font = 9;
                Call((CitizenFX.Core.Native.Hash)0xADA9255D, font);
                long _text = Call<long>(_CREATE_VAR_STRING, 10, "LITERAL_STRING", MenuSubtitle ?? "N/A");
                DisplayText(_text, x, y - (52f / MenuController.ScreenHeight));
                if (MenuController.SetDrawOrder)
                    SetScriptGfxDrawOrder(1);
            }
            #region draw counter + pre-counter text
            if (Size > 0)
            {
                float textSize = (12f * 27f) / MenuController.ScreenHeight;
                SetTextScale(textSize, textSize);
                SetTextColor(135, 135, 135, 255);
                SetTextCentre(true);
                float textMinX = (headerSize.Key / 2f) / MenuController.ScreenWidth;
                float textMaxX = (Width - 10f) / MenuController.ScreenWidth;
                float textY = (menuItemsOffset + 38f * (MathUtil.Clamp(Size, 0, MaxItemsOnScreen) + 1) - 11f) / MenuController.ScreenHeight;
                int font = 23;
                Call((CitizenFX.Core.Native.Hash)0xADA9255D, font);
                long _text = Call<long>(_CREATE_VAR_STRING, 10, "LITERAL_STRING", $"{CurrentIndex + 1} of {Size}");
                DisplayText(_text, textMinX, textY);
            }
            #endregion
            return menuItemsOffset;
        }

        /// <summary>
        /// Draws the background for all visible menu item's as one large rectangle.
        /// </summary>
        /// <param name="menuItemsOffset"></param>
        /// <returns>The new menuItemsOffset value</returns>
        private float DrawBackgroundGradient(float menuItemsOffset)
        {
            if (Size < 1)
            {
                return menuItemsOffset;
            }
            //float x = (Position.Key + ((headerSize.Key) / 2f)) / MenuController.ScreenWidth;
            //float y = ((Position.Value + menuItemsOffset + ((bgHeight + 1f) / 2f) /) / MenuController.ScreenHeight);
            //float width = (headerSize.Key + 16f) / MenuController.ScreenWidth;
            //float height = (bgHeight + 17f) / MenuController.ScreenHeight;
            float bgHeight = 38f * MathUtil.Clamp(Size, 0, MaxItemsOnScreen);
            var currentMenuItem = GetCurrentMenuItem();
            float descriptionBoxHeight = 0f;
            if (currentMenuItem != null && !string.IsNullOrEmpty(currentMenuItem.Description))
            {
                int count = (currentMenuItem.Description.Count((a => { return a == '\n'; })) - 1);
                if (count < 1)
                {
                    descriptionBoxHeight = 42f;
                }
                else
                {
                    descriptionBoxHeight = (38f * count) + 30f;
                }

                bgHeight += descriptionBoxHeight;
            }
            float actualBgYLocation = ((38f + (38f / 2f) + (bgHeight / 2f)) / MenuController.ScreenHeight);
            float x = (Position.Key + (headerSize.Key / 2f)) / MenuController.ScreenWidth;
            float y = ((Position.Value + menuItemsOffset + ((bgHeight + 1f - (headerSize.Value)) / 2f) + 19f) / MenuController.ScreenHeight);
            float width = headerSize.Key / MenuController.ScreenWidth;
            float height = (headerSize.Value + bgHeight + 33f + 38f) / MenuController.ScreenHeight;
            DrawSprite(MenuController._texture_dict, MenuController._header_texture, x, y, width, height, 0f, 0, 0, 0, 240, false);
            DrawSprite(MenuController._texture_dict, MenuController._header_texture, x, y + actualBgYLocation - (descriptionBoxHeight / MenuController.ScreenHeight), width, 38f / MenuController.ScreenHeight, 0f, 55, 55, 55, 255, false);
            menuItemsOffset += bgHeight - descriptionBoxHeight - 1f;
            return menuItemsOffset;
        }

        /// <summary>
        /// Draw menu items that are visible in the current view.
        /// </summary>
        private void DrawActiveMenuItems()
        {
            if (Size < 1)
            {
                return;
            }
            foreach (var item in VisibleMenuItems)
            {
                item.Draw(ViewIndexOffset);
            }
        }

        /// <summary>
        /// Draws the menu item description.
        /// </summary>
        /// <param name="menuItemsOffset"></param>
        /// <param name="descriptionYOffset"></param>
        /// <returns>The new descriptionYOffset value</returns>
        private float DrawDescription(float menuItemsOffset, float descriptionYOffset)
        {
            if (Size < 1)
            {
                return descriptionYOffset;
            }
            var currentMenuItem = GetCurrentMenuItem();
            if (currentMenuItem != null && !string.IsNullOrEmpty(currentMenuItem.Description))
            {
                #region description text
                int font = 0;
                float textSize = (14f * 27f) / MenuController.ScreenHeight;

                SetTextScale(textSize, textSize);
                SetTextCentre(true);
                float textMinX = (headerSize.Key / 2f) / MenuController.ScreenWidth;
                float textMaxX = (Width - 10f) / MenuController.ScreenWidth;
                float textY = menuItemsOffset / MenuController.ScreenHeight + (18f / MenuController.ScreenHeight) + (48f / MenuController.ScreenHeight);
                font = 23;
                Call((CitizenFX.Core.Native.Hash)0xADA9255D, font);
                long _text = Call<long>(_CREATE_VAR_STRING, 10, "LITERAL_STRING", $"{currentMenuItem.Description}");
                DisplayText(_text, textMinX, textY);

                #endregion
            }
            else
            {
                descriptionYOffset += menuItemsOffset / MenuController.ScreenHeight + (2f / MenuController.ScreenHeight) + descriptionYOffset;
            }
            return descriptionYOffset;
        }


        /// <summary>
        /// Calls all Draw functions for all visible menu components.
        /// </summary>
        internal async Task Draw()
        {
            if (!(
                IsScreenFadedIn() &&
                !IsPauseMenuActive() &&
                !IsEntityDead(PlayerPedId())
            ))
            {
                return;
            }
            ProcessButtonPressHandlers();

            MenuItemsYOffset = 0f;
            if (MenuController.SetDrawOrder)
            {
                SetScriptGfxDrawOrder(1);
            }

            MenuItemsYOffset = await DrawHeader(MenuItemsYOffset);

            MenuItemsYOffset = DrawSubtitle(MenuItemsYOffset);

            MenuItemsYOffset = DrawBackgroundGradient(MenuItemsYOffset);

            DrawActiveMenuItems();
            float descriptionYOffset = 0f;
            descriptionYOffset = DrawDescription(MenuItemsYOffset, descriptionYOffset);
            if (MenuController.SetDrawOrder)
            {
                SetScriptGfxDrawOrder(0);
            }
        }
        #endregion
    }
}
