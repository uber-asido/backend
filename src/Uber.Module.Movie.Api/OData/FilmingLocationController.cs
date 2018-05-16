using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Uber.Core.OData;
using Uber.Module.Movie.Abstraction.Model;
using Uber.Module.Movie.Abstraction.Service;

namespace Uber.Module.Movie.Api.OData
{
    public class FilmingLocationController : UberODataController
    {
        private readonly IFilmingLocationService filmingLocationService;

        public FilmingLocationController(IFilmingLocationService filmingLocationService)
        {
            this.filmingLocationService = filmingLocationService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IQueryable<FilmingLocation>> Get()
        {
            var locations = await filmingLocationService.Find();
            return locations.AsQueryable();
        }
    }
}

