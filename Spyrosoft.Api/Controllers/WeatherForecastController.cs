using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Spyrosoft.Api.RequestModels;
using Spyrosoft.Core.Interfaces;


namespace Spyrosoft.Api.Controllers
{
    [ApiController]
    [EnableCors("LocalHostPolicy")]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeeklyWeatherForecastService _weeklyWeatherForecastService;
        private readonly IWeeklyForecastSummaryService _weeklyForecastSummaryService;
        public WeatherForecastController(IWeeklyWeatherForecastService weeklyWeatherForecastService,
                                         IWeeklyForecastSummaryService weeklyForecastSummaryService)
        {
            _weeklyWeatherForecastService = weeklyWeatherForecastService;
            _weeklyForecastSummaryService = weeklyForecastSummaryService;
        }

        [HttpGet("weekly-forecast")]
        public async Task<IActionResult> GetWeeklyForecast([FromQuery] WeatherForecastRequest request)
        {
            await Console.Out.WriteLineAsync($"Validation eror: lon - {request.Longitude} lat - {request.Latitude}");
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                
            }

            var result = await _weeklyWeatherForecastService.GetForecast(request.Latitude, request.Longitude);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet("weekly-summary")]
        public async Task<IActionResult> GetWeeklySummary([FromQuery] WeatherForecastRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _weeklyForecastSummaryService.GetForecast(request.Latitude, request.Longitude);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
