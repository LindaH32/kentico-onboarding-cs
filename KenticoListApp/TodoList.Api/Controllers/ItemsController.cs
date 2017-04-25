using System;
using System.Collections.Generic;
using System.Web.Http;
using TodoList.Api.Models;

namespace TodoList.Api.Controllers
{
    public class ItemsController : ApiController
    {
        private static readonly List<ListItem> SampleItems = new List<ListItem>
            {
                new ListItem(new Guid("00000000-0000-0000-0000-000000000000"), "text"),
                new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe"),
                new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated"),
            };

        
        public IHttpActionResult Post(ListItem item)
        {
            if(item == null)
                return BadRequest("Item is null");

            if (string.IsNullOrWhiteSpace(item.Text))
                return BadRequest("Text is null");

            return Ok(SampleItems[0]);
        }


        public IHttpActionResult Get()
        {
            return Ok(SampleItems);
        }


        public IHttpActionResult Get(Guid id)
        {
            return Ok(SampleItems[1]);
        }

        
        public IHttpActionResult Delete(Guid id)
        {
            return Ok(SampleItems[1]);
        }


        public IHttpActionResult Put(ListItem item)
        {
            return Ok(SampleItems[2]);
        }
    }
}
