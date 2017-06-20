using TodoList.Contracts.Models;

namespace TodoList.Contracts.Interfaces
{
    public interface IListItemUrlGenerator
    {
        string GenerateUrl(ListItem item);
    }
}