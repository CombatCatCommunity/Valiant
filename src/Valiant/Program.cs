using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Octokit;
using Valiant;
using Valiant.Services;
using Valiant.Services.Info;
using Valiant.Services.Metrics;
using ZLogger;
using ZLogger.Providers;

if (!Directory.Exists("data"))
    Directory.CreateDirectory("data");

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
            options.UsePlainTextFormatter(f => LogHelper.ConfigureZFormatter(f));
        });
        logging.AddZLoggerRollingFile(rolling =>
        {
            rolling.UsePlainTextFormatter(f => LogHelper.ConfigureZFormatter(f));
            rolling.FilePathSelector = (dt, x) => $"logs/{dt.ToUniversalTime():yyyy-MM-dd}_{x:000}.log";
            rolling.RollingInterval = RollingInterval.Day;
            rolling.RollingSizeKB = 1024;
        });
    })
    .AddDiscord()
    .ConfigureServices(services =>
    {
        services.AddSingleton(x =>
        {
            var config = x.GetRequiredService<IConfiguration>();
            Constants.GithubRepoId = long.Parse(config["github:repo_id"]);

            return new GitHubClient(new ProductHeaderValue("Valiant"))
            {
                Credentials = new Credentials(config["github:username"], config["github:password"])
            };
        });

        services.AddHostedService<DiscordStartupService>();

        services.AddHostedService<EmoteTracker>();
        services.AddHostedService<GithubIssueService>();
        //services.AddHostedService<StickyRolesService>();
        //services.AddHostedService<NoPingService>();
        //services.AddHostedService<LocaleDirectorService>();

        services.AddHostedService<InteractionHandlingService>();
        services.AddHostedService<CommandHandlingService>();
    })
    .Build();

await host.RunAsync();