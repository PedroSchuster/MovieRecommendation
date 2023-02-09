
namespace PythonIntegration;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();

    }

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	private void navRat_Clicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new RatingPage());
	}

	private void navRec_Clicked(object sender, EventArgs e)
	{
        Navigation.PushAsync(new RecommendationPage());

    }

}

