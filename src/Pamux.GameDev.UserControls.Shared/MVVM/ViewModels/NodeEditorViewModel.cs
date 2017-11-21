using Pamux.GameDev.UserControls.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Pamux.GameDev.UserControls.Panels;
using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.Models;
using System.Windows;
using System.Collections.ObjectModel;

namespace Pamux.GameDev.UserControls.ViewModels
{
    public class NodeEditorViewModel : PamuxViewModelBase
    {
        public NodeEditorViewModel(IPamuxView v)
        {
            V = v;

            var m = new NodeEditorModel(this);
            m.PropertyChanged += (s, e) => { };

            M = m;
        }

        public ObservableCollection<NodePortConnectorModel> PortConnectors
        {
            get
            {
                return ((NodeEditorModel)M).PortConnectors;
            }
            set
            {
                ((NodeEditorModel)M).PortConnectors = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<NodeBaseModel> Nodes
        {
            get
            {
                return ((NodeEditorModel)M).Nodes;
            }
            set
            {
                ((NodeEditorModel)M).Nodes = value;
                OnPropertyChanged();
            }
        }


        private string filePath;
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
                OnPropertyChanged();
            }
        }

        private string coords;
        public string Coords
        {
            get
            {
                return coords;
            }
            set
            {
                coords = value;
                OnPropertyChanged();
            }
        }

        private string nodeCoords;
        public string NodeCoords
        {
            get
            {
                return nodeCoords;
            }
            set
            {
                nodeCoords = value;
                OnPropertyChanged();
            }
        }

        private string captured;
        public string Captured
        {
            get
            {
                return captured;
            }
            set
            {
                captured = value;
                OnPropertyChanged();
            }
        }

    }
}
