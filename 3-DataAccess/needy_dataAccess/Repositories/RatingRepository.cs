using Dapper;
using needy_dataAccess.Interfaces;
using needy_dto;
using needy_logic_abstraction.Parameters;
using Npgsql;

namespace needy_dataAccess.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        #region Properties and Fields

        private readonly PostgreSQLConnection _dbConnection;

        #endregion

        #region Builders

        public RatingRepository(PostgreSQLConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #endregion

        #region Implments IRatingRepository

        public async Task<IEnumerable<RatingData>> GetUserRatingsAsync(string userCI)
        {
            using (var connection = _dbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT *
                            FROM public.""Rating""
                            WHERE ""ReceiverCI"" = @ReceiverCI";

                var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReceiverCI", userCI);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var ratings = new List<RatingData>();

                    while (await reader.ReadAsync())
                    {
                        ratings.Add(await RatingBuilderAsync(reader));
                    }

                    return ratings;
                }
            }
        }

        public async Task<bool> InsertRatingAsync(InsertRatingParameters parameters)
        {
            using var connection = _dbConnection.CreateConnection();

            var query = @"
                            INSERT INTO public.""Rating"" (""CiRequestor"", ""CiHelper"", ""Rating"", ""Comment"")
                            VALUES (@CiRequestor, @CiHelper, @Rating, @Comment)";

            var result = await connection.ExecuteAsync(query, new
            {
                //parameters.CiRequestor,
                //parameters.CiHelper,
                //parameters.Rating,
                parameters.Comment
            });

            return result > 0;
        }

        #endregion

        #region Private Methods

        private async Task<RatingData> RatingBuilderAsync(NpgsqlDataReader reader)
        {
            var rating = new RatingData
            {
                Id = (int)reader["Id"],
                GiverCI = (string)reader["GiverCI"],
                ReceiverCI = (string)reader["ReceiverCI"],
                NeedId = (int)reader["NeedId"],
                Stars = (double)reader["Stars"],
                Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : (string?)reader["Comment"],
            };

            return rating;
        }

        #endregion
    }
}
