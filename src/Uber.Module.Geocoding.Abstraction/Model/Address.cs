using System;

namespace Uber.Module.Geocoding.Abstraction.Model
{
    public class Address
    {
        public Guid Key { get; set; }
        public string FormattedAddress { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
