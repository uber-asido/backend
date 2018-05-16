using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Service;
using Uber.Module.Movie.Abstraction.Store;
using Uber.Module.Search.Abstraction.Model;
using Uber.Module.Search.Abstraction.Service;

namespace Uber.Module.Movie.Service
{
    internal class MovieService : IMovieService
    {
        private readonly FilmingLocationService filmingLocationService;
        private readonly ISearchService searchService;
        private readonly IMovieStore movieStore;

        public MovieService(FilmingLocationService filmingLocationService, ISearchService searchService, IMovieStore movieStore)
        {
            this.filmingLocationService = filmingLocationService;
            this.searchService = searchService;
            this.movieStore = movieStore;
        }

        public async Task<Abstraction.Model.Movie> Find(Guid key)
        {
            var movie = await movieStore.Find(key);
            if (movie != null)
                await ResolveLocations(movie);
            return movie;
        }

        public async Task<Abstraction.Model.Movie> Merge(Abstraction.Model.Movie movie)
        {
            var movieNew = await movieStore.Merge(movie);

            await ResolveLocations(movieNew);
            await CreateSearchEntries(movieNew);

            return movieNew;
        }

        private Task CreateSearchEntries(Abstraction.Model.Movie movie)
        {
            var items = new List<SearchItem>
            {
                new SearchItem { Text = movie.Title, Type = SearchItemType.Movie }
            };

            foreach (var actor in movie.Actors)
                items.Add(new SearchItem { Text = actor.FullName, Type = SearchItemType.Person });
            foreach (var director in movie.Directors)
                items.Add(new SearchItem { Text = director.FullName, Type = SearchItemType.Person });
            foreach (var writer in movie.Writers)
                items.Add(new SearchItem { Text = writer.FullName, Type = SearchItemType.Person });

            foreach (var distributor in movie.Distributors)
                items.Add(new SearchItem { Text = distributor.Name, Type = SearchItemType.Organization });
            foreach (var company in movie.ProductionCompanies)
                items.Add(new SearchItem { Text = company.Name, Type = SearchItemType.Organization });

            return searchService.Merge(movie.Key, items);
        }

        private async Task ResolveLocations(Abstraction.Model.Movie movie)
        {
            await filmingLocationService.ResolveLocations(movie.FilmingLocations);
        }
    }
}
