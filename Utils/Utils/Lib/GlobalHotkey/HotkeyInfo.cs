using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.GlobalHotkey
{
    /// <summary>
    /// Klasse, die die Informationen für einen Hotkey enthält 
    /// </summary>
    public class HotkeyInfo
    {
        /// <summary>
        /// Auszuführende Action bei dieser Tastenkombination (.Invoke())
        /// </summary>
        public Action Action { get; set; }

        ///// <summary>
        ///// Gedrückte Taste
        ///// </summary>
        //public Keys Key { get; set; }

        ///// <summary>
        ///// Gedrückte weitere Tasten (Modifier)
        ///// </summary>
        //public KeyModifiers Modifiers { get; set; }

        /// <summary>
        /// Information der Gedrückten Tasten und Modifier
        /// </summary>
        public HotkeyDefinition Key { get; set; }
    }
}
