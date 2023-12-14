using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.GlobalHotkey
{
    /// <summary>
    /// Klasse zur Verwendung von Globalen Hotkeys
    /// </summary>
    public class GlobalHotkey
    {
        private int currentId;
        private readonly Dictionary<int, HotkeyInfo> hotkeys;

        /// <summary>
        /// Die Konstante WM_HOTKEY ist der Nachrichten-Code für das Hotkey-Ereignis. 
        /// Wenn ein Hotkey registriert ist und vom Benutzer gedrückt wird, 
        /// sendet das Betriebssystem eine Nachricht mit dem Code WM_HOTKEY an das Fenster, 
        /// das das Hotkey-Ereignis behandeln möchte.
        /// </summary>
        public const int WM_HOTKEY = 0x0312;

        /// <summary>
        /// EventHandler, welches bei eintritt, wenn ein Hotkey gedrückt wurde
        /// </summary>
        public event EventHandler<HotkeyEventArgs> HotkeyPressed;

        /// <summary>
        /// Erstellt eine neue Instanz vom Typ GlobalHotkey
        /// </summary>
        public GlobalHotkey()
        {
            currentId = 0;
            hotkeys = new Dictionary<int, HotkeyInfo>();
        }

        /// <summary>
        /// Registriert ein Globales Hotkey mit einer Funktionsdefinition
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifiers"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public void RegisterHotkey(Keys key, KeyModifiers modifiers, Action action)
        {
            // Überprüfen, ob die Tastenkombination bereits registriert ist
            if (IsHotkeyRegistered(key, modifiers))
            {
                // Tastenkombination ist bereits registriert
                throw new ArgumentException("Die Tastenkombination ist bereits registriert.");
            }

            RegisterHotkey(currentId, modifiers, key);

            HotkeyInfo hotkeyInfo = new HotkeyInfo
            {
                Action = action,
                Key = new HotkeyDefinition() { Key = key, Modifiers = modifiers }

                //Key = key,
                //Modifiers = modifiers
            };
            hotkeys[currentId] = hotkeyInfo;

            currentId++;
        }

        /// <summary>
        /// Durchläuft die vorhandenen Hotkeys und vergleicht die Tastenkombinationen mit der angegebenen Tastenkombination.
        /// Wenn eine Übereinstimmung gefunden wird, bedeutet dies, dass die Tastenkombination bereits registriert ist, 
        /// und die Methode gibt true zurück. Andernfalls wird false zurückgegeben
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        private bool IsHotkeyRegistered(Keys key, KeyModifiers modifiers)
        {
            foreach (var hotkeyId in hotkeys.Keys)
            {
                if (hotkeys[hotkeyId].Key.Key == key && hotkeys[hotkeyId].Key.Modifiers == modifiers)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Entfernt alle registrierten Hotkey-Definitionen
        /// </summary>
        public void UnregisterHotkey()
        {
            for (int i = 0; i < hotkeys.Count; i++)
            {
                UnregisterHotkey(i);
            }
        }

        /// <summary>
        /// Entfernt eine registrierte Hotkey-Definition
        /// </summary>
        /// <param name="id">ID des Hotkeys, das entfernt werden soll</param>
        public void UnregisterHotkey(int id)
        {
            if (hotkeys.ContainsKey(id))
            {
                UnregisterHotkeyInternal(id);
                hotkeys.Remove(id);
            }
        }

        private void UnregisterHotkeyInternal(int id)
        {
            UnregisterHotKey(Handle, id);
        }

        /// <summary>
        /// Gibt eine Liste aller registrierten Hotkeys zurück
        /// </summary>
        /// <returns>Liste von HotkeyInfo-Objekten</returns>
        internal List<HotkeyDefinition> GetHotkeys()
        {
            List<HotkeyDefinition> defs = new List<HotkeyDefinition>();

            for(int i = 0; i < hotkeys.Count; i++)
            {
                defs.Add(hotkeys[i].Key);
            }

            return defs;
        }

        /// <summary>
        /// Erstelle eine protexted Override void WndProc mit dem Referenz-Parameter Message,
        /// um GlobalHotkeys anwenden zu können. Wenn m.Msg == GlobalHotkey.WM_HOTKEY, dann ProcessHotkey(m).
        /// Zum Schluss base.WndProc(ref m)
        /// </summary>
        /// <param name="message"></param>
        /// <example>
        /// <code>
        /// protected override void WndProc(ref Message m)
        /// {
        ///     if (m.Msg == GlobalHotkey.WM_HOTKEY)
        ///     {
        ///         globalHotkey.ProcessHotkey(m);
        ///     }
        ///     base.WndProc(ref m);
        /// }</code>
        /// </example>
        public void ProcessHotkey(Message message)
        {
            if (message.Msg == WM_HOTKEY)
            {
                int id = message.WParam.ToInt32();
                if (hotkeys.ContainsKey(id))
                {
                    //var info = new HotkeyInfo() { Action = hotkeys[id].Action, Key = hotkeys[id].Key, Modifiers = hotkeys[id].Modifiers };
                    var info = new HotkeyInfo() { Action = hotkeys[id].Action, Key = hotkeys[id].Key };
                    OnHotkeyPressed(new HotkeyEventArgs(id, info));
                }
            }
        }

        /// <summary>
        /// Tritt ein, wenn ein Hotkey gedrückt wurde
        /// </summary>
        /// <param name="e">HotkeyEvent-Parameter</param>
        protected virtual void OnHotkeyPressed(HotkeyEventArgs e)
        {
            HotkeyPressed?.Invoke(this, e);
        }

        private void RegisterHotkey(int id, KeyModifiers modifiers, Keys key)
        {
            RegisterHotKey(Handle, id, (int)modifiers, (int)key);
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// The Window Handle that the control is bound to
        /// </summary>
        public IntPtr Handle { get; set; }
    }
}
