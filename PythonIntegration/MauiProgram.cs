using Python.Runtime;

namespace PythonIntegration;

public static class MauiProgram
{
    public static MoviesController moviesController = new MoviesController();

	public static MauiApp CreateMauiApp()
	{
        moviesController.Initialize();
        
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
