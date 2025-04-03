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
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                const string query = @"
                    INSERT INTO users (License) 
                    VALUES (@License) 
                    ON CONFLICT (license) DO UPDATE 
                        SET License = EXCLUDED.License 
                    RETURNING Id, License, Character, Position";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@License", license);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var user = new User
                            {
                                Id = reader.GetInt32(0),
                                License = reader.GetString(1),
                                Character = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Position = reader.IsDBNull(3) ? null : ReadPosition(reader.GetString(3)),
                            };
                            return user;
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> UpdateCharacter(string license, string character)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                const string query = @"
                    UPDATE users 
                    SET Character = @Character
                    WHERE License = @License";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@License", license);
                    command.Parameters.AddWithValue("@Character", NpgsqlDbType.Jsonb,
                        string.IsNullOrEmpty(character) ? (object)DBNull.Value : character);

                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows != 0;
                }
            }
        }

        private Position ReadPosition(object dbValue)
        {
            if (dbValue is string strValue)
            {
                var values = strValue.Trim('(', ')').Split(',');
                return new Position
                {
                    X = float.Parse(values[0], CultureInfo.InvariantCulture),
                    Y = float.Parse(values[1], CultureInfo.InvariantCulture),
                    Z = float.Parse(values[2], CultureInfo.InvariantCulture),
                    Heading = float.Parse(values[3], CultureInfo.InvariantCulture),
                };
            }

            return null;
        }
    }
}