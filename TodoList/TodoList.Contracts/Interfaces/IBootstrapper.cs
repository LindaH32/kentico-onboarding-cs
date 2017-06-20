using Microsoft.Practices.Unity;

namespace TodoList.Contracts.Interfaces
{
    public interface IBootstrapper
    {
        IUnityContainer Register(IUnityContainer container);
    }
}