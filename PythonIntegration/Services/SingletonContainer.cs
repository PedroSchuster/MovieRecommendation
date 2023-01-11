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

        public static void Initialize()
        {
            _movieRecVM = new MovieRecommendationVM();

            _recPage = new RecommendationPage();
        }
    }
}
