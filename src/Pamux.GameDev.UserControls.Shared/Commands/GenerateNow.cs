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
    public class GenerateNow : CommandBase
    {
        public override void Execute(IPamuxModel m, IPamuxView v, IPamuxViewModel vm)
        {
            var CVM = vm as GeneratorConfigViewModel;
            var CM = m as GeneratorConfigModel;
            var CV = v as GeneratorConfigPanel;
            
            var V = ViewModelLocator.FindSibling<Generators, GeneratorResultsPanel>(CV);

            var VM = V.VM as GeneratorResultsViewModel;
            var M = VM.M as GeneratorResultsModel;

            var context = new BitmapManipulationContext();
            //context.FillRandomly(32,75);
            //context.FillRandomGrid(10, 10, 10, 10);
            context.PerlinNoise();
            VM.ImageSource = context.Create();
        }
    }
}
