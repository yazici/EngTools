using Pamux.GameDev.UserControls.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Pamux.GameDev.UserControls.Panels;
using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.Models;
using System.Windows;

namespace Pamux.GameDev.UserControls.ViewModels
{
    public class NodePortConnectorViewModel : PamuxViewModelBase
    {
        public NodePortConnectorViewModel(IPamuxView v)
        {
            V = v;
            M = new NodePortConnectorModel(this);
        }

        private Visibility visibility;
        public Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                OnPropertyChanged();
            }
        }


        private double x1;
        public double X1
        {
            get
            {
                return x1;
            }
            set
            {
                x1 = value;
                OnPropertyChanged();
            }
        }

        private double y1;
        public double Y1
        {
            get
            {
                return y1;
            }
            set
            {
                y1 = value;
                OnPropertyChanged();
            }
        }

        private double x2;
        public double X2
        {
            get
            {
                return x2;
            }
            set
            {
                x2 = value;
                OnPropertyChanged();
            }
        }

        private double y2;
        public double Y2
        {
            get
            {
                return y2;
            }
            set
            {
                y2 = value;
                OnPropertyChanged();
            }
        }

    }
}
