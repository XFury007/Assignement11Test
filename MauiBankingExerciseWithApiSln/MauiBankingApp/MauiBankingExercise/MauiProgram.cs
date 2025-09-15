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

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "banking,db3");
#if DEBUG
    		builder.Logging.AddDebug();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<AccountsViewModel>();
            builder.Services.AddSingleton<AccountsPage>();
#endif

            return builder.Build();
        }
    }
}
