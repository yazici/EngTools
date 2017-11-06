using Pamux.GameDev.Lib.Interfaces;
using Pamux.GameDev.Lib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.Lib.Proxies
{
    public class UnityAssetProxy : IUnityAssetProxy
    {
        private IUnityPackageProxy unityPackage;
        private string harvestPath;

        public UnityAssetProxy(IUnityPackageProxy unityPackage, string harvestPath)
        {
            this.unityPackage = unityPackage;
            this.harvestPath = harvestPath;
        }

        public bool Harvest()
        {
            unityPackage.EnsureUnpacked();
            return true;
        }


        

        //private void CopyArchivedAssetFile(string archiveFileName, string realFileName)
        //{
        //    var assetFilePath = $"{hashedDirectory}\\{archiveFileName}";
        //    if (File.Exists(assetFilePath))
        //    {
        //        File.Copy(assetFilePath, $"{realAssetDirectory}\\{realFileName}");
        //    }
        //}
    }
}
