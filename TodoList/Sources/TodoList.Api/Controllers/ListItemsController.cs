using System;
using System.Threading.Tasks;
using System.Web.Http;
using TodoList.Contracts.Api;
using TodoList.Contracts.Models;
using TodoList.Contracts.Repositories;

namespace TodoList.Api.Controllers
{
    public class ListItemsController : ApiController
    {
        private readonly IListItemRepository _listItemsRepository;
        private readonly IListItemUrlGenerator _urlGenerator;

        public ListItemsController(IListItemRepository listItemsRepository, IListItemUrlGenerator generator)
        {
            _listItemsRepository = listItemsRepository;
            _urlGenerator = generator;
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
            return Ok(await _listItemsRepository.GetAsync(id));
        }

        public async Task<IHttpActionResult> PostAsync(ListItem item)
        {
            ValidatePostItem(item);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ListItem createdItem = await _listItemsRepository.CreateAsync(item);
            string location = _urlGenerator.GenerateUrl(createdItem);

            return Created(location, createdItem);
        }

        public async Task<IHttpActionResult> PutAsync(ListItem item) 
            => Ok(await _listItemsRepository.UpdateAsync(item));

        public async Task<IHttpActionResult> DeleteAsync(Guid id) 
            => Ok(await _listItemsRepository.DeleteAsync(id));

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