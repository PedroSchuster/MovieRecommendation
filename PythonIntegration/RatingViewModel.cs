using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PythonIntegration
{
    public class RatingViewModel : INotifyPropertyChanged
    {
        private List<Image> starts = new List<Image>();

        public Movie Movie { get; set; }

        public ICommand SetStars { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
