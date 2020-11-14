using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MockAssessment7b.Models;

namespace MockAssessment7b.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _jsonSerializerOptions;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            HttpClient httpClient = new HttpClient();
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://grandcircusco.github.io/demo-apis/");
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search(int Id)
        {
            var response =  await _httpClient.GetAsync($"donuts/{Id}.json");
            string responseToJson;
            Donut donutResult;
            if (response.IsSuccessStatusCode)
            {
                responseToJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                donutResult = JsonSerializer.Deserialize<Donut>(responseToJson, _jsonSerializerOptions);
            }
            else
            {
                donutResult = new Donut() { Name = "Not available" };
            }
           
            return View(donutResult);
            
        }

       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
