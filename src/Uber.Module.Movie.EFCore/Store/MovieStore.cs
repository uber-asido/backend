using Microsoft.EntityFrameworkCore;
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

        public async Task<Abstraction.Model.Movie> Create(Abstraction.Model.Movie movie)
        {
            db.Insert(new Entity.Movie
            {
                Key = movie.Key,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                FunFacts = movie.FunFacts.ToArray()
            });

            if (movie.Actors.Any())
            {
                var actorNames = movie.Actors.Select(e => e.FullName);
                var existingActors = await db.Actors.Where(e => actorNames.Contains(e.FullName)).ToListAsync();

                foreach (var actor in movie.Actors)
                {
                    var entity = existingActors.SingleOrDefault(e => e.FullName == actor.FullName);

                    if (entity == null)
                    {
                        entity = new Abstraction.Model.Actor
                        {
                            Key = Guid.NewGuid(),
                            FullName = actor.FullName
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieActor { MovieKey = movie.Key, ActorKey = entity.Key });
                    actor.Key = entity.Key;
                }
            }

            if (movie.Distributors.Any())
            {
                var distributorNames = movie.Distributors.Select(e => e.Name);
                var existingDistributors = await db.Distributors.Where(e => distributorNames.Contains(e.Name)).ToListAsync();

                foreach (var distributor in movie.Distributors)
                {
                    var entity = existingDistributors.SingleOrDefault(e => e.Name == distributor.Name);

                    if (entity == null)
                    {
                        entity = new Abstraction.Model.Distributor
                        {
                            Key = Guid.NewGuid(),
                            Name = distributor.Name
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieDistributor { MovieKey = movie.Key, DistributorKey = entity.Key });
                    distributor.Key = entity.Key;
                }
            }

            if (movie.FilmingAddresses.Any())
            {
                // Addresses are in a geocoding module and they have to be pre-filled by the callee.
                if (movie.FilmingAddresses.All(e => e.Key == default(Guid)))
                    throw new ArgumentException("The addresses don't have keys set.", nameof(movie.FilmingAddresses));

                foreach (var address in movie.FilmingAddresses)
                {
                    db.Insert(new Entity.MovieFilmingAddress { MovieKey = movie.Key, AddressKey = address.Key });
                }
            }

            if (movie.ProductionCompanies.Any())
            {
                var companyNames = movie.ProductionCompanies.Select(e => e.Name);
                var existingCompanies = await db.ProductionCompanies.Where(e => companyNames.Contains(e.Name)).ToListAsync();

                foreach (var company in movie.ProductionCompanies)
                {
                    var entity = existingCompanies.SingleOrDefault(e => e.Name == company.Name);

                    if (entity == null)
                    {
                        entity = new Abstraction.Model.ProductionCompany
                        {
                            Key = Guid.NewGuid(),
                            Name = company.Name
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieProductionCompany { MovieKey = movie.Key, ProductionCompanyKey = entity.Key });
                    company.Key = entity.Key;
                }
            }

            if (movie.Writers.Any())
            {
                var writerNames = movie.Writers.Select(e => e.FullName);
                var existingWriters = await db.Writers.Where(e => writerNames.Contains(e.FullName)).ToListAsync();

                foreach (var writer in movie.Writers)
                {
                    var entity = existingWriters.SingleOrDefault(e => e.FullName == writer.FullName);

                    if (entity == null)
                    {
                        entity = new Abstraction.Model.Writer
                        {
                            Key = Guid.NewGuid(),
                            FullName = writer.FullName
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieWriter { MovieKey = movie.Key, WriterKey = entity.Key });
                    writer.Key = entity.Key;
                }
            }

            await db.Commit();

            return movie;
        }
    }
}
