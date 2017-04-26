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
            new ListItem(new Guid("00000000-0000-0000-0000-000000000000"), "text"),
            new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe"),
            new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated"),
        };
                
        public async Task<IHttpActionResult> Post(ListItem item)
        {
            if (item == null)
            {
                return BadRequest("Item is null");
            }

            if (item.Id != Guid.Empty)
            {
                return BadRequest("Guid must be empty");
            }

            if (string.IsNullOrWhiteSpace(item.Text))
            {
                return BadRequest("Text is null or empty");
            }

            return Ok(await Task.FromResult(SampleItems[0]));
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
