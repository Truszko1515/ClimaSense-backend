using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Spyrosoft.CommonInfrastructure.RequestResponses.GenericResult;
using Spyrosoft.CommonInfrastructure.RequestResponses.ResponseModels;
using Spyrosoft.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyrosoft.Core.Services
{

    public class CachedWeeklyWeatherForecastService : IWeeklyWeatherForecastService
    {
        private readonly IWeeklyWeatherForecastService _innerService;
        private readonly IMemoryCache _cache;

        public CachedWeeklyWeatherForecastService(IWeeklyWeatherForecastService innerService, IMemoryCache cache)
        {
            _innerService = innerService;
            _cache = cache;
        }

        public async Task<Result<WeeklyForecastModel>> GetForecast(decimal latitude, decimal longitude)
        {
            var cacheKey = $"{latitude}_{longitude}_weeklyForecast";

            if (_cache.TryGetValue(cacheKey, out WeeklyForecastModel cachedData))
            {
                return Result<WeeklyForecastModel>.Success(data: cachedData);
            }

            var result = await _innerService.GetForecast(latitude, longitude);

            if (result.IsSuccess)
            {
                _cache.Set(cacheKey, result.Data, TimeSpan.FromMinutes(5));
            }

            return result;
        }
    }
}
