using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Core.OData;
using Uber.Module.Movie.Search.Abstraction.Model;
using Uber.Module.Movie.Search.Manager;

namespace Uber.Module.Movie.Search.Api.OData
{
    public class SearchItemController : UberODataController
    {
        private readonly SearchManager searchManager;

        public SearchItemController(SearchManager searchManager)
        {
            this.searchManager = searchManager;
        }

        [EnableQuery]
        [HttpGet]
        public IQueryable<SearchItem> Get() => searchManager.Query();

        [EnableQuery]
        [HttpGet]
        public IQueryable<SearchItem> Get([FromODataUri] Guid key) => searchManager.QuerySingle(key);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SearchItem model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await searchManager.Create(model);
            return Created(result);
        }
    }
}
