using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Utils.Lib.YouTube
{
    /// <summary>
    /// Klasse zur Verwendung wichtiger YouTube-Befehle
    /// </summary>
    public class YouTube
    {
        private string apiKey;
        private YouTubeService youtubeService;

        /// <summary>
        /// Erstellt eine neue, jedoch noch leere Instanz vom Typ <see cref="YouTube"/>.
        /// Wichtig: Die Methode "SetApiKey" muss zwingend ausgeführt werden, weil es sonst zu Fehlern kommen kann
        /// </summary>
        public YouTube()
        {

        }

        /// <summary>
        /// Erstellt eine neue Instanz vom Typ <see cref="YouTube"/>.
        /// </summary>
        /// <param name="apiKey">API-Key zum verwenden der Instanz.
        /// Diese ist bei https://console.cloud.google.com/apis/dashboard?hl=de zu erstellen</param>
        public YouTube(string apiKey)
        {
            SetApiKey(apiKey);
        }

        /// <summary>
        /// Lädt den ApiKey in diese Instanz
        /// </summary>
        /// <param name="key">API-Key zum verwenden der Instanz</param>
        public void SetApiKey(string key)
        {
            this.apiKey = key;

            youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
            });
        }

        /// <summary>
        /// Das aufzurufende Event, wenn das Erstellen der Playlist stattfindet und den aktuellen Progress mitteilt
        /// </summary>
        public event EventHandler<PlaylistProgressEventArgs> PlaylistProgressUpdate;

        private void ReportPlaylistProgress(int current, int max)
        {
            PlaylistProgressUpdate?.Invoke(this, new PlaylistProgressEventArgs(current, max));
        }

        /// <summary>
        /// Überprüft, ob die Playlist mit der angegebenen ID existiert.
        /// </summary>
        /// <param name="playlistID">Youtube-Playlist-ID</param>
        /// <returns>true, wenn die Playlist existiert</returns>
        /// <exception cref="ArgumentNullException">Es kann zu einer Exception kommen, wenn der API-Key fehlt</exception>
        public bool CheckPlaylistExists(string playlistID)
        {
            if (apiKey == null || apiKey == "" || youtubeService == null) { throw new ArgumentNullException("apiKey", "No Api Key detected!"); }

            PlaylistItemsResource.ListRequest requestPlaylistItems = youtubeService.PlaylistItems.List(YouTubeRequestType.Snippet);
            requestPlaylistItems.PlaylistId = playlistID;
            requestPlaylistItems.MaxResults = 2;

            PlaylistItemListResponse responsePlaylistItems;
            while (true)
            {
                try { responsePlaylistItems = requestPlaylistItems.Execute(); }
                catch (Google.GoogleApiException) { return false; }

                if (responsePlaylistItems.Items.Count > 0) { return true; }
                if (responsePlaylistItems.NextPageToken == null) { break; }

                requestPlaylistItems.PageToken = responsePlaylistItems.NextPageToken;
            }

            return false;
        }

        /// <summary>
        /// Gibt den Titel der Playlist zur angegebenen ID zurück
        /// </summary>
        /// <param name="playlistID">Youtube-Playlist-ID</param>
        /// <returns>Titel der angegebenen Playlist-ID</returns>
        /// <exception cref="ArgumentNullException">Es kann zu einer Exception kommen, wenn der API-Key fehlt</exception>
        public string GetPlaylistTitle(string playlistID)
        {
            if (apiKey == null || apiKey == "" || youtubeService == null) { throw new ArgumentNullException("apiKey", "No Api Key detected!"); }

            var requestPlaylistData = youtubeService.Playlists.List(YouTubeRequestType.Snippet);
            requestPlaylistData.Id = playlistID;
            PlaylistListResponse responsePlaylistData;

            try { responsePlaylistData = requestPlaylistData.Execute(); }
            catch (Google.GoogleApiException) { return null; }

            return responsePlaylistData.Items[0].Snippet.Title;
        }

        /// <summary>
        /// Gibt eine Liste mit Playlist-Elementen zurück, basierend auf der Playlist-ID
        /// </summary>
        /// <param name="playlistID">Youtube-Playlist-ID die abgerufen werden soll</param>
        /// <returns>Liste der Playlist-Elemente</returns>
        /// <exception cref="ArgumentNullException">Es kann zu einer Exception kommen, wenn der API-Key fehlt</exception>
        public List<YouTubePlaylistItem> GetPlaylistItems(string playlistID)
        {
            if (apiKey == null || apiKey == "" || youtubeService == null) { throw new ArgumentNullException("apiKey", "No Api Key detected!"); }

            List<YouTubePlaylistItem> playlistItems = new List<YouTubePlaylistItem>();

            PlaylistItemsResource.ListRequest request = youtubeService.PlaylistItems.List(YouTubeRequestType.Snippet);
            request.PlaylistId = playlistID;
            request.MaxResults = 10;// brauchen wir das eigentlich?? Naja vielleicht damit nicht zu viel auf einmal ausgelesen wird..

            PlaylistItemListResponse response;
            int current = 0;
            while (true)
            {
                response = request.Execute();
                foreach (PlaylistItem item in response.Items)
                {
                    YouTubePlaylistItem playlistItem = new YouTubePlaylistItem
                    {
                        VideoId = item.Snippet.ResourceId.VideoId,
                        Title = item.Snippet.Title,
                        VideoOwnerChannelName = item.Snippet.VideoOwnerChannelTitle,
                        Description = item.Snippet.Description,
                        Duration = GetVideoDuration(item.Snippet.ResourceId.VideoId),
                        Thumbnail = GetThumbnail(item.Snippet.Thumbnails.High.Url)
                    };

                    playlistItems.Add(playlistItem);
                    ReportPlaylistProgress(current++, response.PageInfo.TotalResults.Value);
                }
                if (response.NextPageToken == null)
                {
                    break;
                }
                request.PageToken = response.NextPageToken;
            }

            ReportPlaylistProgress(current++, response.PageInfo.TotalResults.Value);

            return playlistItems;
        }

        /// <summary>
        /// Gibt eine neue Liste mit Playlist-Elementen zurück, basierend der alten Informationen
        /// </summary>
        /// <param name="listToCheck">Alte Youtube-Playlist die überprüft werden soll</param>
        /// <returns>Liste der Playlist-Elemente</returns>
        public List<YouTubePlaylistItem> GetPlaylistItems(List<YouTubePlaylistItem> listToCheck)
        {
            List<YouTubePlaylistItem> playlistItems = new List<YouTubePlaylistItem>();

            int current = 0;
            foreach (YouTubePlaylistItem item in listToCheck)
            {
                YouTubePlaylistItem playlistItem = new YouTubePlaylistItem
                {
                    VideoId = item.VideoId,
                    Title = item.Title,
                    VideoOwnerChannelName = item.VideoOwnerChannelName,
                    Description = item.Description,
                    Duration = item.Duration,
                    Thumbnail = item.Thumbnail
                };

                playlistItems.Add(playlistItem);
                ReportPlaylistProgress(current++, listToCheck.Count);
            }

            ReportPlaylistProgress(current++, listToCheck.Count);

            return playlistItems;
        }


        /// <summary>
        /// Gibt das Datum der letzten Bearbeitung der Playlist zurück
        /// </summary>
        /// <param name="playlistID">Zu prüfende Playlist-ID</param>
        /// <returns>Letztes Updatedatum</returns>
        public DateTime PlaylistLastUpdate(string playlistID)
        {
            // Es gibt seitens der API keine Möglichkeit, um geziehlt nach dem letzten Update zu filtern, also mache ich es jetzt wie folgt...
            // 1. Die RAW-HTMl herunterladen
            string html = GetPlaylistHtml(playlistID);

            // 2. Daten nach RegEx-Pattern filtern:
            string filteredValue = GetPlaylistFilteredHtml(html);

            // 3. Jetzt noch in ein gültiges DateTime-Format umwandeln, dabei gibt es jedoch verschiedene Youtube-Seitige Daten
            DateTime theDate = GetPlaylistFilteredDate(filteredValue);

            return theDate;
        }

        private static string GetPlaylistHtml(string playlistID)
        {
            string html = string.Empty;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync("https://www.youtube.com/playlist?list=" + playlistID).Result;
                    html = (response.IsSuccessStatusCode) ? response.Content.ReadAsStringAsync().Result : string.Empty;
                }
            }
            catch (Exception)
            { }

            return html;
        }

        private static string  GetPlaylistFilteredHtml(string html)
        {
            string filteredValue = string.Empty;

            // Regulärer Ausdruck für den Inhalt von
            //      "\"byline\":\\[\\{(.*?)\"simpleText\":\"(.*?)\"\\}\\]\\}\\}\\}"
            string pattern = "\"byline\":\\[\\{(.*?)\"simpleText\":\"(.*?)\"\\}\\]\\}\\}\\}";

            // Regex-Match
            Match match = Regex.Match(html, pattern, RegexOptions.Singleline);

            // Überprüfe, ob ein Match gefunden wurde
            if (match.Success)
            {
                string textSnippet = (match.Success) ? match.Groups[2].Value : string.Empty;
                filteredValue = textSnippet.Substring(textSnippet.IndexOf("[{\"text\":\"") + "[{\"text\":\"".Length);
            }

            return filteredValue;
        }

        private static DateTime GetPlaylistFilteredDate(string filteredValue)
        {
            DateTime theDate = DateTime.MinValue;

            // Fall 1.1: Deutsches Datumsformat
            // Regulärer Ausdruck für das Datumsformat "28.06.2021"
            string pattern = @"(\d{2}.\d{2}.\d{4})";
            Match match = Regex.Match(filteredValue, pattern);

            if (match.Success)
            {
                string dateString = match.Groups[1].Value;
                DateTime.TryParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out theDate);
            }

            // Fall 1.2 Englisches Datumsformat
            // Regulärer Ausdruck für das Datumsformat "5 Feb 2018"
            pattern = @"(\d{1,2} [a-zA-Z]+ \d{4})";
            match = Regex.Match(filteredValue, pattern);

            if (match.Success)
            {
                string dateString = match.Groups[1].Value;
                DateTime.TryParseExact(dateString, "d MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out theDate);
            }

            // Fall 2: "Vor \"},{\"text\":\"4\"},{\"text\":\" Tagen aktualisiert"
            pattern = "\"text\":\"(\\d+)\"";
            match = Regex.Match(filteredValue, pattern);

            if (match.Success)
            {
                string numberString = match.Groups[1].Value;
                int.TryParse(numberString, out int daysUpdated);
                theDate = DateTime.Today.AddDays(-daysUpdated);
            }

            // Fall 3: "Heute aktualisiert"
            if (filteredValue.ToLower().Contains("heute") || filteredValue.ToLower().Contains("today"))
            {
                theDate = DateTime.Today;
            }

            // Fall 4: "Gestern aktualisiert"
            if (filteredValue.ToLower().Contains("gestern") || filteredValue.ToLower().Contains("yesterday"))
            {
                theDate = DateTime.Today.AddDays(-1);
            }

            return theDate;
        }

        private TimeSpan GetVideoDuration(string videoId)
        {
            var vRequest = youtubeService.Videos.List(YouTubeRequestType.Contentdetails);
            vRequest.Id = videoId;
            var vResponse = vRequest.Execute();
            var video = vResponse.Items.FirstOrDefault();

            return XmlConvert.ToTimeSpan(video.ContentDetails.Duration);
        }

        private Image GetThumbnail(string url)
        {
            var httpClient = new HttpClient();
            var imageBytes = httpClient.GetByteArrayAsync(url).Result;

            using MemoryStream ms = new MemoryStream(imageBytes);
            return Image.FromStream(ms);
        }

        /// <summary>
        /// Überprüft, ob der Youtube-Kanal mit der angegebenen ID existiert
        /// (nicht den @-Namen!) und gibt den Namen als Referenz-Parameter zurück
        /// </summary>
        /// <param name="channelId">Youtube-Kanal-ID das überprüft werden soll</param>
        /// <param name="channelName">Kanal-Name dieser ID. Wird ersetzt/gefüllt, sofern dieser Kanal exisitert</param>
        /// <returns>true, wenn der Kanal existiert.</returns>
        /// <exception cref="ArgumentNullException">Es kann zu einer Exception kommen, wenn der API-Key fehlt</exception>
        public bool CheckChannelExists(string channelId, ref string channelName)
        {
            if (apiKey == null || apiKey == "" || youtubeService == null) { throw new ArgumentNullException("apiKey", "No Api Key detected!"); }

            ChannelsResource.ListRequest requestChannelItems = youtubeService.Channels.List(YouTubeRequestType.Snippet);
            requestChannelItems.Id = channelId;
            requestChannelItems.MaxResults = 3;

            ChannelListResponse response;

            try
            {
                response = requestChannelItems.Execute();
            }
            catch (Google.GoogleApiException)
            {
                channelName = "";
                return false;
            }

            while (true)
            {
                if(response.Items == null || response.Items.Count == 0)
                {
                    break;
                }

                foreach (var channelResponse in response.Items)
                {
                    channelName = channelResponse.Snippet.Title;
                    return true;
                }

                if (response.NextPageToken == null)
                {
                    break;
                }
                requestChannelItems.PageToken = response.NextPageToken;
            }

            return false;
        }

        /// <summary>
        /// Überprüft, ob der Youtube-Kanal mit der angegebenen ID existiert. (nicht den @-Namen!)
        /// </summary>
        /// <param name="channelId">Youtube-Kanal-ID das überprüft werden soll</param>
        /// <returns>true, wenn der Kanal existiert.</returns>
        /// <exception cref="ArgumentNullException">Es kann zu einer Exception kommen, wenn der API-Key fehlt</exception>
        public bool CheckChannelExists(string channelId)
        {
            string egal = "";
            return CheckChannelExists(channelId, ref egal);
        }

        /// <summary>
        /// Erhalte eine Liste mit der Anzahl an neuesten Videos eines bestimmten Kanals
        /// </summary>
        /// <param name="channelId">Youtube-Kanal-ID, für die die neuesten Videos gefiltert werden sollen</param>
        /// <param name="maxResults">Anzahl der neuesten Videos für den gewünschten Kanal</param>
        /// <returns>Liste von <see cref="YouTubeVideoItem"/> mit den wichtigsten Parametern</returns>
        /// <exception cref="ArgumentNullException">Es kann zu einer Exception kommen, wenn der API-Key fehlt</exception>
        public List<YouTubeVideoItem> GetNewestVideos(string channelId, int maxResults)
        {
            if (apiKey == null || apiKey == "" || youtubeService == null) { throw new ArgumentNullException("apiKey", "No Api Key detected!"); }

            List<YouTubeVideoItem> videos = new List<YouTubeVideoItem>();

            // Fetch the latest X videos from a specific channel
            var searchListRequest = youtubeService.Search.List(YouTubeRequestType.Snippet);
            searchListRequest.ChannelId = channelId;
            searchListRequest.MaxResults = maxResults;
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            var searchListResponse = searchListRequest.Execute();

            if (searchListResponse.Items != null)
            {
                foreach (var searchResult in searchListResponse.Items)
                {
                    YouTubeVideoItem video = new YouTubeVideoItem
                    {
                        ChannelId = channelId,
                        VideoId = searchResult.Id.VideoId,
                        Title = searchResult.Snippet.Title,
                        Thumbnail = GetThumbnail(searchResult.Snippet.Thumbnails.High.Url),
                        Description = searchResult.Snippet.Description
                    };

                    videos.Add(video);
                }
            }

            return videos;
        }

        /// <summary>
        /// Erhalte eine Liste mit der Anzahl an neuesten Videos von mehreren bestimmten Kanälen
        /// </summary>
        /// <param name="channelIds">Array von Youtube-Kanal-ID, für die die neuesten Videos gefiltert werden sollen</param>
        /// <param name="maxResults">Anzahl der neuesten Videos für die gewünschten Kanäle</param>
        /// <returns>Liste von <see cref="YouTubeVideoItem"/> mit den wichtigsten Parametern</returns>
        /// <exception cref="ArgumentNullException">Es kann zu einer Exception kommen, wenn der API-Key fehlt</exception>
        public List<YouTubeVideoItem> GetNewestVideos(string[] channelIds, int maxResults)
        {
            List<YouTubeVideoItem> videos = new List<YouTubeVideoItem>();

            for (int i = 0; i < channelIds.Length; i++)
            {
                videos.AddRange(GetNewestVideos(channelIds[i], maxResults));
            }

            return videos;
        }
    }

    /// <summary>
    /// Klasse zur Definition des aktuellen Prozesses bei der Lizenzüberprüfung
    /// </summary>
    public class PlaylistProgressEventArgs : EventArgs
    {
        private double progressPercentage;
        private double value;
        private double total;

        /// <summary>
        /// Aktueller Progress
        /// </summary>
        public double ProgressPercentage { get { return progressPercentage; } }

        /// <summary>
        /// Erstellt eine neue Instanz vom Typ <see cref="PlaylistProgressEventArgs"/>
        /// </summary>
        /// <param name="current">Aktueller Progress-Value</param>
        /// <param name="max">Maximaler Progress-Value</param>
        public PlaylistProgressEventArgs(int current, int max)
        {
            value = current;
            total = max;

            double calc = value / total * 100;
            progressPercentage = Math.Round(calc, 2);
        }
    }

}
