using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pamux.GameDev.UserControls.Controls
{
    public sealed partial class Node : UserControl, IPamuxView
    {
        public Node()
        {
            this.InitializeComponent();

            VM = new NodeBaseViewModel(this);
            DataContext = VM;
        }

        public IPamuxModel M { get; set; }
        public IPamuxView V { get { return this; } set { } }
        public IPamuxViewModel VM { get; set; }
    }
}
