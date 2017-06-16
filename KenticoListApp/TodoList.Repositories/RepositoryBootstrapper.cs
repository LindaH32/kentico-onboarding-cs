using Microsoft.Practices.Unity;
using TodoList.Contracts.Interfaces;

namespace TodoList.Repositories
{
    public class RepositoryBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container) 
            => container.RegisterType<IListItemRepository, ListItemRepository>();
    }
}