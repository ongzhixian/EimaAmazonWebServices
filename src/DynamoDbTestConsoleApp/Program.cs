using Eima.ConfigurationServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = GetConfiguration();

ServiceProvider serviceProvider = ConfigureServices(configuration);

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("[START]");

logger.LogInformation("[TEST] {aaa}", configuration["test:sampleSetting1"]);

ConfigurationService configurationService = new ConfigurationService(configuration);

// if (await configurationService.PutConfiguration("TEST", "MyTESTVALUE"))
// {
//     logger.LogInformation("ITEM PUT");
// }
// else
// {
//     logger.LogInformation("ITEM NOT PUT");
// }

if (await configurationService.PutConfiguration("TEST", "AAA"))
{
    logger.LogInformation("ITEM PUT");
}
else
{
    logger.LogInformation("ITEM NOT PUT");
}

logger.LogInformation("[END]");