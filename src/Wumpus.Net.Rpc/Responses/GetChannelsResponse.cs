﻿using Wumpus.Entities;
using Voltaic.Serialization;

namespace Wumpus.Events
{
    /// <summary> xxx </summary>
    public class GetChannelsResponse
    {
        /// <summary> xxx </summary>
        [ModelProperty("channels")]
        public RpcChannel[] Channels { get; set; }
    }
}
