using NLog;

namespace LostAngeles.Server.Repository.Postgres
{
    public class BaseRepository
    {
        protected static readonly Logger Log = LogManager.GetLogger("REPOSITORY");
        
        public string ConnectionString { get; set; }
    }
}