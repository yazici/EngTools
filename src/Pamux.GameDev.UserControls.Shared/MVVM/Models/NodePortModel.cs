using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.UserControls.Models
{
    public class NodePortModel : PamuxModelBase
    {
        public NodePortModel(IPamuxViewModel vm = null)
            : base(vm)
        {
            PortId = $"port:{NextId}";
            Label = "Default Label";
            NodeEdge = NodeEdges.Left;
        }

        private static int nextId = 0;
        public static int NextId => ++nextId;

        public string PortId { get; set; }

        public string Label { get; set; }

        public NodeEdges NodeEdge { get; set; }

        public NodeBaseModel NodeModel;
    }
}
