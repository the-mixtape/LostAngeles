using System;
using System.IO;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using LostAngeles.Server.Config;
using LostAngeles.Server.Core;
using LostAngeles.Server.Core.Player;
using LostAngeles.Server.Repository;
using LostAngeles.Server.Repository.Postgres;
using LostAngeles.Shared;
using NLog;
using NLog.Config;
using NLog.Targets;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LostAngeles.Server
{
    public class ServerMain : BaseScript
    {
        
        const string LogLayout = "${longdate} | ${level:uppercase=true} | [${logger}] ${message} ${exception:format=ToString}";
        
        private static readonly LoggingConfiguration LogConfig = new LoggingConfiguration();
        private static readonly Logger Log = LogManager.GetLogger("SERVERMAIN");
        private static GlobalConfig Config { get; set; }

        public ServerMain()
        {   
            PreInitializeLogger();
            ReadConfig();
            InitializeLogger();
            InitializeAdminHelper();
            InitializeRepository();
            InitializeSpawnHelper();
            
            EventHandlers[ServerEvents.Server.RequestClientConfigEvent] += new Action<CitizenFX.Core.Player>(OnRequestClientConfig);
        }

        private void ReadConfig()
        {
            var filePath = Path.Combine(API.GetResourcePath(API.GetCurrentResourceName()), "lostangeles.yml");
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

        private void PreInitializeLogger()
        {
            var consoleTarget = new ConsoleTarget("console")
            {
                Layout = LogLayout,
            };
            LogConfig.AddTarget(consoleTarget);
            
            foreach (var target in LogConfig.AllTargets)
            {
                LogConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, target);
            }
            
            LogManager.Configuration = LogConfig;
        }
        
        private void InitializeLogger()
        {
            var fileTarget = new FileTarget("file")
            {
                FileName = Path.Combine(Config.LogConfig.LogsPath, "server.log"),
                ArchiveFileName = Path.Combine(Config.LogConfig.LogsPath, "server_${date:format=dd_MM_yyyy}.log"),
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                MaxArchiveFiles = 30,
                Layout = LogLayout,
            };
            LogConfig.AddTarget(fileTarget);

            var minLevel = LogLevel.FromOrdinal(Config.LogConfig.LogLevel);
            foreach (var target in LogConfig.AllTargets)
            {
                LogConfig.AddRule(minLevel, LogLevel.Fatal, target);
            }
            LogManager.Configuration = LogConfig;
            
            Log.Info("Logger configured.");
        }

        private void InitializeRepository()
        {
            Repository.Postgres.PostgresRepository.Initialize(Config.DatabaseConfig.ConnectionString);
            IBlacklist blacklist = PostgresRepository.Blacklist;
            IUser user = PostgresRepository.User;
            
            HardCap.BlacklistRepo = blacklist;
            GameMode.UserRepo = user;
            PositionUpdater.UserRepo = user;
        }

        private void InitializeSpawnHelper()
        {
            SpawnHelper.InitializePositions(Config.SpawnPositions);
        }

        private void InitializeAdminHelper()
        {
            AdminHelper.Initialize(Config.Admins);
        }

        private void OnRequestClientConfig([FromSource] CitizenFX.Core.Player source)
        {
            var config = Config.ClientConfig;
            var data = Converter.ToJson(config);
            TriggerClientEvent(source, ClientEvents.Client.SetupClientConfigEvent, data);
        }
    }
}