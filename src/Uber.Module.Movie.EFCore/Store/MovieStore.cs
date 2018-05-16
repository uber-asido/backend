using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Movie.Abstraction.Model;
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
                             Actors = (from actorRef in db.MovieActors
                                       join actor in db.Actors on actorRef.ActorKey equals actor.Key
                                       where actorRef.MovieKey == movie.Key
                                       select new Actor
                                       {
                                           Key = actor.Key,
                                           FullName = actor.FullName
                                       }).ToList(),
                             Directors = (from directorRef in db.MovieDirectors
                                          join director in db.Directors on directorRef.DirectorKey equals director.Key
                                          where directorRef.MovieKey == movie.Key
                                          select new Director
                                          {
                                              Key = director.Key,
                                              FullName = director.FullName
                                          }).ToList(),
                             Distributors = (from distributorRef in db.MovieDistributors
                                             join distributor in db.Distributors on distributorRef.DistributorKey equals distributor.Key
                                             where distributorRef.MovieKey == movie.Key
                                             select new Distributor
                                             {
                                                 Key = distributor.Key,
                                                 Name = distributor.Name
                                             }).ToList(),
                             FilmingLocations = (from locationRef in db.FilmingLocations
                                                 where locationRef.MovieKey == movie.Key
                                                 select new FilmingLocation
                                                 {
                                                     Key = locationRef.Key,
                                                     AddressKey = locationRef.AddressKey,
                                                     FunFact = locationRef.FunFact
                                                 }).ToList(),
                             ProductionCompanies = (from companyRef in db.MovieProductionCompanies
                                                    join company in db.ProductionCompanies on companyRef.ProductionCompanyKey equals company.Key
                                                    where companyRef.MovieKey == movie.Key
                                                    select new ProductionCompany
                                                    {
                                                        Key = company.Key,
                                                        Name = company.Name
                                                    }).ToList(),
                             Writers = (from writerRef in db.MovieWriters
                                        join writer in db.Writers on writerRef.WriterKey equals writer.Key
                                        where writerRef.MovieKey == movie.Key
                                        select new Writer
                                        {
                                            Key = writer.Key,
                                            FullName = writer.FullName
                                        }).ToList()
                         };
        }

        public Task<Abstraction.Model.Movie> Find(Guid key) => movieQuery.SingleOrDefaultAsync(e => e.Key == key);

        public async Task<Abstraction.Model.Movie> Merge(Abstraction.Model.Movie movieNew)
        {
            var movieOld = await movieQuery.FirstOrDefaultAsync(e => e.Title == movieNew.Title && e.ReleaseYear == movieNew.ReleaseYear);

            if (movieOld == null)
            {
                movieOld = new Abstraction.Model.Movie
                {
                    Key = Guid.NewGuid(),
                    Title = movieNew.Title,
                    ReleaseYear = movieNew.ReleaseYear,
                    Actors = new List<Actor>(),
                    Directors = new List<Director>(),
                    Distributors = new List<Distributor>(),
                    FilmingLocations = new List<FilmingLocation>(),
                    ProductionCompanies = new List<ProductionCompany>(),
                    Writers = new List<Writer>()
                };

                db.Insert(new Entity.Movie
                {
                    Key = movieOld.Key,
                    Title = movieOld.Title,
                    ReleaseYear = movieOld.ReleaseYear
                });
            }

            if (movieNew.Actors.Any())
            {
                var names = movieNew.Actors.Select(e => e.FullName);
                var existingActors = await db.Actors.Where(e => names.Contains(e.FullName)).ToListAsync();

                foreach (var name in names)
                {
                    if (movieOld.Actors.Any(e => e.FullName == name))
                        continue;

                    var entity = existingActors.FirstOrDefault(e => e.FullName == name);

                    if (entity == null)
                    {
                        entity = new Actor
                        {
                            Key = Guid.NewGuid(),
                            FullName = name
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieActor { MovieKey = movieOld.Key, ActorKey = entity.Key });
                }
            }

            if (movieNew.Directors.Any())
            {
                var names = movieNew.Directors.Select(e => e.FullName);
                var existingDirectors = await db.Directors.Where(e => names.Contains(e.FullName)).ToListAsync();

                foreach (var name in names)
                {
                    if (movieOld.Directors.Any(e => e.FullName == name))
                        continue;

                    var entity = existingDirectors.FirstOrDefault(e => e.FullName == name);

                    if (entity == null)
                    {
                        entity = new Director
                        {
                            Key = Guid.NewGuid(),
                            FullName = name
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieDirector { MovieKey = movieOld.Key, DirectorKey = entity.Key });
                }
            }

            if (movieNew.Distributors.Any())
            {
                var names = movieNew.Distributors.Select(e => e.Name);
                var existingDistributors = await db.Distributors.Where(e => names.Contains(e.Name)).ToListAsync();

                foreach (var name in names)
                {
                    if (movieOld.Distributors.Any(e => e.Name == name))
                        continue;

                    var entity = existingDistributors.FirstOrDefault(e => e.Name == name);

                    if (entity == null)
                    {
                        entity = new Distributor
                        {
                            Key = Guid.NewGuid(),
                            Name = name
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieDistributor { MovieKey = movieOld.Key, DistributorKey = entity.Key });
                }
            }

            if (movieNew.FilmingLocations.Any())
            {
                // Addresses are in a geocoding module and they have to be pre-filled by the callee.
                if (movieNew.FilmingLocations.All(e => e.AddressKey == default(Guid)))
                    throw new ArgumentException("The addresses don't have keys set.", nameof(movieNew.FilmingLocations));

                foreach (var location in movieNew.FilmingLocations)
                {
                    if (movieOld.FilmingLocations.Any(e => e.AddressKey == location.AddressKey))
                        continue;

                    db.Insert(new Entity.FilmingLocation
                    {
                        Key = location.Key,
                        MovieKey = movieOld.Key,
                        AddressKey = location.AddressKey,
                        FunFact = location.FunFact
                    });
                }
            }

            if (movieNew.ProductionCompanies.Any())
            {
                var names = movieNew.ProductionCompanies.Select(e => e.Name);
                var existingCompanies = await db.ProductionCompanies.Where(e => names.Contains(e.Name)).ToListAsync();

                foreach (var name in names)
                {
                    if (movieOld.ProductionCompanies.Any(e => e.Name == name))
                        continue;

                    var entity = existingCompanies.FirstOrDefault(e => e.Name == name);

                    if (entity == null)
                    {
                        entity = new ProductionCompany
                        {
                            Key = Guid.NewGuid(),
                            Name = name
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieProductionCompany { MovieKey = movieOld.Key, ProductionCompanyKey = entity.Key });
                }
            }

            if (movieNew.Writers.Any())
            {
                var names = movieNew.Writers.Select(e => e.FullName);
                var existingWriters = await db.Writers.Where(e => names.Contains(e.FullName)).ToListAsync();

                foreach (var name in names)
                {
                    if (movieOld.Writers.Any(e => e.FullName == name))
                        continue;

                    var entity = existingWriters.FirstOrDefault(e => e.FullName == name);

                    if (entity == null)
                    {
                        entity = new Writer
                        {
                            Key = Guid.NewGuid(),
                            FullName = name
                        };
                        db.Insert(entity);
                    }

                    db.Insert(new Entity.MovieWriter { MovieKey = movieOld.Key, WriterKey = entity.Key });
                }
            }

            await db.Commit();

            return await Find(movieOld.Key);
        }
    }
}
