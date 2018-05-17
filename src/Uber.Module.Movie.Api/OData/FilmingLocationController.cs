using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> SearchByFreeText(string text)
        {
            var locations = await filmingLocationService.Find(text);
            return Ok(locations);
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> SearchBySearchItem(Guid searchItemKey)
        {
            var locations = await filmingLocationService.Find(searchItemKey);
            return Ok(locations);
        }
    }
}

