using System;
using System.Collections.Generic;
using Uber.Module.Geocoding.Abstraction.Model;

namespace Uber.Module.Movie.Abstraction.Model
{
    public class Movie
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public IList<Address> FilmingAddresses { get; set; }
        public IList<ProductionCompany> ProductionCompanies { get; set; }
        public IList<Distributor> Distributors { get; set; }
        public IList<Writer> Writers { get; set; }
        public IList<Actor> Actors { get; set; }
        public IList<string> FunFacts { get; set; }
    }
}
