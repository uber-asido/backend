using System;

namespace Uber.Module.Movie.EFCore.Entity
{
    public class MovieProductionCompany
    {
        public Guid MovieKey { get; set; }
        public Guid ProductionCompanyKey { get; set; }
    }
}
