using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using NSubstitute;
using NUnit.Framework;
using TodoList.Api.Controllers;
using TodoList.Api.Tests.Helpers;
using TodoList.Contracts.Interfaces;
using TodoList.Contracts.Models;


namespace TodoList.Api.Tests.Controllers
{
    [TestFixture]
    public class ListItemsControllerTest
    {
        private IListItemRepository _repository;
        private readonly Guid _guidOfFirstItem = new Guid("98DBDE18-639E-49A6-8E51-603CEB2AE92D");
        private readonly Guid _guidOfSecondItem = new Guid("1C353E0A-5481-4C31-BD2E-47E1BAF84DBE");
        private readonly Guid _guidOfThirdItem = new Guid("D69E065C-99B1-4A73-B00C-AD05F071861F");
        private ListItemComparer _comparer;
        private ListItemsController _controller;

        [SetUp]
        public void Init()
        {
            _repository = Substitute.For<IListItemRepository>();

            _comparer = new ListItemComparer();
            _controller = new ListItemsController(_repository)
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage()
            };
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase(null)]
        public void PostAsync_ItemWithNullText_ReturnsErrorMessageAndStatusCode(string postedText)
        {
            var listItem = new ListItem(Guid.Empty, postedText);

            var actionResult = _controller.PostAsync(listItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var modelStateKeys = error.ModelState.Keys;
            var expectedKeys = new[] { "Text" };


            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(modelStateKeys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void DeleteAsync_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem(_guidOfSecondItem, "giraffe");
            _repository.Delete(Guid.Empty).Returns(expectedListItem);
            
            var actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }

        [Test]
        public void GetAsync_ById_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem(_guidOfFirstItem, "text");
            _repository.Get(Guid.Empty).Returns(expectedListItem);

            var actionResult = _controller.GetAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }

        [Test]
        public void GetAsync_WithoutParams_ReturnsTestItems()
        {
            var expectedListItems = new List<ListItem>
            {
                new ListItem(_guidOfFirstItem, "text"),
                new ListItem(_guidOfSecondItem, "giraffe"),
                new ListItem(_guidOfThirdItem, "updated")
            };
            _repository.Get().Returns(expectedListItems);

            var actionResult = _controller.GetAsync().Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            List<ListItem> actualListItems;
            responseMessage.TryGetContentValue(out actualListItems);

            Assert.That(actualListItems, Is.EqualTo(expectedListItems).Using(_comparer));
        }

        [Test]
        public void PostAsync_ItemWithNonEmptyGuid_ReturnsErrorMessageAndStatusCode()
        {
            var listItem = new ListItem(_guidOfFirstItem, "text");

            var actionResult = _controller.PostAsync(listItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var modelStateKeys = error.ModelState.Keys;
            var expectedKeys = new[]{"Id"};

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(modelStateKeys, Is.EqualTo(expectedKeys));
            
        }

        [Test]
        public void PostAsync_WithNullArguments_ReturnsErrorMessageAndStatusCode()
        {
            var actionResult = _controller.PostAsync(null).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var modelStateKeys = error.ModelState.Keys;
            var expectedKeys = new[] { string.Empty };

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(modelStateKeys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void PostAsync_WithValidArguments_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem(_guidOfFirstItem, "text");
            var newListItem = new ListItem(Guid.Empty, "newText");
            _repository.Post(newListItem).Returns(expectedListItem);

            var actionResult = _controller.PostAsync(newListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void PutAsync_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem(_guidOfThirdItem, "updated");
            var updatedListItem = new ListItem(Guid.Empty, "newText");
            _repository.Put(updatedListItem).Returns(expectedListItem);

            var actionResult = _controller.PutAsync(updatedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).Using(_comparer));
        }
    }
}