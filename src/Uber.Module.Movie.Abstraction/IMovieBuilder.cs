using Microsoft.Extensions.DependencyInjection;

namespace Uber.Module.Movie.Abstraction
{
    public interface IMovieBuilder
    {
        IServiceCollection Services { get; }
    }
}
