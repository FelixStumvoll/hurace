namespace Hurace.Core.Logic
{
    public interface IServiceProvider
    {
        T? Resolve<T>() where T : class;
    }
}