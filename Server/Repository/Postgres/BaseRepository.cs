using NLog;
using Npgsql;

namespace LostAngeles.Server.Repository.Postgres
{
    public class BaseRepository
    {
        protected static readonly Logger Log = LogManager.GetLogger("REPOSITORY");

        protected static NpgsqlDataSource DataSource;

        private static string _connectionString;

        public static string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                UpdatedDataSource();
            }
        }

        private static void UpdatedDataSource()
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(ConnectionString);
            dataSourceBuilder.MapComposite<Domain.Position>("character_position_type");
            DataSource = dataSourceBuilder.Build();
        }

    }
}