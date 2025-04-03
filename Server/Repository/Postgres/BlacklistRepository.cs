using System;
using System.Threading.Tasks;
using LostAngeles.Server.Domain;
using Npgsql;

namespace LostAngeles.Server.Repository.Postgres
{
    public class PostgresBlacklist : BaseRepository, IBlacklist
    {
        public async Task<Blacklist> GetByLicenseAsync(string license)
        {
            try
            {
                const string query = "SELECT License, BlockedAt, Reason FROM Blacklist WHERE License = @License";
                using (var command = DataSource.CreateCommand(query))
                {
                    command.Parameters.AddWithValue("@License", license);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Blacklist
                            {
                                License = reader.GetString(0),
                                BlockedAt = reader.GetFieldValue<DateTimeOffset>(1),
                                Reason = reader.IsDBNull(2) ? null : reader.GetString(2)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error Get Blacklist by License: {ex.Message}");
                return null;
            }

            return null;
        }

        public Task AddAsync(Blacklist blacklist)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(Blacklist blacklist)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(string license)
        {
            throw new System.NotImplementedException();
        }
    }
}