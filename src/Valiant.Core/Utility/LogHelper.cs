﻿using ZLogger.Formatters;
using ZLogger;
using Discord;
using Microsoft.Extensions.Logging;

namespace Valiant;

public static class LogHelper
{
    public static PlainTextZLoggerFormatter ConfigureZFormatter(PlainTextZLoggerFormatter formatter)
    {
        formatter.SetPrefixFormatter($"{0} [{1}] {2}:\t", (in MessageTemplate template, in LogInfo info)
            => template.Format(info.Timestamp, info.LogLevel.ToString().Substring(0, 4), info.Category));
        formatter.SetExceptionFormatter((writer, ex) => Utf8StringInterpolation.Utf8String.Format(writer, $"{ex.Message}"));
        return formatter;
    }

    public static Task OnLogAsync(ILogger logger, LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Verbose:
                logger.ZLogInformation($"{msg}");
                break;

            case LogSeverity.Info:
                logger.ZLogInformation($"{msg}");
                break;

            case LogSeverity.Warning:
                logger.ZLogWarning($"{msg}");
                break;

            case LogSeverity.Error:
                logger.ZLogError($"{msg}");
                break;

            case LogSeverity.Critical:
                logger.ZLogCritical($"{msg}");
                break;
        }
        return Task.CompletedTask;
    }
}
