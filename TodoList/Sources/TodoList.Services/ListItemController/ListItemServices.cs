using System.Threading.Tasks;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

namespace TodoList.Services.ListItemController
{
    public class ListItemServices : IListItemServices
    {
        private readonly IListItemGuidGenerator _guidGenerator;
        private readonly IListItemRepository _itemRepository;

        public ListItemServices(IListItemRepository itemRepository, IListItemGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
            _itemRepository = itemRepository;
        }

        public async Task<ListItem> PostAsync(ListItem item)
        {
            ListItem itemWithGuid = item;
            itemWithGuid.Id = _guidGenerator.GenerateGuid();
            return await _itemRepository.CreateAsync(itemWithGuid);
        }
    }
}
