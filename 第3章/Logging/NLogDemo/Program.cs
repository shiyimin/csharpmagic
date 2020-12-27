using System;
using NLog;
using NLog.Config;

namespace NLogDemo
{
    class Program
    {
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            LogManager.LoadConfiguration("nlog.config");

            try
            {
                s_logger.Info("Hello world");
                System.Console.ReadKey();
            }
            catch (Exception ex)
            {
                s_logger.Error(ex, "Goodbye cruel world");
            }
        }
    }
}
