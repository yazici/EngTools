using Pamux.GameDev.UserControls.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Pamux.GameDev.UserControls.Panels;
using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.Models;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls;


// https://libraries.io/
// Fody
namespace Pamux.GameDev.UserControls.ViewModels
{
    public class NodeBaseViewModel : PamuxViewModelBase
    {
        public NodeBaseViewModel(IPamuxView v = null)
        {
            V = v;

            var m = new NodeBaseModel(this);
            m.PropertyChanged += (s, e) => { };

            M = m;
        }

        public ObservableCollection<NodePortModel> Ports => ((NodeBaseModel)M).Ports;

        public IEnumerable<NodePortModel> LeftEdgePorts => Ports.Where(node => node.NodeEdge == NodeEdges.Left);
        public IEnumerable<NodePortModel> RightEdgePorts => Ports.Where(node => node.NodeEdge == NodeEdges.Right);

        public int PortCount => Ports.Count;
        public int LeftEdgePortCount => LeftEdgePorts.Count();
        public int RightEdgePortCount => RightEdgePorts.Count();

        public int MaxVerticalPortCount => Math.Max(LeftEdgePortCount, RightEdgePortCount);

        public const int PortVerticalPadding = 20;
        public const int PortHorizontalPadding = 20;

        public const int DefaultPortHeight = 20;
        public const int DefaultPortWidth = 80;

        public int MinHeightDueToPorts => MaxVerticalPortCount * DefaultPortHeight + 2 * PortVerticalPadding;


        public string Title
        {
            get
            {
                return ((NodeBaseModel) M).Title;
            }
            set
            {
                ((NodeBaseModel)M).Title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                return ((NodeBaseModel)M).Description;
            }
            set
            {
                ((NodeBaseModel)M).Description = value;
                OnPropertyChanged();
            }
        }


        public Image Image
        {
            get
            {
                return ((NodeBaseModel)M).Image;
            }
            set
            {
                ((NodeBaseModel)M).Image = value;
                OnPropertyChanged();
            }
        }


        public double Left
        {
            get
            {
                return ((NodeBaseModel)M).Left;
            }
            set
            {
                ((NodeBaseModel)M).Left = value;
                OnPropertyChanged();
            }
        }

        public double Top
        {
            get
            {
                return ((NodeBaseModel)M).Top;
            }
            set
            {
                ((NodeBaseModel)M).Top = value;
                OnPropertyChanged();
            }
        }

        public double Width
        {
            get
            {
                return ((NodeBaseModel)M).Width;
            }
            set
            {
                ((NodeBaseModel)M).Width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get
            {
                return Math.Max(MinHeightDueToPorts, ((NodeBaseModel)M).Height);
            }
            set
            {
                ((NodeBaseModel)M).Height = value;
                OnPropertyChanged();
            }
        }

        

        public Brush Fill
        {
            get
            {
                return ((NodeBaseModel)M).Fill;
            }
            set
            {
                ((NodeBaseModel)M).Fill = value;
                OnPropertyChanged();
            }
        }
    }
}
