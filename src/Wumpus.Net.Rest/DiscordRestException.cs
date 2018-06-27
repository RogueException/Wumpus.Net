﻿using System;
using System.Net;
using Voltaic;

namespace Wumpus.Net
{
    public class DiscordRestException : Exception
    {
        public HttpStatusCode HttpCode { get; }
        public int? DiscordCode { get; }
        public Utf8String Reason { get; }

        public DiscordRestException(HttpStatusCode httpCode, int? discordCode = null, Utf8String reason = null)
            : base(CreateMessage(httpCode, discordCode, reason))
        {
            HttpCode = httpCode;
            DiscordCode = discordCode;
            Reason = reason;
        }

        private static string CreateMessage(HttpStatusCode httpCode, int? discordCode = null, Utf8String reason = null)
            => $"The server responded with error {discordCode ?? (int)httpCode}: {reason.ToString() ?? httpCode.ToString()}";
    }
}