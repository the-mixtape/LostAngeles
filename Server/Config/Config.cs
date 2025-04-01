using System.Collections.Generic;
using LostAngeles.Shared;
using LostAngeles.Shared.Config;

namespace LostAngeles.Server.Config
{
    public class GlobalConfig
    {
        public List<SpawnPosition> SpawnPositions { get; set; }
        
        public LogConfig LogConfig { get; set; }
        
        // public string DatabaseConnectionString { get; set; }
        
        // public ClientConfig ClientConfig { get; set; }
        
    }

    public class LogConfig
    {
        public string LogsPath { get; set; }
        public int LogLevel { get; set; }
    }
}