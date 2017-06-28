using TodoList.Contracts.Models;

namespace TodoList.Contracts.Services
{
    public interface IListItemGuidGenerator
    {
        ListItem ListItemWithGuid(ListItem item);
    }
}
