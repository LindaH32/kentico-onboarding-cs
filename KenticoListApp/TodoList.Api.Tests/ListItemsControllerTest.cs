using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using NUnit.Framework;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using TodoList.Api.Tests.Comparers;

namespace TodoList.Api.Tests
{
    [TestFixture]
    public class ListItemsControllerTest
    {
        private readonly Guid _nonEmptyGuid = new Guid("10056700-0000-0000-5558-022351086020");
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

        private ListItemComparer _comparer;
        private ListItemsController _controller;

        [TestCase("")]
        [TestCase("    ")]
        [TestCase(null)]
        public void Post_ItemWithNullText_ReturnsErrorMessage(string postedText)
        {
            var listItem = new ListItem(Guid.Empty, postedText);

            var actionResult = _controller.Post(listItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var actualMessage = error.Message;

            Assert.That(actualMessage, Is.EqualTo("Text is null or empty"));
        }

        [Test]
        public void Delete_IsOfCorrectStatusCode()
        {
            var actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Delete_ReturnsCorrectItem()
        {
            var expectedListItem = new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe");

            var actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }

        [Test]
        public void Get_ById_IsOfCorrectStatusCode()
        {
            var actionResult = _controller.GetAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Get_ById_ReturnsTestItem()
        {
            var expected = new ListItem(new Guid("30000000-0000-0000-0000-000000000000"), "text");

            var actionResult = _controller.GetAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Get_ReturnsTestItems()
        {
            var expectedListItems = new List<ListItem>
            {
                new ListItem(new Guid("30000000-0000-0000-0000-000000000000"), "text"),
                new ListItem(new Guid("10000000-0000-0000-0000-000000000000"), "giraffe"),
                new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated")
            };

            var actionResult = _controller.GetAsync().Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            List<ListItem> actualListItems;
            responseMessage.TryGetContentValue(out actualListItems);

            Assert.That(actualListItems, Is.EqualTo(expectedListItems).Using(_comparer));
        }

        [Test]
        public void Post_ItemWithNonEmptyGuid_IsOfCorrectStatusCode()
        {
            var listItem = new ListItem(_nonEmptyGuid, "text");

            var actionResult = _controller.Post(listItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_ItemWithNonEmptyGuid_ReturnsErrorMessage()
        {
            var listItem = new ListItem(_nonEmptyGuid, "text");

            var actionResult = _controller.Post(listItem).Result;
            var repsonseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            repsonseMessage.TryGetContentValue(out error);
            var actualMessage = error.Message;

            Assert.That(actualMessage, Is.EqualTo("Guid must be empty"));
        }

        [Test]
        public void Post_ItemWithNullText_IsOfCorrectStatusCode()
        {
            var listItem = new ListItem(Guid.Empty);

            var actionResult = _controller.Post(listItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_WithNullArguments_IsOfCorrectStatusCode()
        {
            var actionResult = _controller.Post(null).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_WithNullArguments_ReturnsErrorMessage()
        {
            var actionResult = _controller.Post(null).Result;
            var repsonseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            repsonseMessage.TryGetContentValue(out error);
            var actualMessage = error.Message;

            Assert.That(actualMessage, Is.EqualTo("Item is null"));
        }

        [Test]
        public void Post_WithValidArguments_IsOfCorrectStatusCode()
        {
            var newListItem = new ListItem(Guid.Empty, "newText");

            var actionResult = _controller.Post(newListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Post_WithValidArguments_ReturnsCorrectItem()
        {
            var expectedListItem = new ListItem(new Guid("30000000-0000-0000-0000-000000000000"), "text");
            var newListItem = new ListItem(Guid.Empty, "newText");

            var actionResult = _controller.Post(newListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }

        [Test]
        public void Put_IsOfCorrectStatusCode()
        {
            var updated = new ListItem(Guid.Empty, "newText");

            var actionResult = _controller.PutAsync(updated).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        [Test]
        public void Put_ReturnsCorrectItem()
        {
            var expectedListItem = new ListItem(new Guid("20000000-0000-0000-0000-000000000000"), "updated");
            var updatedListItem = new ListItem(Guid.Empty, "newText");

            var actionResult = _controller.PutAsync(updatedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }
    }
}