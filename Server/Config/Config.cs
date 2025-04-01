using System.Collections.Generic;
using LostAngeles.Shared;
using LostAngeles.Shared.Config;

namespace LostAngeles.Server.Config
{
    public class GlobalConfig
    {
        public List<SpawnPosition> SpawnPositions { get; set; }
        public LogConfig LogConfig { get; set; }
        public DatabaseConfig DatabaseConfig { get; set; }
    }

    public class LogConfig
    {
        public string LogsPath { get; set; }
        public int LogLevel { get; set; }
    }

    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
    }
}