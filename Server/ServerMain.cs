using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Server.Config;
using NLog;
using NLog.Config;
using NLog.Targets;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LostAngeles.Server
{
    public class ServerMain : BaseScript
    {
        private static readonly Logger Log = LogManager.GetLogger("SERVERMAIN");
        private static GlobalConfig Config { get; set; }

        public ServerMain()
        {
            ReadConfig();
            InitializeLogger(Config.LogConfig.LogsPath, Config.LogConfig.LogLevel);
        }

        private void ReadConfig()
        {
            var filePath = Path.Combine(API.GetResourcePath(API.GetCurrentResourceName()), "server.yml");
            if (string.IsNullOrEmpty(filePath))
            {
                throw new FileNotFoundException($"The server configuration file could not be found. Config: {filePath}.");
            }

            var yaml = File.ReadAllText(filePath);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

            Config = deserializer.Deserialize<GlobalConfig>(yaml);
            Log.Info("Server config loaded");
        }
        
        private void InitializeLogger(string logPath, int logLevel)
        {
            var config = new LoggingConfiguration();

            const string layout =
                "${longdate} | ${level:uppercase=true} | [${logger}] ${message} ${exception:format=ToString}";
            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = layout
            };
            config.AddTarget(consoleTarget);

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

            var minLevel = LogLevel.FromOrdinal(logLevel);
            foreach (var target in config.AllTargets)
            {
                config.AddRule(minLevel, LogLevel.Fatal, target);
            }
            
            LogManager.Configuration = config;

            Log.Info("NLog configured.");
        }
    }
}