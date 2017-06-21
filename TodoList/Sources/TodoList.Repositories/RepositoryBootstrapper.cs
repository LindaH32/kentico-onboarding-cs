using Microsoft.Practices.Unity;
using TodoList.Contracts.Api;
using TodoList.Contracts.Repositories;

namespace TodoList.Repositories
{
    public class RepositoryBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container) //TODO Choose right lifetime manager
            => container.RegisterType<IListItemRepository, ListItemRepository>(new TransientLifetimeManager());
    }
}