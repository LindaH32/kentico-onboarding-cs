using Microsoft.Practices.Unity;
using TodoList.Contracts.Bootstrap;
using TodoList.Contracts.Services;
using TodoList.Services.ListItemServices;
using TodoList.Services.Wrappers;

namespace TodoList.Services
{
    public class ServicesBootstrapper : IBootstrapper
    {
        public IUnityContainer Register(IUnityContainer container)
            => container
                .RegisterType<IGuidGenerator, GuidGenerator>(new HierarchicalLifetimeManager())
                .RegisterType<IItemCreationService, ItemCreationService>(new HierarchicalLifetimeManager())
                .RegisterType<IItemModificationService, ItemModificationService>(new HierarchicalLifetimeManager())
                .RegisterType<IDateTimeGenerator, DateTimeGenerator>(new HierarchicalLifetimeManager())
                .RegisterType<IItemAcquisitionService, ItemAcquisitionService>(new HierarchicalLifetimeManager());
    }
}
