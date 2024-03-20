using System.Text.Json.Serialization;

namespace WeatherForecast.Services.Meteomatics;

public class Datum
{
    [JsonPropertyName("value")]
    public double Value { get; set; }
}

public class Coordinate
{
    [JsonPropertyName("dates")]
    public List<Datum> Dates { get; set; }
}

public class Data
{
    [JsonPropertyName("coordinates")]
    public List<Coordinate> Coordinates { get; set; }
}

public class RootObject
{
    [JsonPropertyName("data")]
    public List<Data> Data { get; set; }
}