using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeatherForecast.Services.Coordinates;
using WeatherForecast.Services.Weather;

namespace WeatherForecast.Services.Tomorrow;

public class TomorrowService : IWeatherGetterService
{
    private readonly TomorrowConfiguration _options;
    private readonly IMemoryCache _memoryCache;
    private readonly HttpClient _httpClient;

    public TomorrowService(IOptions<TomorrowConfiguration> options, IMemoryCache memoryCache, HttpClient httpClient)
    {
        _memoryCache = memoryCache;
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<double?> GetWeather(CoordinatesModel coordinates, DateTime date)
    {
        if (_memoryCache.TryGetValue($"{coordinates.Latitude}_{coordinates.Longitude}_{date}_tomorrow", out double? temperatureCached))
            return temperatureCached;
        
        var response = await _httpClient.GetStringAsync(string.Format(_options.Url,
            coordinates.Latitude, coordinates.Longitude));
        var root = JsonSerializer.Deserialize<WeatherDataResponse>(response);
        var temperature = root?.Timelines.Daily.FirstOrDefault(x => x.Time.Date == date.Date)?
            .Values.Temperature;

        var toReturn = temperature is null ? (double?)null : Math.Round((double)temperature, 1);
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));
        _memoryCache.Set($"{coordinates.Latitude}_{coordinates.Longitude}_{date}_tomorrow", toReturn, cacheEntryOptions);
        return toReturn;
    }
}