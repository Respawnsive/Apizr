using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Refit;

namespace Apizr.Tests.Apis
{
    [BaseAddress("https://localhost:7138")]
    public interface IApizrTestsApi
    {
        [Get("/weatherforecast")]
        Task<IApiResponse<IReadOnlyList<WeatherForecast>>> GetWeatherForecastAsync([RequestOptions] IApizrRequestOptions options);

        [Get("/weatherforecast")]
        Task<IApiResponse<IReadOnlyList<WeatherForecast>>> GetWeatherForecastAsync([Query] string action, [RequestOptions] IApizrRequestOptions options);
    }

    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
