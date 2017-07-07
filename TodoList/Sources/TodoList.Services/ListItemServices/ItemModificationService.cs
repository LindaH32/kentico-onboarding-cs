using System;
using System.Threading.Tasks;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

namespace TodoList.Services.ListItemServices
{
    internal class ItemModificationService : IItemModificationService
    {
        private readonly IListItemRepository _itemRepository;
        private readonly IDateTimeGenerator _dateTimeGenerator;

        public ItemModificationService(IListItemRepository itemRepository, IDateTimeGenerator dateTimeGenerator)
        {
            _itemRepository = itemRepository;
            _dateTimeGenerator = dateTimeGenerator;
        }

        public async Task<ListItem> UpdateExistingItemAsync(AcquisitionResult acquisitionResult, ListItem modifiedItem)
        {
            if (!acquisitionResult.WasSuccessful)
            {
                throw  new ArgumentException("Item from failed acquisition cannot be updated", nameof(acquisitionResult));
            }

            ListItem existingItem = acquisitionResult.AcquiredItem;
            existingItem.UpdateDateTime = _dateTimeGenerator.GenerateDateTime();
            existingItem.Text = modifiedItem.Text;

            await _itemRepository.UpdateAsync(existingItem);
            return existingItem;
        }
    }
}