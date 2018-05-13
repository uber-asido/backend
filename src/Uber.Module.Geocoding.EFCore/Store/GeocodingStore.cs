﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.Abstraction.Store;

namespace Uber.Module.Geocoding.EFCore.Store
{
    public class GeocodingStore : IGeocodingStore
    {
        public IQueryable<Address> Query()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Address> QuerySingle(Guid key)
        {
            throw new NotImplementedException();
        }

        public Task<Address> Find(Guid key) => QuerySingle(key).SingleOrDefaultAsync();
        public Task<List<Address>> Find(IEnumerable<Guid> keys) => Query().Where(e => keys.Contains(e.Key)).ToListAsync();

        public Task<Address> Find(string unformattedAddress)
        {
            throw new NotImplementedException();
        }

        public Task<Address> Create(string unformattedAddress, Address address)
        {
            throw new NotImplementedException();
        }
    }
}