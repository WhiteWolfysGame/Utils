using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.GlobalHotkey
{
    /// <summary>
    /// Klasse zur Auswertung der Argumente des HotkeyPressed-Ereignisses
    /// </summary>
    public class HotkeyEventArgs : EventArgs
    {
        /// <summary>
        /// ID des Hotkeys
        /// </summary>
        public int HotkeyId { get; }

        /// <summary>
        /// Enthält alle Informationen zu gedrückte Tasten, Modifier sowie die Aktion
        /// </summary>
        public HotkeyInfo HotkeyInfo { get; }

        /// <summary>
        /// Erstellt eine neue Event-Instanz vom Typ HotkeyEventArgs
        /// </summary>
        /// <param name="hotkeyId">ID des Hotkeys</param>
        /// <param name="info">Informationen zur gedrückten Tastenkombination und zugehöriger Action</param>
        public HotkeyEventArgs(int hotkeyId, HotkeyInfo info)
        {
            HotkeyId = hotkeyId;
            HotkeyInfo = info;
        }
    }
}
