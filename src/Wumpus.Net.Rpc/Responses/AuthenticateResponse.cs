﻿using Voltaic.Serialization;
using System;
using Wumpus.Entities;
using Voltaic;

namespace Wumpus.Events
{
    /// <summary> xxx </summary>
    public class AuthenticateResponse
    {
        /// <summary> xxx </summary>
        [ModelProperty("application")]
        public Application Application { get; set; }
        /// <summary> xxx </summary>
        [ModelProperty("expires"), StandardFormat('O')]
        public DateTimeOffset Expires { get; set; }
        /// <summary> xxx </summary>
        [ModelProperty("user")]
        public User User { get; set; }
        /// <summary> xxx </summary>
        [ModelProperty("scopes")]
        public Utf8String[] Scopes { get; set; }
    }
}
