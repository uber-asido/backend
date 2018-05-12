using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Manager;
using Uber.Module.Movie.Abstraction.Store;

namespace Uber.Module.Movie.Manager
{
    public class MovieManager : IMovieManager
    {
        private readonly IMovieStore movieStore;

        public MovieManager(IMovieStore movieStore)
        {
            this.movieStore = movieStore;
        }

        public IQueryable<Abstraction.Model.Movie> Query() => movieStore.Query();
        public IQueryable<Abstraction.Model.Movie> QuerySingle(Guid key) => movieStore.QuerySingle(key);

        public async Task<Abstraction.Model.Movie> Create(Abstraction.Model.Movie movie)
        {
            if (movie.Key == default(Guid))
                movie.Key = Guid.NewGuid();

            await movieStore.Create(movie);
            return movie;
        }
    }
}
