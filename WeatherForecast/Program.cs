using WeatherForecast.Services.Coordinates;
using WeatherForecast.Services.Meteomatics;
using WeatherForecast.Services.OpenMeteo;
using WeatherForecast.Services.Tomorrow;
using WeatherForecast.Services.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

RegisterServices(builder.Services);
RegisterConfigurations(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void RegisterServices(IServiceCollection services)
{
    services.AddScoped<ICoordinatesService, CoordinatesService>();
    services.AddKeyedScoped<IWeatherGetterService, MeteomaticsService>(WeatherProviders.Meteomatics);
    services.AddKeyedScoped<IWeatherGetterService, TomorrowService>(WeatherProviders.Tomorrow);
    services.AddKeyedScoped<IWeatherGetterService, OpenMeteoService>(WeatherProviders.OpenMeteo);
    services.AddScoped<IWeatherService, WeatherService>();
}

void RegisterConfigurations(IServiceCollection services)
{
    services.Configure<CoordinatesConfiguration>(builder.Configuration.GetSection("Coordinates"));
    services.Configure<MeteomaticsConfiguration>(builder.Configuration.GetSection("Meteomatics"));
    services.Configure<TomorrowConfiguration>(builder.Configuration.GetSection("Tomorrow"));
    services.Configure<OpenMeteoConfiguration>(builder.Configuration.GetSection("OpenMeteo"));
}