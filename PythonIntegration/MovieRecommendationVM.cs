using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PythonIntegration
{
    public class MovieRecommendationVM : INotifyPropertyChanged
    {

        private bool _movieIsVisible;
        public bool MovieIsVisible
        {
            get { return _movieIsVisible; }
            set
            {
                _movieIsVisible = value;
                OnPropertyChanged(nameof(MovieIsVisible));
            }
        }

        private bool _ratingIsVisible;
        public bool RatingIsVisible
        {
            get { return _ratingIsVisible; }
            set
            {
                _ratingIsVisible = value;
                OnPropertyChanged(nameof(RatingIsVisible));
            }
        }

        private string _source1 = "star.png";
        public string Source1
        {
            get { return _source1; }
            set
            {
                _source1 = value;
                OnPropertyChanged(nameof(Source1));
            }
        }

        private string _source2 = "star.png";
        public string Source2
        {
            get { return _source2; }
            set
            {
                _source2 = value;
                OnPropertyChanged(nameof(Source2));
            }
        }

        private string _source3 = "star.png";
        public string Source3
        {
            get { return _source3; }
            set
            {
                _source3 = value;
                OnPropertyChanged(nameof(Source3));
            }
        }

        private string _source4 = "star.png";
        public string Source4
        {
            get { return _source4; }
            set
            {
                _source4 = value;
                OnPropertyChanged(nameof(Source4));
            }
        }

        private string _source5 = "star.png";
        public string Source5
        {
            get { return _source5; }
            set
            {
                _source5 = value;
                OnPropertyChanged(nameof(Source5));
            }
        }

        private float _rating;
        public float Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        private int _movieId;
        public int MovieId
        {
            get { return _movieId; }
            set
            {
                _movieId = value;
                OnPropertyChanged(nameof(MovieId));
            }
        }

        private string _movieTitle;
        public string MovieTitle
        {
            get { return _movieTitle; }
            set
            {
                _movieTitle = value;
                OnPropertyChanged(nameof(MovieTitle));
            }
        }

        private ImageSource _movieImage;
        public ImageSource MovieImage
        {
            get { return _movieImage; }
            set
            {
                _movieImage = value;
                OnPropertyChanged(nameof(MovieImage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RatingCommand { get; set; }

        public ICommand RatingEnable { get; set; }

        public ICommand GenerateMovie { get; set; }

        public MovieRecommendationVM()
        {
            GenerateMovieRecommendation();

            RatingCommand = new Command((object n) =>
            {
                Rating = float.Parse(n.ToString());
            });

            GenerateMovie = new Command(async () =>
            {
                if (RatingIsVisible)
                {
                    await MauiProgram.moviesController.
                        WriteRatingData("C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\userrating.csv", MovieId, Rating);
                }
                GenerateMovieRecommendation();
                Rating = 0;
                RatingIsVisible = false;
            });

            RatingEnable = new Command(() =>
            {
                if (MovieIsVisible)
                {
                    RatingIsVisible = true;
                }
            });
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(Rating) && RatingIsVisible)
            {
                ChangeStarImage(int.Parse(Rating.ToString()));
            }
        }

        private void GenerateMovieRecommendation()
        {
            MauiProgram.moviesController.GenerateMovieRecommendation();
        }

        private void ChangeStarImage(int n)
        {
            switch (n)
            {
                case 0:
                    Source1 = "star.png";
                    Source2 = "star.png";
                    Source3 = "star.png";
                    Source4 = "star.png";
                    Source5 = "star.png";
                    break;
                case 1:
                    if (Source1 == "star.png")
                    {
                        Source1 = "star2.png";
                    }
                    else if (Source1 == "star2.png" && Source2 == "star2.png")
                    {
                        Source2 = "star.png";
                        Source3 = "star.png";
                        Source4 = "star.png";
                        Source5 = "star.png";
                    }
                    else
                    {
                        Rating = 0;

                    }
                    break;
                case 2:
                    if (Source2 == "star.png")
                    {
                        Source1 = "star2.png";
                        Source2 = "star2.png";
                    }
                    else
                    {
                        Source3 = "star.png";
                        Source4 = "star.png";
                        Source5 = "star.png";
                    }
                    break;
                case 3:
                    if (Source3 == "star.png")
                    {
                        Source1 = "star2.png";
                        Source2 = "star2.png";
                        Source3 = "star2.png";
                    }
                    else
                    {
                        Source4 = "star.png";
                        Source5 = "star.png";
                    }
                    break;
                case 4:
                    if (Source4 == "star.png")
                    {
                        Source1 = "star2.png";
                        Source2 = "star2.png";
                        Source3 = "star2.png";
                        Source4 = "star2.png";
                    }
                    else
                    {
                        Source5 = "star.png";
                    }
                    break;
                case 5:
                    Source1 = "star2.png";
                    Source2 = "star2.png";
                    Source3 = "star2.png";
                    Source4 = "star2.png";
                    Source5 = "star2.png";
                    break;

            }
        }
    }
}
