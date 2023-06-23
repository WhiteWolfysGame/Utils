using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.GlobalHotkey
{
    /// <summary>
    /// Key-Modifier für Global Hotkeys
    /// </summary>
    [Flags]
    public enum KeyModifiers
    {
        /// <summary>
        /// Keine Zusatz-Taste gedrückt
        /// </summary>
        None = 0,
        /// <summary>
        /// Alt-Taste gedrückt
        /// </summary>
        Alt = 1,
        /// <summary>
        /// Strg-Taste gedrückt
        /// </summary>
        Control = 2,
        /// <summary>
        /// Umschalt/Shift gedrückt
        /// </summary>
        Shift = 4,
        /// <summary>
        /// Windows-Taste gedrückt
        /// </summary>
        Win = 8
    }
}
