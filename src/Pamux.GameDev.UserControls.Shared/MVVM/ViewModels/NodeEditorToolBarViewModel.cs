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
    public class NodeEditorToolBarViewModel : PamuxViewModelBase
    {
        public NodeEditorToolBarViewModel(IPamuxView v)
        {
            V = v;
            M = new NodeEditorToolBarModel(this);
        }

        public ICommand AddNodeCommand { get; } = new AddNode();
    }
}
