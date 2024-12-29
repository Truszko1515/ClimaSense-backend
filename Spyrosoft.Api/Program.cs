
using Microsoft.Extensions.Options;
using Spyrosoft.Api.Middleware;
using Spyrosoft.CommonInfrastructure.Options;
using Spyrosoft.Core.Interfaces;
using Spyrosoft.Core.Services;
using System.Text;

namespace Spyrosoft.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
 
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            // IOptions pattern
            builder.Services.Configure<PvPanelsSettings>(builder.Configuration.GetSection(nameof(PvPanelsSettings)));

            builder.Services.Configure<OpenWeatherApiSettings>(builder.Configuration.GetSection(nameof(OpenWeatherApiSettings)));

            builder.Services.Configure<OpenWeatherDailyVariables>(builder.Configuration.GetSection(nameof(OpenWeatherApiSettings))
                                                                                       .GetSection(nameof(OpenWeatherDailyVariables)));

            builder.Services.Configure<OpenWeatherHourlyVariables>(builder.Configuration.GetSection(nameof(OpenWeatherApiSettings))
                                                                                       .GetSection(nameof(OpenWeatherHourlyVariables)));
            ///////
            Console.WriteLine("Zarejestrowane serwisy IOptions<> i teraz tworznie httpClientow dla serwisow");

            // Registering HttpClients that are injected to services 
            builder.Services.AddHttpClient<IWeeklyWeatherForecastService, WeeklyWeatherForecastService>((ServiceProvider, httpClient) =>
            {
                var settings = ServiceProvider.GetRequiredService<IOptions<OpenWeatherApiSettings>>().Value;
                Console.WriteLine($"IOptions<OpenWeatherApiSettings> settings.BaseUrl = {settings.BaseUrl}");
                httpClient.BaseAddress = new Uri($"{settings.BaseUrl}");
            });
            builder.Services.AddHttpClient<IWeeklyForecastSummaryService, WeeklyForecastSummaryService>((ServiceProvider, httpClient) =>
            {
                var settings = ServiceProvider.GetRequiredService<IOptions<OpenWeatherApiSettings>>().Value;

                httpClient.BaseAddress = new Uri($"{settings.BaseUrl}");
            });

            Console.WriteLine("Dzialajacye Httpclienty dla serwisow");
            // Used Scrutor libray
            builder.Services.Decorate<IWeeklyWeatherForecastService, CachedWeeklyWeatherForecastService>();

            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("LocalHostPolicy",
                    policy =>
                    {
                        policy
                              .SetIsOriginAllowedToAllowWildcardSubdomains()
                              .WithOrigins("https://localhost:4200", "http://localhost:4200", "http://localhost:3000")
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowAnyHeader();
                    });
            });
            Console.WriteLine("dzialajacy cors, builder.Build()");
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("LocalHostPolicy");

            app.UseMiddleware<ExceptionHandlingMiddleware>();


            app.MapControllers();

            app.Run();
        }
    }
}
