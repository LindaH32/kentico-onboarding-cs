using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using NSubstitute;
using NUnit.Framework;
using TodoList.Api.Controllers;
using TodoList.Api.Services;
using TodoList.Api.Tests.Helpers;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;

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
        public void DeleteAsync_ReturnsCorrectResponse()
        {
            var expectedListItem = new ListItem { Id = _guidOfSecondItem, Text = "giraffe" };
            _repository.DeleteAsync(Guid.Empty).Returns(expectedListItem);
            
            var actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }

        [Test]
        public void GetAsync_ById_ReturnsCorrectResponse()
        {
            var expectedListItem = new ListItem { Id = _guidOfFirstItem, Text = "text" };
            _repository.GetAsync(_guidOfFirstItem).Returns(expectedListItem);

            var actionResult = _controller.GetAsync(_guidOfFirstItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }

        [Test]
        public void GetAsync_ById_ReturnsCorrectErrorResponse()
        {
            var actionResult = _controller.GetAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void GetAsync_WithoutParams_ReturnsTestItems_ReturnsCorrectResponse()
        {
            var expectedListItems = new List<ListItem>
            {
                new ListItem { Id = _guidOfFirstItem, Text = "text" },
                new ListItem { Id = _guidOfSecondItem, Text = "giraffe" },
                new ListItem { Id = _guidOfThirdItem, Text = "updated" }
            };
            _repository.GetAllAsync().Returns(expectedListItems);

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
        public void PostAsync_ItemWithNoText_ReturnsCorrectErrorResponse(string postedText)
        {
            var expectedKeys = new[] { "Text" };
            var postedListItem = new ListItem { Id = Guid.Empty, Text = postedText };

            var actionResult = _controller.PostAsync(postedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var actualLocation = responseMessage.Headers.Location;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
            Assert.IsNull(actualLocation);
        }

        [Test]
        public void PostAsync_ItemWithNonEmptyGuid_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { "Id" };
            var postedListItem = new ListItem { Id = _guidOfFirstItem, Text = "text" };

            var actionResult = _controller.PostAsync(postedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var actualLocation = responseMessage.Headers.Location;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
            Assert.IsNull(actualLocation);
        }

        [Test]
        public void PostAsync_WithNullArguments_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { string.Empty };

            var actionResult = _controller.PostAsync(null).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var actualLocation = responseMessage.Headers.Location;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
            Assert.That(actualLocation, Is.EqualTo(null));
        }

        [Test]
        public void PostAsync_WithValidArguments_ReturnsCorrectResponse()
        {
            var expectedListItem = new ListItem { Id = _guidOfFirstItem, Text = "text" };
            var postedListItem = new ListItem { Id = Guid.Empty, Text = "newText" };
            var expectedLocation = new Uri($"api/v1/ListItems/{_guidOfFirstItem}", UriKind.Relative);
            _repository.CreateAsync(postedListItem).Returns(expectedListItem);
            _urlGenerator.GenerateUrl(expectedListItem).Returns(callInfo => $"api/v1/ListItems/{callInfo.Arg<ListItem>().Id}");

            var actionResult = _controller.PostAsync(postedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            var actualLocation = responseMessage.Headers.Location;
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
            _repository.UpdateAsync(updatedListItem).Returns(expectedListItem);

            var actionResult = _controller.PutAsync(updatedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }
    }
}