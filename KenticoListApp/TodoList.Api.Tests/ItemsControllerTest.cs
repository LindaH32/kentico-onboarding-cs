using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using System.Web.Http;
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
        public void Get_ById_ReturnsTestItem()
        {
            ListItem expected = new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe");

            IHttpActionResult result = _controller.Get(Guid.Empty);

            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Get_ById_IsOfCorrectStatusCode()
        {
            IHttpActionResult result = _controller.Get(Guid.Empty);
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);

            Assert.That(action.Result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Get_ReturnsTestItems()
        {
            IHttpActionResult result = _controller.Get();

            List<ListItem> expected = new List<ListItem>
            {
                new ListItem(new Guid("00000000-0000-0000-0000-000000000000"), "text"),
                new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe"),
                new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated"),
            };
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            List<ListItem> actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Post_WithNullArguments_ReturnsErrorMessage()
        {
            IHttpActionResult result = _controller.Post(null);

            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            HttpError error;
            action.Result.TryGetContentValue(out error);
            string actual = error.Message;

            Assert.That(actual, Is.EqualTo("Item is null"));
        }

        [Test]
        public void Post_WithNullArguments_IsOfCorrectStatusCode()
        {
            IHttpActionResult result = _controller.Post(null);
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);

            Assert.That(action.Result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_ItemWithNullText_ReturnsErrorMessage()
        {
            ListItem item = new ListItem(Guid.Empty);

            IHttpActionResult result = _controller.Post(item);
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            HttpError error;
            action.Result.TryGetContentValue(out error);
            string actual = error.Message;

            Assert.That(actual, Is.EqualTo("Text is null"));
        }

        [Test]
        public void Post_ItemWithNullText_IsOfCorrectStatusCode()
        {
            ListItem item = new ListItem(Guid.Empty);

            IHttpActionResult result = _controller.Post(item);
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);

            Assert.That(action.Result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_WithValidArguments_ReturnsCorrectItem()
        {
            ListItem expected = new ListItem(new Guid("00000000-0000-0000-0000-000000000000"), "text");
            ListItem newItem = new ListItem(Guid.Empty, "newText");
            IHttpActionResult result = _controller.Post(newItem);

            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Post_WithValidArguments_IsOfCorrectStatusCode()
        {
            ListItem newItem = new ListItem(Guid.Empty, "newText");

            IHttpActionResult result = _controller.Post(newItem);
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);

            Assert.That(action.Result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        [Test]
        public void Put_ReturnsCorrectItem()
        {
            ListItem expected = new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated");
            ListItem updated = new ListItem(Guid.Empty, "newText");

            IHttpActionResult result = _controller.Put(updated);
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Put_IsOfCorrectStatusCode()
        {
            ListItem updated = new ListItem(Guid.Empty, "newText");

            IHttpActionResult result = _controller.Put(updated);
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);

            Assert.That(action.Result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Delete_ReturnsCorrectItem()
        {
            IHttpActionResult result = _controller.Delete(Guid.Empty);
            ListItem expected = new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe");

            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual); 

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Delete_IsOfCorrectStatusCode()
        {
            IHttpActionResult result = _controller.Delete(Guid.Empty);
            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);

            Assert.That(action.Result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
        


    }
}
