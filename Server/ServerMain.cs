using System.IO;
using System.Xml.XPath;
using CitizenFX.Core;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace LostAngeles.Server
{
    public class ServerMain : BaseScript
    {
        public ServerMain()
        {
            InitializeLogger();
        }

        private void InitializeLogger()
        {
            var config = new LoggingConfiguration();

            const string layout =
                "${longdate} | ${level:uppercase=true} | ${logger} | ${message} ${exception:format=ToString}";
            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = layout
            };
            config.AddTarget(consoleTarget);

            const string logPath = "${basedir}/logs"; //TODO: from config 
            var fileTarget = new FileTarget("file")
            {
                FileName = Path.Combine(logPath, "server.log"),
                ArchiveFileName = Path.Combine(logPath, "server_${date:format=dd_MM_yyyy}.log"),
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                MaxArchiveFiles = 30,
                Layout = layout
            };
            config.AddTarget(fileTarget);

            config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);

            LogManager.Configuration = config;

            var log = LogManager.GetCurrentClassLogger();
            log.Info("NLog configured.");
        }
    }
}