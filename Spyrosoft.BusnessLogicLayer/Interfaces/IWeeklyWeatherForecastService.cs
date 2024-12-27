using Spyrosoft.CommonInfrastructure.DTOs;
using Spyrosoft.CommonInfrastructure.RequestResponses.GenericResult;
using Spyrosoft.CommonInfrastructure.RequestResponses.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyrosoft.Core.Interfaces
{
    public interface IWeeklyWeatherForecastService
    {
        Task<Result<WeeklyForecastModel>> GetForecast(decimal latitude, decimal longitude);
    }
}
