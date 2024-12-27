using Spyrosoft.CommonInfrastructure.DTOs;
using Spyrosoft.CommonInfrastructure.RequestResponses.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spyrosoft.CommonInfrastructure.Helpers
{
    public static class WeatherForecastHelper
    {
        public static WeeklyForecastModel PrepareWeeklyForecastModel(OpenMeteowWeeklyDTO OpenMeteoData, float PanelsCapacity, float PanelsEfficiency)
        {
            var dailyData = new List<DailyForecast>();

            for (int i = 0; i < OpenMeteoData.daily.time.Length; i++)
            {

                dailyData.Add(new DailyForecast
                {
                    Time = OpenMeteoData.daily.time[i],
                    WeatherCode = OpenMeteoData.daily.weather_code[i],
                    TemperatureMax = OpenMeteoData.daily.temperature_2m_max[i],
                    TemperatureMin = OpenMeteoData.daily.temperature_2m_min[i],
                    EstEneryGenereatedkWh = EstimateEnergyGenerated(OpenMeteoData.daily.sunshine_duration[i],
                                                                    PanelsCapacity,
                                                                    PanelsEfficiency)
                });
            }

            return new WeeklyForecastModel
            {
                Data = dailyData.ToArray()
            };
        }

        // Part suffix means this method prepares only part of response model
        public static WeeklyForecastSummaryModel PrepareWeeklySummaryModelPart(WeeklySummaryDTO OpenMeteoData, WeeklyForecastSummaryModel model)
        {
            model.AvgPressureSeaLevel = OpenMeteoData.hourly.pressure_msl.Average();
            model.AvgPressureSurface = OpenMeteoData.hourly.surface_pressure.Average();
            model.AvgSunshineDuration = OpenMeteoData.daily.sunshine_duration.Average();

            return model;
        }

        // Full suffix means this method prepares complete response model
        public static WeeklyForecastSummaryModel PrepareWeeklySummaryModelFull(WeeklySummaryDTO OpenMeteoData)
        {
            WeeklyForecastSummaryModel model = new();

            model.AvgPressureSeaLevel = OpenMeteoData.hourly.pressure_msl.Average();
            model.AvgPressureSurface = OpenMeteoData.hourly.surface_pressure.Average();
            model.AvgSunshineDuration = OpenMeteoData.daily.sunshine_duration.Average();
            model.MaxExpectedTemperature = OpenMeteoData.daily.temperature_2m_max.Max();
            model.MinExpectedTemperature = OpenMeteoData.daily.temperature_2m_min.Max();

            var WeatherCodes = OpenMeteoData.daily.weather_code.Select(d => d).ToArray();
            model.AvgWeatherCode = CalculateEstWeatherCode(WeatherCodes);

            return model;
        }
        public static int CalculateEstWeatherCode(int[] weatherCodes)
        {
            if (weatherCodes == null || weatherCodes.Length == 0)
            {
                throw new ArgumentException("Weather codes array cannot be null or empty");
            }

            // grouping codes, then finding the most recurring one
            return weatherCodes
                .GroupBy(code => code)
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key) // in case of two groups with same count -> take the lower weather code
                .First().Key;
        }
        private static float EstimateEnergyGenerated(float SunlightDuration, float PanelsCapacity, float PanelsEfficiency)
        {
            return SunlightDuration * PanelsCapacity * PanelsEfficiency;
        }
    }
}
