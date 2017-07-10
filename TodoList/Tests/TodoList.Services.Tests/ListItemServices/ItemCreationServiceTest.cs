using System;
using NSubstitute;
using NUnit.Framework;
using TodoList.Contracts.Base.Models;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;
using TodoList.Services.ListItemServices;

namespace TodoList.Services.Tests.ListItemServices
{
    [TestFixture]
    public class ItemCreationServiceTest
    {
        private IGuidGenerator _guidGenerator;
        private IListItemRepository _itemRepository;
        private IDateTimeGenerator _dateTimeGenerator;
        private IItemCreationService _itemCreationService;

        [SetUp]
        public void Init()
        {
            _guidGenerator = Substitute.For<IGuidGenerator>();
            _itemRepository = Substitute.For<IListItemRepository>();
            _dateTimeGenerator = Substitute.For<IDateTimeGenerator>();

            _itemCreationService = new ItemCreationService(_itemRepository, _guidGenerator, _dateTimeGenerator);
        }

        [Test]
        public void CreateNewItemAsync_ReturnsCorrectListItem()
        {
            var itemGuid = new Guid("0478a8c4-4f17-49b1-b61b-df1156465505");
            var date = new DateTime(year: 2017, month: 10, day: 5, hour: 10, minute: 39, second: 4);
            var postedListItem = new ListItem { Id = Guid.Empty, Text = "hippopotamus" };
            var expectedListItem = new ListItem { Id = itemGuid, Text = postedListItem.Text, CreationDateTime = date, UpdateDateTime = date };
            _guidGenerator.GenerateGuid().Returns(itemGuid);
            _dateTimeGenerator.GenerateDateTime().Returns(date, DateTime.MinValue);

            var actualItem = _itemCreationService.CreateNewItemAsync(postedListItem).Result;
            
            Assert.That(actualItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }
    }
}
