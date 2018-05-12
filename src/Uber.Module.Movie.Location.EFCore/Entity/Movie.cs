using System;

namespace Uber.Module.Movie.Location.EFCore.Entity
{
    public class Movie
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string[] FunFacts { get; set; }
    }
}
