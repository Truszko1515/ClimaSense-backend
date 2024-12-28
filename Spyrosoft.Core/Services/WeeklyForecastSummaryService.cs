using Microsoft.Extensions.Caching.Memory;
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
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Spyrosoft.Core.Services
{
    public class WeeklyForecastSummaryService : IWeeklyForecastSummaryService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherApiSettings _reqParams;
        private readonly OpenWeatherDailyVariables _dailyParams;
        private readonly OpenWeatherHourlyVariables _hourlyParams;
        private readonly ILogger<WeeklyWeatherForecastService> _logger;
        private readonly IMemoryCache _cache;
      
        public WeeklyForecastSummaryService(
                    HttpClient httpClient,
                    IOptions<OpenWeatherApiSettings> reqParams,
                    IOptions<OpenWeatherDailyVariables> dailyParams,
                    ILogger<WeeklyWeatherForecastService> logger,
                    IOptions<OpenWeatherHourlyVariables> hourlyParams,
                    IMemoryCache cache)
        {
            _httpClient = httpClient;
            _reqParams = reqParams.Value;
            _dailyParams = dailyParams.Value;
            _hourlyParams = hourlyParams.Value;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Result<WeeklyForecastSummaryModel>> GetForecast(decimal latitude, decimal longitude)
        {
            try
            {
                var cacheKey = $"{latitude}_{longitude}_weeklyForecast";


                // if part of data already exist in Cache -> not to duplicate request to external api
                if (_cache.TryGetValue(cacheKey, out WeeklyForecastModel cachedForecast))
                {
                    var result = await GetWeeklySummaryCached(cachedForecast, latitude, longitude);
                    return result;
                }

                var response = await _httpClient.GetAsync(
                    $"{_reqParams.StartingParam}?latitude={latitude}&longitude={longitude}&hourly={_hourlyParams.PressureSurface},{_hourlyParams.PressureSeaLevel}&daily={_dailyParams.SunshineDuration},{_dailyParams.TemperatureMax},{_dailyParams.TemperatureMin},{_dailyParams.WeatherCode}&{_reqParams.ForecastDays}&{_reqParams.TimezoneParam}");

                var stringyfiedData = await response.Content.ReadAsStringAsync();
                var openMeteoData = JsonSerializer.Deserialize<WeeklySummaryDTO>(stringyfiedData);

                WeeklyForecastSummaryModel model = WeatherForecastHelper.PrepareWeeklySummaryModelFull(openMeteoData);

                return Result<WeeklyForecastSummaryModel>.Success(data: model);
            }
            catch (Exception ex)
            {
                // Not exposing to client what and where went wrong
                _logger.LogError(ex, $"Exception occured: {ex.Message}");
                return Result<WeeklyForecastSummaryModel>.Failure("Failed to fetch weather data.");
            }
        }

        private async Task<Result<WeeklyForecastSummaryModel>> GetWeeklySummaryCached(WeeklyForecastModel cachedForecast, decimal latitude, decimal longitude)
        {
            WeeklyForecastSummaryModel model = new WeeklyForecastSummaryModel();
            model.MaxExpectedTemperature = cachedForecast.Data.Max(d => d.TemperatureMax);
            model.MinExpectedTemperature = cachedForecast.Data.Min(d => d.TemperatureMin);
            
            var weatherCodes = cachedForecast.Data.Select(d => d.WeatherCode).ToArray();
            model.AvgWeatherCode = WeatherForecastHelper.CalculateEstWeatherCode(weatherCodes);

            var response = await _httpClient.GetAsync(
            $"{_reqParams.StartingParam}?latitude={latitude}&longitude={longitude}&hourly={_hourlyParams.PressureSurface},{_hourlyParams.PressureSeaLevel}&daily={_dailyParams.SunshineDuration}&{_reqParams.ForecastDays}&{_reqParams.TimezoneParam}");

            if (!response.IsSuccessStatusCode)
            {
                return Result<WeeklyForecastSummaryModel>.Failure("Failed to fetch weather data.");
            }

            var stringyfiedData = await response.Content.ReadAsStringAsync();
            var openMeteoData = JsonSerializer.Deserialize<WeeklySummaryDTO>(stringyfiedData);

            var modelResult = WeatherForecastHelper.PrepareWeeklySummaryModelPart(openMeteoData, model);

            return Result<WeeklyForecastSummaryModel>.Success(data: modelResult);
        }
    } 
}
