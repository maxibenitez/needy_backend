using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction.Parameters;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needy_dataAccess.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders

        public SkillRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implments ISkillRepository

        public async Task<IEnumerable<Skill>> GetSkillsAsync()
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""Skill""";

                var command = new NpgsqlCommand(query, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var skills = new List<Skill>();

                    while (await reader.ReadAsync())
                    {
                        skills.Add(await SkillBuilderAsync(reader));
                    }

                    return skills;
                }
            }
        }

        public async Task<Skill> GetSkillByIdAsync(int skillId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""Skill""
                            WHERE ""Id"" = @Id";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", skillId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return await SkillBuilderAsync(reader);
                    }

                    return null;
                }
            }
        }

        #endregion

        #region Private Methods

        private async Task<Skill> SkillBuilderAsync(NpgsqlDataReader reader)
        {
            var skill = new Skill
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
            };

            return skill;
        }

        #endregion
    }
}
