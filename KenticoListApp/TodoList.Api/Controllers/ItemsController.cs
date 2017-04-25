using System.Collections.Generic;
using System.Web.Http;
using TodoList.Api.Models;

namespace TodoList.Api.Controllers
{
    public class ItemsController : ApiController
    {
        // TODO change List to IEnumerable
        private static readonly List<ListItem> SampleItems = new List<ListItem>
            {
                new ListItem("42", "text"),
                new ListItem("666", "giraffe"),
                new ListItem("2", "updated"),
            };

        
        public IHttpActionResult Post(string text)
        {
            if (text == null)
            {
                return BadRequest("Text is null");
            }
            return Ok(SampleItems[0]);
        }


        public IHttpActionResult Get()
        {
            return Ok(SampleItems);
        }


        public IHttpActionResult Get(string id)
        {
            return Ok(SampleItems[1]);
        }

        
        public IHttpActionResult Delete(string id)
        {
            return Ok("Item deleted");
        }


        public IHttpActionResult Put(string id, string text)
        {
            return Ok(SampleItems[2]);
        }
    }
}
