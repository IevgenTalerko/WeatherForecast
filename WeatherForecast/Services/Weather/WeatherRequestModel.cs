namespace WeatherForecast.Services.Weather;

public class WeatherRequestModel
{
    public string City { get; set; }
    public string Country { get; set; }
    public DateTime Date { get; set; }
}