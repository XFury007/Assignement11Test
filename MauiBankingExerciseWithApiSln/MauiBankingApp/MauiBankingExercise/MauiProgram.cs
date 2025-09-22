using MauiBankingExercise.Services;
using Microsoft.Extensions.Logging;
using MauiBankingExercise.ViewModel;
using MauiBankingExercise.Views;
using Microsoft.Maui.Devices;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace MauiBankingExercise
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Determine base URL per platform
            string baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7258/" : "https://localhost:7258/";

            // Register ApiService with a configured HttpClient (development-friendly cert handling)
            builder.Services.AddSingleton<IApiService>(sp =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(baseUrl),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                return new ApiService(client);
            });

            builder.Services.AddTransient<AccountsViewModel>();
            builder.Services.AddTransient<AccountsPage>();

            return builder.Build();
        }
    }
}