using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TodoList.Api.Models;

namespace TodoList.Api.Controllers
{
    public class ListItemsController : ApiController
    {
        private static readonly List<ListItem> SampleItems = new List<ListItem>
        {
            new ListItem(new Guid("98DBDE18-639E-49A6-8E51-603CEB2AE92D"), "text"),
            new ListItem(new Guid("1C353E0A-5481-4C31-BD2E-47E1BAF84DBE"), "giraffe"),
            new ListItem(new Guid("D69E065C-99B1-4A73-B00C-AD05F071861F"), "updated")
        };

        public async Task<IHttpActionResult> PostAsync(ListItem item)
        {
            if (item == null)
                return BadRequest("Item is null");

            if (string.IsNullOrWhiteSpace(item.Text))
                return BadRequest("Text is null or empty");

            if (item.Id != Guid.Empty)
                return BadRequest("Guid must be empty");

            return Created("api/v1/items/?id=300...", await Task.FromResult(SampleItems[0]));
        }

        public async Task<IHttpActionResult> GetAsync()
            => Ok(await Task.FromResult(SampleItems));

        public async Task<IHttpActionResult> GetAsync(Guid id)
            => Ok(await Task.FromResult(SampleItems[0]));

        public async Task<IHttpActionResult> DeleteAsync(Guid id)
            => Ok(await Task.FromResult(SampleItems[1]));

        public async Task<IHttpActionResult> PutAsync(ListItem item)
            => Ok(await Task.FromResult(SampleItems[2]));
    }
}