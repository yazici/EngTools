using Pamux.GameDev.Lib.Extensions;
using Pamux.GameDev.Lib.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Pamux.GameDev.Lib.Models
{
    public class UnityAssetMetaData : ContentHierarchy
    {
        public UnityPackageMetaData UnityPackage => Root as UnityPackageMetaData;
        public string TempUnpackPath => $"{UnityPackage.TempUnpackRoot}\\{RelativePath}";
        public string HarvestRoot => $"{UnityPackage.HarvestRoot}\\{Parent.RelativePath}";
        public string HarvestPath => $"{UnityPackage.HarvestRoot}\\{RelativePath}";



        public UnityAssetMetaData(IContentHierarchy parent, string name) 
            : base(parent, name)
        {
        }

        public void Harvest(bool withDependencies)
        {
            HarvestRoot.EnsureDirectory();

            File.Copy(TempUnpackPath, HarvestPath);

            if (withDependencies)
            { 
                foreach (var unityAssetMetaData in Dependencies)
                {
                    unityAssetMetaData.Harvest(true);
                }
            }
        }

        private IReadOnlyCollection<UnityAssetMetaData> Dependencies
        {
            get
            {
                var result = new List<UnityAssetMetaData>();

                return result;
            }
        }

        protected override IContentHierarchy CreateChild(string name)
        {
            return new UnityAssetMetaData(this, name);
        }

    }
}
