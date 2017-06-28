using Microsoft.Practices.Unity;
using TodoList.Contracts.Bootstrap;
using TodoList.Contracts.Services;
using TodoList.Services.ListItemController;

namespace TodoList.Services.Bootstrap
{
    public class ServicesBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container
                .RegisterType<IListItemGuidGenerator, ListItemGuidGenerator>(new HierarchicalLifetimeManager())
                .RegisterType<IListItemServices, ListItemServices>();
    }
}
