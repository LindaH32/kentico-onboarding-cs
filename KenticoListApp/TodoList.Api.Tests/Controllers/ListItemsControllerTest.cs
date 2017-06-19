using System;
using System.Collections.Generic;
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
        private ListItemsController _controller;
        private IListItemUrlGenerator _urlGenerator;

        [SetUp]
        public void Init()
        {
            _repository = Substitute.For<IListItemRepository>();
            _urlGenerator = Substitute.For<IListItemUrlGenerator>();

            _controller = new ListItemsController(_repository, _urlGenerator)
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage(),
            };
        }

        [Test]
        public void DeleteAsync_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem { Id = _guidOfSecondItem, Text = "giraffe" };
            _repository.Delete(Guid.Empty).Returns(expectedListItem);
            
            var actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }

        [Test]
        public void GetAsync_ById_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem { Id = _guidOfFirstItem, Text = "text"};
            _repository.Get(Guid.Empty).Returns(expectedListItem);

            var actionResult = _controller.GetAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }

        [Test]
        public void GetAsync_WithoutParams_ReturnsTestItems_ReturnsCorrectItemsAndStatusCode()
        {
            var expectedListItems = new List<ListItem>
            {
                new ListItem { Id = _guidOfFirstItem, Text = "text" },
                new ListItem { Id = _guidOfSecondItem, Text = "giraffe" },
                new ListItem { Id = _guidOfThirdItem, Text = "updated"}
            };
            _repository.Get().Returns(expectedListItems);

            var actionResult = _controller.GetAsync().Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            List<ListItem> actualListItems;
            responseMessage.TryGetContentValue(out actualListItems);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItems, Is.EqualTo(expectedListItems).UsingListItemComparer());
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase(null)]
        public void PostAsync_ItemWithNoText_ReturnsErrorMessageAndStatusCode(string postedText)
        {
            var expectedKeys = new[] { "Text" };
            var postedListItem = new ListItem { Id = Guid.Empty, Text = postedText };

            var actionResult = _controller.PostAsync(postedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void PostAsync_ItemWithNonEmptyGuid_ReturnsErrorMessageAndStatusCode()
        {
            var expectedKeys = new[] { "Id" };
            var postedListItem = new ListItem { Id = _guidOfFirstItem, Text = "text" };

            var actionResult = _controller.PostAsync(postedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
            
        }

        [Test]
        public void PostAsync_WithNullArguments_ReturnsErrorMessageAndStatusCode()
        {
            var expectedKeys = new[] { string.Empty };

            var actionResult = _controller.PostAsync(null).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void PostAsync_WithValidArguments_ReturnsCorrectItemAndStatusCodeAndLocation() //TODO rename
        {
            var expectedListItem = new ListItem { Id = _guidOfFirstItem, Text = "text"};
            var postedListItem = new ListItem { Id = Guid.Empty, Text = "newText" };
            string expectedLocation = $"api/v1/ListItems/{_guidOfFirstItem}";
            _repository.Post(postedListItem).Returns(expectedListItem);
            _urlGenerator.GenerateUrl(postedListItem).Returns($"api/v1/ListItems/{expectedListItem.Id}");

            var actionResult = _controller.PostAsync(postedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            string actualLocation = responseMessage.Headers.Location.ToString();
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
            Assert.That(actualLocation, Is.EqualTo(expectedLocation));
        }

        [Test]
        public void PutAsync_ReturnsCorrectItemAndStatusCode()
        {
            var expectedListItem = new ListItem { Id = _guidOfThirdItem, Text = "updated" };
            var updatedListItem = new ListItem { Id = Guid.Empty, Text = "newText" };
            _repository.Put(updatedListItem).Returns(expectedListItem);

            var actionResult = _controller.PutAsync(updatedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }
    }
}