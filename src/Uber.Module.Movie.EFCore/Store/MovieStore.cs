﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Store;

namespace Uber.Module.Movie.EFCore.Store
{
    public class MovieStore : IMovieStore
    {
        public IQueryable<Abstraction.Model.Movie> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Abstraction.Model.Movie> QuerySingle(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task Create(Abstraction.Model.Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}