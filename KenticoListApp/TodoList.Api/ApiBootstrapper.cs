using System.Net.Http;
using System.Web;
using Microsoft.Practices.Unity;
using TodoList.Api.Services;
using TodoList.Contracts.Interfaces;

namespace TodoList.Api
{
    internal class ApiBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container
                .RegisterType<HttpRequestMessage>(
                    new InjectionFactory(_ => (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"]))
                .RegisterType<IListItemUrlGenerator, ListItemUrlGenerator>();
    }
}