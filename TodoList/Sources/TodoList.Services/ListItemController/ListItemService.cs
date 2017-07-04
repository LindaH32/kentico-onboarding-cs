using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

[assembly:InternalsVisibleTo("TodoList.Api.Tests")]
namespace TodoList.Services.ListItemController
{
    internal class ListItemService : IListItemService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IListItemRepository _itemRepository;
        private readonly IDateTimeGenerator _dateTimeGenerator;

        public ListItemService(IListItemRepository itemRepository, IGuidGenerator guidGenerator, IDateTimeGenerator dateTimeGenerator)
        {
            _guidGenerator = guidGenerator;
            _itemRepository = itemRepository;
            _dateTimeGenerator = dateTimeGenerator;
        }

        public async Task<ListItem> CreateNewItemAsync(ListItem item)
        {
            DateTime currentDateTime = _dateTimeGenerator.GenerateDateTime();
            ListItem newItem = new ListItem
            {
                Id = _guidGenerator.GenerateGuid(),
                Text = item.Text,
                CreationDateTime = currentDateTime,
                UpdateDateTime = currentDateTime
            };

            await _itemRepository.CreateAsync(newItem);
            return newItem;
        }

        public async Task<ListItem> UpdateExistingItemAsync(ListItem item)
        {
            //TODO
            ListItem existingItem = await _itemRepository.GetAsync(item.Id);
            existingItem.UpdateDateTime = _dateTimeGenerator.GenerateDateTime();
            existingItem.Text = item.Text;

            await _itemRepository.UpdateAsync(existingItem);
            return existingItem;
        }
    }
}
