using System;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;

namespace Uber.Module.Geocoding.Google
{
    public class GoogleGeocodeProvider : IGeocodeProvider
    {
        public Task<Geocode> Geocode(string location)
        {
            throw new NotImplementedException();
        }
    }
}
