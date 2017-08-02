using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using TodoList.Api.Controllers;
using TodoList.Api.Services;
using TodoList.Contracts.Base.Models;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

namespace TodoList.Api.Tests.Controllers
{
    [TestFixture]
    public class ListItemsControllerTest
    {
        private IListItemRepository _repository;
        private readonly Guid _guidOfFirstItem = new Guid("98DBDE18-639E-49A6-8E51-603CEB2AE92D");
        private readonly Guid _guidOfSecondItem = new Guid("1C353E0A-5481-4C31-BD2E-47E1BAF84DBE");
        private readonly Guid _guidOfThirdItem = new Guid("D69E065C-99B1-4A73-B00C-AD05F071861F");
        private readonly Guid _guidOfNoItem = new Guid("0A627543-6890-45B4-BFDF-C94FBACA3E6F");
        private ListItemsController _controller;
        private IListItemUrlGenerator _urlGenerator;
        private IItemCreationService _itemCreationService;
        private IItemModificationService _itemModificationService;
        private IItemAcquisitionService _itemAcquisitionService;

        [SetUp]
        public void Init()
        {
            _repository = Substitute.For<IListItemRepository>();
            _urlGenerator = Substitute.For<IListItemUrlGenerator>();
            _itemCreationService = Substitute.For<IItemCreationService>();
            _itemModificationService = Substitute.For<IItemModificationService>();
            _itemAcquisitionService = Substitute.For<IItemAcquisitionService>();

            _controller = new ListItemsController(_repository, _urlGenerator, _itemCreationService, _itemModificationService, _itemAcquisitionService)
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage(),
            };
        }

        [Test]
        public void DeleteAsync_ReturnsCorrectResponse()
        {
            var expectedListItem = new ListItem { Id = _guidOfSecondItem, Text = "giraffe" };
            _repository.DeleteAsync(_guidOfSecondItem).Returns(expectedListItem);
            
            var actionResult = _controller.DeleteAsync(_guidOfSecondItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }

        [Test]
        public void DeleteAsync_withNonexistingGuid_ReturnsCorrectErrorResponse()
        {
            _repository.DeleteAsync(_guidOfSecondItem).ReturnsNull();

            var actionResult = _controller.DeleteAsync(_guidOfSecondItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void DeleteAsync_withEmptyGuid_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { "Id" };

            var actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            
            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void DeleteAsync_EmptyGuid_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { "Id" };

            var actionResult = _controller.DeleteAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void GetAsync_ById_ReturnsCorrectResponse()
        {
            var expectedListItem = new ListItem { Id = _guidOfFirstItem, Text = "text" };
            var acquisitionResult = AcquisitionResult.Create(expectedListItem);
            _itemAcquisitionService.GetItemAsync(_guidOfFirstItem).Returns(acquisitionResult);

            var actionResult = _controller.GetAsync(_guidOfFirstItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }

        [Test]
        public void GetAsync_EmptyGuid_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { "Id" };

            var actionResult = _controller.GetAsync(Guid.Empty).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            
            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void GetAsync_NonExistingGuid_ReturnsCorrectErrorResponse()
        {
            var acquisitionResult = AcquisitionResult.Create(null);
            _itemAcquisitionService.GetItemAsync(_guidOfNoItem).Returns(acquisitionResult);

            var actionResult = _controller.GetAsync(_guidOfNoItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void GetAsync_WithoutArguments_ReturnsTestItems_ReturnsCorrectResponse()
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
            var expectedLocation = new Uri($"some/test/path/{_guidOfFirstItem}/with/suffix", UriKind.Relative);
            _itemCreationService.CreateNewItemAsync(postedListItem).Returns(expectedListItem);
            _urlGenerator.GenerateUrl(expectedListItem).Returns(callInfo => $"some/test/path/{callInfo.Arg<ListItem>().Id}/with/suffix");

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
        public void PutAsync_WithValidArguments_ReturnsCorrectResponse()
        {
            var databaseListItem = new ListItem { Id = _guidOfThirdItem, Text = "random" };
            var expectedListItem = new ListItem { Id = _guidOfThirdItem, Text = "updated" };
            var updatedListItem = new ListItem { Id = _guidOfThirdItem, Text = "newText" };
            var acquisitionResult = AcquisitionResult.Create(databaseListItem);
            _itemAcquisitionService.GetItemAsync(_guidOfThirdItem).Returns(acquisitionResult);
            _itemModificationService.UpdateExistingItemAsync(acquisitionResult, updatedListItem).Returns(expectedListItem);

            var actionResult = _controller.PutAsync(updatedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            ListItem actualListItem;
            responseMessage.TryGetContentValue(out actualListItem);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(actualListItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }

        [Test]
        public void PutAsync_ItemWithNonExistingId_ReturnsCorrectErrorResponse()
        {
            var updatedListItem = new ListItem { Id = _guidOfNoItem, Text = "newText" };
            var acquisitionResult = AcquisitionResult.Create(null);
            _itemAcquisitionService.GetItemAsync(_guidOfNoItem).Returns(acquisitionResult);

            var actionResult = _controller.PutAsync(updatedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void PutAsync_withEmptyGuid_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { "Id" };
            var editedListItem = new ListItem { Id = Guid.Empty, Text = "text" };

            var actionResult = _controller.PutAsync(editedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase(null)]
        public void PutAsync_ItemWithNoText_ReturnsCorrectErrorResponse(string updatedText)
        {
            var expectedKeys = new[] { "Text" };
            var updatedListItem = new ListItem { Id = _guidOfFirstItem, Text = updatedText };

            var actionResult = _controller.PutAsync(updatedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);
            var actualLocation = responseMessage.Headers.Location;

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
            Assert.IsNull(actualLocation);
        }

        [Test]
        public void PutAsync_WithNullArguments_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { string.Empty };

            var actionResult = _controller.PutAsync(null).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void PutAsync_ItemWithoutText_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { "Text" };
            var postedListItem = new ListItem { Id = _guidOfFirstItem, Text = null };

            var actionResult = _controller.PutAsync(postedListItem).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }

        [Test]
        public void PutAsync_ItemWithEmptyGuid_ReturnsCorrectErrorResponse()
        {
            var expectedKeys = new[] { "Id" };
            var itemWithEmptyGuid = new ListItem { Id = Guid.Empty, Text = "newText" };

            var actionResult = _controller.PutAsync(itemWithEmptyGuid).Result;
            var responseMessage = actionResult.ExecuteAsync(CancellationToken.None).Result;
            HttpError error;
            responseMessage.TryGetContentValue(out error);

            Assert.That(responseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error.ModelState.Keys, Is.EqualTo(expectedKeys));
        }
    }
}