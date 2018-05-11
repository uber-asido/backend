using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Uber.Core.OData;
using Uber.Module.Movie.Search.Abstraction.Model;

namespace Uber.Module.Movie.Search.Api.OData
{
    public class SearchItemController : UberODataController
    {
        private static readonly List<SearchItem> searchItems = new List<SearchItem>
        {
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 1", Type = SearchItemType.Movie },
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 2", Type = SearchItemType.Movie },
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 3", Type = SearchItemType.Movie },
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 4", Type = SearchItemType.Organization },
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 5", Type = SearchItemType.Person },
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 6", Type = SearchItemType.Person },
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 7", Type = SearchItemType.Person },
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 8", Type = SearchItemType.Organization },
            new SearchItem { Key = Guid.NewGuid(), Text = "Item 9", Type = SearchItemType.Organization },
        };

        [EnableQuery]
        [HttpGet]
        public IQueryable<SearchItem> Get() => searchItems.AsQueryable();

        [EnableQuery]
        [HttpGet]
        public IActionResult Get([FromODataUri] Guid key)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = searchItems.SingleOrDefault(e => e.Key == key);
            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
