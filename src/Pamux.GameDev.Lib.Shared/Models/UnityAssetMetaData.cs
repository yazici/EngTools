using Pamux.GameDev.Lib.Extensions;
using Pamux.GameDev.Lib.Interfaces;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;

namespace Pamux.GameDev.Lib.Models
{
    public class UnityAssetMetaData : ContentHierarchy
    {
        public static IDictionary<string, UnityAssetMetaData> GuidToAssetDictionary = new Dictionary<string, UnityAssetMetaData>();

        public UnityPackageMetaData UnityPackage => Root as UnityPackageMetaData;
        public string TempUnpackPath => $"{UnityPackage.TempUnpackRoot}\\{RelativePath}";

        public string TempUnpackUnityMetaFilePath => $"{TempUnpackPath}.meta";

        
        public string HarvestRoot => $"{UnityPackage.HarvestRoot}\\{Parent.RelativePath}";
        public string HarvestPath => $"{UnityPackage.HarvestRoot}\\{RelativePath}";

        public UnityAssetMetaData(IContentHierarchy parent, string name) 
            : base(parent, name)
        {
        }

        public void Harvest(bool withDependencies)
        {
            HarvestRoot.EnsureDirectory();

            try
            {
                File.Copy(TempUnpackPath, HarvestPath, true);
            }
            catch (Exception ex)
            {
                return;
            }

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
                if (Name.EndsWith(".prefab"))
                {
                    return GetPrefabDependencies();
                }
                return new List<UnityAssetMetaData>();
            }
        }

        private IReadOnlyCollection<UnityAssetMetaData> GetPrefabDependencies()
        {
            // I can only parse text based prefabs for now

            var prefabLines = File.ReadAllLines(HarvestPath);
            var results = new List<UnityAssetMetaData>();
            foreach (var line in prefabLines)
            {
                var startIndex = line.IndexOf("guid: ");
                if (startIndex == -1)
                {
                    continue;
                }

                startIndex += "guid: ".Length;

                var endIndex = line.IndexOf(",", startIndex);
                var guid = line.Substring(startIndex, endIndex - startIndex).Trim().ToLower();
                if (!GuidToAssetDictionary.ContainsKey(guid))
                {
                    continue;
                }
                results.Add(GuidToAssetDictionary[guid]);
            }

            return results;
        }

        private UnityAssetMetaData GetChildMetaDataFromGuid(string guid)
        {
            
            return null;
        }

        protected override IContentHierarchy CreateChild(string name)
        {
            return new UnityAssetMetaData(this, name);
        }

        internal void ReadUnityMetaFile()
        {
            if (!File.Exists(TempUnpackUnityMetaFilePath))
            {
                return;
            }
            var lines = File.ReadAllLines(TempUnpackUnityMetaFilePath);

            foreach (var line in lines)
            {
                if (!line.Contains("guid:"))
                {
                    continue;
                }

                var parts = line.Split(':');
                Guid = parts[1].Trim().ToLower();
                GuidToAssetDictionary[Guid] = this;
                return;
            }
        }
    }
}
