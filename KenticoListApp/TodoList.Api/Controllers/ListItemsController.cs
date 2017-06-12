using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TodoList.Contracts.Interfaces;
using TodoList.Contracts.Models;

namespace TodoList.Api.Controllers
{
    public class ListItemsController : ApiController
    {
        private readonly IListItemRepository _repository;

        public ListItemsController(IListItemRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<IHttpActionResult> PostAsync(ListItem item)
        {
            if (item == null)
            {
                ModelState.AddModelError(string.Empty, "Item is null");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(item.Text))
            {
                ModelState.AddModelError(nameof(ListItem.Text), "Text is null or empty");
            }

            if (item.Id != Guid.Empty)
            {
                ModelState.AddModelError(nameof(ListItem.Id), "Guid is not empty");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("api/v1/items/?id=300...", await Task.FromResult(_repository.Post(item)));
        }

        public async Task<IHttpActionResult> GetAsync()
            => Ok(await Task.FromResult(_repository.Get()));

        public async Task<IHttpActionResult> GetAsync(Guid id)
            => Ok(await Task.FromResult(_repository.Get(id)));

        public async Task<IHttpActionResult> DeleteAsync(Guid id)
            => Ok(await Task.FromResult(_repository.Delete(id)));

        public async Task<IHttpActionResult> PutAsync(ListItem item)
            => Ok(await Task.FromResult(_repository.Put(item)));
    }
}