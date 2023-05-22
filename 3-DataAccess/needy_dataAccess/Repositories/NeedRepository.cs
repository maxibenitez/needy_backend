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

                string query = @"
                            SELECT n.*
                            FROM public.""Need"" n";

                var command = new NpgsqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    var needDataList = new List<NeedData>();

                    while (await reader.ReadAsync())
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

                        needDataList.Add(need);
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

                string query = @"
                            SELECT n.*
                            FROM public.""Need"" n
                            WHERE ""RequestedSkillId"" = @skillId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@skillId", skillId);

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    var needDataList = new List<NeedData>();

                    while (await reader.ReadAsync())
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

                        needDataList.Add(need);
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

                string query = @"
                            SELECT n.*
                            FROM public.""Need"" n
                            WHERE n.""Id"" = @needId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

                using (var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    if (await reader.ReadAsync())
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

                     return null;
                }
            }
        }

        public async Task<bool> InsertNeedAsync(InsertNeedParameters parameters)
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"
                            INSERT INTO public.""Need"" (""RequestorCi"", ""Description"", ""CreationDate"", ""NeedDate"",
                                                        ""Status"", ""RequestedSkillId"")
                            VALUES (@RequestorCi, @Description, @CreationDate, @NeedDate, @Status, @RequestedSkillId)";

            var result = await connection.ExecuteAsync(query, new
            {
                //tomar de la sesion la ci
                parameters.Description,
                DateTime.Now,
                parameters.NeedDate,
                //status
                parameters.RequestedSkillId
            });

            return result > 0;
        }

        public async Task<bool> UpdateNeedAsync(int needId, UpdateNeedParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteNeedAsync(int needId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ApplyNeedAsync(int needId, string applierCi)
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"
                            INSERT INTO public.""NeedApply"" (""NeedId"", ""ApplierCi"")
                            VALUES (@NeedId, @ApplierCi)";

            var result = await connection.ExecuteAsync(query, new
            {
                //tomar de la sesion la ci
                //parameters.Description,
                DateTime.Now,
                //parameters.NeedDate,
                //id de status
                //parameters.RequestedSkillId
            });

            return result > 0;
        }

        public async Task<bool> UnapplyNeedAsync(int needId, string applierCi)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AcceptApplierAsync(int needId, string applierCi)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeclineApplierAsync(int needId, string applierCi)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangeStatusAsync(int needId, string status)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
