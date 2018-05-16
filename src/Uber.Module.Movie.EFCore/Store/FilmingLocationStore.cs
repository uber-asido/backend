using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Model;
using Uber.Module.Movie.Abstraction.Store;

namespace Uber.Module.Movie.EFCore.Store
{
    internal class FilmingLocationStore : IFilmingLocationStore
    {
        private readonly DataStore db;

        private readonly IQueryable<FilmingLocation> locationQuery;

        public FilmingLocationStore(DataStore db)
        {
            this.db = db;

            locationQuery = from entity in db.FilmingLocations
                            select new FilmingLocation
                            {
                                Key = entity.Key,
                                MovieKey = entity.MovieKey,
                                AddressKey = entity.AddressKey,
                                FunFact = entity.FunFact
                            };
        }

        public Task<List<FilmingLocation>> Find() => locationQuery.ToListAsync();
    }
}
