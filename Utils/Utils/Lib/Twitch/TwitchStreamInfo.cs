using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.Twitch
{
    /// <summary>
    /// Klasse enthält Informationen zu einem Twitch-Kanal
    /// </summary>
    public class TwitchStreamInfo
    {
        /// <summary>
        /// Stream-Titel
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Startzeitpunkt, wann der Stream gestartet wurde
        /// </summary>
        public DateTime? StartedAt { get; private set; }

        /// <summary>
        /// Stream-Kategorie in der gestreamt wird.
        /// </summary>
        public string GameName { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="TwitchStreamInfo"/>
        /// </summary>
        public TwitchStreamInfo()
        {
            Title = "";
            StartedAt = null;
            GameName = "";
        }

        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="TwitchStreamInfo"/>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="startedAt"></param>
        /// <param name="gameName"></param>
        public TwitchStreamInfo(string title, DateTime startedAt, string gameName)
        {
            Title = title;
            StartedAt = startedAt;
            GameName = gameName;
        }
    }
}
