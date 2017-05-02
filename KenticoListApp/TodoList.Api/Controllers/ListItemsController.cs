using System;
using System.Threading.Tasks;
using System.Web.Http;
using TodoList.Contracts.Interfaces;
using TodoList.Contracts.Models;

namespace TodoList.Api.Controllers
{
    public class ListItemsController : ApiController
    {
        private readonly IListItemRepository repository;

        public async Task<IHttpActionResult> PostAsync(ListItem item)
        {
            if (item == null)
                return BadRequest("Item is null");

            if (string.IsNullOrWhiteSpace(item.Text))
                return BadRequest("Text is null or empty");

            if (item.Id != Guid.Empty)
                return BadRequest("Guid must be empty");

            return Created("api/v1/items/?id=300...", await Task.FromResult(repository.Post(item)));
        }

        public async Task<IHttpActionResult> GetAsync()
            => Ok(await Task.FromResult(repository.Get()));

        public async Task<IHttpActionResult> GetAsync(Guid id)
            => Ok(await Task.FromResult(repository.Get(id)));

        public async Task<IHttpActionResult> DeleteAsync(Guid id)
            => Ok(await Task.FromResult(repository.Delete(id)));

        public async Task<IHttpActionResult> PutAsync(ListItem item)
            => Ok(await Task.FromResult(repository.Put(item)));
    }
}