using Pamux.GameDev.Lib.Models;
using Pamux.GameDev.UserControls.Tabs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Pamux.GameDev.UserControls.Converters
{
    public class TreeItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemWithContextMenu { get; set; }
        public DataTemplate ItemAndDependenciesWithContextMenu { get; set; }
        public DataTemplate ItemWithNoContextMenu { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item != null)
            {
                var contentHierarchy = item as ContentHierarchy;
                if (contentHierarchy == null)
                {
                    return null;
                }

                return contentHierarchy.IsHarvestable
                    ? (contentHierarchy.HasDependencies ? ItemAndDependenciesWithContextMenu : ItemWithContextMenu)
                    : ItemWithNoContextMenu;
            }

            return null;
        }
    }
}
