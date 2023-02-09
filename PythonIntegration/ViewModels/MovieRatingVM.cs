using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PythonIntegration.Services;

namespace PythonIntegration.ViewModels
{
    public class MovieRatingVM : INotifyPropertyChanged
    {
        private bool _isRunning = true;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
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

        public ICommand NextMovie { get; set; }

        public ICommand NeverWatch { get; set; }

        public MovieRatingVM()
        {
            GenerateMovieInfo();

            RatingCommand = new Command((n) =>
            {
                Rating = float.Parse(n.ToString());
            });

            NextMovie = new Command(async () =>
            {
                await MoviesController.
                    WriteRatingData(MovieId, Rating);
                await GenerateMovieInfo();
                Rating = 0;
            });

            NeverWatch = new Command(async () =>
            {
                await GenerateMovieInfo();
                Rating = 0;
            });

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName == nameof(Rating))
            {
                ChangeStarImage(int.Parse(Rating.ToString()));
            }
        }

        private async Task GenerateMovieInfo()
        {
            Tuple<int, string, string> movieInfo = new Tuple<int, string, string>(0, null, null);

            IsRunning = true;

            movieInfo = await MoviesController.GenerateRandomMovie();

            MovieId = movieInfo.Item1;
            MovieTitle = movieInfo.Item2;
            MovieImage = ImageSource.FromFile(movieInfo.Item3);

            IsRunning = false;
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
