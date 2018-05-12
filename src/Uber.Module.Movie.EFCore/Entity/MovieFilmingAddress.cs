using System;

namespace Uber.Module.Movie.EFCore.Entity
{
    public class MovieFilmingAddress
    {
        public Guid MovieKey { get; set; }
        public Guid AddressKey { get; set; }
    }
}
