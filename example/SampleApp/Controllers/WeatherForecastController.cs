using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using kr.bbon.AspNetCore;
using kr.bbon.AspNetCore.Mvc;
using kr.bbon.AspNetCore.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using kr.bbon.AspNetCore.Models;
using SampleApp.Models;

namespace SampleApp.Controllers
{
    [ApiVersion(DefaultValues.ApiVersion)]
    [ApiController]
    [Route(DefaultValues.RouteTemplate)]
    [Area(DefaultValues.AreaName)]
    [ApiExceptionHandlerFilter]
    public class WeatherForecastController : ApiControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Produces(typeof(ApiResponseModel<IEnumerable<WeatherForecast>>))]
        public IActionResult Get()
        {
            var rng = new Random();
            var items = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return StatusCode(System.Net.HttpStatusCode.OK, items);
        }   
    }
}
