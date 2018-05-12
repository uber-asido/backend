using System;

namespace Uber.Module.Movie.EFCore.Entity
{
    public class MovieDistributor
    {
        public Guid MovieKey { get; set; }
        public Guid DistributorKey { get; set; }
    }
}
