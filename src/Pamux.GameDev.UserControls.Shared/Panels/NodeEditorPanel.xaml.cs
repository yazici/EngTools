using Pamux.GameDev.UserControls.Controls;
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
    /// Interaction logic for NodeEditorPanel.xaml
    /// </summary>
    public partial class NodeEditorPanel : UserControl, IPamuxView
    {
        public NodeEditorPanel()
        {
            InitializeComponent();

            VM = new NodeEditorViewModel(this);

            DataContext = VM;
        }

        private Node capturedNode = null;

        private Point actionStartNodeTopLeft;
        private Point actionStartNodeBottomRight;
        
        private Point actionStartAbsolutePos;

        private Point currentAbsolutePosition;
        private Point currentCapturedNodePosition;

        private ActionTypes currentAction = ActionTypes.None;
        public enum ActionTypes
        {
            None,
            Resize,
            Drag,
            Port
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutRoot.Children.Clear();

            var node = AddNode("ABC");
            node = AddNode("DEF");

        }

        public Node AddNode(string title, int left = 10, int top = 10)
        {
            var node = new Node();

            ((NodeBaseViewModel)node.VM).Title = title;

            node.MouseMove += OnMouseMove;
            node.MouseLeftButtonDown += OnMouseLeftButtonDown;
            node.MouseLeftButtonUp += OnMouseLeftButtonUp;



            LayoutRoot.Children.Add(node);

            Canvas.SetLeft(node, left);
            Canvas.SetTop(node, top);
            return node;
        }

        private bool HitTest(out DependencyObject visualHit, out ActionTypes action, out NodeBaseModel model, out Node view, out NodeBaseViewModel viewModel)
        {
            model = null;
            view = null;
            viewModel = null;
            action = ActionTypes.None;

            var result = VisualTreeHelper.HitTest(LayoutRoot, Mouse.GetPosition(LayoutRoot));
            visualHit = result?.VisualHit;
            if (visualHit == null)
            {
                return false;
            }

            var shape = (visualHit as Polygon) as Shape;
            if (shape == null)
            {
                shape = (visualHit as Rectangle) as Shape;
                if (shape == null)
                {
                    shape = (visualHit as Ellipse) as Shape;
                }
            }
            
            viewModel = (shape?.DataContext) as NodeBaseViewModel;
            if (viewModel == null)
            {
                return false;
            }

            view = viewModel.V as Node;
            if (view == null)
            {
                return false;
            }

            model = viewModel.M as NodeBaseModel;
            if (model == null)
            {
                return false;
            }

            if (shape.Name == null)
            {
                return false;
            }

            switch (shape.Name)
            {
                case "ResizeCorner":
                    action = ActionTypes.Resize;
                    return true;
                case "NodePort":
                    action = ActionTypes.Port;
                    return true;
            }

            action = ActionTypes.Drag;
            return true;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (sender is Canvas)
            //{
            //    return;
            //}
            currentAbsolutePosition = e.GetPosition(LayoutRoot);

            //actionStartNodeBottomRight.Y - actionStartNodeTopLeft.Y + actionPosDelta.Y



            if (HitTest(out DependencyObject visualHit, out currentAction, out NodeBaseModel model, out Node view, out NodeBaseViewModel viewModel))
            {
                actionStartAbsolutePos = currentAbsolutePosition;

                //actionStartNodeTopLeft = model.TopLeft;
                actionStartNodeBottomRight = model.BottomRight;

                capturedNode = view;

                actionStartNodeTopLeft.X = Canvas.GetLeft(capturedNode);
                actionStartNodeTopLeft.Y = Canvas.GetTop(capturedNode);
                actionStartNodeBottomRight.X = actionStartNodeTopLeft.X + model.Width;
                actionStartNodeBottomRight.Y = actionStartNodeTopLeft.Y + model.Height;
                //actionStartNodeTopLeft = currentCapturedNodePosition;



                if (currentAction == ActionTypes.Drag)
                {
                    Mouse.Capture(capturedNode);
                }

            }
        }

        private void HandleDrag(Point actionPosDelta)
        {
            currentCapturedNodePosition = new Point(actionStartNodeTopLeft.X + actionPosDelta.X, actionStartNodeTopLeft.Y + actionPosDelta.Y);

            ((NodeEditorViewModel)VM).NodeCoords = $" Drag {currentCapturedNodePosition.X:N3}:{currentCapturedNodePosition.Y:N3}"; // Yellow


            
            //((NodeBaseViewModel)capturedNode.VM).Left = currentCapturedNodePosition.X;
            //((NodeBaseViewModel)capturedNode.VM).Top = currentCapturedNodePosition.Y;

            //((NodeBaseViewModel)capturedNode.VM).Left = (int)(actionStartNodeBottomRight.X - actionStartNodeTopLeft.X + actionPosDelta.X);
            //((NodeBaseViewModel)capturedNode.VM).Top = (int)(actionStartNodeBottomRight.Y - actionStartNodeTopLeft.Y + actionPosDelta.Y);


            Canvas.SetLeft(capturedNode, currentCapturedNodePosition.X);
            Canvas.SetTop(capturedNode, currentCapturedNodePosition.Y);
        }


        private void HandleFreeMove(object sender, MouseEventArgs e)
        {
            //((NodeEditorViewModel)VM).NodeCoords = $"{currentAbsolutePosition.X:N3}:{currentAbsolutePosition.Y:N3}"; // Yellow
        }

        private void HandleResize(Point actionPosDelta)
        {
            ((NodeBaseViewModel)capturedNode.VM).Width = (int)(actionStartNodeBottomRight.X - actionStartNodeTopLeft.X + actionPosDelta.X);
            ((NodeBaseViewModel)capturedNode.VM).Height = (int)(actionStartNodeBottomRight.Y - actionStartNodeTopLeft.Y + actionPosDelta.Y);
        }

        
        
        private void HandleNodePort(Point actionPosDelta)
        {
            //((NodeEditorViewModel)VM).NodePortConnectorX1 = actionStartAbsolutePos.X;
            //((NodeEditorViewModel)VM).NodePortConnectorY1 = actionStartAbsolutePos.Y;

            //((NodeEditorViewModel)VM).NodePortConnectorX2 = currentAbsolutePosition.X;
            //((NodeEditorViewModel)VM).NodePortConnectorY2 = currentAbsolutePosition.Y;


            //((NodeEditorViewModel)VM).NodeCoords = $"{((NodeEditorViewModel)VM).NodePortConnectorX1},{((NodeEditorViewModel)VM).NodePortConnectorY1}-{((NodeEditorViewModel)VM).NodePortConnectorX2},{((NodeEditorViewModel)VM).NodePortConnectorY2}";
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            currentAbsolutePosition = e.GetPosition(LayoutRoot);
            if (capturedNode == null)
            {
                HandleFreeMove(sender, e);
                return;
            }

            var actionPosDelta = new Point(currentAbsolutePosition.X - actionStartAbsolutePos.X, currentAbsolutePosition.Y - actionStartAbsolutePos.Y);

            switch (currentAction)
            {
                case ActionTypes.Drag:
                    HandleDrag(actionPosDelta);
                    break;
                case ActionTypes.Resize:
                    HandleResize(actionPosDelta);
                    break;
                case ActionTypes.Port:
                    HandleNodePort(actionPosDelta);
                    break;
                default:
                    HandleFreeMove(sender, e);
                    break;
            }

        }
        


        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            capturedNode = null;

            

            //((NodeEditorViewModel)VM).NodePortConnectorVisibility = Visibility.Hidden;
        }

        public IPamuxModel M { get; set; }
        public IPamuxView V { get { return this; }  set { } }
        public IPamuxViewModel VM { get; set; }
    }
}
