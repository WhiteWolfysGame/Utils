using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.YouTube
{
    /// <summary>
    /// Klasse mit Daten für ein Playlist-Item einer YouTube-Playlist
    /// </summary>
    public class YouTubePlaylistItem
    {
        /// <summary>
        /// ID des Youtube-Videos
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// Titel des Youtube-Videos
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Das Vorschaubild des Youtube-Videos
        /// </summary>
        public Image Thumbnail { get; set; }

        /// <summary>
        /// Beschreibungstext des Youtube-Videos
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kanalname des Eigentümers dieses Youtube-Videos
        /// </summary>
        public string VideoOwnerChannelName { get; set; }

        /// <summary>
        /// Länge des Videos
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
