using System.Web.Http;
using Microsoft.Practices.Unity;
using TodoList.Api.Resolvers;
using TodoList.Repositories;

namespace TodoList.Api
{
    public static class UnityResolverConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterRepositoryTypes();
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}