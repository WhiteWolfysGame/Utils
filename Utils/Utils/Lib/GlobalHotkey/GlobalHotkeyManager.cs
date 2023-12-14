using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.GlobalHotkey
{
    public class GlobalHotkeyManager
    {
        private readonly GlobalHotkey globalHotkey;
        private readonly string hotkeysFilePath = "hotkeys.json"; // Dateipfad zum Speichern/Laden der Hotkeys

        /// <summary>
        /// Die Konstante WM_HOTKEY ist der Nachrichten-Code für das Hotkey-Ereignis. 
        /// Wenn ein Hotkey registriert ist und vom Benutzer gedrückt wird, 
        /// sendet das Betriebssystem eine Nachricht mit dem Code WM_HOTKEY an das Fenster, 
        /// das das Hotkey-Ereignis behandeln möchte.
        /// </summary>
        public const int WM_HOTKEY = 0x0312;
        public int Count { get; private set; }

        public GlobalHotkeyManager(IntPtr handle)
        {
            globalHotkey = new GlobalHotkey { Handle = handle };
            globalHotkey.HotkeyPressed += GlobalHotkey_HotkeyPressed;
        }

        public GlobalHotkeyManager(IntPtr handle, params Action[] functions)
        {
            globalHotkey = new GlobalHotkey { Handle = handle };
            globalHotkey.HotkeyPressed += GlobalHotkey_HotkeyPressed;

            // Lade Hotkeys aus der Datei
            LoadHotkeys(functions);
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
            //if (message.Msg == WM_HOTKEY)
            //{
            //    int id = message.WParam.ToInt32();
            //    if (hotkeys.ContainsKey(id))
            //    {
            //        //var info = new HotkeyInfo() { Action = hotkeys[id].Action, Key = hotkeys[id].Key, Modifiers = hotkeys[id].Modifiers };
            //        var info = new HotkeyInfo() { Action = hotkeys[id].Action, Key = hotkeys[id].Key };
            //        OnHotkeyPressed(new HotkeyEventArgs(id, info));
            //    }
            //}
            globalHotkey.ProcessHotkey(message);
        }


        private void GlobalHotkey_HotkeyPressed(object sender, HotkeyEventArgs e)
        {
            //// Handle Hotkey press event
            //Console.WriteLine($"Hotkey {e.HotkeyId} pressed: {e.HotkeyInfo.Modifiers} + {e.HotkeyInfo.Key}");

            // Weiterleitung des Events an registrierte Handler im Hauptprogramm
            OnHotkeyPressed(e);
        }

        // Öffentliche Methode, um Eventhandler für HotkeyPressed zu registrieren
        public void RegisterHotkeyPressedHandler(EventHandler<HotkeyEventArgs> handler)
        {
            globalHotkey.HotkeyPressed += handler;
        }

        // Öffentliche Methode, um Eventhandler für HotkeyPressed zu deregistrieren
        public void UnregisterHotkeyPressedHandler(EventHandler<HotkeyEventArgs> handler)
        {
            globalHotkey.HotkeyPressed -= handler;
        }

        /// <summary>
        /// EventHandler, welches bei eintritt, wenn ein Hotkey gedrückt wurde
        /// </summary>
        public event EventHandler<HotkeyEventArgs> HotkeyPressed;

        // Weiterleitung des HotkeyPressed-Events an registrierte Handler im Hauptprogramm
        protected virtual void OnHotkeyPressed(HotkeyEventArgs e)
        {
            HotkeyPressed?.Invoke(this, e);
        }

        public void RegisterHotkey(Keys key, KeyModifiers modifiers, Action action)
        {
            globalHotkey.RegisterHotkey(key, modifiers, action);

            // Speichere Hotkeys nach jeder Registrierung
            SaveHotkeys();
        }

        public void UnregisterHotkey(int id)
        {
            globalHotkey.UnregisterHotkey(id);

            // Speichere Hotkeys nach jeder Deaktivierung
            SaveHotkeys();
        }

        private void SaveHotkeys()
        {
            // Serialisiere Hotkeys und speichere sie in einer Datei
            var hotkeyDefs = new List<HotkeyDefinition>(globalHotkey.GetHotkeys());
            string hotkeyJson = JsonConvert.SerializeObject(hotkeyDefs);
            File.WriteAllText(hotkeysFilePath, hotkeyJson);
        }

        private void LoadHotkeys(Action[] functions)
        {
            if (File.Exists(hotkeysFilePath))
            {
                // Lade Hotkeys aus der Datei und registriere sie
                string hotkeyJson = File.ReadAllText(hotkeysFilePath);
                var hotkeyDefs = JsonConvert.DeserializeObject<List<HotkeyDefinition>>(hotkeyJson);

                for(int i = 0; i < hotkeyDefs.Count; i++)
                {
                    globalHotkey.RegisterHotkey(hotkeyDefs[i].Key, hotkeyDefs[i].Modifiers, functions[i]);
                    Count++;
                }

                //foreach (var info in hotkeyDefs)
                //{
                //    globalHotkey.RegisterHotkey(info.Key, info.Modifiers, info.Action);
                //}
            }
        }

        public List<HotkeyDefinition> GetHotkeyDefinitions()
        {
            return globalHotkey.GetHotkeys();
        }
    }
}
