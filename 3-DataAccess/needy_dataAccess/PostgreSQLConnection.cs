using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

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

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}
