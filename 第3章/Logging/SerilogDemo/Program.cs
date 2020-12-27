using System;
using Serilog;
using Serilog.Formatting.Compact;

namespace SerilogDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var separator = System.IO.Path.DirectorySeparatorChar;
            var log = new LoggerConfiguration()
                        .WriteTo.Console(new CompactJsonFormatter())
                        .WriteTo.File(new CompactJsonFormatter(), $"logs{separator}myapp.txt", 
                                       rollingInterval: RollingInterval.Day)
                        // .MinimumLevel.Debug()
                        // .WriteTo.Console()
                        // .WriteTo.File($"logs{separator}myapp.txt", 
                        //               rollingInterval: RollingInterval.Day)
                        .CreateLogger();
            /*
            log.Information("Hello, world!");
            log.Debug("This is debug message");
            log.Warning("Some warnings");
            log.Error("Error occurs");
            */

            // 结构化日志示例
            var fruit = new[] { "Apple", "Pear", "Orange" };
            log.Information("篮子里共有水果：{fruit}", fruit);
            var location = new { Latitude = 25, Longitude = 134 };
            log.Information("JSON输出格式 {@Location}", location);
            log.Information("普通输出格式 {Location}", location);
            log.Information("强制ToSting {$fruit}", fruit);
        }
    }
}
