﻿using System;

namespace Voltaic.Logging
{
    public interface ILogger
    {
        string Name { get; }

        void Log(LogSeverity severity, Exception exception = null);
        void Log(LogSeverity severity, string message, Exception exception = null);
        void Log(LogSeverity severity, FormattableString message, Exception exception = null);

        void Critical(Exception exception);
        void Critical(string message, Exception exception = null);
        void Critical(FormattableString message, Exception exception = null);

        void Error(Exception exception);
        void Error(string message, Exception exception = null);
        void Error(FormattableString message, Exception exception = null);

        void Warning(Exception exception);
        void Warning(string message, Exception exception = null);
        void Warning(FormattableString message, Exception exception = null);

        void Info(Exception exception);
        void Info(string message, Exception exception = null);
        void Info(FormattableString message, Exception exception = null);

        void Verbose(Exception exception);
        void Verbose(string message, Exception exception = null);
        void Verbose(FormattableString message, Exception exception = null);

        void Debug(Exception exception);
        void Debug(string message, Exception exception = null);
        void Debug(FormattableString message, Exception exception = null);
    }
}
