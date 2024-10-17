using Microsoft.Extensions.Logging;
//using static Android.Net.Http.SslCertificate;

namespace JollyCactus.Maui
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
            /*builder.Services.AddDbContext<PlantHomeDbContext>(
                options => options.UseSqlite(
                    $"Filename={GetDatabasePath()}", 
                    x => x.MigrationsAssembly(nameof(JollyCactus.PlantsHome))));
            */
            builder.Services.AddSingleton<ViewModel.JollyCactusVM>();
            builder.Services.AddTransient<Views.LocationTablePage>();
            builder.Services.AddTransient<Views.PlantTablePage>();

            builder.Services.AddSingleton<Data.IPropertySharedService, Data.PropertySharedService>();
            builder.Services.AddTransient<Views.PlantPage>();
            builder.Services.AddTransient<Views.ModifyPropertyPage>();
            

            return builder.Build();
        }

        
    }
}
