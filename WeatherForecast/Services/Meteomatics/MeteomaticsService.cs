using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeatherForecast.Services.Coordinates;
using WeatherForecast.Services.Weather;

namespace WeatherForecast.Services.Meteomatics;

public class MeteomaticsService : IWeatherGetterService
{
    private readonly MeteomaticsConfiguration _options;
    private readonly IMemoryCache _memoryCache;
    private readonly HttpClient _httpClient;

    public MeteomaticsService(IOptions<MeteomaticsConfiguration> options, IMemoryCache memoryCache, HttpClient httpClient)
    {
        _memoryCache = memoryCache;
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<double?> GetWeather(CoordinatesModel coordinates, DateTime date)
    {
        if (_memoryCache.TryGetValue($"{coordinates.Latitude}_{coordinates.Longitude}_{date}_meteomatics", out double? temperatureCached))
            return temperatureCached;
        
        var token = await GetToken();

        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await _httpClient.GetStringAsync(string.Format(_options.WeatherUrl,
            date.ToString("s") + "Z", coordinates.Latitude, coordinates.Longitude));

        var root = JsonSerializer.Deserialize<RootObject>(response);
        var temperature = root?.Data.SingleOrDefault()?
                .Coordinates.SingleOrDefault()?
                .Dates.SingleOrDefault()?
                .Value;
        
        var temperatureToReturn = temperature is null ? (double?)null : Math.Round((double)temperature, 1);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));
        _memoryCache.Set($"{coordinates.Latitude}_{coordinates.Longitude}_{date}_meteomatics", temperatureToReturn, cacheEntryOptions);

        return temperatureToReturn;
    }

    private async Task<string?> GetToken()
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{_options.Login}:{_options.Password}");
        var encodedAuthParams =  Convert.ToBase64String(plainTextBytes);
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedAuthParams}");
        var tokenResponse = await _httpClient.GetStringAsync(_options.TokenUrl);
        return JsonSerializer.Deserialize<TokenResponse>(tokenResponse)?.Token;
    }
}