using System.Web.Http;
using Microsoft.Practices.Unity;
using TodoList.Api.Resolvers;
using TodoList.Repositories;

namespace TodoList.Api
{
    internal static class UnityResolverConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .RegisterRepositoryTypes();
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}