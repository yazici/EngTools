using Pamux.GameDev.UserControls.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace Pamux.GameDev.UserControls.Models
{
    public class GeneratorResultsModel : PamuxModelBase
    {
        public GeneratorResultsModel(IPamuxViewModel vm)
            : base(vm)
        {
        }

        public BitmapImage ImageSource;
    }
}
