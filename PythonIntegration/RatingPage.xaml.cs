using System.Net;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Drawing;
using System.Globalization;
using TMDbLib.Objects.Movies;
using System.Drawing.Imaging;


namespace PythonIntegration;

public partial class RatingPage : ContentPage
{
	public RatingPage()
	{
		InitializeComponent();
        BindingContext = new MovieRatingVM();
	}
   
}