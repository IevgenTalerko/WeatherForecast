using System.Text.Json.Serialization;

namespace WeatherForecast.Services.Coordinates;

public class CoordinatesResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("latitude")]
    public float Latitude { get; set; }
    
    [JsonPropertyName("longitude")]
    public float Longitude { get; set; }
    
    [JsonPropertyName("country")]
    public string Country { get; set; }
}