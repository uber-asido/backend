using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Geocoding.Abstraction.Store;

namespace Uber.Module.Geocoding.EFCore.Store
{
    public class NotFoundStore : INotFoundStore
    {
        private readonly DataStore db;

        public NotFoundStore(DataStore db)
        {
            this.db = db;
        }

        public Task<LocationNotFound> Find(string unformattedAddress)
            => db.LocationsNotFound.FirstOrDefaultAsync(e => e.UnformattedAddress == unformattedAddress);

        public Task Insert(LocationNotFound location) => db.InsertAndCommit(location);
    }
}
