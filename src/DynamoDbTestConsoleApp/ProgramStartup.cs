using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace

public partial class Program
{
    static ServiceProvider ConfigureServices(IConfigurationRoot configuration)
    {
        var services = new ServiceCollection();
        
        AddConfiguration(services, configuration);
        
        AddLogging(services);

        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider;
    }

    private static void AddConfiguration(ServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddSingleton<IConfiguration>(configuration);
    }

    private static void AddLogging(ServiceCollection services)
    {
        // _services.AddLogging(configure => configure.AddConsole(options => options.FormatterName = "minimal"));
        // _services.AddLogging(configure => configure.AddConsoleFormatter<MinimalConsoleFormatter, Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions>(f =>
        // {
        //     f.IncludeScopes = false;
        // }));
        
        // var loggerFactory = LoggerFactory.Create(
        //     builder => builder
        //         // add console as logging target
        //         .AddConsole()
        //         
        //         // set minimum level to log
        //         .SetMinimumLevel(LogLevel.Debug)
        // );
        
        services.AddLogging(loggerBuilder =>
        {
            loggerBuilder.ClearProviders();

            // Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions options2 = 
            //     new Microsoft.Extensions.Logging.Console.ConsoleFormatterOptions();
            
            loggerBuilder.AddConsole(options =>
            {
                //options.FormatterName = System.Console.OutputEncoding.WebName;
                //options.TimestampFormat = "yyyy-MM-dd HH:mm:ss";
            });
        });
    }

    static IConfigurationRoot GetConfiguration()
    {
        var configurationRoot = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets(typeof(Program).Assembly)
            .Build();

        return configurationRoot;
    }
    
}
