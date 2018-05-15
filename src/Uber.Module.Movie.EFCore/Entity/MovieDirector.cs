using System;

namespace Uber.Module.Movie.EFCore.Entity
{
    public class MovieDirector
    {
        public Guid MovieKey { get; set; }
        public Guid DirectorKey { get; set; }
    }
}
