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
using Fasterflect;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace PythonIntegration.Services
{
    public static class MoviesController
    {
        private static string dir;
        private static string lastFilePath = string.Empty;
        private static List<object> pyResult;

        public static Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> _ratedMovies =
            new Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>>();
        public static Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> RatedMovies { get { return _ratedMovies; } }

        private static Dictionary<int, Tuple<string, ICollection<string>>> _movies = new Dictionary<int, Tuple<string, ICollection<string>>>();
        public static Dictionary<int, Tuple<string, ICollection<string>>> Movies { get { return _movies; } }

        private static Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> _moviesRating =
                new Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>>();
        public static Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> MovieRatings { get { return _moviesRating; } }


        private static void LoadMoviesData(string pathMovies)
        {

            using (StreamReader sr = File.OpenText(pathMovies))
            {
                while (!sr.EndOfStream)
                {
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

                }
            }
        }

        private static void LoadRatingsData(string pathRatings, Dictionary<int, Tuple<Dictionary<int, float>, ICollection<string>>> dict)
        {
            try
            {
                using (StreamReader sr = File.OpenText(pathRatings))
                {
                    while (!sr.EndOfStream)
                    {
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
            catch (FileNotFoundException)
            {
                File.Create(pathRatings);
            }
        }

        public static Task WriteRatingData(int movieId, float rating)
        {
            string path = dir + "\\Data\\userrating.csv";
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(movieId + "," + rating);
                }

                ICollection<string> genres = _movies[movieId].Item2;
                RatedMovies.Add(movieId, new Tuple<Dictionary<int, float>, ICollection<string>>(new Dictionary<int, float>(), genres));
            }
            catch(Exception e)
            {
                return null;
            }
            return Task.CompletedTask;
        }

        public static void RemoveImageCache()
        {
            string path = dir + "\\Resources\\Images";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (var item in files)
            {
                try
                {
                    item.Delete();
                }
                catch
                {
                    continue;
                }
            }
        } 

        private static async Task<Tuple<int, string, string>> GenerateMovieInfo(int id)
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

            int movieId = results.Results.FirstOrDefault().Id;
            Movie movie = await client.GetMovieAsync(movieId);

            string tmp = Regex.Replace(movieName, "[^0-9a-zA-Z]+", "");
            string fileName = tmp.Replace(" ", "").ToLower() + ".jpg";
            string filePath = dir + "Resources\\Images\\" + fileName;

            if (lastFilePath != filePath)
            {
                if (lastFilePath != string.Empty)
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



            return new Tuple<int, string, string>(id, movieName, filePath);

        }

        public static async Task<Tuple<int, string, string>> GenerateRandomMovie()
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

        public static async Task<Tuple<int, string, string>> GenerateMovieRecommendation()
        {

            Tuple<int, string, string> result = null;

            if (pyResult != null && pyResult.Count > 0)
            {
                do
                {
                    var movie = pyResult[0];

                    var movieId = Convert.ToInt32(Convert.ToDecimal(movie.GetElement(0).ToString().Replace(".0", "")));
                    result = await GenerateMovieInfo(movieId);
                    pyResult.RemoveAt(0);

                } while (result == null);
            }

            return result;
        }

        public static void Initialize()
        {
            int trashIndex = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).IndexOf("bin");
            dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Remove(trashIndex);

            string pathMovies = dir + "Data\\movies2.csv";
            //string pathRating = "C:\\Users\\Usuario\\Desktop\\Programacao\\Aulas\\Python\\PythonIntegration\\PythonIntegration\\ratings2.csv";
            string pathUserRating = dir + "Data\\userrating.csv";
            LoadMoviesData(pathMovies);
            LoadRatingsData(pathUserRating, _ratedMovies);


        }

        public static void RunPythonCode()
        {
            string dllPath = dir + "bin\\python38.dll";

            Environment.SetEnvironmentVariable("PATH", dir, EnvironmentVariableTarget.Process);

            Environment.SetEnvironmentVariable("PYTHONHOME", dir, EnvironmentVariableTarget.Process);

            Environment.SetEnvironmentVariable("PYTHONPATH", dir, EnvironmentVariableTarget.Process);

            if (Runtime.PythonDLL == null)
            {
                Runtime.PythonDLL = dllPath;
            }

            try
            {
                string scriptFile = dir + "Scripts\\sistema_recomendacao.py";
                string pythonCode = "";
                using (var streamReader = new StreamReader(scriptFile, Encoding.UTF8))
                {
                    pythonCode = streamReader.ReadToEnd();
                }
                PythonEngine.Initialize();


                using (Py.GIL())
                {
                    var scope = Py.CreateScope();
                    dynamic a = scope.Exec(pythonCode);
                    var iListPyResult = (IList<object>)scope.Get<object>("result");
                    pyResult = iListPyResult.ToList();
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }
    }
}
