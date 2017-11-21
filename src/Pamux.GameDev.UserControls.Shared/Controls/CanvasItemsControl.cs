using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Pamux.GameDev.UserControls.Controls
{
    public class CanvasItemsControl : ItemsControl
    {
        protected override void PrepareContainerForItemOverride(
            DependencyObject element,
            object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var contentPresenter = element as ContentPresenter;

            if ((contentPresenter == null) || (item == null))
            {
                return;
            }

            BindingOperations.SetBinding(
                contentPresenter, 
                Canvas.LeftProperty, 
                new Binding { Source = item, Path = new PropertyPath("Left") });

            BindingOperations.SetBinding(
                contentPresenter,
                Canvas.TopProperty,
                new Binding { Source = item, Path = new PropertyPath("Top") });
        }
    }
}
