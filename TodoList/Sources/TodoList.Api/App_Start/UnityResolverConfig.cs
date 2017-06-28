using System.Web.Http;
using Microsoft.Practices.Unity;
using TodoList.Api.Bootstrap;
using TodoList.Api.Resolvers;
using TodoList.Contracts.Bootstrap;
using TodoList.Repositories;
using TodoList.Services.Bootstrap;

namespace TodoList.Api
{
    internal static class UnityResolverConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer()
                .RegisterTypes<RepositoryBootstrapper>()
                .RegisterTypes<ApiBootstrapper>()
                .RegisterType<ServicesBootstrapper>();
                
            config.DependencyResolver = new UnityResolver(container);
        }

        private static IUnityContainer RegisterTypes<TBootstrapper>(this IUnityContainer container)
            where TBootstrapper : IBootstrapper, new()
            => new TBootstrapper().Register(container);
    }
}