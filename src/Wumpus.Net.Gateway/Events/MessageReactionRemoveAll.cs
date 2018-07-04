﻿using Voltaic.Serialization;

namespace Wumpus.Events
{
    /// <summary> 
    ///     Sent when a user explicitly removes all reactions from a message.
    ///     https://discordapp.com/developers/docs/topics/gateway#message-reaction-remove-all
    /// </summary>
    public class MessageReactionRemoveAllEvent
    {
        /// <summary> The id of the <see cref="Entities.Channel"/>. </summary>
        [ModelProperty("channel_id")]
        public Snowflake ChannelId { get; set; }
        /// <summary> The id of the <see cref="Entities.Message"/>. </summary>
        [ModelProperty("message_id")]
        public Snowflake MessageId { get; set; }
    }
}
