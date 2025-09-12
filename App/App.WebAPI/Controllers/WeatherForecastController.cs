using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using App.Domain.Entities;
using System.Text.Json;

namespace POS.Api.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
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
        [Route("WeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            
            IEnumerable<WeatherForecast> result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            string jsonString = JsonSerializer.Serialize(result);

            _logger.LogError(new Exception(jsonString), "GetWeatherForecast");

            return result;
        }

        [HttpGet]
        [Route("Weather")]
        public IEnumerable<WeatherForecast> GetWeather()
        {

            IEnumerable<WeatherForecast> result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            //string jsonString = JsonSerializer.Serialize(result);

            //_logger.LogError(new Exception(jsonString), "GetWeatherForecast");

            return result;
        }
    }
}
