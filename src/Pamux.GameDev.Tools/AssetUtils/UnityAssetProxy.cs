using Pamux.GameDev.Tools.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamux.GameDev.Tools.AssetUtils
{
    public class UnityAssetProxy
    {
        private UnityPackageProxy unityPackage;
        private string harvestPath;



        


        public UnityAssetProxy(UnityPackageProxy unityPackage, string harvestPath)
        {
            this.unityPackage = unityPackage;
            this.harvestPath = harvestPath;
        }

        internal bool Harvest()
        {
            var unpackedFolder = unityPackage.EnsureUnpacked();


           

            

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
