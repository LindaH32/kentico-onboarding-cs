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

            IHttpActionResult result = _controller.Get(new Guid("10000000-0000-0000-0000-000000000000"));

            Task<HttpResponseMessage> action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Get_ById_IsOfCorrectType()
        {
            IHttpActionResult result = _controller.Get(new Guid("10000000-0000-0000-0000-000000000000"));

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
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

            var action = result.ExecuteAsync(CancellationToken.None);
            HttpError error;
            action.Result.TryGetContentValue(out error);
            string actual = error.Message;

            Assert.That(actual, Is.EqualTo("Item is null"));
        }

        [Test]
        public void Post_WithNullArguments_IsOfCorrectType()
        {
            IHttpActionResult result = _controller.Post(null);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void Post_ItemWithNullText_ReturnsErrorMessage()
        {
            ListItem item = new ListItem(new Guid("85000000-0000-0000-0000-000000000000"));

            IHttpActionResult result = _controller.Post(item);

            var action = result.ExecuteAsync(CancellationToken.None);
            HttpError error;
            action.Result.TryGetContentValue(out error);
            string actual = error.Message;

            Assert.That(actual, Is.EqualTo("Text is null"));
        }

        [Test]
        public void Post_ItemWithNullText_IsOfCorrectType()
        {
            ListItem item = new ListItem(new Guid("85000000-0000-0000-0000-000000000000"));
            IHttpActionResult result = _controller.Post(item);

            Assert.IsInstanceOf<BadRequestErrorMessageResult>(result);
        }

        [Test]
        public void Post_WithValidArguments_ReturnsCorrectItem()
        {
            ListItem expected = new ListItem(new Guid("00000000-0000-0000-0000-000000000000"), "text");
            ListItem newItem = new ListItem(new Guid("08563400-0000-0000-0000-000000000000"), "newText");
            IHttpActionResult result = _controller.Post(newItem);

            var action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Post_WithValidArguments_IsOfCorrectType()
        {
            ListItem newItem = new ListItem(new Guid("08563400-0000-0000-0000-000000000000"), "newText");
            IHttpActionResult result = _controller.Post(newItem);

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }


        [Test]
        public void Put_ReturnsCorrectItem()
        {
            ListItem expected = new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated");
            ListItem updated = new ListItem(new Guid("08563400-0000-0000-0000-000000000000"), "newText");

            IHttpActionResult result = _controller.Put(updated);
            var action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual);

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Put_IsOfCorrectType()
        {
            ListItem updated = new ListItem(new Guid("08563400-0000-0000-0000-000000000000"), "newText");
            IHttpActionResult result = _controller.Put(updated);

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }

        [Test]
        public void Delete_ReturnsCorrectItem()
        {
            IHttpActionResult result = _controller.Delete(new Guid("15000000-0000-0000-0000-000000000000"));
            ListItem expected = new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe");

            var action = result.ExecuteAsync(CancellationToken.None);
            ListItem actual;
            action.Result.TryGetContentValue(out actual); 

            Assert.That(actual, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Delete_IsOfCorrectType()
        {
            IHttpActionResult result = _controller.Delete(new Guid("15000000-0000-0000-0000-000000000000"));

            Assert.IsInstanceOf<OkNegotiatedContentResult<ListItem>>(result);
        }
        


    }
}
