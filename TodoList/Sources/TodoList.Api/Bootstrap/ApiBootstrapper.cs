using System.Net.Http;
using System.Web;
using Microsoft.Practices.Unity;
using TodoList.Api.Services;
using TodoList.Contracts.Bootstrap;
using TodoList.Contracts.Repositories;

namespace TodoList.Api.Bootstrap
{
    internal class ApiBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container
                .RegisterType<HttpRequestMessage>(
                    new HierarchicalLifetimeManager(), 
                    new InjectionFactory(_ => GetCurrentContextRequestMessage()))
                .RegisterType<IListItemUrlGenerator, ListItemUrlGenerator>(new HierarchicalLifetimeManager())
                .RegisterType<IConnectionDetails, ConnectionDetails>(new ContainerControlledLifetimeManager());

        private HttpRequestMessage GetCurrentContextRequestMessage() 
            => (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
    }
}