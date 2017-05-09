using Microsoft.Practices.Unity;
using TodoList.Contracts.Interfaces;

namespace TodoList.Repositories
{
    public class ContainerBootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IListItemRepository, ListItemRepository>();
        }
    }
}