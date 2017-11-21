using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.UserControls.Models
{
    public class NodeEditorModel : PamuxModelBase
    {
        public NodeEditorModel(IPamuxViewModel vm)
            : base(vm)
        {

            var x = new NodeBaseViewModel
            {
                Title = "Some Node 1"
            };


            Nodes.Add(x.M as NodeBaseModel);

            //x = new NodeBaseViewModel
            //{
            //    Title = "Some Node 2"
            //};

            //Nodes.Add(x.M as NodeBaseModel);

        }

        public ObservableCollection<NodePortConnectorModel> PortConnectors { get; set; } = new ObservableCollection<NodePortConnectorModel>();

        public ObservableCollection<NodeBaseModel> Nodes { get; set; } = new ObservableCollection<NodeBaseModel>();
    }
}
