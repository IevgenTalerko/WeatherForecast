using System.Text.Json.Serialization;

namespace WeatherForecast.Services.OpenMeteo;

public class DailyData
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; }
    [JsonPropertyName("temperature_2m_max")]
    public List<double> Temperature_2m_max { get; set; }
}

public class WeatherData
{
    [JsonPropertyName("daily")]
    public DailyData Daily { get; set; }
}