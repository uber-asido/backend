using System;

namespace Uber.Module.Movie.EFCore.Entity
{
    public class FilmingLocation
    {
        public Guid MovieKey { get; set; }
        public Guid AddressKey { get; set; }
        public string FunFact { get; set; }
    }
}
