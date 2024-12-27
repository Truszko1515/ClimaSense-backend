using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Spyrosoft.CommonInfrastructure.DTOs
{
        public class OpenMeteowWeeklyDTO
        {


            public float latitude { get; set; }
            public float longitude { get; set; }
            public string timezone { get; set; } = string.Empty;
            public float generationtime_ms { get; set; }
            public Daily daily { get; set; }
        }

        

        public class Daily
        {
            public string[]? time { get; set; }
            public int[]? weather_code { get; set; }
            public float[]? temperature_2m_max { get; set; }
            public float[]? temperature_2m_min { get; set; }
            public float[]? sunshine_duration { get; set; }
        }
}
