using System.Configuration;
using TodoList.Contracts.Repositories;

namespace TodoList.Api.Bootstrap
{
    internal class ConnectionDetails : IConnectionDetails
    {
        private const string DefaultConnectionStringName = "DefaultConnection";

        public string ConnectionString => _connectionString;

        private readonly string _connectionString = ConfigurationManager
            .ConnectionStrings[DefaultConnectionStringName]
            .ConnectionString;
    }
}