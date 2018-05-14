using System.Collections.Generic;

namespace Uber.Module.File.FileProcessor
{
    internal class ParseResult
    {
        public readonly List<Movie.Abstraction.Model.Movie> Movies = new List<Movie.Abstraction.Model.Movie>();
        public readonly List<string> Errors = new List<string>();
    }

    internal interface IFileProcessor
    {
        ParseResult Parse(byte[] data);
    }
}
