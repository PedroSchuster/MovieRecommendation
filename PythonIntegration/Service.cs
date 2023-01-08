using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace PythonIntegration
{
    public class Service
    {
        /*private static async Task<Tuple<string, Exception>> DownloadFileAsync(string fileName, Uri fileUri,
            Constants.FileType fileType, CancellationToken ct)
        {
            if (fileUri != null)
            {
                string pathDirectory = String.Empty;
                string extension = String.Empty;
                switch (fileType)
                {
                    case Constants.FileType.BackgroundImage:
                        pathDirectory = Constants.BackgroundMovieDirectory;
                        extension = Constants.ImageFileExtension;
                        break;
                    case Constants.FileType.CoverImage:
                        pathDirectory = Constants.CoverMoviesDirectory;
                        extension = Constants.ImageFileExtension;
                        break;
                    case Constants.FileType.PosterImage:
                        pathDirectory = Constants.PosterMovieDirectory;
                        extension = Constants.ImageFileExtension;
                        break;
                    case Constants.FileType.DirectorImage:
                        pathDirectory = Constants.DirectorMovieDirectory;
                        extension = Constants.ImageFileExtension;
                        break;
                    case Constants.FileType.ActorImage:
                        pathDirectory = Constants.ActorMovieDirectory;
                        extension = Constants.ImageFileExtension;
                        break;
                    case Constants.FileType.TorrentFile:
                        pathDirectory = Constants.TorrentDirectory;
                        extension = Constants.TorrentFileExtension;
                        break;
                    default:
                        return new Tuple<string, Exception>(fileName, new Exception());
                }
                string downloadToDirectory = pathDirectory + fileName + extension;


                if (!Directory.Exists(pathDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(pathDirectory);
                    }
                    catch (Exception e)
                    {
                        return new Tuple<string, Exception>(fileName, e);
                    }
                }

                using (var webClient = new NoKeepAliveWebClient())
                {
                    ct.Register(webClient.CancelAsync);
                    if (!File.Exists(downloadToDirectory))
                    {
                        try
                        {
                            await webClient.DownloadFileTaskAsync(fileUri,
                                downloadToDirectory);

                            try
                            {
                                FileInfo fi = new FileInfo(downloadToDirectory);
                                if (fi.Length == 0)
                                {
                                    return new Tuple<string, Exception>(fileName, new Exception());
                                }
                            }
                            catch (Exception e)
                            {
                                return new Tuple<string, Exception>(fileName, e);
                            }

                        }
                        catch (WebException e)
                        {
                            return new Tuple<string, Exception>(fileName, e);
                        }
                        catch (Exception e)
                        {
                            return new Tuple<string, Exception>(fileName, e);
                        }
                    }
                    else
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(downloadToDirectory);
                            if (fi.Length == 0)
                            {
                                try
                                {
                                    File.Delete(downloadToDirectory);
                                    try
                                    {
                                        await
                                            webClient.DownloadFileTaskAsync(fileUri, downloadToDirectory);

                                        FileInfo newfi = new FileInfo(downloadToDirectory);
                                        if (newfi.Length == 0)
                                        {
                                            return new Tuple<string, Exception>(fileName, new Exception());
                                        }
                                    }
                                    catch (WebException e)
                                    {
                                        return new Tuple<string, Exception>(fileName, e);
                                    }
                                    catch (Exception e)
                                    {
                                        return new Tuple<string, Exception>(fileName, e);
                                    }
                                }
                                catch (Exception e)
                                {
                                    return new Tuple<string, Exception>(fileName, e);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            return new Tuple<string, Exception>(fileName, e);
                        }
                    }
                }

                return new Tuple<string, Exception>(fileName, null);
            }

            return new Tuple<string, Exception>(fileName, new Exception());
        }

        public async Task<Tuple<string, IEnumerable<Exception>>> DownloadMovieBackgroundImageAsync(string imdbCode,
            CancellationTokenSource cancellationToken)
        {
            List<Exception> ex = new List<Exception>();
            string backgroundImage = String.Empty;

            TMDbClient tmDbclient = new TMDbClient("7eedcbe5bf96088f5f0abe418c8bf0ea");
            await tmDbclient.GetConfigAsync();

            try
            {
                TMDbLib.Objects.Movies.Movie movie = await tmDbclient.GetMovieAsync(imdbCode, MovieMethods.Images);
                if (movie.ImdbId != null)
                {
                    Uri imageUri = tmDbclient.GetImageUrl("original",
                        movie.Images.Backdrops.Aggregate((i1, i2) => i1.VoteAverage > i2.VoteAverage ? i1 : i2).FilePath);

                    try
                    {
                        Tuple<string, Exception> res =
                            await
                                DownloadFileAsync(imdbCode, imageUri, 0,
                                    cancellationToken.Token);

                        if (res != null)
                        {
                            if (res.Item2 == null)
                            {
                                backgroundImage = Path.GetTempPath() + "MovieRecommendation\\Backgrounds\\" + imdbCode +
                                                   ".jpg";
                            }
                            else
                            {
                                ex.Add(res.Item2);
                            }
                        }
                        else
                        {
                            ex.Add(new Exception());
                        }
                    }
                    catch (WebException webException)
                    {
                        ex.Add(webException);
                    }
                    catch (TaskCanceledException e)
                    {
                        ex.Add(e);
                    }
                }
                else
                {
                    ex.Add(new Exception());
                }
            }
            catch (Exception e)
            {
                ex.Add(e);
            }

            return new Tuple<string, IEnumerable<Exception>>(backgroundImage, ex);
        }*/
    }
}
