﻿using System.Collections.Generic;
using LostAngeles.Shared;
using LostAngeles.Shared.Config;

namespace LostAngeles.Server.Config
{
    public class GlobalConfig
    {
        public List<PlayerPosition> SpawnPositions { get; set; }
        public LogConfig LogConfig { get; set; }
        public DatabaseConfig DatabaseConfig { get; set; }
        public ClientConfig ClientConfig { get; set; }
        public HashSet<string> Admins { get; set; }
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