using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PythonIntegration.Services;

namespace PythonIntegration.ViewModels
{
    public class MovieRecommendationVM : INotifyPropertyChanged
    {

        private bool scriptExecuted;

        private bool _isRunning;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }

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

        public ICommand GenerateMovie { get; set; }

        public MovieRecommendationVM()
        {


            GenerateMovie = new Command(async () =>
            {
                await GenerateMovieRecommendationAsync();

            });

        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private async Task GenerateMovieRecommendationAsync()
        {
            Tuple<int, string, string> movieInfo = new Tuple<int, string, string>(0, null, null);

            if (!scriptExecuted)
            {
                IsRunning = true;

                Task p = Task.Run(()=> MoviesController.RunPythonCode());
                await p;

                IsRunning = false;
                scriptExecuted = true;

                if (!p.IsCompletedSuccessfully)
                {
                    return;
                }

            }

            IsRunning = true;

            movieInfo = await Task.Run(MoviesController.GenerateMovieRecommendation);

            IsRunning = false;

            if (movieInfo == null)
            {
                scriptExecuted = false;
                return;
            }

            MovieId = movieInfo.Item1;
            MovieTitle = movieInfo.Item2;
            MovieImage = ImageSource.FromFile(movieInfo.Item3);
            MovieIsVisible = true;

        }

    }
}
