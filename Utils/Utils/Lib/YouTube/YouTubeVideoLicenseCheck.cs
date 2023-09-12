using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.YouTube
{
    /// <summary>
    /// Klasse zur Überprüfung von Lizenzen auf Youtube-Videos
    /// </summary>
    public class YouTubeVideoLicenseCheck
    {
        private const string YOUTUBE = "youtube";
        private const string CREATIVE_COMMON = "creativeCommon";

        private YouTubeService youTubeService;
        private string apiKey;
        private string url;
        private string videoId;

        private string videoTitle;
        private string channelName;

        private LicenseStatus resultLicenseByApiDescription;
        private LicenseStatus resultLicenseByApiStatusAndContentDetails;
        private LicenseStatus resultLicenseByRawHtmlYoutube;
        private LicenseStatus resultLicenseByLemnoslife;
        private LicenseStatus finalStatus;

        private YouTubeVideoLicenseStatus videoStatus;

        /// <summary>
        /// Ergebnisdaten der Lizenzüberprüfung
        /// </summary>
        public YouTubeVideoLicenseStatus VideoStatus { get {  return videoStatus; } }

        /// <summary>
        /// Titel des überprüften Videos
        /// </summary>
        public string Title { get { return videoTitle; } }

        /// <summary>
        /// Name des Youtube-Kanals
        /// </summary>
        public string Channel { get {  return channelName; } }

        /// <summary>
        /// Das aufzurufende Event, wenn die Überprüfung stattfindet und den aktuellen Progress mitteilt
        /// </summary>
        public event EventHandler<ProgressEventArgs> ProgressUpdate;

        private void ReportProgress(int progressPercentage)
        {
            ProgressUpdate?.Invoke(this, new ProgressEventArgs(progressPercentage));
        }

        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="YouTubeVideoLicenseCheck"/> zur Überprüfung der Lizenzen
        /// </summary>
        /// <param name="api">Deine Youtube-Api-Key</param>
        public YouTubeVideoLicenseCheck(string api)
        {
            videoId = "";
            url = "";
            apiKey = api;
            youTubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = api,
            });
        }

        /// <summary>
        /// Startet die Lizen-Überprüfung des übermittelten Youtube-URLs
        /// </summary>
        /// <param name="youtubeUrl">Youtube-Url die zu überprüfen gilt</param>
        public void StartCheck(string youtubeUrl)
        {
            url = youtubeUrl.Trim();
            ExtractVideoIdFromUrl();
            
            if(videoId == "")
            {
                ReportProgress(100);
                return;
            }

            CheckByYoutubeApi();
            CheckByYoutubeRawHtml();
            CheckByLemnoslife();

            BuildFinalLicenseStatus();
        }

        /// <summary>
        /// Extrahiert die Video-ID aus der Youtube-URL
        /// </summary>
        private void ExtractVideoIdFromUrl()
        {
            string https = "https://";
            string youtubeUrlBase = "https://www.youtube.com/watch?v=";

            if (url.StartsWith(https))
            {
                if (url.StartsWith(youtubeUrlBase))
                {
                    videoId = url.Substring(url.IndexOf(youtubeUrlBase) + youtubeUrlBase.Length);

                    // weitere Überprüfung, weil die Url weitere Parameter besitzt (Bsp mit einer Playlist id etc.)
                    if (videoId.Contains("&"))
                    {
                        int indexAmp = videoId.IndexOf("&");
                        videoId = videoId.Substring(0, indexAmp);
                    }
                }
            }

            ReportProgress(10);
        }

        /// <summary>
        /// Zieht die Youtube-Api heran, um Informationen zu Lizenzen zu erhalten
        /// </summary>
        private void CheckByYoutubeApi()
        {
            var videoRequest = youTubeService.Videos.List(YouTubeRequestType.SnippetContentdetailsAndStatus);
            videoRequest.Id = videoId;
            VideoListResponse videoResponse = videoRequest.Execute();

            if (videoResponse != null && videoResponse.Items != null && videoResponse.Items.Count > 0)
            {
                // Es gibt genau ein Video (mehr sind in dieser Abfrage nicht zu erwarten!
                Video video = videoResponse.Items[0];
                videoTitle = video.Snippet.Title;
                channelName = video.Snippet.ChannelTitle;

                // Mögliche Quellen mit Lizenzinformationen
                string licenseByDescription = video.Snippet.Description;
                bool licenseByContentdetails = video.ContentDetails.LicensedContent.Value;
                string licenseByStatus = video.Status.License;

                // Ergebnisfilter
                resultLicenseByApiDescription = GetLicenseFromText(licenseByDescription);
                ReportProgress(25);

                resultLicenseByApiStatusAndContentDetails = GetLicenseStatus(licenseByStatus, licenseByContentdetails);
                ReportProgress(40);
            }
        }

        /// <summary>
        /// Überprüft Textinhalte nach möglichen fest definierten Keywords bezüglich Lizenzen
        /// </summary>
        /// <param name="fromText">Der zu überprüfende Text</param>
        /// <returns>Ergebnis des Status' der Lizenzprüfung</returns>
        private LicenseStatus GetLicenseFromText(string fromText)
        {
            string checkText = fromText.ToLower();

            if (
                checkText.Contains("no copyright") ||
                checkText.Contains("royalty free") ||
                checkText.Contains("no rights reserved") ||
                checkText.Contains("cc0") ||
                checkText.Contains("public domain") ||
                checkText.Contains("free to use") ||
                checkText.Contains("free for commercial use") ||
                checkText.Contains("copyright free"))
            {
                return LicenseStatus.NoCopyright;
            }
            else if (
                checkText.Contains("creative common") ||
                checkText.Contains("creative commons") ||
                checkText.Contains("cc by") ||
                checkText.Contains("creativecommon"))
            {
                return LicenseStatus.CreativeCommons;
            }
            else
            {
                return LicenseStatus.Licensed;
            }
        }

        /// <summary>
        /// Überprüft die Youtube-API Parameter-Kombinationen bezüglch Status und licensedContent
        /// </summary>
        /// <param name="licenseStatus"></param>
        /// <param name="licensedContent"></param>
        /// <returns>Ergebnis des Status' der Lizenzprüfung</returns>
        private LicenseStatus GetLicenseStatus(string licenseStatus, bool licensedContent)
        {
            if (licenseStatus == YOUTUBE && licensedContent == false)
            {
                return LicenseStatus.NoCopyright;
            }
            else if (licenseStatus == YOUTUBE && licensedContent == true)
            {
                // Im Zweifel lizenziert
                return LicenseStatus.Licensed;
            }
            else if (licenseStatus == CREATIVE_COMMON && licensedContent == false)
            {
                return LicenseStatus.CreativeCommons;
            }
            else if (licenseStatus == CREATIVE_COMMON && licensedContent == true)
            {
                // Creative Commons mit Erweiterten Lizenzen, im Zweifel als Lizenziert anzusehen
                //return LicenseStatus.CreativeCommonsWithExtendedLicense;
                return LicenseStatus.Licensed;
            }
            else
            {
                return LicenseStatus.Licensed;
            }
        }

        /// <summary>
        /// Überprüft das HTML-Skript und filtert Informationen zu Lizenzen
        /// </summary>
        private void CheckByYoutubeRawHtml()
        {
            if(videoId != null)
            {
                string videoPageSource = DownloadVideoPageSource();

                if (string.IsNullOrEmpty(videoPageSource))
                {
                    // Handling benötigt?
                    MessageBox.Show("Fehler beim Herunterladen der Youtube-Videoseite.");
                }

                resultLicenseByRawHtmlYoutube = GetLicenseFromText(videoPageSource);
                ReportProgress(70);
            }
        }

        /// <summary>
        /// Lädt das gesamte Youtube-Html-Script der aktuellen Seite herunter
        /// </summary>
        /// <returns>Html-Quelltext der Youtube-Video-Seite</returns>
        private string DownloadVideoPageSource()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    return (response.IsSuccessStatusCode) ? response.Content.ReadAsStringAsync().Result : string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Überprüfung eines Youtube-Videos mit Hilfe vom JSON-Ergebnis von Lemnoslife's Websource
        /// </summary>
        private void CheckByLemnoslife()
        {
            using (HttpClient client = new HttpClient())
            {
                string lemnoslifeUrl = $"https://yt.lemnoslife.com/videos?part=musics&id={videoId}";
                try
                {
                    HttpResponseMessage response = client.GetAsync(lemnoslifeUrl).Result;
                    if(response.IsSuccessStatusCode)
                    {
                        string jsonString = response.Content.ReadAsStringAsync().Result;
                        Json.Json json = new Json.Json();
                        LemnoslifeJsondata data = json.GetObjectFromJsonString<LemnoslifeJsondata>(jsonString);

                        if (data.items != null && data.items.Count > 0)
                        {
                            resultLicenseByLemnoslife = (data.items[0].musics.Count > 0) ? CheckLemnoslifeMusicData(data) : LicenseStatus.NoCopyright;
                        }
                    }
                    else
                    {
                        // Im Zweifel muss von Lizenziert ausgegangen werden!
                        resultLicenseByLemnoslife = LicenseStatus.Licensed;
                    }
                }
                catch (Exception)
                {
                    // Fehlerbehandlung... Im Zweifel muss von Lizenziert ausgegangen werden!
                    resultLicenseByLemnoslife = LicenseStatus.Licensed;
                }
            }
        }

        /// <summary>
        /// Überprüft das Json-Ergebnis von Lemnoslife auf Basis der Music-Daten
        /// </summary>
        /// <param name="data">Json-Objektdaten von Lemnoslife</param>
        /// <returns></returns>
        private LicenseStatus CheckLemnoslifeMusicData(LemnoslifeJsondata data)
        {
            LicenseStatus resultFinal = LicenseStatus.NoCopyright;
            LicenseStatus result;

            foreach (var item in data.items[0].musics)
            {
                string licenses = item.licenses;
                if (licenses == null) licenses = "";
                result = GetLicenseFromText(licenses);

                if (resultFinal == LicenseStatus.NoCopyright)
                {
                    if (result == LicenseStatus.CreativeCommons)
                    {
                        resultFinal = LicenseStatus.CreativeCommons;
                    }
                    else
                    {
                        if (result == LicenseStatus.Licensed)
                        {
                            return LicenseStatus.Licensed;
                        }
                    }
                }
                else
                {
                    if (resultFinal == LicenseStatus.CreativeCommons)
                    {
                        if (result == LicenseStatus.Licensed)
                        {
                            return LicenseStatus.Licensed;
                        }
                    }
                }
            }

            return resultFinal;
        }

        /// <summary>
        /// Erstellt das Endresultat der überprüften Quellen zu den Lizenzen
        /// </summary>
        private void BuildFinalLicenseStatus()
        {
            // Berechnung Lizenz nach Gewichtung
            // ResultLicenseByApiStatusAndContentDetails am höchsten gewichtet, weil zuverlässigstes Ergebnis
            // ResultLicenseByLemnoslife zweithöchste Gewichtung, weil liefert auch zuverlässige Ergebnisse, aber weniger gut als das erste
            // ResultLicenseByApiDescription weniger gute Gewichtung, weil nicht immer Daten gegeben
            // ResultLicenseByRawHtmlYoutube niedrigste Gewichtung, weil oft zweifelhaft, aber nicht unmöglich

            int weightApiStatusAndContentDetails = 5;
            int weightLemnoslife = 4;
            int weightApiDescription = 2;
            int weightRawHtmlYoutube = 1;

            int totalWeight = weightApiStatusAndContentDetails + weightLemnoslife + weightApiDescription + weightRawHtmlYoutube;

            int weightedSum = (int)resultLicenseByApiStatusAndContentDetails * weightApiStatusAndContentDetails +
                              (int)resultLicenseByLemnoslife * weightLemnoslife +
                              (int)resultLicenseByApiDescription * weightApiDescription +
                              (int)resultLicenseByRawHtmlYoutube * weightRawHtmlYoutube;

            // Berechnung des endgültigen Ergebnisses basierend auf Gewichtung
            int finalResultValue = (weightedSum + (totalWeight / 2)) / totalWeight; // Aufrunden

            // Konvertiert den berechneten Wert zurück in eine Lizenzstatus-Enumeration
            finalStatus = (LicenseStatus)finalResultValue;

            // ResultLicenseByApiStatusAndContentDetails & ResultLicenseByLemnoslife sind sehr vertrauenswürdig
            // Je nach Status muss die Endgültige Lizenz-Definition nochmals gegengeprüft werden
            // um im Zweifel das richtigere Ergebnis zu erlangen
            if (finalStatus != LicenseStatus.Licensed)
            {
                LicenseStatus[] finalSourcesCheck = {
                    resultLicenseByApiStatusAndContentDetails,
                    resultLicenseByLemnoslife
                };

                finalStatus = GetHighestPriorityLicense(finalSourcesCheck);
            }

            videoStatus = new YouTubeVideoLicenseStatus(finalStatus);
            ReportProgress(100);
        }

        /// <summary>
        /// Wählt das Ergebnis mit Präferenz aus der Hierarchie aus
        /// </summary>
        /// <param name="sources">vertrauenswürdigste Quellen</param>
        /// <returns>Status mit der höchsten zu bewertenden Priorität</returns>
        private LicenseStatus GetHighestPriorityLicense(LicenseStatus[] sources)
        {
            LicenseStatus highestPriority = LicenseStatus.NoCopyright;

            foreach (var source in sources)
            {
                if (source > highestPriority)
                {
                    highestPriority = source;
                }
            }

            return highestPriority;
        }
    }

    /// <summary>
    /// Lizenz-Status
    /// </summary>
    public enum LicenseStatus
    {
        /// <summary>
        /// Ohne Copyright
        /// </summary>
        NoCopyright = 0,
        /// <summary>
        /// Creative Commons
        /// </summary>
        CreativeCommons = 1,
        /// <summary>
        /// Lizenziert
        /// </summary>
        Licensed = 2,
        //CreativeCommonsWithExtendedLicense = 3,
    }

    /// <summary>
    /// Klasse für die Ergebnisdaten nach der Lizenzüberprüfung
    /// </summary>
    public class YouTubeVideoLicenseStatus
    {
        private string status;
        private string statusDesc;
        private LicenseStatus licenseStatus;
        private Color textColor;
        private Color backColor;

        /// <summary>
        /// Kürzel des Status-Ergebnisses
        /// </summary>
        public string StatusLabel { get { return status; } }

        /// <summary>
        /// Beschreibung des Status-Ergebnisses
        /// </summary>
        public string StatusDescription { get { return statusDesc; } }

        /// <summary>
        /// Prägnante Textfarbe
        /// </summary>
        public Color TextColor { get { return textColor; } }

        /// <summary>
        /// Prägnante Hintergrundfarbe
        /// </summary>
        public Color BackColor { get { return backColor; } }

        /// <summary>
        /// Erstellt eine neue Instanz vom Typ <see cref="YouTubeVideoLicenseStatus"/>
        /// </summary>
        /// <param name="licenseStatus">Lizenzstatus, aus der die Daten abgeleitet werden</param>
        public YouTubeVideoLicenseStatus(LicenseStatus licenseStatus)
        {
            switch (licenseStatus)
            {
                case LicenseStatus.NoCopyright:
                    WriteFields(licenseStatus, "NCS", "No Copyright Music", Color.White, Color.FromArgb(76, 175, 80));
                    //CreateCheckmarkImage();
                    break;
                case LicenseStatus.CreativeCommons:
                    WriteFields(licenseStatus, "Creative Commons", "Verwendung mit Quellenangabe gestattet, Credit Autor", Color.White, Color.FromArgb(252, 175, 80));
                    break;
                case LicenseStatus.Licensed:
                default:
                    WriteFields(licenseStatus, "Lizenziert", "Verwendung verboten oder blockiert", Color.FromArgb(244, 67, 54), Color.White);
                    break;
            }
        }

        private void WriteFields(LicenseStatus status, string statusShort, string description, Color text, Color bg)
        {
            this.licenseStatus = status;
            this.status = statusShort;
            this.statusDesc = description;
            this.textColor = text;
            this.backColor = bg;
        }
    }

    /// <summary>
    /// Klasse zur Definition des aktuellen Prozesses bei der Lizenzüberprüfung
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Aktueller Progress
        /// </summary>
        public int ProgressPercentage { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz vom Typ <see cref="ProgressEventArgs"/>
        /// </summary>
        /// <param name="progressPercentage">Aktueller Progress-Value</param>
        public ProgressEventArgs(int progressPercentage)
        {
            ProgressPercentage = progressPercentage;
        }
    }

    internal class LemnoslifeJsondata
    {
        public string kind { get; set; }

        public string etag { get; set; }

        public List<Item> items { get; set; }
    }

    internal class Artist
    {
        public string title { get; set; }

        public string channelId { get; set; }
    }

    internal class Item
    {
        public string kind { get; set; }

        public string etag { get; set; }

        public string id { get; set; }

        public List<Music> musics { get; set; }
    }

    internal class Music
    {
        public Song song { get; set; }

        public List<Artist> artists { get; set; }

        public string album { get; set; }

        public List<string> writers { get; set; }

        public string licenses { get; set; }
    }

    internal class Song
    {
        public string title { get; set; }

        public string videoId { get; set; }
    }

}
