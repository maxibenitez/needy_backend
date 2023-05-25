using Microsoft.Extensions.Configuration;
using Npgsql;

namespace needy_dataAccess
{
    public class PostgreSQLConnection
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PostgreSQLConnection(IConfiguration configuration) 
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("PostgreSqlConnection");
        }

        public NpgsqlConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}
