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

namespace Pamux.GameDev.UserControls
{
    /// <summary>
    /// Interaction logic for FolderPicker.xaml
    /// </summary>
    public partial class FolderPicker : UserControl
    {
        public FolderPicker()
        {
            InitializeComponent();
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyVarIconX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(FolderPicker), new UIPropertyMetadata("Folder"));


        public string Folder
        {
            get { return (string)GetValue(FolderProperty); }
            set { SetValue(FolderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyVarIconX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FolderProperty =
            DependencyProperty.Register("Folder", typeof(string), typeof(FolderPicker), new UIPropertyMetadata(null));

    }
}
