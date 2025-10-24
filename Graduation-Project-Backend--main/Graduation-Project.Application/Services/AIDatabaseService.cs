using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Graduation_Project.Application.Services
{
    public class AIDatabaseService
    {
        private readonly string _connectionString;

        public AIDatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> ExecuteQueryAsync(string sqlQuery)
        {
            await using var connection = new SqlConnection(_connectionString);
            var results = await connection.QueryAsync(sqlQuery);
            return System.Text.Json.JsonSerializer.Serialize(results); 
        }
    }
}
