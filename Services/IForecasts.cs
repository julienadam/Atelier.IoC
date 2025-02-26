
namespace Atelier.IoC.Services
{
    public interface IForecasts
    {
        Task<IEnumerable<WeatherForecast>> GetForecastsAsync();
    }
}