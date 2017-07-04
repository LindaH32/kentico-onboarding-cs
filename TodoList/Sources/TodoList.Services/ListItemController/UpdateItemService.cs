using System.Threading.Tasks;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

namespace TodoList.Services.ListItemController
{
    public class UpdateItemService : IUpdateItemService
    {
        private readonly IListItemRepository _itemRepository;
        private readonly IDateTimeGenerator _dateTimeGenerator;

        public UpdateItemService(IListItemRepository itemRepository, IDateTimeGenerator dateTimeGenerator)
        {
            _itemRepository = itemRepository;
            _dateTimeGenerator = dateTimeGenerator;
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