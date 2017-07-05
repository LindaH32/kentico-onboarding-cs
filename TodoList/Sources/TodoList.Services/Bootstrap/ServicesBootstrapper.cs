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
                .RegisterType<ICreateItemService, CreateItemService>(new HierarchicalLifetimeManager())
                .RegisterType<IUpdateItemService, UpdateItemService>(new HierarchicalLifetimeManager())
                .RegisterType<IDateTimeGenerator, DateTimeGenerator>(new HierarchicalLifetimeManager());
    }
}
