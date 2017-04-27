using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using NUnit.Framework;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using TodoList.Api.Tests.Helpers;

namespace TodoList.Api.Tests.Controllers
{
    [TestFixture]
    public class ListItemsControllerTest
    {
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

        private readonly Guid _guidOfFirstItem = new Guid("30000000-0000-0000-0000-000000000000");
        private readonly Guid _guidOfSecondItem = new Guid("10000000-0000-0000-0000-000000000000");
        private readonly Guid _guidOfThirdItem = new Guid("20000000-0000-0000-0000-000000000000");
        private ListItemComparer _comparer;
        private ListItemsController _controller;

        [TestCase("")]
        [TestCase("    ")]
        [TestCase(null)]
        public void Post_ItemWithNullText_ReturnsErrorMessageAndStatusCode(string postedText)
        {
            var listItem = new ListItem(Guid.Empty, postedText);

            var actionResult = _controller.PostAsync(listItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var actualMessage = error.Message;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(actualMessage, Is.EqualTo("Text is null or empty"));
        }

        [Test]
        public void Delete_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem(_guidOfSecondItem, "giraffe");

            var actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }

        [Test]
        public void Get_ById_ReturnsCorrectItemAndStatusCode()
        {
            var expected = new ListItem(_guidOfFirstItem, "text");

            var actionResult = _controller.GetAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expected).Using(_comparer));
        }

        [Test]
        public void Get_ReturnsTestItems()
        {
            var expectedListItems = new List<ListItem>
            {
                new ListItem(_guidOfFirstItem, "text"),
                new ListItem(_guidOfSecondItem, "giraffe"),
                new ListItem(_guidOfThirdItem, "updated")
            };

            var actionResult = _controller.GetAsync().Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            List<ListItem> actualListItems;
            responseMessage.TryGetContentValue(out actualListItems);

            Assert.That(actualListItems, Is.EqualTo(expectedListItems).Using(_comparer));
        }

        [Test]
        public void Post_ItemWithNonEmptyGuid_ReturnsErrorMessageAndStatusCode()
        {
            var listItem = new ListItem(_guidOfFirstItem, "text");

            var actionResult = _controller.PostAsync(listItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var actualMessage = error.Message;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(actualMessage, Is.EqualTo("Guid must be empty"));
        }

        [Test]
        public void Post_ItemWithNullText_IsOfCorrectStatusCode()
        {
            var listItem = new ListItem(Guid.Empty);

            var actionResult = _controller.PostAsync(listItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Post_WithNullArguments_ReturnsErrorMessageAndStatusCode()
        {
            var actionResult = _controller.PostAsync(null).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var actualMessage = error.Message;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(actualMessage, Is.EqualTo("Item is null"));
        }

        [Test]
        public void Post_WithValidArguments_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem(_guidOfFirstItem, "text");
            var newListItem = new ListItem(Guid.Empty, "newText");

            var actionResult = _controller.PostAsync(newListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void Put_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem(_guidOfThirdItem, "updated");
            var updatedListItem = new ListItem(Guid.Empty, "newText");

            var actionResult = _controller.PutAsync(updatedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }
    }
}