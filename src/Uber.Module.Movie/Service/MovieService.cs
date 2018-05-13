using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Store;

namespace Uber.Module.Movie.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieStore movieStore;

        public MovieService(IMovieStore movieStore)
        {
            this.movieStore = movieStore;
        }

        public IQueryable<Abstraction.Model.Movie> Query() => movieStore.Query();
        public IQueryable<Abstraction.Model.Movie> QuerySingle(Guid key) => movieStore.QuerySingle(key);

        public Task<Abstraction.Model.Movie> Create(Abstraction.Model.Movie movie)
        {
            if (movie.Key == default(Guid))
                movie.Key = Guid.NewGuid();

            return movieStore.Create(movie);
        }
    }
}
