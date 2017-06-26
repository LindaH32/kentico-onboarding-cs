using TodoList.Contracts.Models;

namespace TodoList.Api.Services
{
    public interface IListItemUrlGenerator
    {
        string GenerateUrl(ListItem item);
    }
}