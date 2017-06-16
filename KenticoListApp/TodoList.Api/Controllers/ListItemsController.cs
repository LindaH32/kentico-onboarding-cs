using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using TodoList.Api.Services;
using TodoList.Contracts.Interfaces;
using TodoList.Contracts.Models;

namespace TodoList.Api.Controllers
{
    public class ListItemsController : ApiController
    {
        private readonly IListItemRepository _repository;
        private readonly ListItemUrlGenerator _urlGenerator;

        public ListItemsController(IListItemRepository repository)
        {
            //var message = (HttpRequestMessage) HttpContext.Current.Items["MS_HttpRequestMessage"];
            //var helper = new UrlHelper(message);
            _repository = repository;
            _urlGenerator = new ListItemUrlGenerator(Url);
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
           
            //var message = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
            //var helper = new UrlHelper(message);
            var location = _urlGenerator.GenerateUrl(item);
            //var location2 = helper.Route("DefaultApiV1", new { id = item.Id });

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