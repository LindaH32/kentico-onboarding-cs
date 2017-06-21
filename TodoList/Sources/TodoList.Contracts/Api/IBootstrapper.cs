using Microsoft.Practices.Unity;

namespace TodoList.Contracts.Api
{
    public interface IBootstrapper
    {
        IUnityContainer Register(IUnityContainer container);
    }
}