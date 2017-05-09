using System.Web.Http;
using Microsoft.Practices.Unity;
using TodoList.Api.Controllers;
using TodoList.Contracts.Interfaces;
using TodoList.Repositories;

namespace TodoList.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            UnityContainer container = new UnityContainer();
            ContainerBootstrapper.RegisterTypes(container);
            ListItemsController controller = container.Resolve<ListItemsController>();
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApiV1",
                "api/v1/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );
        }
    }
}