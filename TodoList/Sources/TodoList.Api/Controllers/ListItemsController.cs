﻿using System;
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
        private readonly IItemCreationService _itemCreationService;
        private readonly IItemModificationService _itemModificationService;
        private readonly IItemAcquisitionService _itemAcquisitionService;

        public ListItemsController(IListItemRepository listItemsRepository, IListItemUrlGenerator urlGenerator,
            IItemCreationService itemCreationService, IItemModificationService itemModificationService, IItemAcquisitionService itemAcquisitionService)
        {
            _listItemsRepository = listItemsRepository;
            _urlGenerator = urlGenerator;
            _itemCreationService = itemCreationService;
            _itemModificationService = itemModificationService;
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
            
            AcquisitionResult acquisitionResult = await _itemAcquisitionService.GetItemAsync(id);
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

            ListItem createdItem = await _itemCreationService.CreateNewItemAsync(item);
            string location = _urlGenerator.GenerateUrl(createdItem);

            return Created(location, createdItem);
        }

        public async Task<IHttpActionResult> PutAsync(ListItem item)
        {
            ValidateItemId(item.Id);
            ValidateItemText(item.Text);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AcquisitionResult acquisitionResult = await _itemAcquisitionService.GetItemAsync(item.Id);
            if (!acquisitionResult.WasSuccessful)
            {
                return NotFound();
            }

            ListItem updatedItem = await _itemModificationService.UpdateExistingItemAsync(acquisitionResult, item);

            return Ok(updatedItem);
        }

        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            ValidateItemId(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ListItem deletedItem = await _listItemsRepository.DeleteAsync(id);

            if (deletedItem == null)
            {
                return NotFound();
            }

            return Ok(deletedItem);
        }

        private void ValidateItemId(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(ListItem.Id), "Guid is empty");
            }
        }

        private void ValidateItemText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ModelState.AddModelError(nameof(ListItem.Text), "Text is null or empty");
            }
        }

        private void ValidatePostItem(ListItem item)
        {
            if (item == null)
            {
                ModelState.AddModelError(String.Empty, "Item is null");
                return;
            }

            ValidateItemText(item.Text);

            if (item.Id != Guid.Empty)
            {
                ModelState.AddModelError(nameof(ListItem.Id), "Guid is not empty");
            }
        }
    }
}