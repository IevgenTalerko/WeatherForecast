using System.Text.Json.Serialization;

namespace WeatherForecast.Services.Tomorrow;

public class Timelines
{
    [JsonPropertyName("daily")]
    public List<DailyWeatherData> Daily { get; set; }
}

public class DailyWeatherData
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }
    [JsonPropertyName("values")]
    public HourlyWeatherValues Values { get; set; }
}

public class HourlyWeatherValues
{
    [JsonPropertyName("temperatureAvg")]
    public double Temperature { get; set; }
    
}

public class WeatherDataResponse
{
    [JsonPropertyName("timelines")]
    public Timelines Timelines { get; set; }
    [JsonPropertyName("location")]
    public Location Location { get; set; }
}

public class Location
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }
    [JsonPropertyName("lon")]
    public double Lon { get; set; }
}