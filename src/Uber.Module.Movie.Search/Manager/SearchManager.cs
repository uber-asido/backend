﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Search.Abstraction.Manager;
using Uber.Module.Movie.Search.Abstraction.Model;

namespace Uber.Module.Movie.Search.Manager
{
    public class SearchManager : ISearchManager
    {
        public Task<SearchItem> Create(SearchItem search)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SearchItem> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<SearchItem> QuerySingle(Guid key)
        {
            throw new NotImplementedException();
        }
    }
}
