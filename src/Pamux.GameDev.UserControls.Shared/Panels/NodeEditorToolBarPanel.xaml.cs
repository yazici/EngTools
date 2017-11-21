using Pamux.GameDev.UserControls.Models;
using Pamux.GameDev.UserControls.MVVM;
using Pamux.GameDev.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pamux.GameDev.UserControls.Panels
{
    /// <summary>
    /// Interaction logic for NodeEditorToolBarPanel.xaml
    /// </summary>
    public partial class NodeEditorToolBarPanel : UserControl, IPamuxView
    {

        public NodeEditorToolBarPanel()
        {
            InitializeComponent();

            VM = new NodeEditorToolBarViewModel(this);

            DataContext = VM;
        }

        public IPamuxModel M { get; set; }
        public IPamuxView V { get { return this; }  set { } }
        public IPamuxViewModel VM { get; set; }

        
    }
}
