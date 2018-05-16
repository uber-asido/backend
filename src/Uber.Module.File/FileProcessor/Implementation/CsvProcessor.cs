using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uber.Module.Movie.Abstraction.Model;

namespace Uber.Module.File.FileProcessor
{
    public class CsvRow
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string Location { get; set; }
        public string FunFact { get; set; }
        public string ProductionCompany { get; set; }
        public string Distributor { get; set; }
        public string Directors { get; set; }
        public string Writers { get; set; }
        public string Actor1 { get; set; }
        public string Actor2 { get; set; }
        public string Actor3 { get; set; }
    }

    public sealed class CsvColumnMap : ClassMap<CsvRow>
    {
        public CsvColumnMap()
        {
            Map(e => e.Title).Name("Title");
            Map(e => e.ReleaseYear).Name("Release Year");
            Map(e => e.Location).Name("Locations");
            Map(e => e.FunFact).Name("Fun Facts");
            Map(e => e.ProductionCompany).Name("Production Company");
            Map(e => e.Distributor).Name("Distributor");
            Map(e => e.Directors).Name("Director");
            Map(e => e.Writers).Name("Writer");
            Map(e => e.Actor1).Name("Actor 1");
            Map(e => e.Actor2).Name("Actor 2");
            Map(e => e.Actor3).Name("Actor 3");
        }
    }

    internal class CsvProcessor : IFileProcessor
    {
        public ParseResult Parse(byte[] data)
        {
            var result = new ParseResult();

            var csv = new CsvReader(new StringReader(Encoding.UTF8.GetString(data)));
            csv.Configuration.RegisterClassMap<CsvColumnMap>();

            var movieMap = new Dictionary<string, Movie.Abstraction.Model.Movie>();
            var i = 0;

            foreach (var row in csv.GetRecords<CsvRow>())
            {
                i++;

                row.Actor1 = row.Actor1.Trim();
                row.Actor2 = row.Actor2.Trim();
                row.Actor3 = row.Actor3.Trim();
                row.Directors = row.Directors.Trim();
                row.Distributor = row.Distributor.Trim();
                row.FunFact = row.FunFact.Trim();
                row.Location = row.Location.Trim();
                row.ProductionCompany = row.ProductionCompany.Trim();
                row.Title = row.Title.Trim();
                row.Writers = row.Writers.Trim();

                if (string.IsNullOrWhiteSpace(row.Title))
                {
                    result.Errors.Add($"Row {i}: No movie title");
                    continue;
                }

                if (movieMap.TryGetValue(row.Title, out Movie.Abstraction.Model.Movie movie))
                {
                    if (movie.ReleaseYear != row.ReleaseYear)
                        result.Errors.Add($"Row {i}: Overwrites release year '{movie.ReleaseYear}' with '{row.ReleaseYear}'");
                }
                else
                {
                    movie = new Movie.Abstraction.Model.Movie
                    {
                        Title = row.Title,
                        Actors = new List<Actor>(),
                        Directors = new List<Director>(),
                        Distributors = new List<Distributor>(),
                        FilmingLocations = new List<FilmingLocation>(),
                        ProductionCompanies = new List<ProductionCompany>(),
                        Writers = new List<Writer>()
                    };
                    movieMap.Add(movie.Title, movie);
                }

                movie.ReleaseYear = row.ReleaseYear;

                if (row.ProductionCompany.Length > 0 && !movie.ProductionCompanies.Any(e => e.Name == row.ProductionCompany))
                    movie.ProductionCompanies.Add(new ProductionCompany { Name = row.ProductionCompany });

                if (row.Distributor.Length > 0 && !movie.Distributors.Any(e => e.Name == row.Distributor))
                    movie.Distributors.Add(new Distributor { Name = row.Distributor });

                if (row.Location.Length > 0 && !movie.FilmingLocations.Any(e => e.FormattedAddress == row.Location))
                    movie.FilmingLocations.Add(new FilmingLocation { FormattedAddress = row.Location, FunFact = row.FunFact });

                foreach (var name in ParseNames(row.Directors))
                {
                    if (!movie.Directors.Any(e => e.FullName == name))
                        movie.Directors.Add(new Director { FullName = name });
                }

                foreach (var name in ParseNames(row.Writers))
                {
                    if (!movie.Writers.Any(e => e.FullName == name))
                        movie.Writers.Add(new Writer { FullName = name });
                }

                foreach (var name in new[] { row.Actor1, row.Actor2, row.Actor3 })
                {
                    if (name.Length > 0 && !movie.Actors.Any(e => e.FullName == name))
                        movie.Actors.Add(new Actor { FullName = name });
                }
            }

            result.Movies.AddRange(movieMap.Values);

            return result;
        }

        private IEnumerable<string> ParseNames(string name)
        {
            return name
                .Split(new[] { ",", "&", " and " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .Where(e => e.Length > 0);
        }
    }
}
