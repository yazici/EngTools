using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Pamux.GameDev.UserControls.MVVM
{
    public class PamuxViewModelBase : IPamuxViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public IPamuxModel M { get; set; }
        public IPamuxView V { get; set; }
        public IPamuxViewModel VM { get { return this; } set { } }
    }
}
