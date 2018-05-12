using Microsoft.Extensions.DependencyInjection;

namespace Uber.Module.Search.Abstraction
{
    public interface ISearchBuilder
    {
        IServiceCollection Services { get; }
    }
}
