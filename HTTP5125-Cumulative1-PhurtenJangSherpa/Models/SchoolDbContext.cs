using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace School.Models
{
    public class SchoolDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SchoolDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SchoolDb");
        }

        /// <summary>
        /// returns a MySQL connection object connected to the school database
        /// </summary>
        /// <returns>MySqlConnection</returns>
        public MySqlConnection AccessDatabase()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
