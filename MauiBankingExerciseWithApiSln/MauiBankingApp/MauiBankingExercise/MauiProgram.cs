
using MauiBankingExercise.Services;
using Microsoft.Extensions.Logging;
using MauiBankingExercise.ViewModel;
using MauiBankingExercise.Views;

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

            // Register services for dependency injection
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddTransient<AccountsViewModel>();
            builder.Services.AddTransient<AccountsPage>();

            return builder.Build();
        }
    }
}