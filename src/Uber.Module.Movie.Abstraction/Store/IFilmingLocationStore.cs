using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Model;

namespace Uber.Module.Movie.Abstraction.Store
{
    public interface IFilmingLocationStore
    {
        Task<List<FilmingLocation>> Find();
        Task<List<FilmingLocation>> Find(IEnumerable<Guid> movieKeys);
    }
}
