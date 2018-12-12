using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuAPI
{
    public class MenuItemButton : MenuItem
    {
        /*
        
            WIP CLASS
            TODO: EVERYTHING
            
            */

        private MenuItem _item;

        public MenuItemButton(string text) : this(text, null) { }

        public MenuItemButton(string text, string description) : base(text, description)
        {

        }


    }
}
