using System.Configuration;
using TodoList.Contracts.Repositories;

namespace TodoList.Api.Bootstrap
{
    internal class ConnectionDetails : IConnectionDetails
    {
        private const string DefaultConnectionStringName = "DefaultConnection";

        public string ConnectionString { get; } = ConfigurationManager
            .ConnectionStrings[DefaultConnectionStringName]
            .ConnectionString;
    }
}