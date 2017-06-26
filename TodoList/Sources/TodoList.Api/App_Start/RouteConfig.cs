using System.Web.Http;

namespace TodoList.Api
{
    internal static class RouteConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApiV1",
                "api/v1/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}