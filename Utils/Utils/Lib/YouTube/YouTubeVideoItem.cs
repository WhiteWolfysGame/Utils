using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.YouTube
{
    /// <summary>
    /// Klasse mit Daten für ein Youtube-Video
    /// </summary>
    public class YouTubeVideoItem
    {
        /// <summary>
        /// ID des Youtube-Kanals
        /// </summary>
        public string ChannelId { get; set; }

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
    }
}
