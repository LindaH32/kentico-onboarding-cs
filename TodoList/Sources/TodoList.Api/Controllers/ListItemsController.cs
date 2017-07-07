using System;
using System.Threading.Tasks;
using System.Web.Http;
using TodoList.Api.Services;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;
using TodoList.Contracts.Services;

namespace TodoList.Api.Controllers
{
    public class ListItemsController : ApiController
    {
        private readonly IListItemRepository _listItemsRepository;
        private readonly IListItemUrlGenerator _urlGenerator;
        private readonly ICreateItemService _createItemService;
        private readonly IUpdateItemService _updateItemService;
        private readonly IItemAcquisitionService _itemAcquisitionService;

        public ListItemsController(IListItemRepository listItemsRepository, IListItemUrlGenerator urlGenerator,
            ICreateItemService createItemService, IUpdateItemService updateItemService, IItemAcquisitionService itemAcquisitionService)
        {
            _listItemsRepository = listItemsRepository;
            _urlGenerator = urlGenerator;
            _createItemService = createItemService;
            _updateItemService = updateItemService;
            _itemAcquisitionService = itemAcquisitionService;
        }

        public async Task<IHttpActionResult> GetAsync() 
            => Ok(await _listItemsRepository.GetAllAsync());

        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            ValidateItemId(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //TODO 
            var acquisitionResult = await _itemAcquisitionService.GetItemAsync(id);
            if (!acquisitionResult.WasSuccessful)
            {
                return NotFound();
            }

            return Ok(acquisitionResult.AcquiredItem);
        }

        public async Task<IHttpActionResult> PostAsync(ListItem item)
        {
            ValidatePostItem(item);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ListItem createdItem = await _createItemService.CreateNewItemAsync(item);
            string location = _urlGenerator.GenerateUrl(createdItem);

            return Created(location, createdItem);
        }

        public async Task<IHttpActionResult> PutAsync(ListItem item)
        {
            var acquisitionResult = await _itemAcquisitionService.GetItemAsync(item.Id);
            if (!acquisitionResult.WasSuccessful)
            {
                return NotFound();
            }

            var updatedItem = await _updateItemService.UpdateExistingItemAsync(acquisitionResult, item);

            return Ok(updatedItem);
        }

        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            ValidateItemId(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _listItemsRepository.DeleteAsync(id));
        }

        private void ValidatePostItem(ListItem item)
        {
            if (item == null)
            {
                ModelState.AddModelError(String.Empty, "Item is null");
                return;
            }

            if (string.IsNullOrWhiteSpace(item.Text))
            {
                ModelState.AddModelError(nameof(ListItem.Text), "Text is null or empty");
            }

            if (item.Id != Guid.Empty)
            {
                ModelState.AddModelError(nameof(ListItem.Id), "Guid is not empty");
            }
        }

        private void ValidateItemId(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(ListItem.Id), "Guid is empty");
            }
        }
    }
}