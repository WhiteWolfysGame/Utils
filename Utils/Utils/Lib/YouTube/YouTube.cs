using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                }
                if (response.NextPageToken == null)
                {
                    break;
                }
                request.PageToken = response.NextPageToken;
            }

            return playlistItems;
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
}
