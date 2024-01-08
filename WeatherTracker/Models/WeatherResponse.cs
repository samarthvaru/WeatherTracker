using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherTracker.Models
{
    public class WeatherResponse
    {
        public Request Request { get; set; }
        public Location Location { get; set; }
        public Current Current { get; set; }
    }

    public class Request
    {
        public string Type { get; set; }
        public string Query { get; set; }
        // Other properties...
    }

    public class Location
    {
        public string Name { get; set; }
        public string Country { get; set; }
        // Other properties...
    }

    public class Current
    {
        [JsonProperty("observation_time")]
        public string ObservationTime { get; set; }
        public int Temperature { get; set; }

        [JsonProperty("weather_descriptions")]
        public List<string> WeatherDescriptions { get; set; }
        // Other properties...
    }
}
