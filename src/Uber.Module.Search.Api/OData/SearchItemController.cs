using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Core.OData;
using Uber.Module.Search.Abstraction.Service;
using Uber.Module.Search.Abstraction.Model;

namespace Uber.Module.Search.Api.OData
{
    public class SearchItemController : UberODataController
    {
        private readonly ISearchService searchService;

        public SearchItemController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [EnableQuery]
        [HttpGet]
        public IQueryable<SearchItem> Get() => searchService.Query();

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get([FromODataUri] Guid key)
        {
            var search = await searchService.Find(key);
            if (search == null)
                return NotFound();

            return Ok(search);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SearchItem model)
        {
            if (!ModelState.IsValid)
                return ODataBadRequest();

            var result = await searchService.Create(model);
            return Created(result);
        }
    }
}
