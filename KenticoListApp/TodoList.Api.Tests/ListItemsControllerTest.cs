using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using System.Web.Http;
using NUnit.Framework;
using TodoList.Api.Tests.Comparers;


namespace TodoList.Api.Tests
{
    [TestFixture]
    public class ListItemsControllerTest
    {
        private ListItemComparer _comparer;
        private ListItemsController _controller;

        [SetUp]
        public void Init()
        {
            _comparer = new ListItemComparer();
            _controller = new ListItemsController
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };
        }

        [Test]
        public void Get_ById_ReturnsTestItem()
        {
            var expected = new ListItem(new Guid("00000000-0000-0000-0000-000000000000"), "text");

            IHttpActionResult actionResult = _controller.GetAsync(Guid.Empty).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Get_ById_IsOfCorrectStatusCode()
        {
            IHttpActionResult actionResult = _controller.GetAsync(Guid.Empty).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Get_ReturnsTestItems()
        {
            var expectedListItems = new List<ListItem>
            {
                new ListItem(new Guid("00000000-0000-0000-0000-000000000000"), "text"),
                new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe"),
                new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated"),
            };

            IHttpActionResult actionResult = _controller.GetAsync().Result;
            HttpResponseMessage responseMessage= actionResult.ExecuteAsync(CancellationToken.None).Result;
            List<ListItem> actualListItems;
            responseMessage.TryGetContentValue(out actualListItems);

            Assert.That(actualListItems, Is.EqualTo(expectedListItems).Using(_comparer));
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase(null)]
        public void Post_ItemWithNullText_ReturnsErrorMessage(string postedText)
        {
            var listItem = new ListItem(Guid.Empty, postedText);

            IHttpActionResult actionResult = _controller.Post(listItem).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            string actualMessage = error.Message;

            Assert.That(actualMessage, Is.EqualTo("Text is null or empty"));
        }

        [Test]
        public void Post_WithNullArguments_ReturnsErrorMessage()
        {
            IHttpActionResult actionResult = _controller.Post(null).Result;
            HttpResponseMessage repsonseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            repsonseMessage.TryGetContentValue(out error);
            string actualMessage = error.Message;

            Assert.That(actualMessage, Is.EqualTo("Item is null"));
        }

        [Test]
        public void Post_WithNullArguments_IsOfCorrectStatusCode()
        {
            IHttpActionResult actionResult = _controller.Post(null).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_ItemWithNullText_IsOfCorrectStatusCode()
        {
            var listItem = new ListItem(Guid.Empty);

            IHttpActionResult actionResult = _controller.Post(listItem).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_ItemWithNonEmptyGuid_ReturnsErrorMessage()
        {
            var listItem = new ListItem(new Guid("05200000-0000-0000-0000-000000000000"), "text");

            IHttpActionResult actionResult = _controller.Post(listItem).Result;
            HttpResponseMessage repsonseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            repsonseMessage.TryGetContentValue(out error);
            string actualMessage = error.Message;

            Assert.That(actualMessage, Is.EqualTo("Guid must be empty"));
        }

        [Test]
        public void Post_ItemWithNonEmptyGuid_IsOfCorrectStatusCode()
        {
            var listItem = new ListItem(new Guid("05200000-0000-0000-0000-000000000000"), "text");

            IHttpActionResult actionResult = _controller.Post(listItem).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_WithValidArguments_ReturnsCorrectItem()
        {
            var expectedListItem = new ListItem(new Guid("00000000-0000-0000-0000-000000000000"), "text");
            var newListItem = new ListItem(Guid.Empty, "newText");

            IHttpActionResult actionResult = _controller.Post(newListItem).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }

        [Test]
        public void Post_WithValidArguments_IsOfCorrectStatusCode()
        {
            var newListItem = new ListItem(Guid.Empty, "newText");

            IHttpActionResult actionResult = _controller.Post(newListItem).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        [Test]
        public void Put_ReturnsCorrectItem()
        {
            var expectedListItem = new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated");
            var updatedListItem = new ListItem(Guid.Empty, "newText");

            IHttpActionResult actionResult = _controller.PutAsync(updatedListItem).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }

        [Test]
        public void Put_IsOfCorrectStatusCode()
        {
            var updated = new ListItem(Guid.Empty, "newText");

            IHttpActionResult actionResult = _controller.PutAsync(updated).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Delete_ReturnsCorrectItem()
        {
            var expectedListItem = new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe");

            IHttpActionResult actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem); 

            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }

        [Test]
        public void Delete_IsOfCorrectStatusCode()
        {
            IHttpActionResult actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            HttpResponseMessage responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
