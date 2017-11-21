using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.UserControls.Models
{
    public class NodePortConnectorModel : PamuxModelBase
    {
        public NodePortConnectorModel(IPamuxViewModel vm = null)
            : base(vm)
        {
            PortId = $"pc:{NextId}";
        }

        private static int nextId = 0;
        public static int NextId => ++nextId;

        public string PortId { get; set; }

        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
    }
}
