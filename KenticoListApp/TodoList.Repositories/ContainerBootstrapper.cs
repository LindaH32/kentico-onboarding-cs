using Microsoft.Practices.Unity;
using TodoList.Contracts.Interfaces;

namespace TodoList.Repositories
{
    public static class ContainerBootstrapper
    {
        public static void RegisterRepositoryTypes(this IUnityContainer container)
            => container
            .RegisterType<IListItemRepository, ListItemRepository>();
    }
}