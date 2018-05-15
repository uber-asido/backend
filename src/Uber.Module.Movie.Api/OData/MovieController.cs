using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Uber.Core.OData;
using Uber.Module.Movie.Abstraction.Service;

namespace Uber.Module.Movie.Api.OData
{
    public class MovieController : UberODataController
    {
        private readonly IMovieService movieService;

        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var movie = await movieService.Find(key);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
    }
}

