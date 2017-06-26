using System.Net.Http;
using System.Web.Http.Routing;
using TodoList.Contracts.Models;

namespace TodoList.Api.Services
{
    internal class ListItemUrlGenerator : IListItemUrlGenerator
    {
        private readonly UrlHelper _helper;

        public ListItemUrlGenerator(HttpRequestMessage message)
        {
            _helper = new UrlHelper(message);
        }

        public string GenerateUrl(ListItem item) 
            => _helper.Route("DefaultApiV1", new { id = item.Id });
    }
}