using CommunityToolkit.Maui;
using JollyCactus.Maui.Settings;
using Microsoft.Extensions.Logging;

namespace JollyCactus.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().UseMauiCommunityToolkit();
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
            /*builder.Services.AddDbContext<PlantHomeDbContext>(
                options => options.UseSqlite(
                    $"Filename={GetDatabasePath()}", 
                    x => x.MigrationsAssembly(nameof(JollyCactus.PlantsHome))));
            */
            builder.Services.AddSingleton<ViewModel.JollyCactusVM>();
            builder.Services.AddSingleton<JCSettings>();

            builder.Services.AddSingleton<Views.LocationTablePage>();
            builder.Services.AddSingleton<Views.PlantTablePage>();
            builder.Services.AddSingleton<Views.SettingsPage>();

            builder.Services.AddTransient<Views.PlantPage>();
            builder.Services.AddTransient<Views.ModifyPropertyPage>();

            return builder.Build();
        }

        
    }
}
