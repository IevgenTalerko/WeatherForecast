using WeatherForecast.Services.Coordinates;

namespace WeatherForecast.Services.Weather;

public interface IWeatherGetterService
{
    Task<double?> GetWeather(CoordinatesModel coordinates, DateTime date);
}