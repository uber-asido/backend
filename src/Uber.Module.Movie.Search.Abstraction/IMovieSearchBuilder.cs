using Microsoft.Extensions.DependencyInjection;

namespace Uber.Module.Movie.Search.Abstraction
{
    public interface IMovieSearchBuilder
    {
        IServiceCollection Services { get; }
    }
}
