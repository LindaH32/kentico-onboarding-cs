using System;
using NSubstitute;
using NUnit.Framework;
using TodoList.Contracts.Base.Models;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;
using TodoList.Services.ListItemServices;
using Assert = NUnit.Framework.Assert;

namespace TodoList.Services.Tests.Services
{
    [TestFixture]
    public class ItemModificationServiceTest
    {
        private IListItemRepository _itemRepository;
        private IDateTimeGenerator _dateTimeGenerator;
        private IItemModificationService _itemModificationService;

        [SetUp]
        public void Init()
        {
            _itemRepository = Substitute.For<IListItemRepository>();
            _dateTimeGenerator = Substitute.For<IDateTimeGenerator>();

            _itemModificationService = new ItemModificationService(_itemRepository, _dateTimeGenerator);
        }

        [Test]
        public void SetupAndUpdateItemAsync_WithValidArguments_ReturnsCorrectListItem()
        {
            var itemGuid = new Guid("0478a8c4-4f17-49b1-b61b-df1156465505");
            var creationTime = new DateTime(year: 2017, month: 8, day: 1, hour: 12, minute: 55, second: 55);
            var updateTime = new DateTime(year: 2018, month: 9, day: 2, hour: 13, minute: 56, second: 56);
            var databaseListItem = new ListItem { Id = itemGuid, Text = "oldText", CreationDateTime = creationTime, UpdateDateTime = creationTime };
            var postedListItem = new ListItem { Id = itemGuid, Text = "hippopotamus" };
            var expectedListItem = new ListItem { Id = itemGuid, Text = postedListItem.Text, CreationDateTime = creationTime, UpdateDateTime = updateTime };
            var acquisitionResult = AcquisitionResult.Create(databaseListItem);
            _dateTimeGenerator.GenerateDateTime().Returns(updateTime, DateTime.MinValue);

            var actualItem = _itemModificationService.UpdateExistingItemAsync(acquisitionResult, postedListItem).Result;

            Assert.That(actualItem, Is.EqualTo(expectedListItem).UsingListItemComparer());
        }

        [Test]
        public void SetupAndUpdateItemAsync_WithFailedAcquisitionResult_ThrowsCorrectException()
        {
            var itemGuid = new Guid("0478a8c4-4f17-49b1-b61b-df1156465505");
            var postedListItem = new ListItem { Id = itemGuid, Text = "hippopotamus" };
            var acquisitionResult = AcquisitionResult.Create(null);
            //TODO fix aggregated exception throw
            try
            {
                _itemModificationService.UpdateExistingItemAsync(acquisitionResult, postedListItem);
            }
            catch (AggregateException e)
            {
                if (e.InnerException.GetType() == typeof(ArgumentException))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("There is no ArgumentException m8");
                }
            }


        }

    }
}