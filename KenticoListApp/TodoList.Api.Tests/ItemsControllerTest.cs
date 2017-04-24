using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using TodoList.Api.Comparers;


namespace TodoList.Api.Tests
{
    [TestFixture]
    public class ItemsControllerTest
    {
        ListItemComparer comparer;
        private ItemsController controller ;


        [SetUp]
        public void Init()
        {
            comparer = new ListItemComparer();
            controller = new ItemsController();

            controller.Configuration = new HttpConfiguration();
            controller.Request = new HttpRequestMessage();
        }

        [Test]
        public void GetItemById_ReturnsTestItem()
        {
            ListItem expected = new ListItem("666", "zirafa");

            IHttpActionResult result = controller.GetItem("4");

            var action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(comparer));
        }

        [Test]
        public void GetItemById_IsOfCorrectType()
        {
            IHttpActionResult result = controller.GetItem("4");

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }

        [Test]
        public void GetItems_ReturnsTestItems()
        {
            IHttpActionResult result = controller.GetItems();

            List<ListItem> expected = new List<ListItem>(new ListItem[]
            {
                new ListItem("42", "text"),
                new ListItem("666", "zirafa"),
                new ListItem("2", "updated"),
            });
            var action = result.ExecuteAsync(CancellationToken.None);
            List<ListItem> actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(comparer));
        }

        [Test]
        public void AddItem_WithNullArguments_ReturnsErrorMessage()
        {
            IHttpActionResult result = controller.AddItem(null);

            var action = result.ExecuteAsync(CancellationToken.None);
            string actual;
            action.Result.Content.ToString(); //BadRequest doesnt have value?

            Assert.That(action.Result.Content.ToString(), Is.EqualTo("text is null"));
        }

        [Test]
        public void AddItem_WithNullArguments_IsOfCorrectType()
        {
            IHttpActionResult result = controller.AddItem(null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void AddItem_WithValidArguments_ReturnsCorrectItem()
        {
            ListItem expected = new ListItem("42", "text");

            IHttpActionResult result = controller.AddItem("4");

            var action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(comparer));
        }

        [Test]
        public void AddItem_WithValidArguments_IsOfCorrectType()
        {
            IHttpActionResult result = controller.AddItem("5");

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }


        [Test]
        public void UpdateItem_ReturnsCorrectItem()
        {
            ListItem expected = new ListItem("2", "updated");

            IHttpActionResult result = controller.UpdateItem("4","randomText");

            var action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(comparer));
        }

        [Test]
        public void UpdateItem_IsOfCorrectType()
        {
            IHttpActionResult result = controller.UpdateItem("5","newText");

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }

        [Test]
        public void DeleteItem_ReturnsCorrectMessage()
        {
            IHttpActionResult result = controller.DeleteItem("5");

            var action = result.ExecuteAsync(CancellationToken.None);
            string actual;
            action.Result.TryGetContentValue(out actual); //BadRequest doesnt have value?

            Assert.That(actual, Is.EqualTo("Item deleted"));
        }

        [Test]
        public void DeleteItem_IsOfCorrectType()
        {
            IHttpActionResult result = controller.DeleteItem("5");

            Assert.IsInstanceOf<OkNegotiatedContentResult<string>>(result);
        }
        


    }
}
