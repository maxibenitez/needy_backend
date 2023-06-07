using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction.Parameters;
using Npgsql;

namespace needy_dataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders

        public UserRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implments IUserRepository

        public async Task<IEnumerable<UserData>> GetUsersAsync()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""User""";

                var command = new NpgsqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var users = new List<UserData>();

                    while (await reader.ReadAsync())
                    {
                        users.Add(await UserBuilderAsync(reader));
                    }

                    return users;
                }
            }
        }

        public async Task<IEnumerable<UserData>> GetUsersBySkillAsync(int skillId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""User""
                            WHERE ""SkillId"" = @SkillId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@SkillId", skillId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var users = new List<UserData>();

                    while (await reader.ReadAsync())
                    {
                        users.Add(await UserBuilderAsync(reader));
                    }

                    return users;
                }
            }
        }

        public async Task<UserData> GetUserByCIAsync(string userCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""User""
                            WHERE ""CI"" = @CI";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@CI", userCI);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return await UserBuilderAsync(reader);
                    }

                    return null;
                }
            }
        }

        public async Task<UserData> GetUserByEmailAsync(string email)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""User""
                            WHERE ""Email"" = @Email";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return await UserBuilderAsync(reader);
                    }

                    return null;
                }
            }
        }

        public async Task<bool> InsertUserAsync(RegisterParameters parameters)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            INSERT INTO public.""User"" (""CI"", ""FirstName"", ""LastName"", ""Address"", ""Zone"",
                                                        ""Phone"", ""Gender"", ""BirthDate"", ""Email"", ""Password"")
                            VALUES (@CI, @FirstName, @LastName, @Address, @Zone, @Phone, @Gender, @BirthDate, @Email, @Password)";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@CI", parameters.CI);
                command.Parameters.AddWithValue("@FirstName", parameters.FirstName);
                command.Parameters.AddWithValue("@LastName", parameters.LastName);
                command.Parameters.AddWithValue("@Address", parameters.Address);
                command.Parameters.AddWithValue("@Zone", parameters.Zone);
                command.Parameters.AddWithValue("@Phone", parameters.Phone);
                command.Parameters.AddWithValue("@Gender", parameters.Gender);
                command.Parameters.AddWithValue("@BirthDate", parameters.BirthDate);
                command.Parameters.AddWithValue("@Email", parameters.Email);
                command.Parameters.AddWithValue("@Password", parameters.Password);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> InsertUserSkillAsync(string userCI, int skillId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        INSERT INTO public.""UserSkill"" (""SkillId"", ""UserCI"")
                        VALUES (@SkillId, @UserCI)";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserCI", userCI);
                command.Parameters.AddWithValue("@SkillId", skillId);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> UpdateUserAsync(string userCI, UpdateUserParameters parameters)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        UPDATE public.""User""
                        SET ""FirstName"" = @FirstName,
                            ""LastName"" = @LastName,
                            ""Address"" = @Address,
                            ""Zone"" = @Zone,
                            ""Phone"" = @Phone,
                            ""Gender"" = @Gender,
                            ""BirthDate"" = @BirthDate,
                            ""AboutMe"" = @AboutMe
                        WHERE ""CI"" = @CI";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@CI", userCI);
                command.Parameters.AddWithValue("@FirstName", parameters.FirstName);
                command.Parameters.AddWithValue("@LastName", parameters.LastName);
                command.Parameters.AddWithValue("@Address", parameters.Address);
                command.Parameters.AddWithValue("@Zone", parameters.Zone);
                command.Parameters.AddWithValue("@Phone", parameters.Phone);
                command.Parameters.AddWithValue("@Gender", parameters.Gender);
                command.Parameters.AddWithValue("@BirthDate", parameters.BirthDate);
                command.Parameters.AddWithValue("@AboutMe", parameters.AboutMe);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> DeleteUserSkillsAsync(string userCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            DELETE
                            FROM public.""UserSkill""
                            WHERE ""UserCI"" = @UserCI";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserCI", userCI);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        #endregion

        #region Private Methods

        private async Task<UserData> UserBuilderAsync(NpgsqlDataReader reader)
        {
            var user = new UserData
            {
                CI = (string)reader["CI"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Address = (string)reader["Address"],
                Zone = (string)reader["Zone"],
                Phone = (string)reader["Phone"],
                Gender = (string)reader["Gender"],
                BirthDate = (DateTime)reader["BirthDate"],
                Email = (string)reader["Email"],
                Password = (string)reader["Password"],
                AboutMe = reader.IsDBNull(reader.GetOrdinal("AboutMe")) ? null : (string?)reader["AboutMe"],
            };

            return user;
        }

        #endregion
    }
}
