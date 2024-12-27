using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyrosoft.CommonInfrastructure.RequestResponses.ResponseModels
{
    public class WeeklyForecastModel
    {
        public DailyForecast[] Data { get; set; }
    }

    public class DailyForecast
    {
        public string Time { get; set; }
        public int WeatherCode { get; set; }
        
        // Temperature unit is Celcius 
        public float TemperatureMin { get; set; }
        public float TemperatureMax { get; set; }
        public float EstEneryGenereatedkWh { get; set; }
    }
}
