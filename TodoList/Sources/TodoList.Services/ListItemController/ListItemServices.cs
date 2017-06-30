using System.Threading.Tasks;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

namespace TodoList.Services.ListItemController
{
    public class ListItemServices : IListItemServices
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IListItemRepository _itemRepository;
        private readonly IDateTimeGenerator _dateTimeGenerator;

        public ListItemServices(IListItemRepository itemRepository, IGuidGenerator guidGenerator, IDateTimeGenerator dateTimeGenerator)
        {
            _guidGenerator = guidGenerator;
            _itemRepository = itemRepository;
            _dateTimeGenerator = dateTimeGenerator;

        }

        public async Task<ListItem> PostAsync(ListItem item)
        {
            item.Id = _guidGenerator.GenerateGuid();
            item.CreationDateTime = item.UpdateDateTime = _dateTimeGenerator.GenerateDateTime();
            return await _itemRepository.CreateAsync(item);
        }

        public async Task<ListItem> PutAsync(ListItem item)
        {
            item.UpdateDateTime = _dateTimeGenerator.GenerateDateTime();
            return await _itemRepository.UpdateAsync(item);
        }
    }
}
