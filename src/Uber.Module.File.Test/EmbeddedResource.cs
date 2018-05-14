using System.Reflection;
using Uber.Core;

namespace Uber.Module.File.Test
{
    public class EmbeddedResource
    {
        public static byte[] GetBytes(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Uber.Module.File.Test." + name);
            return stream.ReadToEnd();
        }
    }
}
