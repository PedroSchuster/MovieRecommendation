using PythonIntegration.Services;
using PythonIntegration.ViewModels;

namespace PythonIntegration;

public partial class RecommendationPage : ContentPage
{
	public RecommendationPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
        BindingContext = SingletonContainer.MovieRecVM;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}