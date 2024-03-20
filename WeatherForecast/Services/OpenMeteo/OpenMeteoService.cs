using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeatherForecast.Services.Coordinates;
using WeatherForecast.Services.Weather;

namespace WeatherForecast.Services.OpenMeteo;

public class OpenMeteoService : IWeatherGetterService
{
    private readonly OpenMeteoConfiguration _options;
    private readonly IMemoryCache _memoryCache;
    private readonly HttpClient _httpClient;

    public OpenMeteoService(IOptions<OpenMeteoConfiguration> options, IMemoryCache memoryCache, HttpClient httpClient)
    {
        _memoryCache = memoryCache;
        _httpClient = httpClient;
        _options = options.Value;
    }
    
    public async Task<double?> GetWeather(CoordinatesModel coordinates, DateTime date)
    {
        if (_memoryCache.TryGetValue($"{coordinates.Latitude}_{coordinates.Longitude}_{date}_openmeteo", out double? temperatureCached))
            return temperatureCached;
        
        var response = await _httpClient.GetStringAsync(string.Format(_options.Url, coordinates.Latitude, coordinates.Longitude));
        var data = JsonSerializer.Deserialize<WeatherData>(response);
        if (data is null)
            throw new ArgumentException("Data for selected city and date was not found");
        
        var index =  Array.FindIndex(data.Daily.Time.ToArray(), x => x == date.Date.ToString("yyyy-MM-dd"));
        if (index < 0 || index > data.Daily.Temperature_2m_max.Count - 1)
            throw new JsonException("Data for selected date was not found");
        
        var temperature = data.Daily.Temperature_2m_max[index];
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));
        _memoryCache.Set($"{coordinates.Latitude}_{coordinates.Longitude}_{date}_openmeteo", Math.Round(temperature, 1), cacheEntryOptions);
        return Math.Round(temperature, 1);
    }
}