using Pamux.GameDev.Lib.Extensions;
using Pamux.GameDev.Lib.Interfaces;
using Pamux.GameDev.Lib.Models;
using Pamux.GameDev.Lib.Utilities;
using Pamux.GameDev.UserControls.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pamux.GameDev.UserControls.Tabs
{
    /// <summary>
    /// Interaction logic for AssetLibrary.xaml
    /// </summary>
    public partial class AssetLibrary : UserControl, IQueryAndResults<UnityPackageMetaData>
    {
        public static AssetLibrary INSTANCE;

        //private TreeNode treeAssetContents_ContextMenuNode;
        private string harvestPath;


        public static readonly IList<UnityPackageMetaData> AllAssets = new List<UnityPackageMetaData>();
        public static IList<UnityPackageMetaData> FilteredAssets { get; private set; }

        private static readonly IList<UnityPackageMetaData> FilteredAssets_A = new List<UnityPackageMetaData>();
        private static readonly IList<UnityPackageMetaData> FilteredAssets_B = new List<UnityPackageMetaData>();

       

        public AssetLibrary()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;

            FilterAssets();

            

            //treeAssetContentsContextMenuStrip = new ContextMenuStrip();
            //treeAssetContentsContextMenuStrip.Opening += new CancelEventHandler(treeAssetContentsContextMenuStrip_Opening);
            //toolStripItemHarvestAsset.Click += new EventHandler(toolStripItemHarvestAsset_Click);


        }

        //private void treeAssetContents_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    treeAssetContents_ContextMenuNode = treeAssetContents.GetNodeAt(e.X, e.Y);
        //}

        //private void treeAssetContentsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        //{
        //    harvestPath = null;
        //    treeAssetContentsContextMenuStrip.Items.Clear();

        //    if (treeAssetContents_ContextMenuNode == null)
        //    {
        //        if (treeAssetContents == null || treeAssetContents.SelectedNode == null)
        //        {
        //            e.Cancel = true;
        //            return;
        //        }

        //        treeAssetContents_ContextMenuNode = treeAssetContents.SelectedNode;
        //    }

        //    try
        //    {
        //        if (treeAssetContents_ContextMenuNode.Nodes.Count != 0)
        //        {
        //            e.Cancel = true;
        //            return;
        //        }

        //        treeAssetContents.SelectedNode = treeAssetContents_ContextMenuNode;


        //        toolStripItemHarvestAsset.Text = $"Harvest Asset: {treeAssetContents_ContextMenuNode.Text}";
        //        harvestPath = treeAssetContents_ContextMenuNode.FullPath;
        //        treeAssetContentsContextMenuStrip.Items.Add(toolStripItemHarvestAsset);
        //        e.Cancel = false;
        //    }
        //    finally
        //    {
        //        treeAssetContents_ContextMenuNode = null;
        //    }
        //}

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

        public void CanExecuteCommand(object sender,  CanExecuteRoutedEventArgs e)
        {
            var unityPackageMetaData = e.Parameter as UnityPackageMetaData;
            e.CanExecute = unityPackageMetaData != null;
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

        private void ShowMetaData(UnityPackageMetaData ap)
        {
            //treeAssetContents.Tag = ap;
            //treeAssetContents.Nodes.Clear();
            //var mappedNodes = new Dictionary<string, TreeNode>();

            //foreach (string asset in ap.Assets)
            //{
            //    var assetParts = asset.Split('/');
            //    var path = "";

            //    TreeNode currentNode = null;
            //    for (int i = 0; i < assetParts.Length; ++i)
            //    {
            //        path += $"{assetParts[i]}/";


            //        if (mappedNodes.ContainsKey(path))
            //        {
            //            currentNode = mappedNodes[path];
            //        }
            //        else
            //        {
            //            if (currentNode == null)
            //            {
            //                currentNode = mappedNodes[path] = treeAssetContents.Nodes.Add(assetParts[i]);
            //            }
            //            else
            //            {
            //                currentNode = mappedNodes[path] = currentNode.Nodes.Add(assetParts[i]);
            //            }

            //            currentNode.ContextMenuStrip = treeAssetContentsContextMenuStrip;
            //        }


            //    }
            //}

            //if (treeAssetContents.Nodes.Count > 0)
            //{
            //    treeAssetContents.Nodes[0].Expand();
            //    foreach (TreeNode n in treeAssetContents.Nodes[0].Nodes)
            //    {
            //        n.Expand();
            //    }
            //}
        }

        //private void lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    //Process.Start(e.Link.LinkData as string);
        //}


        //private void results_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        //{
        //    foreach (DataGridViewColumn column in results.Columns)
        //    {
        //        column.ContextMenuStrip = resultsContextMenuStrip;
        //    }

        //    e.ContextMenuStrip = resultsContextMenuStrip;

        //}

        //private DataGridViewCellEventArgs mouseLocation;

        //private UnityPackageMetaData AssetAtMouseLocation => FilteredAssets[mouseLocation.RowIndex];

        //private void toolStripItemHarvestAsset_Click(object sender, EventArgs args)
        //{
        //    if (string.IsNullOrWhiteSpace(harvestPath))
        //    {
        //        return;
        //    }

        //    var unityPackageMetaData = treeAssetContents.Tag as UnityPackageMetaData;
        //    if (unityPackageMetaData == null)
        //    {
        //        return;
        //    }

        //    var unityAsset = new UnityAssetProxy(unityPackageMetaData.UnityPackage, harvestPath);
        //    unityAsset.Harvest();
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

        // Deal with hovering over a cell.
        //private void results_CellMouseEnter(object sender,
        //    DataGridViewCellEventArgs location)
        //{
        //    mouseLocation = location;
        //}

        //private void results_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    Process.Start(AssetAtMouseLocation.UnityPackageFolder);
        //}

        //private void results_RowEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    ShowMetaData(FilteredAssets[e.RowIndex]);
        //}

    }
}
