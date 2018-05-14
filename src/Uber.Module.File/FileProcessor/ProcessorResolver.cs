using System.IO;

namespace Uber.Module.File.FileProcessor
{
    internal class ProcessorResolver
    {
        public static IFileProcessor Resolve(string filename)
        {
            switch (Path.GetExtension(filename))
            {
                case ".csv": return new CsvProcessor();
                default: return null;
            }
        }
    }
}
