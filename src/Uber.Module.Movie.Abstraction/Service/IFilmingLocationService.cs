using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Model;

namespace Uber.Module.Movie.Abstraction.Service
{
    public interface IFilmingLocationService
    {
        Task<List<FilmingLocation>> Find();
        Task<List<FilmingLocation>> Find(Guid searchItemKey);
        Task<List<FilmingLocation>> Find(string freeText);
    }
}
