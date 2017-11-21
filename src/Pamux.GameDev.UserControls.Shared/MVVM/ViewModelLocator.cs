using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Pamux.GameDev.UserControls.ViewModels
{
    public class ViewModelLocator
    {
        public static T FindSibling<TParent, T>(UIElement uieSearchStart)
            where TParent : DependencyObject
            where T : DependencyObject
        {
            return FindChild<T>(FindParent<TParent>(uieSearchStart));
        }


        public static T FindParent<T>(UIElement uieSearchStart)
            where T: DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(uieSearchStart);
            while (parent != null && (parent.GetType() != typeof(T) && parent.GetType().BaseType != typeof(T)))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return (T) parent;
        }

        public static T FindChild<T>(DependencyObject uieSearchStart)
            where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(uieSearchStart); ++i)
            {
                var child = VisualTreeHelper.GetChild(uieSearchStart, i);
                if (child != null && (child.GetType() == typeof(T) || child.GetType().BaseType == typeof(T)))
                {
                    return (T) child;
                }

                child = FindChild<T>(child);

                if (child != null)
                {
                    return (T) child;
                }
            }

            return null;
        }

        public static UIElement FindMainWindow(UIElement node)
        {
            while (true)
            {
                var parent = VisualTreeHelper.GetParent(node);
                if (parent == null)
                {
                    return node;
                }

                node = (UIElement) parent;
            }
        }
    }
}
