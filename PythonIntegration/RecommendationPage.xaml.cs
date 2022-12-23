namespace PythonIntegration;

public partial class RecommendationPage : ContentPage
{
	public RecommendationPage()
	{
		InitializeComponent();
		BindingContext = new MovieRecommendationVM();
	}
}