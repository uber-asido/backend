using System;

namespace Uber.Module.Movie.EFCore.Entity
{
    public class MovieWriter
    {
        public Guid MovieKey { get; set; }
        public Guid WriterKey { get; set; }
    }
}
