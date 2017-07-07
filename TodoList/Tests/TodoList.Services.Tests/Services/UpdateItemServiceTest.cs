using System;
using NSubstitute;
using NUnit.Framework;
using TodoList.Contracts.Base.Models;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;
using TodoList.Services.ListItemController;

namespace TodoList.Services.Tests.Services
{
    [TestFixture]
    public class UpdateItemServiceTest
    {
        private IListItemRepository _itemRepository;
        private IDateTimeGenerator _dateTimeGenerator;
        private IUpdateItemService _updateItemService;

        [SetUp]
        public void Init()
        {
            _itemRepository = Substitute.For<IListItemRepository>();
            _dateTimeGenerator = Substitute.For<IDateTimeGenerator>();

            _updateItemService = new UpdateItemService(_itemRepository, _dateTimeGenerator);
        }

        [Test]
        public void SetupAndUpdateItemAsync_ReturnsCorrectListItem()
        {
            var itemGuid = new Guid("0478a8c4-4f17-49b1-b61b-df1156465505");
            var creationTime = new DateTime(year: 2017, month: 8, day: 1, hour: 12, minute: 55, second: 55);
            var updateTime = new DateTime(year: 2017, month: 9, day: 2, hour: 13, minute: 56, second: 56);
            var postedItem = new ListItem { Id = Guid.Empty, Text = "hippopotamus", CreationDateTime = creationTime, UpdateDateTime = creationTime };
            var expectedItem = new ListItem { Id = itemGuid, Text = postedItem.Text, CreationDateTime = creationTime, UpdateDateTime = updateTime };
            _itemRepository.GetAsync(Arg.Any<Guid>()).Returns(expectedItem); //TODO expectedItem is already set, doesnt test properly
            _itemRepository.UpdateAsync(Arg.Any<ListItem>()).Returns(info => info.Arg<ListItem>());
            _dateTimeGenerator.GenerateDateTime().Returns(updateTime, DateTime.MinValue);

            var actualItem = _updateItemService.UpdateExistingItemAsync(postedItem).Result;

            Assert.That(actualItem, Is.EqualTo(expectedItem).UsingListItemComparer());
        }
    }
}