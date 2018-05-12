using System;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Model;
using Uber.Module.Movie.Abstraction.Store;

namespace Uber.Module.Movie.EFCore.Store
{
    public class MovieStore : IMovieStore
    {
        private readonly DataStore db;

        private readonly IQueryable<Abstraction.Model.Movie> movieQuery;

        public MovieStore(DataStore db)
        {
            this.db = db;

            movieQuery = from movie in db.Movies
                         select new Abstraction.Model.Movie
                         {
                             Key = movie.Key,
                             Title = movie.Title,
                             ReleaseYear = movie.ReleaseYear,
                             FunFacts = movie.FunFacts,
                             Actors = (from actorRef in db.MovieActors
                                       join actor in db.Actors on actorRef.ActorKey equals actor.Key
                                       where actorRef.MovieKey == movie.Key
                                       select new Abstraction.Model.Actor
                                       {
                                           Key = actor.Key,
                                           FullName = actor.FullName
                                       }).ToList(),
                             Distributors = (from distributorRef in db.MovieDistributors
                                             join distributor in db.Distributors on distributorRef.DistributorKey equals distributor.Key
                                             where distributorRef.MovieKey == movie.Key
                                             select new Abstraction.Model.Distributor
                                             {
                                                 Key = distributor.Key,
                                                 Name = distributor.Name
                                             }).ToList(),
                             FilmingAddresses = (from addressRef in db.MovieFilmingAddresses
                                                 where addressRef.MovieKey == movie.Key
                                                 select new Address { Key = addressRef.AddressKey }).ToList(),
                             ProductionCompanies = (from companyRef in db.MovieProductionCompanies
                                                    join company in db.ProductionCompanies on companyRef.ProductionCompanyKey equals company.Key
                                                    where companyRef.MovieKey == movie.Key
                                                    select new Abstraction.Model.ProductionCompany
                                                    {
                                                        Key = company.Key,
                                                        Name = company.Name
                                                    }).ToList(),
                             Writers = (from writerRef in db.MovieWriters
                                        join writer in db.Writers on writerRef.WriterKey equals writer.Key
                                        where writerRef.MovieKey == movie.Key
                                        select new Abstraction.Model.Writer
                                        {
                                            Key = writer.Key,
                                            FullName = writer.FullName
                                        }).ToList()
                         };
        }

        public IQueryable<Abstraction.Model.Movie> Query() => movieQuery;
        public IQueryable<Abstraction.Model.Movie> QuerySingle(Guid key) => movieQuery.Where(e => e.Key == key);

        public Task Create(Abstraction.Model.Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
