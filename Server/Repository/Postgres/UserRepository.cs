using System;
using System.Globalization;
using System.Threading.Tasks;
using LostAngeles.Server.Domain;
using Npgsql;
using NpgsqlTypes;

namespace LostAngeles.Server.Repository.Postgres
{
    public class UserRepository : BaseRepository, IUser
    {
        public async Task<User> GetOrCreate(string license)
        {
            const string query = @"
                INSERT INTO users (License) 
                VALUES (@License) 
                ON CONFLICT (license) DO UPDATE 
                    SET License = EXCLUDED.License 
                RETURNING Id, License, Character, Position";

            using (var command = DataSource.CreateCommand(query))
            {
                command.Parameters.AddWithValue("@License", license);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (!await reader.ReadAsync()) return null;
                    var user = new User
                    {
                        Id = reader.GetInt32(0),
                        License = reader.GetString(1),
                        Character = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Position = reader.IsDBNull(3) ? null : reader.GetFieldValue<Position>(3),
                    };
                    return user;
                }
            }
        }

        public async Task<bool> UpdateCharacter(string license, string character)
        {
            const string query = @"
                UPDATE users 
                SET Character = @Character
                WHERE License = @License";

            using (var command = DataSource.CreateCommand(query))
            {
                command.Parameters.AddWithValue("@License", license);
                command.Parameters.AddWithValue("@Character", NpgsqlDbType.Jsonb,
                    string.IsNullOrEmpty(character) ? (object)DBNull.Value : character);

                var affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows != 0;
            }
        }

        public async Task<bool> UpdatePosition(string license, Position position)
        {
            const string query = @"
                UPDATE users 
                SET Position = @Position
                WHERE License = @License";

            using (var command = DataSource.CreateCommand(query))
            {
                command.Parameters.AddWithValue("@License", license);
                command.Parameters.AddWithValue("@Position", position);

                var affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows != 0;
            }
        }
    }
}