using System.Net;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Drawing;
using System.Globalization;
using TMDbLib.Objects.Movies;
using System.Drawing.Imaging;
using PythonIntegration.ViewModels;
using PythonIntegration.Services;

namespace PythonIntegration;

public partial class RatingPage : ContentPage
{
	public RatingPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = SingletonContainer.RatingVM;
    }

    protected override void OnDisappearing()
	{
		base.OnDisappearing();
    }

}