using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace WeatherForecast.Services.Coordinates;

public interface ICoordinatesService
{
    Task<CoordinatesModel?> GetCoordinates(string city, string country);
}

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string? Token { get; set; }
}

public class CoordinatesService : ICoordinatesService
{
    private readonly CoordinatesConfiguration _options;
    private readonly IMemoryCache _memoryCache;
    private readonly HttpClient _httpClient;

    public CoordinatesService(IOptions<CoordinatesConfiguration> options, IMemoryCache memoryCache, HttpClient httpClient)
    {
        _memoryCache = memoryCache;
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<CoordinatesModel?> GetCoordinates(string city, string country)
    {
        if (_memoryCache.TryGetValue($"{city}_{country}", out CoordinatesModel? coordinatesModelCached))
            return coordinatesModelCached;
        
        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _options.Password);
        var response = await _httpClient.GetStringAsync(string.Format(_options.Url, city, country));
        var coordinatesResponse = JsonSerializer.Deserialize<List<CoordinatesResponse>>(response);

        var coordinatesModel = coordinatesResponse?.Select(x => new CoordinatesModel
        {
            Latitude = x.Latitude,
            Longitude = x.Longitude
        }).FirstOrDefault();

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromDays(100));
        _memoryCache.Set($"{city}_{country}", coordinatesModel, cacheEntryOptions);

        return coordinatesModel;
    }
}