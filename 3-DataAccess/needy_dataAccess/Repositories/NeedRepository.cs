using Dapper;
using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction.Parameters;
using Npgsql;
using System.Data;

namespace needy_dataAccess.Repositories
{
    public class NeedRepository : INeedRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders

        public NeedRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implments INeedRepository

        public async Task<IEnumerable<NeedData>> GetNeedsAsync()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""Need""";

                var command = new NpgsqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var needDataList = new List<NeedData>();

                    while (await reader.ReadAsync())
                    {
                        needDataList.Add(await NeedDataBuilderAsync(reader));
                    }

                    return needDataList;
                }
            }
        }

        public async Task<IEnumerable<NeedData>> GetNeedsBySkillAsync(int skillId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""Need""
                            WHERE ""RequestedSkillId"" = @SkillId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@SkillId", skillId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var needDataList = new List<NeedData>();

                    while (await reader.ReadAsync())
                    {
                        needDataList.Add(await NeedDataBuilderAsync(reader));
                    }

                    return needDataList;
                }
            }
        }

        public async Task<NeedData> GetNeedByIdAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""Need""
                            WHERE ""Id"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return await NeedDataBuilderAsync(reader);
                    }

                     return null;
                }
            }
        }

        public async Task<string[]> GetNeedAppliersListAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT ""AppliersCI""
                            FROM public.""Need""
                            WHERE ""Id"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                string[] appliersCI = null;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        appliersCI = (string[])reader["Appliers"];
                    }

                    return appliersCI;
                }
            }
        }

        public async Task<bool> InsertNeedAsync(string userCI, InsertNeedParameters parameters)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        INSERT INTO public.""Need"" (""RequestorCI"", ""Description"", ""CreationDate"", 
                                                    ""NeedDate"", ""Status"", ""RequestedSkillId"")
                        VALUES (@RequestorCI, @Description, @CreationDate, @NeedDate, @Status, @RequestedSkillId)";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@RequestorCI", userCI);
                command.Parameters.AddWithValue("@Description", parameters.Description);
                command.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                command.Parameters.AddWithValue("@NeedDate", parameters.NeedDate);
                command.Parameters.AddWithValue("@Status", "Pendiente");
                command.Parameters.AddWithValue("@RequestedSkillId", parameters.RequestedSkillId);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> UpdateNeedAsync(int needId, UpdateNeedParameters parameters)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        UPDATE public.""Need""
                        SET ""Description"" = @Description,
                            ""NeedDate"" = @NeedDate,
                            ""RequestedSkillId"" = @RequestedSkillId
                        WHERE ""Id"" = @NeedId;";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);
                command.Parameters.AddWithValue("@Description", parameters.Description);
                command.Parameters.AddWithValue("@NeedDate", parameters.NeedDate);
                command.Parameters.AddWithValue("@RequestedSkillId", parameters.RequestedSkillId);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> DeleteNeedAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            DELETE
                            FROM public.""Need""
                            WHERE ""Id"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> UpdateAppliersListAsync(int needId, string[] appliersCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        UPDATE public.""Need""
                        SET ""AppliersCI"" = @AppliersCI
                        WHERE ""Id"" = @NeedId;";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);
                command.Parameters.AddWithValue("@AppliersCI", appliersCI);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> AcceptApplierAsync(int needId, string applierCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        UPDATE public.""Need""
                        SET ""AcceptedApplierCI"" = @AcceptedApplierCI,
                            ""AcceptedDate"" = @AcceptedDate
                        WHERE ""Id"" = @NeedId;";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);
                command.Parameters.AddWithValue("@AcceptedApplierCI", applierCI);
                command.Parameters.AddWithValue("@AcceptedDate", DateTime.Now);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> ChangeStatusAsync(int needId, string status)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        UPDATE public.""Need""
                        SET ""Status"" = @Status
                        WHERE ""Id"" = @NeedId;";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);
                command.Parameters.AddWithValue("@Status", status);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        #endregion

        #region Private Methods

        private async Task<NeedData> NeedDataBuilderAsync(NpgsqlDataReader reader)
        {
            var need = new NeedData
            {
                Id = (int)reader["Id"],
                RequestorCI = (string)reader["RequestorCI"],
                AppliersCI = reader.IsDBNull(reader.GetOrdinal("AppliersCI")) ? null : (IEnumerable<string?>)reader["AppliersCI"],
                AcceptedApplierCI = reader.IsDBNull(reader.GetOrdinal("AcceptedApplierCI")) ? null : (string?)reader["AcceptedApplierCI"],
                Status = (string)reader["Status"],
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : (string?)reader["Description"],
                CreationDate = (DateTime)reader["CreationDate"],
                NeedDate = reader.IsDBNull(reader.GetOrdinal("NeedDate")) ? null : (DateTime?)reader["NeedDate"],
                AcceptedDate = reader.IsDBNull(reader.GetOrdinal("AcceptedDate")) ? null : (DateTime?)reader["AcceptedDate"],
                RequestedSkillId = (int)reader["RequestedSkillId"],
            };

            return need;
        }

        #endregion
    }
}
