using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Services.Weather;

namespace WeatherForecast.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherForecastController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public Task<WeatherModel> Get(string city, string country, DateTime date)
    {
        return _weatherService.GetWeather(city, country, date);
    }
}