using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Model;
using Uber.Module.Movie.Abstraction.Service;

namespace Uber.Core.Test.Mock
{
    public class MovieServiceMock : IMovieService
    {
        private readonly List<Movie> movies = new List<Movie>();

        public Task<Movie> Find(Guid key) => Task.FromResult(movies.SingleOrDefault(e => e.Key == key));

        public Task<Movie> Merge(Movie movie)
        {
            if (movie.Key == default(Guid))
            {
                movie.Key = Guid.NewGuid();
                movies.Add(movie);
            }
            else
            {
                var existing = Find(movie.Key);
                if (existing == null)
                    movies.Add(movie);
            }

            return Task.FromResult(movie);
        }
    }
}
