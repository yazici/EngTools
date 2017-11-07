using Pamux.GameDev.Lib.Interfaces;
using Pamux.GameDev.Lib.Models;
using Pamux.GameDev.UserControls.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Pamux.GameDev.UserControls.Tabs
{
    public partial class AssetLibrary : UserControl, IQueryAndResults<UnityPackageMetaData>
    {
        public string Query
        {
            get { return (string)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public static readonly DependencyProperty QueryProperty = DependencyProperty.Register(
            "Query",
            typeof(string),
            typeof(AssetLibrary),
            new FrameworkPropertyMetadata(
                "",
                FrameworkPropertyMetadataOptions.None,
                new PropertyChangedCallback(OnQueryChanged),
                new CoerceValueCallback(OnValueCoerce)
            ),
            new ValidateValueCallback(IsValidQuery)
        );

        private static bool IsValidQuery(object value)
        {
            return true;
        }

        private static object OnValueCoerce(DependencyObject d, object baseValue)
        {
            return baseValue;
        }

        private static void OnQueryChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var assetLibrary = sender as AssetLibrary;
            if (assetLibrary != null)
            {
                assetLibrary.FilterAssets();
            }
        }


        public IEnumerable<UnityPackageMetaData> Results
        {
            get { return (IEnumerable<UnityPackageMetaData>)GetValue(ResultsProperty); }
            set { SetValue(ResultsProperty, value); }
        }
        public static readonly DependencyProperty ResultsProperty =
            DependencyProperty.Register("Results", typeof(List<UnityPackageMetaData>), typeof(AssetLibrary), new UIPropertyMetadata(new List<UnityPackageMetaData>()));

        private DataTemplateSelector rowDetailsTemplateSelector = new AssetLibraryDetailsTemplateSelector();
        public DataTemplateSelector RowDetailsTemplateSelector => rowDetailsTemplateSelector;

    }
}
