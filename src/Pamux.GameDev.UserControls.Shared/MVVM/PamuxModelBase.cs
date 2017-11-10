using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pamux.GameDev.UserControls.MVVM
{

    public abstract class PamuxModelBase : IPamuxModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private IPamuxViewModel vm;

        public PamuxModelBase(IPamuxViewModel vm)
        {
            this.vm = vm;
        }
        

        public IPamuxModel M { get { return  this; } set { } }
        public IPamuxView V { get { return vm.V; } set { } }
        public IPamuxViewModel VM { get { return vm; } set { vm = value; } }
    }
}
