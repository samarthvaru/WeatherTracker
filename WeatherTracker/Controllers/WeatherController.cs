using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherTracker.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherTracker.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly BlobStorageService blobStorageService;
        private readonly WeatherSettings _weatherSettings;

        public WeatherController(IHttpClientFactory clientFactory, IConfiguration configuration,IOptions<WeatherSettings> weatherSettings)
        {
            _clientFactory = clientFactory;
            blobStorageService = new BlobStorageService(configuration);
            _weatherSettings = weatherSettings.Value;

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email,string cityName)
        {


            IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
            var client = _clientFactory.CreateClient();

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(cityName))
            {
                await blobStorageService.StoreUserCity(email, cityName);
            }
            string apiKey = _weatherSettings.WeatherApiKey;
            string apiUrl = $"http://api.weatherstack.com/current?access_key={apiKey}&query={cityName}";

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                WeatherResponse weather = JsonConvert.DeserializeObject<WeatherResponse>(json);

                // Pass the weather object to the view
                return View("WeatherResult",weather);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
