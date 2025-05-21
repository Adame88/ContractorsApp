using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ContractorsApp.Data
{

    internal class DatabaseFactory
    {
        private static readonly string _connectionString;

        static DatabaseFactory()
        {
            _connectionString = GetConnectionString();
        }


        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // dla konsoli/WinForms
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        private static string GetConnectionString(string name = "DefaultConnection")
        {
            var config = GetConfiguration();
            string? connectionString = config.GetConnectionString(name);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string '{name}' is not found or is empty.");
            }
            return connectionString;
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

    }
}

