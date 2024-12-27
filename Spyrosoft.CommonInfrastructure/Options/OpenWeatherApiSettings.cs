using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyrosoft.CommonInfrastructure.Options
{
    public class OpenWeatherApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string StartingParam { get; set; } = string.Empty;
        public string ForecastDays { get; set; } = string.Empty;
        public string TimezoneParam { get; set; } = string.Empty;
    }

    
}
