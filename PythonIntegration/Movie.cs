using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonIntegration
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public float Rating { get; set; }

        public ICollection<string> Genres { get; set; }
    }
}
