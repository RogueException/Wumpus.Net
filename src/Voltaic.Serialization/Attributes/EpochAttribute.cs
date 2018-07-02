﻿using System;

namespace Voltaic.Serialization
{
    public enum EpochType
    {
        UnixNanos,
        UnixMillis,
        UnixSeconds
    }

    public class EpochAttribute : Attribute
    {
        public EpochType Type { get; }

        public EpochAttribute(EpochType type)
        {
            Type = type;
        }
    }
}
