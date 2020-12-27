using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LoggerBuilderDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection()
                      .AddLogging(config => config.AddConsole())
                      .BuildServiceProvider();
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Logging information.");
            logger.LogCritical("Logging critical information.");
            logger.LogDebug("Logging debug information.");
            logger.LogError("Logging error information.");
            logger.LogTrace("Logging trace");
            logger.LogWarning("Logging warning.");

            ((IDisposable) services)?.Dispose();
            // Console.ReadKey();
        }
    }
}
