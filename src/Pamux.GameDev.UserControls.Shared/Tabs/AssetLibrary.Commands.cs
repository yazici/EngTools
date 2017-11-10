using Pamux.GameDev.Lib.Interfaces;
using Pamux.GameDev.Lib.Models;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Collections.Generic;
using Pamux.GameDev.Lib.Extensions;

namespace Pamux.GameDev.UserControls.Tabs
{
    public partial class AssetLibrary : UserControl, IQueryAndResults<UnityPackageMetaData>
    {
        public void CanExecuteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            e.CanExecute = unityPackageMetaData != null;
        }

        public static RoutedCommand HarvestAssetCommand = new RoutedCommand();
        public static RoutedCommand HarvestAssetWithDependenciesCommand = new RoutedCommand();

        public void HarvestAsset(object sender, ExecutedRoutedEventArgs e)
        {
            HarvestAssetInternal(e.Parameter, false);
        }

        public void HarvestAssetWithDependencies(object sender, ExecutedRoutedEventArgs e)
        {
            HarvestAssetInternal(e.Parameter, true);
        }

        private void HarvestAssetInternal(object content, bool withDependencies)
        {
            var unityAssetMetaData = content as UnityAssetMetaData;
            if (unityAssetMetaData == null)
            {
                return;
            }

            unityAssetMetaData.UnityPackageHarvestRoot.EnsureDirectory();
            unityAssetMetaData.EnsureUnpacked();

            unityAssetMetaData.Harvest(withDependencies);
        }


        

        public void CanExecutePerAssetCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var content = e.Parameter as IContentHierarchy;
            e.CanExecute = content != null;
        }



        public static RoutedCommand SearchInAssetStoreCommand = new RoutedCommand();
        public void SearchInAssetStore(object sender, ExecutedRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            if (unityPackageMetaData == null)
            {
                return;
            }
            DoAssetStoreSearch(unityPackageMetaData);
        }

        public static RoutedCommand OpenAssetFolderCommand = new RoutedCommand();
        public void OpenAssetFolder(object sender, ExecutedRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            if (unityPackageMetaData == null)
            {
                return;
            }
            Process.Start(unityPackageMetaData.UnityPackageFolder);
        }
        public static RoutedCommand OpenMetadataFolderCommand = new RoutedCommand();
        public void OpenMetadataFolder(object sender, ExecutedRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            if (unityPackageMetaData == null)
            {
                return;
            }
            Process.Start(unityPackageMetaData.PamuxMetaDataDirectory);
        }
        public static RoutedCommand ViewMetadataCommand = new RoutedCommand();
        public void ViewMetadata(object sender, ExecutedRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            if (unityPackageMetaData == null)
            {
                return;
            }
            Process.Start(unityPackageMetaData.PamuxMetaDataPath);
        }
        public static RoutedCommand UnpackUnityPackageCommand = new RoutedCommand();

        public void UnpackUnityPackage(object sender, ExecutedRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            if (unityPackageMetaData == null)
            {
                return;
            }
            unityPackageMetaData.EnsureUnpacked();

            Process.Start(unityPackageMetaData.UnpackedContentDirectory);
        }

    }
}
