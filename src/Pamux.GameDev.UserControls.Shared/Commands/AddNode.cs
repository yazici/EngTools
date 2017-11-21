using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Pamux.GameDev.UserControls.ViewModels;
using Pamux.GameDev.UserControls.MVVM;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Pamux.GameDev.UserControls.Models;
using Pamux.GameDev.UserControls.Panels;
using Pamux.GameDev.UserControls.Tabs;
using System.Windows;

namespace Pamux.GameDev.UserControls.Commands
{
    public class AddNode : CommandBase
    {
        public override void Execute(IPamuxModel m, IPamuxView v, IPamuxViewModel vm)
        {
            var CVM = vm as NodeEditorToolBarViewModel;
            var CM = m as NodeEditorToolBarModel;
            var CV = v as NodeEditorToolBarPanel;
            
            var V = ViewModelLocator.FindSibling<NodeEditor, NodeEditorPanel>(CV);
            V.AddNode("Xyz");
            var VM = V.VM as NodeEditorViewModel;
            var M = VM.M as NodeEditorModel;

            var context = new BitmapManipulationContext();
            //context.FillRandomly(32,75);
            //context.FillRandomGrid(10, 10, 10, 10);
            //context.PerlinNoise();
            //VM.ImageSource = context.Create();
        }
    }
}
