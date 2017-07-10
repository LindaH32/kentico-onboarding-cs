using System;
using NSubstitute;
using NUnit.Framework;
using TodoList.Contracts.Base.Models;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;
using TodoList.Services.ListItemServices;

namespace TodoList.Services.Tests.Services
{
    public class ItemAcquisitionServiceTest
    {
        private IListItemRepository _itemRepository;
        private IItemAcquisitionService _itemAcquisitionService;

        [SetUp]
        public void Init()
        {
            _itemRepository = Substitute.For<IListItemRepository>();

            _itemAcquisitionService = new ItemAcquisitionService(_itemRepository);
        }

        [Test]
        public void GetItemAsync_ById_ReturnsCorrectAcquisitionResult()
        {
            var itemGuid = new Guid("0478a8c4-4f17-49b1-b61b-df1156465505");
            var date = new DateTime(year: 2017, month: 10, day: 5, hour: 10, minute: 39, second: 4);
            var databaseListItem = new ListItem { Id = itemGuid, Text = "hippopotamus", CreationDateTime = date, UpdateDateTime = date };
            var expectedAcquisitionResult = AcquisitionResult.Create(databaseListItem);
            _itemRepository.GetAsync(itemGuid).Returns(databaseListItem);

            var actualAcquisitionResult = _itemAcquisitionService.GetItemAsync(itemGuid).Result;

            Assert.That(actualAcquisitionResult.AcquiredItem, Is.EqualTo(expectedAcquisitionResult.AcquiredItem).UsingListItemComparer());
            Assert.That(actualAcquisitionResult.WasSuccessful, Is.EqualTo(expectedAcquisitionResult.WasSuccessful)); //TODO redundant
        }

        [Test]
        public void GetItemAsync_NonExistingGuid_ReturnsCorrectAcquisitionResult()
        {
            var itemGuid = new Guid("FD33B000-D838-49A9-B5F8-83C3FBF4A4EB");
            var expectedAcquisitionResult = AcquisitionResult.Create(null);
            _itemRepository.GetAsync(itemGuid).Returns((ListItem) null); //TODO type

            var actualAcquisitionResult = _itemAcquisitionService.GetItemAsync(itemGuid).Result;

            Assert.That(actualAcquisitionResult.AcquiredItem, Is.EqualTo(expectedAcquisitionResult.AcquiredItem).UsingListItemComparer());
            Assert.That(actualAcquisitionResult.WasSuccessful, Is.EqualTo(expectedAcquisitionResult.WasSuccessful)); //TODO redundant
        }
    }
}