using System.Collections.Generic;
using System.IO;

using Pamux.GameDev.Tools.Tabs;
using Pamux.GameDev.Tools.AssetUtils;

namespace Pamux.GameDev.Tools.Models
{
    public class AssetMetaData
    {
        public string Id;
        public string Name {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }
                name = value;
            }
        }
        public string Type { get; set; }
        public string Company { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }

        public string assetFileName;
        public string AssetFileName
        {
            get
            {
                return assetFileName;
            }
            set
            {
                if (assetFileName == value)
                {
                    return;
                }
                assetFileName = value;
                Name = Path.GetFileNameWithoutExtension(assetFileName);
            }
        }

        private string fullPath;
        public string FullPath
        {
            get
            {
                return fullPath;
            }
            set
            {
                if (fullPath == value)
                {
                    return;
                }
                fullPath = value;
                AssetFileName = Path.GetFileName(fullPath);
            }
        }


        private string name;
        private string assetSubFolder;
        public string AssetSubFolder
        {
            get
            {
                return assetSubFolder;
            }

            set
            {
                if (assetSubFolder == value)
                {
                    return;
                }

                assetSubFolder = value;
                Type = value;
            }
        }

        public string UnityPackageFolder => $"{Settings.Unity3DAssetsFolderPath}\\{Company}\\{AssetSubFolder}";

        public string MetaDataFolder => $"{Settings.Unity3DAssetDatabaseFolderPath}\\{Company}\\{AssetSubFolder}";
        public string MetaDataPath => $"{MetaDataFolder}\\{Name}.{Settings.MetadataExtension}";

        public string ProducerAssetVersion;
        public string Url;

        public readonly List<string> Assets = new List<string>();


        private ISet<string> keywords = new HashSet<string>();
        private UnityPackageProxy unityPackage;
        public UnityPackageProxy UnityPackage
        {
            get
            {
                if (unityPackage == null)
                { 
                    unityPackage = new UnityPackageProxy(this.FullPath);
                }
                return unityPackage;
            }
        }

        public AssetMetaData()
        {
        }

        public void Initialize()
        {
            if (HaveFreshMetadataFile())
            {
                LoadMetaData();
            }
            else
            {
                UnityPackage.ExtractMetaData(Assets);
                SaveMetaData();
            }

            GenerateKeywords();
        }

        public void Harvest(string harvestPath)
        {
        }

        internal void SaveMetaData()
        {
            var dir = this.MetaDataPath.Substring(0, this.MetaDataPath.LastIndexOf('\\'));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllLines(this.MetaDataPath, Assets);
        }



        private bool HaveFreshMetadataFile()
        {
            return File.Exists(this.MetaDataPath) && File.GetLastWriteTimeUtc(this.MetaDataPath) > File.GetLastWriteTimeUtc(this.FullPath);
        }

        private void LoadMetaData()
        {
            Assets.AddRange(File.ReadAllLines(this.MetaDataPath));
        }

             
        

        private void GenerateKeywords()
        {
            GenerateKeywords(Name);
            GenerateKeywords(Type);
            GenerateKeywords(Company);

            foreach (string asset in Assets)
            {
                GenerateKeywords(asset.Substring("Assets/".Length), false);
            }
        }

        public static char[] Separators = { ' ', ',', '\t', '.', ';', '/', '_' };
        private void GenerateKeywords(string keyword, bool prefixes = true)
        {
            keyword = keyword.ToLower();

            if (prefixes)
            {
                foreach (string kw in keyword.Split(Separators))
                {
                    GenerateKeywordRecursively(kw);
                }
            }
            else
            {
                foreach (string kw in keyword.Split(Separators))
                {
                    string k = kw.Trim();


                    if (!IsGenericKeyword(keyword.EndsWith("." + k), k))
                    {

                        this.keywords.Add(k);
                    }
                }
            }
        }

        private void GenerateKeywordRecursively(string keyword)
        {
            if (IsGenericKeyword(false, keyword))
            {
                return;
            }
            this.keywords.Add(keyword);
            GenerateKeywordRecursively(keyword.Substring(0, keyword.Length - 1).Trim());
        }

        string[] generics = new string[] { "the", "and", "but" };

        bool IsGenericKeyword(bool extension, string keyword)
        {
            if (extension)
            {
                if (keyword == "unity")
                {
                    return true;
                }
            }
            if (keyword.Length < 2)
            {
                return true;
            }

            if (keyword.Length == 2)
            {
                return keyword != "3d";
            }



            foreach (string g in generics)
            {
                if (keyword == g)
                {
                    return true;
                }
            }

            if (keyword.Length == 3)
            {
                return false;
            }
            return false;
        }

        internal bool Match(AssetQuery query)
        {
            foreach (string token in query.tokens)
            {
                if (!keywords.Contains(token))
                {
                    return false;
                }
            }
            return true;
        }




        internal string GetThumbnailHTML()
        {
            return AssetLibrary.GetHTMLAnchor("results-entity",
                                                        "preview-entity-871e593cbe2aad188e74984ac385d8ca",
                                                        GetUrl(),
                                                        GetImageUrl(),
                                                        GetInfoHTML());
            //<a class="results-entity" id="preview-entity-871e593cbe2aad188e74984ac385d8ca" style='background-image: url("/3dw/getbinary?subjectId=871e593cbe2aad188e74984ac385d8ca&amp;subjectClass=entity&amp;name=lt");' href="model.html?id=871e593cbe2aad188e74984ac385d8ca"></a>



        }

        private string GetInfoHTML()
        {
            return AssetLibrary.GetHTMLDiv("results-entity-name", "", Name)
                    + AssetLibrary.GetHTMLDiv("results-entity-type", "", Type)
                    + AssetLibrary.GetHTMLDiv("results-entity-company", "", Company)/*
                    + AssetLibrary.GetHTMLDiv("results-entity-description", "", description)
                    + AssetLibrary.GetHTMLDiv("results-entity-price", "", price.ToString())*/;
        }

        private string GetImageUrl()
        {
            return "/images/image.jpg";
        }

        private string GetUrl()
        {
            return "url.html";
        }
    }
}
