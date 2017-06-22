using Microsoft.Practices.Unity;
using TodoList.Contracts.Bootstrap;
using TodoList.Contracts.Repositories;

namespace TodoList.Repositories
{
    public class RepositoryBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container.RegisterType<IListItemRepository, ListItemRepository>(new HierarchicalLifetimeManager());
    }
}