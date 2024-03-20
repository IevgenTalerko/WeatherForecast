using WeatherForecast.Services.Coordinates;

namespace WeatherForecast.Services.Weather;

public class WeatherService : IWeatherService
{
    private readonly IWeatherGetterService _meteomaticsService;
    private readonly IWeatherGetterService _tomorrowService;
    private readonly IWeatherGetterService _openMeteoService;
    private readonly ICoordinatesService _coordinatesService;

    public WeatherService(
        [FromKeyedServices(WeatherProviders.Meteomatics)]IWeatherGetterService meteomaticsService,
        [FromKeyedServices(WeatherProviders.Tomorrow)]IWeatherGetterService tomorrowService,
        [FromKeyedServices(WeatherProviders.OpenMeteo)]IWeatherGetterService openMeteoService,
        ICoordinatesService coordinatesService)
    {
        _meteomaticsService = meteomaticsService;
        _tomorrowService = tomorrowService;
        _openMeteoService = openMeteoService;
        _coordinatesService = coordinatesService;
    }

    public async Task<WeatherModel> GetWeather(string city, string country, DateTime date)
    {
        if (date.Date < DateTime.Now.Date || date.Date > DateTime.Now.AddDays(5).Date)
            throw new ArgumentException("You can set date not earlier than today and not later than 5 days after today");
        
        var coordinates = await _coordinatesService.GetCoordinates(city, country);
        if (coordinates is null)
            throw new ArgumentException("Coordinates for selected city and country was not found");
        
        var meteomaticsTemperature = await _meteomaticsService.GetWeather(coordinates, date);
        var tomorrowTemperature = await _tomorrowService.GetWeather(coordinates, date);
        var openMeteoTemperature = await _openMeteoService.GetWeather(coordinates, date);

        return new WeatherModel
        {
            MeteomaticsTemperature = meteomaticsTemperature,
            TomorrowTemperature = tomorrowTemperature,
            OpenMeteoTemperature = openMeteoTemperature
        };
    }
}