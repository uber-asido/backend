using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Uber.Module.Geocoding.Abstraction.Service;

namespace Uber.Module.Geocoding.Google
{
    public class GoogleGeocodeProvider : IGeocodeProvider
    {
        private const string geocodingApi = "https://maps.googleapis.com/maps/api/geocode/json";

        private readonly string apiKey;

        public GoogleGeocodeProvider(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<Geocode> Geocode(string location)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(geocodingApi + $"?address={location}&key={apiKey}");
                var content = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<GeocodeResponse>(content);

                if (json.Status == "ZERO_RESULTS" || json.Status == "OVER_QUERY_LIMIT")
                    return null;

                if (json.Status != "OK")
                    throw new Exception($"Failed to query location. Status={json.Status}. Location={location}");

                var geocode = new Geocode
                {
                    Latitude = json.Results[0].Geometry.Location.Latitude,
                    Longitude = json.Results[0].Geometry.Location.Longitude,
                    FormattedAddress = json.Results[0].FormattedAddress,
                };
                return geocode;
            }
        }
    }
}
