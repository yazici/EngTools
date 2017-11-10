using Pamux.GameDev.Lib.Interfaces;

using Pamux.GameDev.Lib.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System;
using System.Windows.Input;
using System.Diagnostics;
using Pamux.GameDev.Lib.Extensions;
using SharpCompress.Archives;
using System.Linq;

namespace Pamux.GameDev.Lib.Models
{
    public class UnityPackageMetaData : ContentHierarchy
    {
        public string Id;
        
        public string Type { get; set; }
        public string Company { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }

        //public string assetFileName;
        //public string AssetFileName
        //{
        //    get
        //    {
        //        return assetFileName;
        //    }
        //    set
        //    {
        //        if (assetFileName == value)
        //        {
        //            return;
        //        }
        //        assetFileName = value;
        //        Name = Path.GetFileNameWithoutExtension(assetFileName);
        //    }
        //}

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
                //AssetFileName = Path.GetFileName(fullPath);
            }
        }


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

        protected override IContentHierarchy CreateChild(string name)
        {
            return new UnityAssetMetaData(this, name);
        }

        public string UnityPackageFolder => $"{Settings.Unity3DAssetsFolderPath}\\{Company}\\{AssetSubFolder}";

        public string PamuxMetaDataDirectory => $"{Settings.Unity3DAssetDatabaseFolderPath}\\{Company}\\{AssetSubFolder}";
        public string PamuxMetaDataPath => $"{PamuxMetaDataDirectory}\\{Name}.{Settings.MetadataExtension}";

        public string ProducerAssetVersion;
        public string Url;

        public readonly List<string> Assets = new List<string>();

        private string unpackedContentDirectory;
        public string UnpackedContentDirectory => unpackedContentDirectory;

        private ISet<string> keywords = new HashSet<string>();
        

        public string TempUnpackRoot => $"{Settings.EngTempUnpackRoot}\\{Name}";
        public string HarvestRoot => $"{Settings.EngHarvestRoot}\\{Name}";

        public UnityPackageMetaData(string unityPackageFileFullPath, string company, string assetSubFolder)
            : base(null, Path.GetFileName(unityPackageFileFullPath))
        {
            this.FullPath = unityPackageFileFullPath;
            this.Company = company;
            this.AssetSubFolder = assetSubFolder;
            Parent = null;
        }

        public void InitializePackageContent()
        {
            foreach (string asset in Assets)
            {
                var unityAssetMetaData = Add(this, asset) as UnityAssetMetaData;
                if (unityAssetMetaData == null)
                {
                    continue;
                }

                if (!unityAssetMetaData.IsLeaf)
                {
                    unityAssetMetaData.PreviewImage = null;
                    continue;
                }
                unityAssetMetaData.ReadUnityMetaFile();
            }
        }

        public void Initialize()
        {
            if (HaveFreshMetadataFile())
            {
                LoadMetaData();
            }
            else
            {
                ExtractMetaData(FullPath, Assets);
                SaveMetaData();
            }


            InitializePackageContent();
            GenerateKeywords();
        }
        
        public void SaveMetaData()
        {
            var dir = this.PamuxMetaDataPath.Substring(0, this.PamuxMetaDataPath.LastIndexOf('\\'));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllLines(this.PamuxMetaDataPath, Assets);
        }



        private bool HaveFreshMetadataFile()
        {
            return File.Exists(this.PamuxMetaDataPath) && File.GetLastWriteTimeUtc(this.PamuxMetaDataPath) > File.GetLastWriteTimeUtc(this.FullPath);
        }

        private void LoadMetaData()
        {
            Assets.AddRange(File.ReadAllLines(this.PamuxMetaDataPath));
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

        public void EnsureUnpacked()
        {
            if (string.IsNullOrWhiteSpace(unpackedContentDirectory) || !Directory.Exists(unpackedContentDirectory))
            {
                if (!UnpackContent())
                {
                    return;
                }

                unpackedContentDirectory = TempUnpackRoot;
            }
        }

        private bool SevenZip(string inputFilePath, string outputDirectory)
        {
            if (!File.Exists(Settings.SevenZipCli))
            {
                throw new FileNotFoundException("Needs 7z.exe to unpack a unityPackage content");
            }
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException($"{inputFilePath} doesn't exist");
            }
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = Settings.SevenZipCli,
                Arguments = $"x \"{inputFilePath}\" -o\"{outputDirectory}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            var p = Process.Start(startInfo);
            while (!p.StandardOutput.EndOfStream)
            {
                Console.WriteLine(p.StandardOutput.ReadLine());
            }

            return p.ExitCode == 0;
        }


        private bool UnpackContent()
        {
            TempUnpackRoot.RemoveDirectoryRecursively();

            var context = new UnpackingContext
            {
                PamuxMetaDataDirectory = PamuxMetaDataDirectory,
                OutputDirectory = TempUnpackRoot
            };

            if (!SevenZip(FullPath, context.FirstStepOutputDirectory))
            {
                return false;
            }

            foreach (var file in context.FirstStepOutputDirectory.EnumerateFiles("*"))
            {
                if (!SevenZip(file, context.SecondStepOutputDirectory))
                {
                    return false;
                }
            }

            context.FirstStepOutputDirectory.RemoveDirectoryRecursively();

            foreach (var hashedDirectory in context.SecondStepOutputDirectory.EnumerateDirectories())
            {
                context.HashedDirectory = hashedDirectory;
                if (!context.CopyAssetToItsPath())
                {
                    return false;
                }
            }

            context.SecondStepOutputDirectory.RemoveDirectoryRecursively();

            return true;
        }


        public static void ExtractMetaData(string unityPackageFullPath, IList<string> assets)
        {
            var archive = ArchiveFactory.Open(unityPackageFullPath);
            if (archive == null)
            {
                return;
            }

            foreach (var volume in archive.Entries)
            {
                ProcessArchiveVolume(volume, assets);
            }
        }


        private static void ProcessArchiveVolume(IArchiveEntry volume, IList<string> assets)
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

            var volumeStreamFilePath = Path.GetTempFileName();
            try
            {
                using (var tempStream = File.Open(volumeStreamFilePath, FileMode.Open))
                {
                    volume.WriteTo(tempStream);
                    tempStream.Flush();

                    using (var tempArchive = ArchiveFactory.Open(tempStream))
                    {
                        var pathEntries = from entry in tempArchive.Entries.ToArray()
                                          where Path.GetFileName(entry.Key).Contains("pathname")
                                                && !entry.IsDirectory
                                          select entry;

                        var toExtract = pathEntries.ToDictionary(
                            pathEntry => Path.GetDirectoryName(pathEntry.Key),
                            pathEntry => getFirstLine(pathEntry));

                        var assetEntries = from entry in tempArchive.Entries.ToArray()
                                           where Path.GetFileName(entry.Key).Contains("asset")
                                                 && !entry.IsDirectory
                                           select new
                                           {
                                               entry = entry,
                                               path = toExtract[Path.GetDirectoryName(entry.Key)]
                                           };


                        foreach (var asset in assetEntries)
                        {
                            assets.Add(asset.path);
                        }
                    }

                }
            }
            finally
            {
                if (volumeStreamFilePath != null && File.Exists(volumeStreamFilePath))
                {
                    File.Delete(volumeStreamFilePath);
                }
            }
        }
    }
}
