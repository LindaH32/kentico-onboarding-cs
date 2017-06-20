using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace TodoList.Api
{
    public static class ResponseConfig
    {
        internal static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }
    }
}