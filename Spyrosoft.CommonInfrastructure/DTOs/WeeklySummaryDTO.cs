using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyrosoft.CommonInfrastructure.DTOs
{

    public class WeeklySummaryDTO
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public float generationtime_ms { get; set; }
        public string timezone { get; set; }
        public Hourly? hourly { get; set; }
        public Daily? daily { get; set; }
    }

    public class Hourly
    {
        public string[]? time { get; set; }
        public int[]? weather_code { get; set; }
        public float[]? pressure_msl { get; set; }
        public float[]? surface_pressure { get; set; }
        public int[]? sunshine_duration { get; set; }
    }
}
