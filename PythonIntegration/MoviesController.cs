using System;
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
using Python.Runtime;
<<<<<<< HEAD
using Fasterflect;
=======
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655

namespace PythonIntegration
{
    public class MoviesController
    {

        private string lastFilePath = String.Empty;

<<<<<<< HEAD
        public Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> _ratedMovies =
=======
        public Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> _ratedMovies = 
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
            new Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>>();
        public Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> RatedMovies { get { return _ratedMovies; } }

        private Dictionary<int, Tuple<string, ICollection<string>>> _movies = new Dictionary<int, Tuple<string, ICollection<string>>>();
        public Dictionary<int, Tuple<string, ICollection<string>>> Movies { get { return _movies; } }

        private Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> _moviesRating =
                new Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>>();
        public Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> MovieRatings { get { return _moviesRating; } }


        public void LoadMoviesData(string pathMovies)
        {

            using (StreamReader sr = File.OpenText(pathMovies))
            {
                while (!sr.EndOfStream)
                {
<<<<<<< HEAD

                    if (sr.ReadLine() == "id,title,genres")
                        continue;


                    string[] colunms = sr.ReadLine()?.Split(",");
                    if (colunms != null)
                    {
                        int movieId = int.Parse(colunms[0]);
                        string title = colunms[1].Split("(")[0];
                        ICollection<string> genres = colunms[2].Split("|");

                        _movies.Add(movieId, new Tuple<string, ICollection<string>>(title, genres));
                    }



=======
                    string[] colunms = sr.ReadLine().Split(",");
                    int movieId = int.Parse(colunms[0]);
                    string title = colunms[1];
                    ICollection<string> genres = colunms[2].Split("|");

                    _movies.Add(movieId, new Tuple<string, ICollection<string>>(title, genres));
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
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
<<<<<<< HEAD
                        if (sr.ReadLine() == "id,rating")
                            continue;


                        string[] colunms = sr.ReadLine()?.Split(",");
                        if (colunms != null)
                        {
                            int userId = 0;
                            int movieId = int.Parse(colunms[0]);
                            float rating = float.Parse(colunms[1]);

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
            }
            catch (System.IO.FileNotFoundException)
=======
                        string[] colunms = sr.ReadLine().Split(",");
                        int userId = 0;
                        int movieId = int.Parse(colunms[0]);
                        float rating = float.Parse(colunms[1]);

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
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
            {
                File.Create(pathRatings);
            }
        }

        public Task WriteRatingData(string path, int movieId, float rating)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(movieId + "," + rating);
            }

            ICollection<string> genres = _movies[movieId].Item2;
            RatedMovies.Add(movieId, new Tuple<Dictionary<int, float>, ICollection<string>>(new Dictionary<int, float>(), genres));

            return Task.CompletedTask;
        }

<<<<<<< HEAD
        private async Task<Tuple<int, string, string>> GenerateMovieInfo(int id)
        {
            TMDbClient client = new TMDbClient("7eedcbe5bf96088f5f0abe418c8bf0ea");

            string movieName = null;
            SearchContainer<SearchMovie> results = new SearchContainer<SearchMovie>();

            if (_movies.ContainsKey(id))
            {
                movieName = _movies[id].Item1;
            }
            try
            {
                results = await client.SearchMovieAsync(movieName);
            }
            catch (TMDbLib.Objects.Exceptions.GeneralHttpException ex)
            {
                return null;
            }

            if (results.Results == null || results.Results.Count == 0 || results.Results.FirstOrDefault().PosterPath == null || RatedMovies.ContainsKey(id))
                return null;
=======
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
            } while (results.Results == null || results.Results.Count == 0 || results.Results.FirstOrDefault().PosterPath == null || RatedMovies.ContainsKey(localMovieId));

>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655

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



<<<<<<< HEAD
            return new Tuple<int, string, string>(id, movieName, filePath);

        }

        public async Task<Tuple<int, string, string>> GenerateRandomMovie()
        {
            Tuple<int, string, string> result;
            int movieId = 0;

            do
            {
                Random random = new Random();

                int count = _movies.Keys.Count;

                int randInt = random.Next(1, count);

                movieId = _movies.ElementAt(randInt).Key;

                result = await GenerateMovieInfo(movieId);

            } while (result == null);
           

            return result;
        }

        public async Task<Tuple<int, string, string>> GenerateMovieRecommendation()
        {
            var resultFromPy = RunPythonCode();
            var movie = resultFromPy.GetElement(0);

            Tuple<int, string, string> result;
            int index = 0;

            do
            {
                var movieId = Convert.ToInt32(Convert.ToDecimal(movie.GetElement(index).ToString().Replace(".0", "")));
                result = await GenerateMovieInfo(movieId);
                index += 1;

            } while (result == null);

            return result;
=======
            return new Tuple<int, string, string>(localMovieId, movieName, filePath);

        }

        public void GenerateMovieRecommendation()
        {
            RunPythonCode();
            var b = 2;
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
        }

        public void Initialize()
        {
            string pathMovies = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\movies2.csv";
            //string pathRating = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\ratings2.csv";
            string pathUserRating = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\userrating.csv";
            LoadMoviesData(pathMovies);
            LoadRatingsData(pathUserRating, RatedMovies);

        }

<<<<<<< HEAD
        public object RunPythonCode()
        {
            object returnedVariable = new object();
=======
        public void RunPythonCode()
        {
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655

            try
            {
                string scriptFile = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\sistema_recomendacao.py";
                string pythonCode = "";
                using (var streamReader = new StreamReader(scriptFile, Encoding.UTF8))
                {
                    pythonCode = streamReader.ReadToEnd();
                }
                PythonEngine.Initialize();

<<<<<<< HEAD
=======
                object returnedVariable = new object();
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655

                using (Py.GIL())
                {
                    var scope = Py.CreateScope();
                    dynamic a = scope.Exec(pythonCode);
                    returnedVariable = scope.Get<object>("result");
                }
<<<<<<< HEAD

            }
            catch (Exception ex)
=======
            }
            catch (Exception ex) 
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
            {
                Console.Write(ex.Message);
            }

<<<<<<< HEAD
            return returnedVariable;

=======
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
        }
    }
}
