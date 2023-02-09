using PythonIntegration.Services;

namespace PythonIntegration;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        Window window = base.CreateWindow(activationState);

        window.Destroying += (s, e) =>
        {
            MoviesController.RemoveImageCache();
        };

        return window;
    }


}
