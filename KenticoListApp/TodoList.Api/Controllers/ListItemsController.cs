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
        private IListItemUrlGenerator _urlGenerator;

        public ListItemsController(IListItemRepository repository, IListItemUrlGenerator generator)
        {
            _repository = repository;
            _urlGenerator = generator;
        }

        public async Task<IHttpActionResult> GetAsync()
            => Ok(await Task.FromResult(_repository.Get()));

        public async Task<IHttpActionResult> GetAsync(Guid id)
            => Ok(await Task.FromResult(_repository.Get(id)));

        public async Task<IHttpActionResult> PostAsync(ListItem item)
        {
            ValidatePostItem(item);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            string location = _urlGenerator.GenerateUrl(item);

            return Created(location, await Task.FromResult(_repository.Post(item)));
        }

        public async Task<IHttpActionResult> PutAsync(ListItem item)
            => Ok(await Task.FromResult(_repository.Put(item)));

        public async Task<IHttpActionResult> DeleteAsync(Guid id)
            => Ok(await Task.FromResult(_repository.Delete(id)));

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