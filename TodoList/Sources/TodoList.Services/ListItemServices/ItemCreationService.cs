using System;
using System.Threading.Tasks;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

namespace TodoList.Services.ListItemServices
{
    internal class ItemCreationService : IItemCreationService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IListItemRepository _itemRepository;
        private readonly IDateTimeGenerator _dateTimeGenerator;

        public ItemCreationService(IListItemRepository itemRepository, IGuidGenerator guidGenerator, IDateTimeGenerator dateTimeGenerator)
        {
            _guidGenerator = guidGenerator;
            _itemRepository = itemRepository;
            _dateTimeGenerator = dateTimeGenerator;
        }

        public async Task<ListItem> CreateNewItemAsync(ListItem item)
        {
            DateTime currentDateTime = await _dateTimeGenerator.GetCurrentDateTime();
            ListItem newItem = new ListItem
            {
                Id = await _guidGenerator.GenerateGuid(),
                Text = item.Text,
                CreationDateTime = currentDateTime,
                UpdateDateTime = currentDateTime
            };

            await _itemRepository.CreateAsync(newItem);
            return newItem;
        }
    }
}
