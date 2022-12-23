﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using Image = System.Drawing.Image;
using TMDbLib.Objects.Movies;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace PythonIntegration
{
    public class MoviesController
    {

        private string lastFilePath = String.Empty;

        private List<int> ratedMoviesId = new List<int>();

        public Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> _ratedMovies = 
            new Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>>();
        public Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> RatedMovies { get { return _ratedMovies; } }

        private Dictionary<int, Tuple<string, ICollection<string>>> _movies = new Dictionary<int, Tuple<string, ICollection<string>>>();
        public Dictionary<int, Tuple<string, ICollection<string>>> Movies { get { return _movies; } }

        private Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> _moviesRating =
                new Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>>();
        public Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> MoviesRating { get { return _moviesRating; } }


        public void LoadMoviesData(string pathMovies)
        {

            using (StreamReader sr = File.OpenText(pathMovies))
            {
                while (!sr.EndOfStream)
                {
                    string[] colunms = sr.ReadLine().Split(",");
                    int movieId = int.Parse(colunms[0]);
                    string title = colunms[1];
                    ICollection<string> genres = colunms[2].Split("|");

                    _movies.Add(movieId, new Tuple<string, ICollection<string>>(title, genres));
                }
            }
        }

        public void LoadRatingsData(string pathRatings, Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> dict)
        {
            try
            {
                using (StreamReader sr = File.OpenText(pathRatings))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] colunms = sr.ReadLine().Split(",");
                        int userId = int.Parse(colunms[0]);
                        int movieId = int.Parse(colunms[1]);
                        float rating = float.Parse(colunms[2]);

                        if (_movies.ContainsKey(movieId))
                        {
                            ICollection<string> genres = _movies[movieId].Item2;

                            if (!dict.ContainsKey(movieId))
                            {
                                dict.Add(movieId, new Tuple<Dictionary<int, float>, ICollection<string>>(new Dictionary<int, float>(), genres));
                            }
                            dict[movieId].Item1.Add(userId, rating);
                        }

                    }
                }
            }
            catch(System.IO.FileNotFoundException)
            {
                File.Create(pathRatings);
            }
        }

        public Task WriteRatingData(string path, int userId, int movieId, float rating)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(userId + "," + movieId + "," + rating);
            }

            PopulateRatedMovies(movieId);

            return Task.CompletedTask;
        }

        public async Task<Tuple<int,string,string>> GenerateMovieInfo()
        {
            TMDbClient client = new TMDbClient("7eedcbe5bf96088f5f0abe418c8bf0ea");

            Random random = new Random();

            int localMovieId = 0;

            int count = _movies.Keys.Count;

            string movieName = null;
            SearchContainer<SearchMovie> results = new SearchContainer<SearchMovie>();
            do
            {
                int randInt = random.Next(1, count);

                localMovieId = _movies.ElementAt(randInt).Key;

                if (_movies.ContainsKey(randInt))
                {
                    movieName = _movies[localMovieId].Item1.Split("(")[0];
                }
                try
                {
                    results = await client.SearchMovieAsync(movieName);
                }
                catch(TMDbLib.Objects.Exceptions.GeneralHttpException ex)
                {
                    continue;
                }
            } while (results.Results == null || results.Results.Count == 0 || results.Results.FirstOrDefault().PosterPath == null || ratedMoviesId.Contains(localMovieId));


            int movieId = results.Results.FirstOrDefault().Id;
            Movie movie = await client.GetMovieAsync(movieId);

            string tmp = Regex.Replace(movieName, "[^0-9a-zA-Z]+", "");
            string fileName = tmp.Replace(" ", "").ToLower() + ".jpg";
            string filePath = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\Resources\\Images\\" + fileName;

            if (lastFilePath != filePath)
            {
                if (lastFilePath != String.Empty)
                {
                    File.Delete(lastFilePath);
                }
                lastFilePath = filePath;
            }

            string posterPath = "https://www.themoviedb.org/t/p/original/" + movie.PosterPath;
            if (!File.Exists(filePath))
            {
                try
                {
                    Task.Run(async () =>
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            byte[] data = webClient.DownloadData(new Uri(posterPath));
                            using (MemoryStream mem = new MemoryStream(data))
                            {
                                using (var yourImage = Image.FromStream(mem))
                                {
                                    yourImage.Save(filePath, ImageFormat.Jpeg);
                                }
                            }
                        }
                    }).Wait();
                }
                catch (AggregateException ex)
                {
                    foreach (var item in ex.InnerExceptions)
                    {
                        Debug.WriteLine(item);
                    }
                }
            }



            return new Tuple<int, string, string>(localMovieId, movieName, filePath);

        }

        public void PopulateRatedMovies(int? id)
        {
            if (ratedMoviesId.Count <= 0)
            {
                ratedMoviesId = RatedMovies.Keys.ToList();
            }
            if (id != null)
            {
                ratedMoviesId.Add(id.Value);
            }
        }
    }
}
