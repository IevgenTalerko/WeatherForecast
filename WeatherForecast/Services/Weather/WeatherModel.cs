namespace WeatherForecast.Services.Weather;

public class WeatherModel
{
    public double? MeteomaticsTemperature { get; set; }
    public double? OpenMeteoTemperature { get; set; }
    public double? TomorrowTemperature { get; set; }
}