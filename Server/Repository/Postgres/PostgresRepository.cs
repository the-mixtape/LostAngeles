using System;
using System.Linq;

namespace LostAngeles.Server.Repository.Postgres
{
    public static class PostgresRepository
    {
        public static UserRepository User { get; } = new UserRepository();
        public static PostgresBlacklist Blacklist { get; } = new PostgresBlacklist();

        public static void Initialize(string connectionString)
        {
            User.ConnectionString = connectionString;
            Blacklist.ConnectionString = connectionString;
        }
    }
}