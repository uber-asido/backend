using System;

namespace Uber.Module.Movie.Abstraction.Model
{
    public class FilmingLocation
    {
        public Guid Key { get; set; }
        public Guid AddressKey { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string FormattedAddress { get; set; }
        public string FunFact { get; set; }
    }
}
