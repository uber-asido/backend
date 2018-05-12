using System;

namespace Uber.Module.Movie.EFCore.Entity
{
    public class MovieActor
    {
        public Guid MovieKey { get; set; }
        public Guid ActorKey { get; set; }
    }
}
