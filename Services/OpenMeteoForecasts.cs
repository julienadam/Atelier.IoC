
namespace Atelier.IoC.Services
{
    public class OpenMeteoForecasts : IForecasts
    {
        private readonly HttpClient client;
        private readonly string url = "forecast?latitude=47.62776555681033&longitude=-2.7778276602474627&daily=weather_code,temperature_2m_max&timezone=Europe%2FBerlin&models=meteofrance_seamless";

        public OpenMeteoForecasts()
        {
            this.client = new HttpClient() { BaseAddress = new Uri("https://api.open-meteo.com/v1/") };
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastsAsync()
        {
            var response = await client.GetFromJsonAsync<OpenMeteoResponse>(url);

            if(response == null)
            {
                return Enumerable.Empty<WeatherForecast>();
            }

            return response.daily.time
                .Zip(response.daily.temperature_2m_max, response.daily.weather_code)
                .Select(d =>
                {
                    var (time, temp, code) = d;
                    return new WeatherForecast
                    {
                        Date = DateOnly.ParseExact(time, "yyyy-MM-dd"),
                        TemperatureC = (int)(temp ?? 0),
                        Summary = CodeToSummary(code)
                    };
                });
        }

        private class OpenMeteoResponse
        {
            public float latitude { get; set; }
            public float longitude { get; set; }
            public float generationtime_ms { get; set; }
            public int utc_offset_seconds { get; set; }
            public string timezone { get; set; }
            public string timezone_abbreviation { get; set; }
            public float elevation { get; set; }
            public Daily_Units daily_units { get; set; }
            public Daily daily { get; set; }
        }

        private class Daily_Units
        {
            public string time { get; set; }
            public string weather_code { get; set; }
            public string temperature_2m_max { get; set; }
        }

        private class Daily
        {
            public string[] time { get; set; }
            public int?[] weather_code { get; set; }
            public float?[] temperature_2m_max { get; set; }
        }

        private static string CodeToSummary(int? code)
        {
            return code switch
            {
                0 => "Sunny",
                1 => "Mainly Sunny",
                2 => "Partly Cloudy",
                3 => "Cloudy",
                45 => "Foggy",
                48 => "Rime Fog",
                51 => "Light Drizzle",
                53 => "Drizzle",
                55 => "Heavy Drizzle",
                56 => "Light Freezing Drizzle",
                57 => "Freezing Drizzle",
                61 => "Light Rain",
                63 => "Rain",
                65 => "Heavy Rain",
                66 => "Light Freezing Rain",
                67 => "Freezing Rain",
                71 => "Light Snow",
                73 => "Snow",
                75 => "Heavy Snow",
                77 => "Snow Grains",
                80 => "Light Showers",
                81 => "Showers",
                82 => "Heavy Showers",
                85 => "Light Snow Showers",
                86 => "Snow Showers",
                95 => "Thunderstorm",
                96 => "Light Thunderstorms With Hail",
                99 => "Thunderstorm With Hail",
                _ => "Unknown",
            };
        }
    }
}