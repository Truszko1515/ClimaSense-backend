using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyrosoft.CommonInfrastructure.Options
{
    public class OpenWeatherDailyVariables
    {
        public string WeatherCode { get; set; } = string.Empty;
        public string TemperatureMax { get; set; } = string.Empty;
        public string TemperatureMin { get; set; } = string.Empty;
        public string SunshineDuration { get; set; } = string.Empty;
    }
}
