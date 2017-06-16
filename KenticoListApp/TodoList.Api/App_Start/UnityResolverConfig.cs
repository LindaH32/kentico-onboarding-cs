using System.Web.Http;
using Microsoft.Practices.Unity;
using TodoList.Api.Resolvers;
using TodoList.Contracts.Interfaces;
using TodoList.Repositories;

namespace TodoList.Api
{
    internal static class UnityResolverConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .RegisterTypes<RepositoryBootstrapper>();
                
            config.DependencyResolver = new UnityResolver(container);
        }

        private static IUnityContainer RegisterTypes<TBootstrapper>(this IUnityContainer container)
            where TBootstrapper : IBootstrapper, new()
            => new TBootstrapper().Register(container);
    }
}