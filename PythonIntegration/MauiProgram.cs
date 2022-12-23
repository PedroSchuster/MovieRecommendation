using Python.Runtime;

namespace PythonIntegration;

public static class MauiProgram
{
    public static MoviesController moviesController = new MoviesController();

	public static MauiApp CreateMauiApp()
	{
        Initialize();
        
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

    public static void Initialize()
    {
        string pathMovies = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\movies.csv";
        string pathRating = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\ratings.csv";
        string pathUserRating = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\userrating.csv";
        moviesController.LoadMoviesData(pathMovies);
        moviesController.LoadRatingsData(pathRating, moviesController.MoviesRating);
        moviesController.LoadRatingsData(pathUserRating, moviesController.RatedMovies);

        string pythonDll = @"C:\Users\Usuario\AppData\Local\Programs\Python\Python38\python38.dll";
        Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);
        PythonEngine.Initialize();
    }

    public static object RunPythonCode(string path, object parameter, string parameterName, string returnedVarName)
    {
        object returnedVariable = new object();

        Initialize();

        dynamic result = null;
        using (Py.GIL())
        {
            dynamic os = Py.Import("os");
            dynamic sys = Py.Import("sys");
            sys.path.append(os.getcwd());

            dynamic test = Py.Import("sistema_recomendacao");
            result = test.func("Leonardo");
        }

        return result;
    }
}
