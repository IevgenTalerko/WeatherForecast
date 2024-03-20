namespace WeatherForecast.Services.Weather;

public interface IWeatherService
{
    Task<WeatherModel> GetWeather(string city, string country, DateTime date);
}