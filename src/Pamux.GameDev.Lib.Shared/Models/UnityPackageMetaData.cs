using Pamux.GameDev.Lib.Interfaces;
using Pamux.GameDev.Lib.Proxies;
using Pamux.GameDev.Lib.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System;

namespace Pamux.GameDev.Lib.Models
{
    public class UnityPackageMetaData
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

        private IList<IContentHierarchy> packageContent = new List<IContentHierarchy>();
        public IList<IContentHierarchy> PackageContent => packageContent;

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
        private IUnityPackageProxy unityPackage;
        public IUnityPackageProxy UnityPackage
        {
            get
            {
                if (unityPackage == null)
                { 
                    unityPackage = new UnityPackageProxy(this);
                }
                return unityPackage;
            }
        }

        public string TempUnpackRoot => $"{Settings.EngTempUnpackRoot}\\{Name}";

        public UnityPackageMetaData()
        {
        }

        public class AssetOrFolder : IContentHierarchy
        {
            public AssetOrFolder(IContentHierarchy parent, string name)
            {
                
                Parent = parent;

                if (Parent == null)
                {
                    Depth = 1;
                }
                else
                {
                    Depth = Parent.Depth + 1;
                    Parent.Children.Add(this);
                }
                Name = name;
                IsExpanded = Depth <= 3;
                Children = new List<IContentHierarchy>();
            }


            public int Depth { get; set; }
            public string Name { get; set; }

            public bool IsExpanded { get; set; }

            public IContentHierarchy Parent { get; set; }
            public IList<IContentHierarchy> Children { get; set; }

            internal static void Add(IContentHierarchy packageRoot, string assetPath)
            {
                var node = packageRoot;
                if (node == null)
                {
                    return;
                }

                var parts = assetPath.Split('/');
                
                for (int i = 0; i < parts.Length; ++i)
                {
                    node = node.EnsureChild(parts[i]);
                }
            }

            public IContentHierarchy EnsureChild(string name)
            {
                foreach (var child in Children)
                {
                    if (child.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return child as AssetOrFolder;
                    }
                }

                return new AssetOrFolder(this, name);
            }
        }

        public UnityPackageMetaData(string unityPackageFileFullPath, string company, string assetSubFolder)
        {
            this.FullPath = unityPackageFileFullPath;
            this.Company = company;
            this.AssetSubFolder = assetSubFolder;

            
        }

        public void InitializePackageContent()
        {
            var root = new AssetOrFolder(null, "Package Contents");
            foreach (string asset in Assets)
            {
                AssetOrFolder.Add(root, asset);
            }

            PackageContent.Clear();
            PackageContent.Add(root);
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


            InitializePackageContent();
            GenerateKeywords();
        }
        
        public void SaveMetaData()
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

        public bool Match(AssetQuery query)
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




        public string GetThumbnailHTML()
        {
            return HtmlGen.GetHTMLAnchor("results-entity",
                                                        "preview-entity-871e593cbe2aad188e74984ac385d8ca",
                                                        GetUrl(),
                                                        GetImageUrl(),
                                                        GetInfoHTML());
            //<a class="results-entity" id="preview-entity-871e593cbe2aad188e74984ac385d8ca" style='background-image: url("/3dw/getbinary?subjectId=871e593cbe2aad188e74984ac385d8ca&amp;subjectClass=entity&amp;name=lt");' href="model.html?id=871e593cbe2aad188e74984ac385d8ca"></a>



        }

        private string GetInfoHTML()
        {
            return HtmlGen.GetHTMLDiv("results-entity-name", "", Name)
                    + HtmlGen.GetHTMLDiv("results-entity-type", "", Type)
                    + HtmlGen.GetHTMLDiv("results-entity-company", "", Company)/*
                    + HtmlGen.GetHTMLDiv("results-entity-description", "", description)
                    + HtmlGen.GetHTMLDiv("results-entity-price", "", price.ToString())*/;
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
