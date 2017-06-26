using Microsoft.Practices.Unity;

namespace TodoList.Contracts.Bootstrap
{
    public interface IBootstrapper
    {
        IUnityContainer Register(IUnityContainer container);
    }
}