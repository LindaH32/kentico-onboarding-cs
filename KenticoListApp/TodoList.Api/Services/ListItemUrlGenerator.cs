using System.Web.Http.Routing;
using TodoList.Contracts.Models;

namespace TodoList.Api.Services
{
    public class ListItemUrlGenerator
    {
        private readonly UrlHelper _helper;

        public ListItemUrlGenerator(UrlHelper helper)
        {
            _helper = helper;
        }

        public string GenerateUrl(ListItem item) 
            => _helper.Route("DefaultApiV1/", new { id = item.Id });
    }
}