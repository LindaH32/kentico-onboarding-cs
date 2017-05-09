using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
