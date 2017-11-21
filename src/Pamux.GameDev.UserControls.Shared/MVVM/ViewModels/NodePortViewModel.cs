using Pamux.GameDev.UserControls.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Pamux.GameDev.UserControls.Panels;
using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.Models;

namespace Pamux.GameDev.UserControls.ViewModels
{
    public class NodePortViewModel : PamuxViewModelBase
    {
        public NodePortViewModel(IPamuxView v)
        {
            V = v;
            M = new NodePortModel(this);
        }

        public string Label
        {
            get
            {
                return ((NodePortModel)M).Label;
            }
            set
            {
                ((NodePortModel)M).Label = value;
                OnPropertyChanged();
            }
        }

        public NodeEdges NodeEdge
        {
            get
            {
                return ((NodePortModel)M).NodeEdge;
            }
            set
            {
                ((NodePortModel)M).NodeEdge = value;
                OnPropertyChanged();
            }
        }
    }
}
