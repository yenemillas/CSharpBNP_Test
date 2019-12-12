using Serilog;
using System;
using System.Configuration;


namespace CSharpBNP
{
    public static class LogUtils
    {
        static string pathLog { get => ConfigurationManager.AppSettings["FileLog"]; }


        public static void LogInformation(string message)
        {
            using (var log = new LoggerConfiguration().WriteTo.File(pathLog).CreateLogger())
            {
                log.Information(message);
            }
        }

        public static void LogError(Exception ex)
        {
            using (var log = new LoggerConfiguration().WriteTo.File(pathLog).CreateLogger())
            {
                log.Error(ex.Message);
            }
        }
    }
}
