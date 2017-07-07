using System;
using System.Threading.Tasks;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

namespace TodoList.Services.ListItemServices
{
    public class ItemAcquisitionService: IItemAcquisitionService
    {
        private readonly IListItemRepository _itemRepository;

        public ItemAcquisitionService(IListItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }
        
        public async Task<AcquisitionResult> GetItemAsync(Guid id)
        {
            var acquiredItem = await _itemRepository.GetAsync(id);
            return AcquisitionResult.Create(acquiredItem);
        }
    }
}