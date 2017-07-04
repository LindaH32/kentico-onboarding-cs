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
                .RegisterType<IGuidGenerator, GuidGenerator>(new HierarchicalLifetimeManager())
                .RegisterType<IListItemService, ListItemService>(new HierarchicalLifetimeManager())
                .RegisterType<IDateTimeGenerator, DateTimeGenerator>(new HierarchicalLifetimeManager());
    }
}
