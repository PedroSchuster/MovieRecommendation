
using PythonIntegration.Scripts;
using PythonIntegration.Services;

namespace PythonIntegration;

public static class MauiProgram
{

	public static MauiApp CreateMauiApp()
	{
        MoviesController.Initialize();
		SingletonContainer.Initialize();

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        return builder.Build();
	}

   
}
