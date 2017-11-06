using Pamux.GameDev.Lib.Interfaces;
using Pamux.GameDev.Lib.Models;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

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

        public void HarvestAsset(object sender, ExecutedRoutedEventArgs e)
        {
            var content = e.Parameter as IContentHierarchy;
            if (content == null)
            {
                return;
            }

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
            Process.Start(unityPackageMetaData.MetaDataFolder);
        }
        public static RoutedCommand ViewMetadataCommand = new RoutedCommand();
        public void ViewMetadata(object sender, ExecutedRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            if (unityPackageMetaData == null)
            {
                return;
            }
            Process.Start(unityPackageMetaData.MetaDataPath);
        }
        public static RoutedCommand UnpackUnityPackageCommand = new RoutedCommand();

        public void UnpackUnityPackage(object sender, ExecutedRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            if (unityPackageMetaData == null)
            {
                return;
            }
            unityPackageMetaData.UnityPackage.EnsureUnpacked();

            Process.Start(unityPackageMetaData.UnityPackage.UnpackedContentDirectory);
        }

    }
}
