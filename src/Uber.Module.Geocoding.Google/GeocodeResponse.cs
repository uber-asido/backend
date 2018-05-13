using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Module.Geocoding.Google
{
    internal class ResponseLocation
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }

    internal class ResponseGeometry
    {
        [JsonProperty("location")]
        public ResponseLocation Location { get; set; }
    }

    internal class ResponseResults
    {
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public ResponseGeometry Geometry { get; set; }
    }

    internal class GeocodeResponse
    {
        [JsonProperty("results")]
        public List<ResponseResults> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
