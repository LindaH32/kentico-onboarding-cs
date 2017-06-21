using TodoList.Contracts.Models;

namespace TodoList.Contracts.Api
{
    public interface IListItemUrlGenerator
    {
        string GenerateUrl(ListItem item);
    }
}