using needy_dataAccess.Interfaces;
using needy_dto;
using Npgsql;

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

        public async Task<IEnumerable<Skill>> GetUserSkillsAsync(string userCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT s.""Id"", s.""Name""
                            FROM public.""UserSkill"" u
                            INNER JOIN public.""Skill"" s ON u.""SkillId"" = s.""Id""
                            WHERE u.""UserCI"" = @UserCI";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserCI", userCI);

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

        public async Task<IEnumerable<Skill>> GetNeedSkillsAsync(int needId)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT s.""Id"", s.""Name""
                            FROM public.""NeedSkill"" n
                            INNER JOIN public.""Skill"" s ON n.""SkillId"" = s.""Id""
                            WHERE n.""NeedId"" = @NeedId";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@NeedId", needId);

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
