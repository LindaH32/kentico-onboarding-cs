using System;
using System.Threading.Tasks;
using System.Web.Http;
using TodoList.Contracts.Interfaces;
using TodoList.Contracts.Models;

namespace TodoList.Api.Controllers
{
    public class ListItemsController : ApiController
    {
        private readonly IListItemRepository _repository;
        private readonly IListItemUrlGenerator _urlGenerator;

        public ListItemsController(IListItemRepository repository, IListItemUrlGenerator generator)
        {
            _repository = repository;
            _urlGenerator = generator;
        }

        public async Task<IHttpActionResult> GetAsync()
        {
            var items = await _repository.GetAllAsync(); 
            return Ok(items);
        }

        public async Task<IHttpActionResult> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return Ok(item);
        }

        public async Task<IHttpActionResult> PostAsync(ListItem item)
        {
            ValidatePostItem(item);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ListItem createdItem = await _repository.CreateAsync(item);
            string location = _urlGenerator.GenerateUrl(createdItem);

            return Created(location, createdItem);
        }

        public async Task<IHttpActionResult> PutAsync(ListItem item)
        {
            var updatedItem = await _repository.UpdateAsync(item);
            return Ok(updatedItem);
        }

        public async Task<IHttpActionResult> DeleteAsync(Guid id)
        {
            var deletedItem = await _repository.DeleteAsync(id); 
            return Ok(deletedItem);

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
    }
}