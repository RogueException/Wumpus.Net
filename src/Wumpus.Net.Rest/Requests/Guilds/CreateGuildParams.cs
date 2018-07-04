﻿using Wumpus.Entities;
using Voltaic.Serialization;
using Voltaic;

namespace Wumpus.Requests
{
    /// <summary> https://discordapp.com/developers/docs/resources/guild#create-guild-json-params </summary>
    public class CreateGuildParams
    {
        /// <summary> Name of the <see cref="Guild"/>. </summary>
        [ModelProperty("name")]
        public Utf8String Name { get; }
        /// <summary> <see cref="VoiceRegion"/> id. </summary>
        [ModelProperty("region")]
        public Utf8String RegionId { get; }
        /// <summary> Base64 128x128 jpeg image for the <see cref="Guild"/> icon. </summary>
        [ModelProperty("icon")]
        public Optional<Image?> Icon { get; set; }
        /// <summary> <see cref="Entities.VerificationLevel"/>. </summary>
        [ModelProperty("verification_level")]
        public Optional<VerificationLevel> VerificationLevel { get; set; }
        /// <summary> <see cref="Entities.DefaultMessageNotifications"/> level. </summary>
        [ModelProperty("default_message_notifications")]
        public Optional<DefaultMessageNotifications> DefaultMessageNotifications { get; set; }
        /// <summary> New <see cref="Guild"/> <see cref="Role"/>s. </summary>
        [ModelProperty("roles")]
        public Optional<Role[]> Roles { get; set; }
        /// <summary> New <see cref="Guild"/>'s <see cref="Channel"/>s. </summary>
        [ModelProperty("channels")]
        public Optional<Channel[]> Channels { get; set; }

        public CreateGuildParams(Utf8String name, Utf8String regionId)
        {
            Name = name;
            RegionId = regionId;
        }

        public void Validate()
        {
            Preconditions.NotNullOrWhitespace(Name, nameof(Name));
            Preconditions.LengthAtLeast(Name, Guild.MinNameLength, nameof(Name));
            Preconditions.LengthAtMost(Name, Guild.MaxNameLength, nameof(Name));

            Preconditions.NotNullOrWhitespace(RegionId, nameof(RegionId));
        }
    }
}
