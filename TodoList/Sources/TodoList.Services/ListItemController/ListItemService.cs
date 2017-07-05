using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

[assembly:InternalsVisibleTo("TodoList.Api.Tests")]
namespace TodoList.Services.ListItemController
{
    internal class CreateItemService : ICreateItemService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IListItemRepository _itemRepository;
        private readonly IDateTimeGenerator _dateTimeGenerator;

        public CreateItemService(IListItemRepository itemRepository, IGuidGenerator guidGenerator, IDateTimeGenerator dateTimeGenerator)
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
    }
}
