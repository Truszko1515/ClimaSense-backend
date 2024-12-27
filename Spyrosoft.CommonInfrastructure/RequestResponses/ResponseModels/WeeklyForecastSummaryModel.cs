namespace Spyrosoft.CommonInfrastructure.RequestResponses.ResponseModels
{
    public class WeeklyForecastSummaryModel
    {
        public float? AvgPressureSurface { get; set; }
        public float? AvgPressureSeaLevel { get; set; }
        public float? AvgSunshineDuration { get; set; }
        public float? MaxExpectedTemperature { get; set; }
        public float? MinExpectedTemperature { get; set; }
        public int? AvgWeatherCode {get; set; }
    }
}
