using Pamux.GameDev.Lib.Models;
using Pamux.GameDev.UserControls.Tabs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Pamux.GameDev.UserControls.Converters
{
    class AssetLibraryDetailsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element && item != null && item is UnityPackageMetaData)
            {
                return element.FindResource("AssetDetailsTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
