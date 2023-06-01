using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Utils.Lib.Twitch
{
    /// <summary>
    /// Klasse zur Verwendung Twitch-spezifischer Funktionen
    /// </summary>
    public class Twitch
    {
        private string clientId = "";
        private string clientSecret = "";
        private string redirectUri = "";
        private string oauth = null;

        private TwitchAPI api = new TwitchAPI();
        private TwitchClient client = new TwitchClient();
        private string client_username = "";

        /// <summary>
        /// Erstellt eine neue Instanz vom Typ <see cref="Twitch"/> um mit Twitch zu interagieren.
        /// Du musst deine Anwendung zuvor auf https://dev.twitch.tv/console/apps registrieren! Callback URI kann http://localhost:8000 sein.
        /// Hier kannst du dann deine ClientID und ClientSecret abholen
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="redirectUri"></param>
        public Twitch(string clientId, string clientSecret, string redirectUri)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUri = redirectUri;

            api.Settings.ClientId = clientId;
            api.Settings.Secret = clientSecret;
        }

        /// <summary>
        /// Generiert eine aufrufbare URL um die Oauth mit den spezifischen Scopes zu erstellen
        /// </summary>
        /// <param name="scopes">Verwende TwitchScopes um deine Scopes für die Oauth zu generieren</param>
        /// <returns>URL zur Generierung einer Oauth für diese Anwendung</returns>
        public string GetOauthUri(params string[] scopes)
        {
            return MakeSaveUri(scopes);
        }

        /// <summary>
        /// Generiert eine sichere URL, welches die Twitch-Scopes vernünftig verarbeitet
        /// </summary>
        /// <param name="scopes">Liste der zu nutzenden Twitch-Scopes</param>
        /// <returns>URL mit allen Parametern zur Generierung eines Scopes</returns>
        private string MakeSaveUri(params string[] scopes)
        {
            string _doppelpunkt = "%3A";
            string url = @"https://" +
                          "id.twitch.tv/oauth2/authorize" +
                          "?client_id=" + clientId +
                          "&redirect_uri=" + redirectUri +
                          "&response_type=" + "token"
                          //"&response_type=" + "code"
                          ;
            StringBuilder sb = new StringBuilder();
            sb.Append(url);

            for (int i = 0; i < scopes.Length; i++)
            {
                if (i == 0) sb.Append("&scope=");
                else sb.Append("+");

                sb.Append(scopes[i].Replace(":", _doppelpunkt));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Teile deinen Oauth-Token mit! Diese Funktion ist essentiell
        /// </summary>
        /// <param name="token">Oauth-Token für diese Anwendung</param>
        public void SetOauth(string token)
        {
            oauth = token;
            api.Settings.AccessToken = token;
        }

        /// <summary>
        /// Erhalte aktuelle Informationen zum <paramref name="channel"/> mit Stream-Titel, Kategorie sowie wann der Stream gestartet ist.
        /// </summary>
        /// <param name="channel">Den Kanal, zu der du Informationen haben möchtest</param>
        /// <returns>Gibt Twitch Stream-Informationen zurück</returns>
        public TwitchStreamInfo GetStreamInformation(string channel)
        {
            if (this.oauth != null)
            {
                List<string> channels = new List<string>();
                channels.Add(channel);
                var streamResponse = api.Helix.Streams.GetStreamsAsync(userLogins: channels).Result;

                if (streamResponse.Streams.Length > 0)
                {
                    TwitchLib.Api.Helix.Models.Streams.GetStreams.Stream stream = streamResponse.Streams[0];
                    return new TwitchStreamInfo(stream.Title, stream.StartedAt, stream.GameName);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Erhalte eine Liste mit allen Followern zum <paramref name="channel"/>
        /// </summary>
        /// <param name="channel">Den Kanal, zu der du die Liste der Follower erhalten möchtest</param>
        /// <returns>Gibt ein Array mit Follower zurück</returns>
        public string[] GetFollower(string channel)
        {
            var userResponse = api.Helix.Users.GetUsersAsync(logins: new List<string> { channel }).Result;

            if (userResponse.Users.Length > 0)
            {
                User user = userResponse.Users[0];

                var followerResponse = api.Helix.Users.GetUsersFollowsAsync(toId: user.Id).Result;

                string[] follower = new string[followerResponse.TotalFollows];

                for (int i = 0; i < followerResponse.TotalFollows; i++)
                {
                    follower[i] = followerResponse.Follows[i].FromUserName;
                }

                return follower;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Verbinde dich mit dem Twitch-Client, um zum Beispiel Twitch-Bots auf Chat-Nachrichten reagieren zu lassen
        /// </summary>
        /// <param name="username">Der Name deines Bots/Benutzerkontos</param>
        public void ConnectClient(string username)
        {
            if (oauth != null)
            {
                ConnectionCredentials credentials = new ConnectionCredentials(username, oauth);
                client.Initialize(credentials, channel: username);
                client.Connect();

                client.JoinChannel(username);

                client.OnConnected += Client_OnConnected;
                client.OnMessageReceived += Client_OnMessageReceived;
                client.OnWhisperSent += Client_OnWhisperSent;

                client_username = username;
            }
        }

        /// <summary>
        /// Ereignis welches auftritt, wenn der Client eine Verbindung aufgebaut hat (etwa für ein ChatBot)
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        private void Client_OnConnected(object? sender, OnConnectedArgs e)
        {
            var eventArgs = new ConnectedEventArgs(e);
            Connected?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Ereigenis welches auftritt, wenn im Chat eine Nachricht erhalten wurde. Flüsternachrichten sind dabei nicht betroffen!
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        
        private void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            var eventArgs = new MessageReceivedEventArgs(e);
            MessageReceived?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Ereignis welches auftritt, wenn wir eine Flüsternachricht senden
        /// </summary>
        public event EventHandler<WhisperSentEventArgs> WhisperSent;
        
        private void Client_OnWhisperSent(object? sender, OnWhisperSentArgs e)
        {
            var eventArgs = new WhisperSentEventArgs(e);
            WhisperSent?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Trennt die Verbindung mit dem Twitch-Client. Diese funktion ist essentiell, wenn ConnectClient verwendet wird.
        /// In einer WinForms-Anwendung ist diese in FormClosing-Event zu hinterlegen dringend empfohlen.
        /// </summary>
        public void DisconnectClient()
        {
            if (client.IsConnected)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Sendet eine Nachricht in den Twitch-Chat.
        /// </summary>
        /// <param name="msg">Die Nachricht, die gesendet werden soll</param>
        public void SendChatMessage(string msg)
        {
            if (client.IsConnected)
            {
                client.JoinChannel(client_username);
                JoinedChannel channel = client.GetJoinedChannel(client_username);

                client.SendMessage(channel, msg);
            }
        }

        private TwitchUserResponse client_user;

        /// <summary>
        /// Gibt den Benutzernamen des zugehörigen Oauth zurück
        /// </summary>
        /// <returns>Login-Username, dem dieser Oauth zugehörig ist</returns>
        public async Task<string> GetOauthUser()
        {
            //return GetUsername();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Client-ID", clientId);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {oauth}");

                HttpResponseMessage response = await client.GetAsync("https://api.twitch.tv/helix/users");
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Token validation successful
                    client_user = Newtonsoft.Json.JsonConvert.DeserializeObject<TwitchUserResponse>(responseBody);
                    string username = client_user.Data[0].Login;
                    Console.WriteLine($"The OAuth token belongs to the username: {username}");
                    return username;
                }
                else
                {
                    // Token validation failed
                    return "OAuth token validation failed.";
                }
            }
        }

        private class TwitchUserResponse
        {
            public TwitchUser[] Data { get; set; }
        }

        private class TwitchUser
        {
            public string Id { get; set; }
            public string Login { get; set; }
            public string Display_Name { get; set; }
            public string Description { get; set; }
            public DateTime Created_At { get; set; }
        }
    }

    /// <summary>
    /// Args representing connected event. Implements EventArgs
    /// </summary>
    public class ConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Bot-Username
        /// </summary>
        public string BotUsername { get; }

        /// <summary>
        /// Repräsentiert verbundene Channel
        /// </summary>
        public string AutoJoinChannel { get; }

        /// <summary>
        /// Args representing connected event
        /// </summary>
        /// <param name="args"></param>
        public ConnectedEventArgs(OnConnectedArgs args)
        {
            BotUsername = args.BotUsername;
            AutoJoinChannel = args.AutoJoinChannel;
        }
    }

    /// <summary>
    /// Args representing message received event. Implements EventArgs
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Inhalt einer Twitch-Nachricht
        /// </summary>
        public string ChatMessage { get; }

        /// <summary>
        /// Verfasser der Nachricht
        /// </summary>
        public string Sender { get; }

        /// <summary>
        /// Args representing message received event
        /// </summary>
        /// <param name="args"></param>
        public MessageReceivedEventArgs(OnMessageReceivedArgs args)
        {
            ChatMessage = args.ChatMessage.Message;
            Sender = args.ChatMessage.Username;
        }
    }

    /// <summary>
    /// Args representing whisper sent event. Implements EventArgs
    /// </summary>
    public class WhisperSentEventArgs : EventArgs
    {
        /// <summary>
        /// Empfänger der Nachricht
        /// </summary>
        public string Receiver { get; }
        /// <summary>
        /// Inhalt der zu sendenden Nachricht
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Benutzername des Bots, der die Nachricht abschickt
        /// </summary>
        public string BotUsername { get; }

        /// <summary>
        /// Args representing whisper sent event
        /// </summary>
        /// <param name="args"></param>
        public WhisperSentEventArgs(OnWhisperSentArgs args)
        {
            Receiver = args.Receiver;
            Message = args.Message;
            BotUsername = args.Username;
        }
    }
}
