using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Lib.Twitch
{
    /// <summary>
    /// Für Twitch zu verwendene Scopes zur Authentifizierung definierte Konfigurationen, um mit der Twitch-API zu interagieren.
    /// Diese Scopes müssen für den Oauth-Token freigegeben sein!
    /// </summary>
    public static class TwitchScopes
    {
        /// <summary>
        /// View analytics data for the Twitch Extensions owned by the authenticated account.
        /// </summary>
        public static string AnalyticsReadExtensions { get { return "analytics:read:extensions"; } }

        /// <summary>
        /// View analytics data for the games owned by the authenticated account.
        /// </summary>
        public static string AnalyticsReadGames { get { return "analytics:read:games"; } }

        /// <summary>
        /// View Bits information for a channel
        /// </summary>
        public static string BitsRead { get { return "bits:read"; } }

        /// <summary>
        /// Run commercials on a channel.
        /// </summary>
        public static string ChannelEditCommercial { get { return "channel:edit:commercial"; } }

        /// <summary>
        /// Manage a channel’s broadcast configuration, including updating channel configuration and managing stream markers and stream tags.
        /// </summary>
        public static string ChannelManageBroadcast { get { return "channel:manage:broadcast"; } }

        /// <summary>
        /// Read charity campaign details and user donations on your channel.
        /// </summary>
        public static string ChannelReadCharity { get { return "channel:read:charity"; } }

        /// <summary>
        /// Manage a channel’s Extension configuration, including activating Extensions.
        /// </summary>
        public static string ChannelManageExtensions { get { return "channel:manage:extensions"; } }

        /// <summary>
        /// Add or remove the moderator role from users in your channel.
        /// </summary>
        public static string ChannelManageModerators { get { return "channel:manage:moderators"; } }

        /// <summary>
        /// Manage a channel’s polls.
        /// </summary>
        public static string ChannelManagePolls { get { return "channel:manage:polls"; } }

        /// <summary>
        /// Manage of channel’s Channel Points Predictions
        /// </summary>
        public static string ChannelManagePredictions { get { return "channel:manage:predictions"; } }

        /// <summary>
        /// Manage a channel raiding another channel.
        /// </summary>
        public static string ChannelManageRaids { get { return "channel:manage:raids"; } }

        /// <summary>
        /// Manage Channel Points custom rewards and their redemptions on a channel.
        /// </summary>
        public static string ChannelManageRedemptions { get { return "channel:manage:redemptions"; } }

        /// <summary>
        /// Manage a channel’s stream schedule.
        /// </summary>
        public static string ChannelManageSchedule { get { return "channel:manage:schedule"; } }

        /// <summary>
        /// Manage a channel’s videos, including deleting videos.
        /// </summary>
        public static string ChannelManageVideos { get { return "channel:manage:videos"; } }

        /// <summary>
        /// View a list of users with the editor role for a channel.
        /// </summary>
        public static string ChannelReadEditors { get { return "channel:read:editors"; } }

        /// <summary>
        /// View Creator Goals for a channel.
        /// </summary>
        public static string ChannelReadGoals { get { return "channel:read:goals"; } }

        /// <summary>
        /// View Hype Train information for a channel.
        /// </summary>
        public static string ChannelReadHype_train { get { return "channel:read:hype_train"; } }

        /// <summary>
        /// View a channel’s polls.
        /// </summary>
        public static string ChannelReadPolls { get { return "channel:read:polls"; } }

        /// <summary>
        /// View a channel’s Channel Points Predictions.
        /// </summary>
        public static string ChannelReadPredictions { get { return "channel:read:predictions"; } }

        /// <summary>
        /// View Channel Points custom rewards and their redemptions on a channel.
        /// </summary>
        public static string ChannelReadRedemptions { get { return "channel:read:redemptions"; } }

        /// <summary>
        /// View an authorized user’s stream key.
        /// </summary>
        public static string ChannelReadStream_key { get { return "channel:read:stream_key"; } }

        /// <summary>
        /// View a list of all subscribers to a channel and check if a user is subscribed to a channel.
        /// </summary>
        public static string ChannelReadSubscriptions { get { return "channel:read:subscriptions"; } }

        /// <summary>
        /// Read the list of VIPs in your channel.
        /// </summary>
        public static string ChannelReadVips { get { return "channel:read:vips"; } }

        /// <summary>
        /// Add or remove the VIP role from users in your channel.
        /// </summary>
        public static string ChannelManageVips { get { return "channel:manage:vips"; } }

        /// <summary>
        /// Manage Clips for a channel.
        /// </summary>
        public static string ClipsEdit { get { return "clips:edit"; } }

        /// <summary>
        /// View a channel’s moderation data including Moderators, Bans, Timeouts, and Automod settings.
        /// </summary>
        public static string ModerationRead { get { return "moderation:read"; } }

        /// <summary>
        /// Send announcements in channels where you have the moderator role.
        /// </summary>
        public static string ModeratorManageAnnouncements { get { return "moderator:manage:announcements"; } }

        /// <summary>
        /// Manage messages held for review by AutoMod in channels where you are a moderator.
        /// </summary>
        public static string ModeratorManageAutomod { get { return "moderator:manage:automod"; } }

        /// <summary>
        /// View a broadcaster’s AutoMod settings.
        /// </summary>
        public static string ModeratorReadAutomod_settings { get { return "moderator:read:automod_settings"; } }

        /// <summary>
        /// Manage a broadcaster’s AutoMod settings.
        /// </summary>
        public static string ModeratorManageAutomod_settings { get { return "moderator:manage:automod_settings"; } }

        /// <summary>
        /// Ban and unban users.
        /// </summary>
        public static string ModeratorManageBanned_users { get { return "moderator:manage:banned_users"; } }

        /// <summary>
        /// View a broadcaster’s list of blocked terms.
        /// </summary>
        public static string ModeratorReadBlocked_terms { get { return "moderator:read:blocked_terms"; } }

        /// <summary>
        /// Manage a broadcaster’s list of blocked terms.
        /// </summary>
        public static string ModeratorManageBlocked_terms { get { return "moderator:manage:blocked_terms"; } }

        /// <summary>
        /// Delete chat messages in channels where you have the moderator role
        /// </summary>
        public static string ModeratorManageChat_messages { get { return "moderator:manage:chat_messages"; } }

        /// <summary>
        /// View a broadcaster’s chat room settings.
        /// </summary>
        public static string ModeratorReadChat_settings { get { return "moderator:read:chat_settings"; } }

        /// <summary>
        /// Manage a broadcaster’s chat room settings.
        /// </summary>
        public static string ModeratorManageChat_settings { get { return "moderator:manage:chat_settings"; } }

        /// <summary>
        /// View the chatters in a broadcaster’s chat room.
        /// </summary>
        public static string ModeratorReadChatters { get { return "moderator:read:chatters"; } }

        /// <summary>
        /// Read the followers of a broadcaster.
        /// </summary>
        public static string ModeratorReadFollowers { get { return "moderator:read:followers"; } }

        /// <summary>
        /// View a broadcaster’s Shield Mode status.
        /// </summary>
        public static string ModeratorReadShield_mode { get { return "moderator:read:shield_mode"; } }

        /// <summary>
        /// Manage a broadcaster’s Shield Mode status.
        /// </summary>
        public static string ModeratorManageShield_mode { get { return "moderator:manage:shield_mode"; } }

        /// <summary>
        /// View a broadcaster’s shoutouts.
        /// </summary>
        public static string ModeratorReadShoutouts { get { return "moderator:read:shoutouts"; } }

        /// <summary>
        /// Manage a broadcaster’s shoutouts.
        /// </summary>
        public static string ModeratorManageShoutouts { get { return "moderator:manage:shoutouts"; } }

        /// <summary>
        /// Manage a user object.
        /// </summary>
        public static string UserEdit { get { return "user:edit"; } }

        /// <summary>
        /// Deprecated. Was previously used for “Create User Follows” and “Delete User Follows.”
        /// See Deprecation of Create and Delete Follows API Endpoints.
        /// </summary>
        public static string UserEditFollows { get { return "user:edit:follows"; } }

        /// <summary>
        /// Manage the block list of a user.
        /// </summary>
        public static string UserManageBlocked_users { get { return "user:manage:blocked_users"; } }

        /// <summary>
        /// View the block list of a user.
        /// </summary>
        public static string UserReadBlocked_users { get { return "user:read:blocked_users"; } }

        /// <summary>
        /// View a user’s broadcasting configuration, including Extension configurations.
        /// </summary>
        public static string UserReadBroadcast { get { return "user:read:broadcast"; } }

        /// <summary>
        /// Update the color used for the user’s name in chat.Update User Chat Color
        /// </summary>
        public static string UserManageChat_color { get { return "user:manage:chat_color"; } }

        /// <summary>
        /// View a user’s email address.
        /// </summary>
        public static string UserReadEmail { get { return "user:read:email"; } }

        /// <summary>
        /// View the list of channels a user follows.
        /// </summary>
        public static string UserReadFollows { get { return "user:read:follows"; } }

        /// <summary>
        /// View if an authorized user is subscribed to specific channels.
        /// </summary>
        public static string UserReadSubscriptions { get { return "user:read:subscriptions"; } }

        /// <summary>
        /// Read whispers that you send and receive, and send whispers on your behalf.
        /// </summary>
        public static string UserManageWhispers { get { return "user:manage:whispers"; } }

        /// <summary>
        /// Perform moderation actions in a channel. The user requesting the scope must be a moderator in the channel.
        /// </summary>
        public static string ChannelModerate { get { return "channel:moderate"; } }

        /// <summary>
        /// Send live stream chat messages.
        /// </summary>
        public static string ChatEdit { get { return "chat:edit"; } }

        /// <summary>
        /// View live stream chat messages.
        /// </summary>
        public static string ChatRead { get { return "chat:read"; } }

        /// <summary>
        /// View your whisper messages.
        /// </summary>
        public static string WhispersRead { get { return "whispers:read"; } }

        /// <summary>
        /// Send whisper messages.
        /// </summary>
        public static string WhispersEdit { get { return "whispers:edit"; } }
    }
}
