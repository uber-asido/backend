﻿using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                    movie = new Movie.Abstraction.Model.Movie();
                    movieMap.Add(row.Title, movie);
                }

                movie.ReleaseYear = row.ReleaseYear;
                movie.FilmingLocations.Add(new Movie.Abstraction.Model.FilmingLocation { FormattedAddress = row.Location, FunFact = row.FunFact });
                movie.ProductionCompanies.Add(new Movie.Abstraction.Model.ProductionCompany { Name = row.ProductionCompany });
                movie.Distributors.Add(new Movie.Abstraction.Model.Distributor { Name = row.Distributor });
                // TODO: Implement directors entity.
                //movie.Directors.AddRange(ParseNames(row.Directors).Select(name => new Movie.Abstraction.Model.Actor { FullName = name }));
                foreach (var writer in ParseNames(row.Writers).Select(name => new Movie.Abstraction.Model.Writer { FullName = row.Writers }))
                    movie.Writers.Add(writer);

                foreach (var name in new[] { row.Actor1, row.Actor2, row.Actor3 })
                {
                    if (!string.IsNullOrEmpty(name))
                        movie.Actors.Add(new Movie.Abstraction.Model.Actor { FullName = row.Actor1 });
                }
            }

            result.Movies.AddRange(movieMap.Values);

            return result;
        }

        private IEnumerable<string> ParseNames(string name)
        {
            return name
                .Split(',', '&')
                .Select(e => e.Trim())
                .Where(e => e.Length > 0);
        }
    }
}