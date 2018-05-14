using System;

namespace Uber.Module.Geocoding.EFCore.Entity
{
    public class Location
    {
        public Guid Key { get; set; }
        public Guid AddressKey { get; set; }
        public string UnformattedAddress { get; set; }
    }
}
