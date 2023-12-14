using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.GlobalHotkey
{
    /// <summary>
    /// Klasse, die die Informationen der zu drückenden Tasten enthält 
    /// </summary>
    public class HotkeyDefinition
    {
        /// <summary>
        /// Gedrückte Taste
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// Gedrückte weitere Tasten (Modifier)
        /// </summary>
        public KeyModifiers Modifiers { get; set; }
    }
}
