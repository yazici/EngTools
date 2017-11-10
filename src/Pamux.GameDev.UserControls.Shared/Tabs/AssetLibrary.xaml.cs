using Pamux.GameDev.Lib.Extensions;
using Pamux.GameDev.Lib.Interfaces;
using Pamux.GameDev.Lib.Models;
using Pamux.GameDev.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Pamux.GameDev.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for AssetLibrary.xaml
    /// </summary>
    public partial class AssetLibrary : UserControl, IQueryAndResults<UnityPackageMetaData>
    {
        public static AssetLibrary INSTANCE;

        public static readonly IList<UnityPackageMetaData> AllAssets = new List<UnityPackageMetaData>();
        public static IList<UnityPackageMetaData> FilteredAssets { get; private set; }

        private static readonly IList<UnityPackageMetaData> FilteredAssets_A = new List<UnityPackageMetaData>();
        private static readonly IList<UnityPackageMetaData> FilteredAssets_B = new List<UnityPackageMetaData>();

        public AssetLibrary()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            FilterAssets();
        }

        private void AssetLibrary_Load(object sender, EventArgs e)
        {
            INSTANCE = this;

            FilterAssets();
        }

        public string GetGDODataPath(string fileName)
        {
            return $"{Settings.EngData}\\{fileName}.tsv";
        }

        //private void LoadGDOData(string dbName, CheckedListBox.ObjectCollection items)
        //{
        //    foreach (var aLine in File.ReadAllLines(GetGDODataPath(dbName)))
        //    {
        //        if (string.IsNullOrWhiteSpace(aLine))
        //        {
        //            continue;
        //        }
        //        var line = aLine.Trim();
        //        if (line.StartsWith("#"))
        //        {
        //            continue;
        //        }
        //        items.Add(line);
        //    }
        //}

        private void EnumerateUnityPackageFiles()
        {
            AllAssets.Clear();
            EnumerateUnityPackageFiles(Settings.Unity3DAssetsFolderPath, new List<string>());
            foreach (var asset in AllAssets)
            {
                asset.Initialize();
            }
        }

        private void EnumerateUnityPackageFiles(string directory, IList<string> nameStack)
        {
            if (nameStack.Count < 2)
            {
                var lastSubDirectoryIndex = directory.Length + 1;
                foreach (var subDirectory in directory.EnumerateDirectories())
                {
                    nameStack.Add(subDirectory.Substring(lastSubDirectoryIndex));
                    EnumerateUnityPackageFiles(subDirectory, nameStack);
                }
            }
            else
            {
                foreach (var unityPackageFileFullPath in directory.EnumerateFiles("*.unitypackage"))
                {
                    AllAssets.Add(new UnityPackageMetaData(unityPackageFileFullPath, nameStack[0], nameStack[1]));
                }
            }

            if (nameStack.Count > 0)
            {
                nameStack.RemoveAt(nameStack.Count - 1);
            }
        }

        private void FilterAssets()
        {
            if (!NeedRequery())
            {
                return;
            }

            if (ShouldUpdateAssetLibraryDatabase())
            {
                EnumerateUnityPackageFiles();
            }

            if (Results == FilteredAssets_B)
            {
                FilteredAssets = FilteredAssets_A;
            }
            else
            {
                FilteredAssets = FilteredAssets_B;
            }

            FilteredAssets.Clear();
            var query = new AssetQuery(Query);
            foreach (var a in AllAssets)
            {
                if (a.Match(query))
                {
                    FilteredAssets.Add(a);
                }
            }

            Results = FilteredAssets;

            //File.WriteAllText(Settings.LocalHtmlPath, GenerateHTML());
            //// browser.Url = Settings.LocalHtmlUri;
        }

        private string GenerateHTML()
        {
            return $"<!DOCTYPE html><html lang=\"en\" dir=\"ltr\" class=\"client-nojs\"><head>{HtmlGen.GetHTMLHead()}</head><body>{GetHTMLBody()}</body></html>";
        }

        private string GetHTMLBody()
        {
            return HtmlGen.GetHTMLDiv("search-results", "multi-line-results-search-results", GetAllSearchResults());
        }

        private string GetAllSearchResults()
        {
            string result = "";
            foreach (var asset in FilteredAssets)
            {
                result += asset.GetThumbnailHTML();
            }
            return result;
        }

        private bool NeedRequery()
        {
            return true;
        }

        private bool ShouldUpdateAssetLibraryDatabase()
        {
            return AllAssets.Count == 0;
        }

        private void txtQuery_TextChanged(object sender, EventArgs e)
        {
            FilterAssets();
        }

        //private void lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    //Process.Start(e.Link.LinkData as string);
        //}

            
        private void DoAssetStoreSearch(UnityPackageMetaData assetPackage)
        {
            var nameParts = assetPackage.Name.Split(' ');
            var query = "";
            foreach (var namePart in nameParts)
            {
                query = $"{query}&q={namePart}";
            }

            Process.Start($"https://assetstore.unity.com/search?{query.TrimStart('&')}");
        }
    }
}
