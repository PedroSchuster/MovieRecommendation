using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PythonIntegration.ViewModels;
namespace PythonIntegration.Services
{
    public static class SingletonContainer
    {

        private static MovieRecommendationVM _movieRecVM;
        public static MovieRecommendationVM MovieRecVM { get { return _movieRecVM; } }

        private static RecommendationPage _recPage;
        public static RecommendationPage RecPage { get { return _recPage; } }

        private static MovieRatingVM _ratingVM;
        public static MovieRatingVM RatingVM { get { return _ratingVM; } }

        private static RatingPage _ratingPage;
        public static RatingPage RatingPage { get { return _ratingPage; } }

        public static void Initialize()
        {
            _movieRecVM = new MovieRecommendationVM();

            _recPage = new RecommendationPage();

            _ratingVM = new MovieRatingVM();

            _ratingPage = new RatingPage();
        }
    }
}
