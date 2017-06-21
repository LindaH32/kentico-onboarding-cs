using System.Net.Http;
using System.Web;
using Microsoft.Practices.Unity;
using TodoList.Api.Services;
using TodoList.Contracts.Api;
using IListItemUrlGenerator = TodoList.Api.Services.IListItemUrlGenerator;

namespace TodoList.Api
{
    internal class ApiBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container
            .RegisterType<HttpRequestMessage>(
                new HierarchicalLifetimeManager(), 
                new InjectionFactory(_ => GetCurrentContextRequestMessage()))
            .RegisterType<IListItemUrlGenerator, ListItemUrlGenerator>(new HierarchicalLifetimeManager());

        private HttpRequestMessage GetCurrentContextRequestMessage() 
            => (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
    }
}