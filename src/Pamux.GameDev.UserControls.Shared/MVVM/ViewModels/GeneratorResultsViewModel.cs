using Pamux.GameDev.UserControls.Models;
using Pamux.GameDev.UserControls.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pamux.GameDev.UserControls.ViewModels
{
    public class GeneratorResultsViewModel : PamuxViewModelBase
    {
        public GeneratorResultsViewModel(IPamuxView v)
        {
            // Model or list of models should be a member of viewmodel
            /// ObservableCollection<Song> _songs = new ObservableCollection<Song>();

            V = v;

            var m = new GeneratorResultsModel(this);
            m.PropertyChanged += (s, e) => { };

            M = m;
        }

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                imageSource = value;
                OnPropertyChanged();
            }
        }
    }
}
