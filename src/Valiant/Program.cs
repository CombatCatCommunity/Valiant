using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valiant;
using Valiant.Services;
using ZLogger;
using ZLogger.Formatters;
using ZLogger.Providers;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("_config.json");
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Debug);
        logging.AddZLoggerConsole(options =>
        {
            options.UsePlainTextFormatter(f => ConfigureZFormatter(f));
        });
        logging.AddZLoggerRollingFile(rolling =>
        {
            rolling.UsePlainTextFormatter(f => ConfigureZFormatter(f));
            rolling.FilePathSelector = (dt, x) => $"logs/{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log";
            rolling.RollingInterval = RollingInterval.Day;
            rolling.RollingSizeKB = 1024;
        });
    })
    .AddDiscord()
    .AddTwitch()
    .ConfigureServices(services =>
    {
        services.AddHostedService<DiscordStartupService>();
        services.AddHostedService<StickyRolesService>();
        //services.AddHostedService<NoPingService>();
        //services.AddHostedService<LocaleDirectorService>();
        services.AddHostedService<InteractionHandlingService>();
        services.AddHostedService<CommandHandlingService>();
    })
    .Build();

await host.RunAsync();


static PlainTextZLoggerFormatter ConfigureZFormatter(PlainTextZLoggerFormatter formatter)
{
    formatter.SetPrefixFormatter($"{0} [{1}] {2}:\t", (in MessageTemplate template, in LogInfo info) 
        => template.Format(info.Timestamp, info.LogLevel.ToString().Substring(0, 4), info.Category));
    formatter.SetExceptionFormatter((writer, ex) => Utf8StringInterpolation.Utf8String.Format(writer, $"{ex.Message}"));
    return formatter;
}