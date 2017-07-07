using System;
using NSubstitute;
using NUnit.Framework;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;
using TodoList.Api.Tests.Api.Helpers;
using TodoList.Services.ListItemController;

namespace TodoList.Services.Tests.Services
{
    class CreateItemServiceTest
    {
        private IGuidGenerator _guidGenerator;
        private IListItemRepository _itemRepository;
        private IDateTimeGenerator _dateTimeGenerator;
        private ICreateItemService _createItemService;

        [SetUp]
        public void Init()
        {
            _guidGenerator = Substitute.For<IGuidGenerator>();
            _itemRepository = Substitute.For<IListItemRepository>();
            _dateTimeGenerator = Substitute.For<IDateTimeGenerator>();

            _createItemService = new CreateItemService(_itemRepository, _guidGenerator, _dateTimeGenerator);
        }

        [Test]
        public void SetupAndCreateItemAsync_ReturnsCorrectListItem()
        {
            var itemGuid = new Guid("0478a8c4-4f17-49b1-b61b-df1156465505");
            var date = new DateTime(year: 2017, month: 10, day: 5, hour: 10, minute: 39, second: 4);
            var postedItem = new ListItem { Id = Guid.Empty, Text = "hippopotamus" };
            var expectedItem = new ListItem { Id = itemGuid, Text = postedItem.Text, CreationDateTime = date, UpdateDateTime = date };
            _guidGenerator.GenerateGuid().Returns(itemGuid);
            //_itemRepository.CreateAsync(Arg.Any<ListItem>()).Returns(info => info.Arg<ListItem>());
            _dateTimeGenerator.GenerateDateTime().Returns(date, DateTime.MinValue);

            var actualItem = _createItemService.CreateNewItemAsync(postedItem).Result;
            
            Assert.That(actualItem, Is.EqualTo(expectedItem).UsingListItemComparer());
        }
    }
}
