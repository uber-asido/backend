using System.Threading.Tasks;

namespace Uber.Module.Geocoding.Abstraction.Manager
{
    public class Geocode
    {
        public string FormattedAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public interface IGeocodeProvider
    {
        Task<Geocode> Geocode(string location);
    }
}
