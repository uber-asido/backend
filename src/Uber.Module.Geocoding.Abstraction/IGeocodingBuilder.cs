using Microsoft.Extensions.DependencyInjection;

namespace Uber.Module.Geocoding.Abstraction
{
    public interface IGeocodingBuilder
    {
        IServiceCollection Services { get; }
    }
}
