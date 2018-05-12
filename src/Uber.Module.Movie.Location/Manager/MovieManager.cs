using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Location.Abstraction.Manager;
using Uber.Module.Movie.Location.Abstraction.Store;

namespace Uber.Module.Movie.Location.Manager
{
    public class MovieManager : IMovieManager
    {
        private readonly IMovieStore movieStore;

        public MovieManager(IMovieStore movieStore)
        {
            this.movieStore = movieStore;
        }

        public IQueryable<Abstraction.Model.Movie> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Abstraction.Model.Movie> QuerySingle(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<Abstraction.Model.Movie> Create(Abstraction.Model.Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
