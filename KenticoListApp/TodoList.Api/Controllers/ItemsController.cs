using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using TodoList.Api.Models;

namespace TodoList.Api.Controllers
{
    public class ItemsController : ApiController
    {
        // TODO change List to IEnumerable
        private static List<ListItem> sampleItems = new List<ListItem>(
            new ListItem[]
            {
                new ListItem("42", "text"),
                new ListItem("666", "zirafa"),
                new ListItem("2", "updated"),
            }
        );


        [HttpPost]
        public IHttpActionResult AddItem(string text)
        {
            if (text == null)
            {
                return BadRequest("Text is null");
            }
            return Ok(sampleItems[0]);
        }


        [HttpGet]
        public IHttpActionResult GetItems()
        {
            return Ok(sampleItems);
        }


        [HttpGet]
        public IHttpActionResult GetItem(string id)
        {
            return Ok(sampleItems[1]);
        }


        [HttpDelete]
        public IHttpActionResult DeleteItem(string id)
        {
            return Ok("Item deleted");
        }


        [HttpPut]
        public IHttpActionResult UpdateItem(string id, string text)
        {
            return Ok(sampleItems[2]);
        }
    }
}
