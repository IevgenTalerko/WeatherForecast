namespace WeatherForecast.Services.Meteomatics;

public class MeteomaticsConfiguration
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string TokenUrl { get; set; }
    public string WeatherUrl { get; set; }
}