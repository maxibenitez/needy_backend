using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction.Parameters;
using Npgsql;

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
                            FROM public.""Needs""";

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
                            FROM public.""Needs""
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

        public async Task<IEnumerable<NeedData>> GetNeedsBySkillNameAsync(string skillName)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = "";

                if (!string.IsNullOrEmpty(skillName))
                {
                    query = @"
                            SELECT n.*
                            FROM public.""Needs"" n
                            INNER JOIN public.""NeedsSkills"" ns ON n.""Id"" = ns.""NeedId""
                            INNER JOIN public.""Skills"" s ON ns.""SkillId"" = s.""Id""
                            WHERE s.""Name"" LIKE '%' || @SkillName || '%'";
                }
                else
                {
                    query = @"
                            SELECT n.*
                            FROM public.""Needs"" n
                            INNER JOIN public.""NeedsSkills"" ns ON n.""Id"" = ns.""NeedId""
                            INNER JOIN public.""Skills"" s ON ns.""SkillId"" = s.""Id""";
                }

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@SkillName", skillName);

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
                            FROM public.""Needs""
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

        public async Task<IEnumerable<string>> GetNeedAppliersAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT ""ApplierCI""
                            FROM public.""NeedsAppliers""
                            WHERE ""NeedId"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var appliersList = new List<string>();

                    while (await reader.ReadAsync())
                    {
                        appliersList.Add((string)reader["ApplierCI"]);
                    }

                    return appliersList;
                }
            }
        }

        public async Task<string> GetNeedRequestorAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT ""RequestorCI""
                            FROM public.""Needs""
                            WHERE ""Id"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    string requestorCI = null;

                    if (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            requestorCI = reader.GetString(0);
                        }
                    }

                    return requestorCI;
                }
            }
        }

        public async Task<string> GetNeedAcceptedApplierAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT ""AcceptedApplierCI""
                            FROM public.""Needs""
                            WHERE ""Id"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    string acceptedApplierCI = null;

                    if (await reader.ReadAsync())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            acceptedApplierCI = reader.GetString(0);
                        }
                    }

                    return acceptedApplierCI;
                }
            }
        }

        public async Task<IEnumerable<NeedData>> GetUserCreatedNeedsAsync(string userCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""Needs""
                            WHERE ""RequestorCI"" = @UserCI";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserCI", userCI);

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

        public async Task<IEnumerable<NeedData>> GetUserAppliedNeedsAsync(string userCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT n.*
                            FROM public.""Needs"" n
                            INNER JOIN public.""NeedsAppliers"" a ON n.""Id"" = a.""NeedId""
                            WHERE a.""ApplierCI"" = @UserCI";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserCI", userCI);

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

        public async Task<int> InsertNeedAsync(string userCI, InsertNeedParameters parameters)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var query = @"
                        INSERT INTO public.""Needs"" (""Title"", ""RequestorCI"", ""Description"", ""CreationDate"", 
                                                    ""NeedDate"", ""Status"", ""Modality"", ""NeedAddress"")
                        VALUES (@Title, @RequestorCI, @Description, @CreationDate, @NeedDate, @Status, @Modality, @NeedAddress);
                        SELECT LASTVAL();";

                        var command = new NpgsqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Title", parameters.Title);
                        command.Parameters.AddWithValue("@RequestorCI", userCI);
                        command.Parameters.AddWithValue("@Description", parameters.Description);
                        command.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                        command.Parameters.AddWithValue("@NeedDate", parameters.NeedDate);
                        command.Parameters.AddWithValue("@Status", "Waiting");
                        command.Parameters.AddWithValue("@Modality", parameters.Modality);
                        command.Parameters.AddWithValue("@NeedAddress", parameters.NeedAddress);

                        var id = await command.ExecuteScalarAsync();

                        transaction.Commit();

                        return Convert.ToInt32(id);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        public async Task<bool> InsertNeedSkillAsync(int needId, int skillId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        INSERT INTO public.""NeedsSkills"" (""SkillId"", ""NeedId"")
                        VALUES (@SkillId, @NeedId)";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);
                command.Parameters.AddWithValue("@SkillId", skillId);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> UpdateNeedAsync(UpdateNeedParameters parameters)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        UPDATE public.""Needs""
                        SET ""Title"" = @Title,
                            ""Description"" = @Description,
                            ""Modality"" = @Modality,
                            ""NeedDate"" = @NeedDate
                        WHERE ""Id"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", parameters.NeedId);
                command.Parameters.AddWithValue("@Title", parameters.Title);
                command.Parameters.AddWithValue("@Description", parameters.Description);
                command.Parameters.AddWithValue("@Modality", parameters.Modality);
                command.Parameters.AddWithValue("@NeedDate", parameters.NeedDate);

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
                            FROM public.""Needs""
                            WHERE ""Id"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> DeleteNeedSkillsAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            DELETE
                            FROM public.""NeedsSkills""
                            WHERE ""NeedId"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> DeleteNeedAppliersAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            DELETE
                            FROM public.""NeedsAppliers""
                            WHERE ""NeedId"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> ApplyNeedAsync(int needId, string applierCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        INSERT INTO public.""NeedsAppliers"" (""NeedId"", ""ApplierCI"")
                        VALUES (@NeedId, @ApplierCI)";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);
                command.Parameters.AddWithValue("@ApplierCI", applierCI);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> DeleteNeedApplierAsync(int needId, string applierCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            DELETE
                            FROM public.""NeedsAppliers""
                            WHERE ""NeedId"" = @NeedId AND ""ApplierCI"" = @ApplierCI";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);
                command.Parameters.AddWithValue("@ApplierCI", applierCI);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
        }

        public async Task<bool> AcceptApplierAsync(ManageApplierParameters parameters)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                        UPDATE public.""Needs""
                        SET ""AcceptedApplierCI"" = @AcceptedApplierCI,
                            ""AcceptedDate"" = @AcceptedDate
                        WHERE ""Id"" = @NeedId;";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", parameters.NeedId);
                command.Parameters.AddWithValue("@AcceptedApplierCI", parameters.ApplierCI);
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
                        UPDATE public.""Needs""
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
                Title = (string)reader["Title"],
                RequestorCI = (string)reader["RequestorCI"],
                AcceptedApplierCI = reader.IsDBNull(reader.GetOrdinal("AcceptedApplierCI")) ? null : (string?)reader["AcceptedApplierCI"],
                Status = (string)reader["Status"],
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : (string?)reader["Description"],
                CreationDate = (DateTime)reader["CreationDate"],
                NeedDate = (DateTime)reader["NeedDate"],
                AcceptedDate = reader.IsDBNull(reader.GetOrdinal("AcceptedDate")) ? null : (DateTime?)reader["AcceptedDate"],
                NeedAddress = (string)reader["NeedAddress"],
                Modality = (string)reader["Modality"],
            };

            return need;
        }

        #endregion
    }
}
