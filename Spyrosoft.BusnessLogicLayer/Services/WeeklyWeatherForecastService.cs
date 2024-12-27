using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spyrosoft.CommonInfrastructure.DTOs;
using Spyrosoft.CommonInfrastructure.Helpers;
using Spyrosoft.CommonInfrastructure.Options;
using Spyrosoft.CommonInfrastructure.RequestResponses.GenericResult;
using Spyrosoft.CommonInfrastructure.RequestResponses.ResponseModels;
using Spyrosoft.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Spyrosoft.Core.Services
{
    public class WeeklyWeatherForecastService : IWeeklyWeatherForecastService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherApiSettings _reqParams;
        private readonly OpenWeatherDailyVariables _dailyParams;
        private readonly PvPanelsSettings _panelsSpecification;
        private readonly ILogger<WeeklyWeatherForecastService> _logger;
        public WeeklyWeatherForecastService(
                    HttpClient httpClient, 
                    IOptions<OpenWeatherApiSettings> reqParams,
                    IOptions<OpenWeatherDailyVariables> dailyParams,
                    ILogger<WeeklyWeatherForecastService> logger,
                    IOptions<PvPanelsSettings> panelsSpecification)
        {
            _httpClient = httpClient;
            _reqParams = reqParams.Value;
            _dailyParams = dailyParams.Value;
            _panelsSpecification = panelsSpecification.Value;
            _logger = logger;
        }

        public async Task<Result<WeeklyForecastModel>> GetForecast(decimal latitude, decimal longitude)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                $"{_reqParams.StartingParam}?latitude={latitude}&longitude={longitude}&daily={_dailyParams.WeatherCode},{_dailyParams.TemperatureMin},{_dailyParams.TemperatureMax},{_dailyParams.SunshineDuration}&{_reqParams.ForecastDays}&{_reqParams.TimezoneParam}");

                if (!response.IsSuccessStatusCode)
                {
                    return Result<WeeklyForecastModel>.Failure("Failed to fetch weather data.");
                }

                var stringyfiedData = await response.Content.ReadAsStringAsync();
                var openMeteoData = JsonSerializer.Deserialize<OpenMeteowWeeklyDTO>(stringyfiedData);

                // using Helper to prepare response model
                var ResultData = WeatherForecastHelper.PrepareWeeklyForecastModel(openMeteoData,
                                                                                  _panelsSpecification.PanelsCapacity,
                                                                                  _panelsSpecification.PanelsEfficiency);

                return Result<WeeklyForecastModel>.Success(data: ResultData);
                
            }
            catch (Exception ex)
            {
                // Not exposing to client what and where went wrong
                _logger.LogError(ex, $"Exception occured: {ex.Message}");
                return Result<WeeklyForecastModel>.Failure("Failed to fetch weather data.");
            }
        }
    } 
}
