using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Threading.Tasks;
using Pamux.GameDev.Tools.Tabs;
using SharpCompress.Archives;
using SharpCompress.Common.Tar;

namespace Pamux.GameDev.Tools.Models
{
    public class AssetPackage
    {
        public string id;
        public string name { get; set; }
        public string type { get; set; }
        public string company { get; set; }
        public float price { get; set; }
        public string description { get; set; }

        public string assetsRootFolder;
        public string companyFolder;
        public string assetFolder;
        public string assetFileName;
        public string fullPath;
        public string metaDataPath;
        public string producerAssetVersion;
        public string url;

        public readonly List<string> Assets = new List<string>();

        public string AssetFolder => $"{assetsRootFolder}\\{companyFolder}\\{assetFolder}";

        private ISet<string> keywords = new HashSet<string>();

        private AssetPackage(string fullPath, string assetsRootFolder, string companyFolder, string assetFolder, string assetFileName)
        {
            this.assetsRootFolder = assetsRootFolder;
            this.companyFolder = companyFolder;
            this.assetFolder = assetFolder;
            this.fullPath = fullPath;
            this.metaDataPath = $"{fullPath}.{Settings.MetadataExtension}";
            this.assetFileName = assetFileName;

            this.company = companyFolder;
            this.type = assetFolder;
            this.name = assetFileName.Substring(0, assetFileName.LastIndexOf('.'));

            this.metaDataPath = $"{Settings.Unity3DAssetDatabaseFolderPath}\\{companyFolder}\\{assetFolder}\\{name}.{Settings.MetadataExtension}";

            if (HaveFreshMetadataFile())
            {
                LoadMetaData();
            }
            else
            {
                ExtractMetaData();
                SaveMetaData();
            }

            GenerateKeywords();
        }

        private void SaveMetaData()
        {
            var dir = this.metaDataPath.Substring(0, this.metaDataPath.LastIndexOf('\\'));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllLines(this.metaDataPath, Assets);
        }

        private bool HaveFreshMetadataFile()
        {
            return File.Exists(this.metaDataPath) && File.GetLastWriteTimeUtc(this.metaDataPath) > File.GetLastWriteTimeUtc(this.fullPath);
        }

        private void LoadMetaData()
        {
            Assets.AddRange(File.ReadAllLines(this.metaDataPath));
        }

        private void ExtractMetaData()
        {
            var archive = ArchiveFactory.Open(this.fullPath);

            if (archive == null)
            {
                return;
            }

            foreach (var volume in archive.Entries)
            {
                ProcessArchiveVolume(volume);
            }
        }
        
        private void ProcessArchiveVolume(IArchiveEntry volume)
        {
            Func<IArchiveEntry, string> getFirstLine = (entry) =>
            {
                var path = string.Empty;
                using (var sr = new StreamReader(entry.OpenEntryStream()))
                {
                    path = sr.ReadLine();
                }
                return path;
            };

            var tempFile = Path.GetTempFileName();
            try
            {
                using (var tempStream = File.Open(tempFile, FileMode.Open))
                {
                    volume.WriteTo(tempStream);
                    tempStream.Flush();

                    using (var tempArchive = ArchiveFactory.Open(tempStream))
                    {
                        IDictionary<string, string> toExtract = null;

                        var pathEntries = from entry in tempArchive.Entries.ToArray()
                                          where Path.GetFileName(entry.Key).Contains("pathname")
                                            && !entry.IsDirectory
                                          select entry;

                        toExtract = pathEntries.ToDictionary(
                            pathEntry => Path.GetDirectoryName(pathEntry.Key),
                            pathEntry => getFirstLine(pathEntry));
                        
                        var assets = from entry in tempArchive.Entries.ToArray()
                                     where Path.GetFileName(entry.Key).Contains("asset")
                                        && !entry.IsDirectory
                                     select new
                                     {
                                         entry = entry,
                                         path = toExtract[Path.GetDirectoryName(entry.Key)]
                                     };


                        foreach (var a in assets)
                        {
                            Assets.Add(a.path);
                        }
                    }

                }
            }
            finally
            {
                if (tempFile != null && File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        private void GenerateKeywords()
        {
            GenerateKeywords(name);
            GenerateKeywords(type);
            GenerateKeywords(company);

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
        public static AssetPackage CreateFromPath(string fullPath, string assetsRootFolder, string companyFolder, string assetFolder, string assetFileName)
        {
            AssetPackage a = new AssetPackage(fullPath, assetsRootFolder, companyFolder, assetFolder, assetFileName);
            return a;
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
            return AssetLibrary.GetHTMLDiv("results-entity-name", "", name)
                    + AssetLibrary.GetHTMLDiv("results-entity-type", "", type)
                    + AssetLibrary.GetHTMLDiv("results-entity-company", "", company)/*
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
