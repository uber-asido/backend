using Microsoft.Extensions.DependencyInjection;

namespace Uber.Module.Movie.Location.Abstraction
{
    public interface IFilmMovieBuilder
    {
        IServiceCollection Services { get; }
    }
}
