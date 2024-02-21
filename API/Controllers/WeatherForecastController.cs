using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController] //contains the endpoints that are HTTPS requests, returns HTTP response
[ApiExplorerSettings(IgnoreApi = true)] // Block it from being viewed on Swagger UI
[Route("[controller]")] // GET https://localhost:5001/WeatherForecast //[square brackets] imply placeholder
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" //summaries of weather
    };

    private readonly ILogger<WeatherForecastController> _logger; //assiging logger to private readonly

    public WeatherForecastController(ILogger<WeatherForecastController> logger) 
    {
        _logger = logger;   
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray(); //data passed out to an array and sent back to the clients
    }
}
