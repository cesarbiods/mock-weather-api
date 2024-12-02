using Microsoft.AspNetCore.Mvc;

namespace SimpleWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger = logger;

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        WeatherForecast[] weatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

        foreach (var forecast in weatherForecast)
        {
            if (forecast.TemperatureC is > 40 or < -10)
            {
                _logger.LogWarning("Extreme temperature F: {@Forecast}", forecast.TemperatureF);
            }
            else
            {
                _logger.LogInformation("Normal temperature F: {@Forecast}", forecast.TemperatureF);
            }
        }

        return weatherForecast;
    }

    [HttpPost(Name = "CreateWeatherForecast")]
    public IActionResult Post([FromBody] WeatherForecast? forecast)
    {
        if (forecast == null)
        {
            _logger.LogError("Received null WeatherForecast in POST");
            return BadRequest("Forecast cannot be null");
        }

        if (forecast.TemperatureC is < -100 or > 100)
        {
            _logger.LogWarning("Temperature out of realistic range F: {@Forecast}", forecast.TemperatureF);
            return BadRequest("Temperature out of range");
        }

        _logger.LogInformation("Forecast successfully created F: {@Forecast}", forecast.TemperatureF);
        return Ok(forecast);
    }
}