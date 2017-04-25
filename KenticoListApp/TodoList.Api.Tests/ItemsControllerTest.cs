using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using TodoList.Api.Tests.Comparers;


namespace TodoList.Api.Tests
{
    [TestFixture]
    public class ItemsControllerTest
    {
        private ListItemComparer _comparer;
        private ItemsController _controller;

        [SetUp]
        public void Init()
        {
            _comparer = new ListItemComparer();
            _controller = new ItemsController
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };

        }

        [Test]
        public void GetItemById_ReturnsTestItem()
        {
            ListItem expected = new ListItem("666", "giraffe");

            IHttpActionResult result = _controller.Get("4");

            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void GetItemById_IsOfCorrectType()
        {
            IHttpActionResult result = _controller.Get("4");

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }

        [Test]
        public void GetItems_ReturnsTestItems()
        {
            IHttpActionResult result = _controller.Get();

            List<ListItem> expected = new List<ListItem>
            {
                new ListItem("42", "text"),
                new ListItem("666", "giraffe"),
                new ListItem("2", "updated"),
            };
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            List<ListItem> actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void AddItem_WithNullArguments_ReturnsErrorMessage()
        {
            IHttpActionResult result = _controller.Post(null);

            var action = result.ExecuteAsync(CancellationToken.None);
            HttpError error;
            action.Result.TryGetContentValue(out error);
            string actual = error.Message;

            Assert.That(actual, Is.EqualTo("Text is null"));
        }

        [Test]
        public void AddItem_WithNullArguments_IsOfCorrectType()
        {
            IHttpActionResult result = _controller.Post(null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void AddItem_WithValidArguments_ReturnsCorrectItem()
        {
            ListItem expected = new ListItem("42", "text");

            IHttpActionResult result = _controller.Post("4");

            var action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void AddItem_WithValidArguments_IsOfCorrectType()
        {
            IHttpActionResult result = _controller.Post("5");

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }


        [Test]
        public void UpdateItem_ReturnsCorrectItem()
        {
            ListItem expected = new ListItem("2", "updated");

            IHttpActionResult result = _controller.Put("4","randomText");

            var action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void UpdateItem_IsOfCorrectType()
        {
            IHttpActionResult result = _controller.Put("5","newText");

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }

        [Test]
        public void DeleteItem_ReturnsCorrectMessage()
        {
            IHttpActionResult result = _controller.Delete("5");

            var action = result.ExecuteAsync(CancellationToken.None);
            string actual;
            action.Result.TryGetContentValue(out actual); 

            Assert.That(actual, Is.EqualTo("Item deleted"));
        }

        [Test]
        public void DeleteItem_IsOfCorrectType()
        {
            IHttpActionResult result = _controller.Delete("5");

            Assert.IsInstanceOf<OkNegotiatedContentResult<string>>(result);
        }
        


    }
}
