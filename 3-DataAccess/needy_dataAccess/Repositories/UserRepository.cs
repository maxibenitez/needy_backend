using Dapper;
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

        public async Task<IEnumerable<User>> GetUsersAsync()
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
                    var users = new List<User>();

                    while (await reader.ReadAsync())
                    {
                        users.Add(await UserBuilderAsync(reader));
                    }

                    return users;
                }
            }
        }

        public async Task<User> GetUserByCIAsync(string userCI)
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

        public async Task<User> GetUserByEmailAsync(string email)
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

        public async Task<bool> InsertUserAsync(InsertUserParameters parameters)
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

        public Task<bool> InsertUserSkillAsync(int skilId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private async Task<User> UserBuilderAsync(NpgsqlDataReader reader)
        {
            var user = new User
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
            };

            return user;
        }

        #endregion
    }
}
