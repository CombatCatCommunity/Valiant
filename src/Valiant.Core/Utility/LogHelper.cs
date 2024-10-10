using Discord;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Valiant.Utility;

public static class LogHelper
{
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