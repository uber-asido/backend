using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Model;

namespace Uber.Module.Movie.Abstraction.Service
{
    public interface IFilmingLocationService
    {
        Task<List<FilmingLocation>> Find();
    }
}
