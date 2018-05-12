using Microsoft.Extensions.DependencyInjection;

namespace Uber.Module.Movie.Abstraction
{
    public interface IFilmMovieBuilder
    {
        IServiceCollection Services { get; }
    }
}
