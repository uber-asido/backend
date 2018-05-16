using System;

namespace Uber.Module.Geocoding.Abstraction.Model
{
    public class LocationNotFound
    {
        public Guid Key { get; set; }
        public string UnformattedAddress { get; set; }
    }
}
