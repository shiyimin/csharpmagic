using System;
using System.Diagnostics;
using Serilog;
using Microsoft.Extensions.Logging;

namespace LoggerFactoryDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = new LoggerConfiguration()
                        .MinimumLevel.Error()
                        .WriteTo.File($"serilogdemo.log", 
                                       rollingInterval: RollingInterval.Day)
                        .CreateLogger();

            var loggerFactory = new LoggerFactory()
                .AddDebug()
                .AddConsole()
                .AddSerilog(log);
                /*
                 * AddTraceSource方法在dotnet core中不支持，只能运行在.NET Framework工程下面
                 * 参考文档：https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#tracesource-provider
                .AddTraceSource(
                    new SourceSwitch("DemoSourceSwitch", "All"),
                    new TextWriterTraceListener("TraceSourceDemo.log")
                );
                */
            var logger = loggerFactory.CreateLogger<Program>();
            
            logger.LogInformation("Logging information.");
            logger.LogCritical("Logging critical information.");
            logger.LogDebug("Logging debug information.");
            logger.LogError("Logging error information.");
            logger.LogTrace("Logging trace");
            logger.LogWarning("Logging warning.");

            Console.ReadLine();
        }
    }
}
