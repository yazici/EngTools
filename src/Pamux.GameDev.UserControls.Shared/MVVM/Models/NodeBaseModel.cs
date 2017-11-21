using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pamux.GameDev.UserControls.Models
{
    public class NodeBaseModel : PamuxModelBase
    {
        public const double MinHeight = 140;
        public const double MinWidth = 200;

        public static readonly Brush DefaultFill = Brushes.LightGray;

        public NodeBaseModel(IPamuxViewModel vm = null)
            : base(vm)
        {

            Ports.Add(new NodePortModel
            {
                NodeModel = this,
                Label = "In 1",
                NodeEdge = NodeEdges.Left
            });

            Ports.Add(new NodePortModel
            {
                NodeModel = this,
                Label = "In 2",
                NodeEdge = NodeEdges.Left
            });

            Ports.Add(new NodePortModel
            {
                NodeModel = this,
                Label = "Out 1",
                NodeEdge = NodeEdges.Right
            });

            Ports.Add(new NodePortModel
            {
                NodeModel = this,
                Label = "Out 2",
                NodeEdge = NodeEdges.Right
            });
            
        }

        public Point TopLeft => new Point(Left, Top);
        public Point BottomRight => new Point(Left + Width, Top + Height);

        public double Width { get; set; } = MinWidth;
        public double Height { get; set; } = MinHeight;
        public Brush Fill = DefaultFill;
        public double Left  { get; set; }
        public double Top { get; set; }
        public Image Image;
        public string Title = "Default Title";
        public string Description = "Default Description";
        public ObservableCollection<NodePortModel> Ports { get; set; } = new ObservableCollection<NodePortModel>();
    }
}
