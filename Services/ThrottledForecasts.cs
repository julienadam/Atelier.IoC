namespace Atelier.IoC.Services
{
    public class ThrottledForecasts<T> : IForecasts
        where T : IForecasts
    {
        private readonly T source;
        private readonly ILogger logger;
        private WeatherForecast[] currentForecasts = [];
        private DateTime? lastCall = null;

        public ThrottledForecasts(T source, ILogger<ThrottledForecasts<T>> logger)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastsAsync()
        {
            if (lastCall == null || (DateTime.Now - lastCall) > TimeSpan.FromSeconds(10))
            {
                lastCall = DateTime.Now;
                logger.LogInformation($"Updating weather data from source {source.GetType()}");
                currentForecasts = (await source.GetForecastsAsync()).ToArray();
            }
            else
            {
                logger.LogInformation($"Returning data cached at {lastCall}");
            }

            return currentForecasts;
        }
    }
}