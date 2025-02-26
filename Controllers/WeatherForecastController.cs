using Atelier.IoC.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atelier.IoC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IForecasts forecasts;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IForecasts forecasts)
        {
            _logger = logger;
            this.forecasts = forecasts;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await forecasts.GetForecastsAsync();
        }
    }
}
