namespace PythonIntegration;

public partial class RatingPage : ContentPage
{
	public RatingPage()
	{
		InitializeComponent();
	}
    private void star1_Clicked(object sender, EventArgs e)
    {
        if (star1.Source.ToString() == "File: star.png")
        {
            star1.Source = "star2.png";
        }
        else
        {
            star2.Source = "star.png";
            star3.Source = "star.png";
            star4.Source = "star.png";
            star5.Source = "star.png";
        }
    }

    private void star2_Clicked(object sender, EventArgs e)
    {

        if (star2.Source.ToString() == "File: star.png")
        {
            star1.Source = "star2.png";
            star2.Source = "star2.png";
        }
        else
        {
            star3.Source = "star.png";
            star4.Source = "star.png";
            star5.Source = "star.png";

        }

    }

    private void star3_Clicked(object sender, EventArgs e)
    {
        if (star3.Source.ToString() == "File: star.png")
        {
            star1.Source = "star2.png";
            star2.Source = "star2.png";
            star3.Source = "star2.png";
        }
        else
        {
            star4.Source = "star.png";
            star5.Source = "star.png";
        }


    }

    private void star4_Clicked(object sender, EventArgs e)
    {
        if (star4.Source.ToString() == "File: star.png")
        {
            star1.Source = "star2.png";
            star2.Source = "star2.png";
            star3.Source = "star2.png";
            star4.Source = "star2.png";
        }
        else
        {
            star5.Source = "star.png";
        }

    }

    private void star5_Clicked(object sender, EventArgs e)
    {
        star1.Source = "star2.png";
        star2.Source = "star2.png";
        star3.Source = "star2.png";
        star4.Source = "star2.png";
        star5.Source = "star2.png";

    }

    private void nvr_watch_Clicked(object sender, EventArgs e)
    {

    }
}