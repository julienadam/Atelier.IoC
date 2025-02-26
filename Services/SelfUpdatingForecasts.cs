
namespace Atelier.IoC.Services
{
    public class SelfUpdatingForecasts : IForecasts
    {
        private static readonly string[] Summaries =
            [
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            ];

        private WeatherForecast[] currentForecasts = [];

        public SelfUpdatingForecasts()
        {
            UpdateForecasts();
            Timer t = new(_ =>
            {
                UpdateForecasts();
            }, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        public Task<IEnumerable<WeatherForecast>> GetForecastsAsync()
        {
            return Task.FromResult<IEnumerable<WeatherForecast>>(currentForecasts);
        }

        private void UpdateForecasts()
        {
            this.currentForecasts = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                .ToArray();
        }
    }
}