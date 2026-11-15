using Frontend.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui; 

namespace Frontend
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() // 2. Chain this exactly here!
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configure HttpMessageHandler to bypass SSL certificate validation for local development
            var handler = new HttpsClientHandlerService();
            builder.Services.AddHttpClient<IApiService, ApiService>(client =>
            {
                // HttpClient configuration is done in ApiService constructor
            }).ConfigurePrimaryHttpMessageHandler(() => handler.GetPlatformMessageHandler());

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Register Views
            builder.Services.AddTransient<Frontend.Views.LoginPage>();
            builder.Services.AddTransient<Frontend.MainPage>();
            builder.Services.AddTransient<Frontend.Views.GameDetailsPage>();
            builder.Services.AddTransient<Frontend.Views.CreateGamePage>();
            builder.Services.AddTransient<Frontend.Views.EditGamePage>();

            // Register ViewModels
            builder.Services.AddTransient<Frontend.ViewModels.LoginViewModel>();
            builder.Services.AddTransient<Frontend.ViewModels.MainViewModel>();
            builder.Services.AddTransient<Frontend.ViewModels.GameDetailsViewModel>();
            builder.Services.AddTransient<Frontend.ViewModels.CreateGameViewModel>();
            builder.Services.AddTransient<Frontend.ViewModels.EditGameViewModel>();

            return builder.Build();
        }
    }
}