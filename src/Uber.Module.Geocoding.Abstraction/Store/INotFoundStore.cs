using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Geocoding.Abstraction.Store
{
    public interface INotFoundStore
    {
        Task<LocationNotFound> Find(string unformattedAddress);
        Task Insert(LocationNotFound location);
    }
}
