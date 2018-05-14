using Microsoft.Extensions.DependencyInjection;

namespace Uber.Module.File.Abstraction
{
    public interface IFileBuilder
    {
        IServiceCollection Services { get; }
    }
}
