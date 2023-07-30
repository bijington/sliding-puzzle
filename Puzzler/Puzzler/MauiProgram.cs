using Microsoft.Extensions.Logging;
using Puzzler.Pages;

namespace Puzzler;

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
                fonts.AddFont("ionicons.ttf", "IonIcons");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // Register the data layer.
        builder.Services.AddSingleton<Context>();

        // Register the pages.
        builder.Services.AddTransient<LevelPage>();
        builder.Services.AddTransient<LevelSelectionPage>();

        // Register the routes to the pages.
        Routing.RegisterRoute(nameof(LevelPage), typeof(LevelPage));
        Routing.RegisterRoute(nameof(LevelSelectionPage), typeof(LevelSelectionPage));

        return builder.Build();
	}
}

