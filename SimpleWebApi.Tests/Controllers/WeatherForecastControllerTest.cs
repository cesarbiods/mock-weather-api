using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SimpleWebApi.Tests.Controllers;

public class WeatherForecastTests
{
    private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

    [Fact]
    public async Task SimulateWeatherForecastTraffic()
    {
        for (var i = 0; i < 50; i++)
        {
            HttpResponseMessage response = await _client.GetAsync("/WeatherForecast");
            Assert.True(response != null, "API call failed");
        }
    }
}