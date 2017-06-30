using System;
using NSubstitute;
using NUnit.Framework;
using TodoList.Api.Tests.Api.Helpers;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;
using TodoList.Services.ListItemController;

namespace TodoList.Api.Tests.Services
{
    class ListItemServicesTest
    {
        private IListItemServices _listItemServices;
        private IListItemGuidGenerator _guidGenerator;
        private IListItemRepository _itemRepository;
        private IListItemDateTimeGenerator _dateTimeGenerator;

        [SetUp]
        public void Init()
        {
            _guidGenerator = Substitute.For<IListItemGuidGenerator>();
            _itemRepository = Substitute.For<IListItemRepository>();
            _dateTimeGenerator = Substitute.For<IListItemDateTimeGenerator>();

            _listItemServices = new ListItemServices(_itemRepository, _guidGenerator, _dateTimeGenerator);
        }

        [Test]
        public void PostAsync_ReturnsCorrectListItem()
        {
            var itemGuid = new Guid("0478a8c4-4f17-49b1-b61b-df1156465505");
            var date = DateTime.Parse("30/6/2017 11:34:52");
            var postedItem = new ListItem() { Id = Guid.Empty, Text = "hippopotamus" };
            var expectedItem = new ListItem() { Id = itemGuid, Text = "hippopotamus", CreationDateTime = date, UpdateDateTime = date };
            _guidGenerator.GenerateGuid().Returns(itemGuid);
            _itemRepository.CreateAsync(postedItem).Returns(expectedItem);
            _dateTimeGenerator.GenerateDateTime().Returns(date);

            var actualItem = _listItemServices.PostAsync(postedItem).Result;
            
            Assert.That(actualItem, Is.EqualTo(expectedItem).UsingListItemComparer());
        }
    }
}
