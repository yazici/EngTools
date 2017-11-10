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

namespace Pamux.GameDev.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for Generators.xaml
    /// </summary>
    public partial class Generators : UserControl
    {
        public Generators()
        {
            InitializeComponent();
        }

        public void CanGenerateNow(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = true;
        }



        public static RoutedCommand GenerateNowCommand = new RoutedCommand();
        public void GenerateNow(object sender, ExecutedRoutedEventArgs e)
        {
        }
    }
}
