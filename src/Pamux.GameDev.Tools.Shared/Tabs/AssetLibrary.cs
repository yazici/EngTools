// ------------------------------------------------------------------------------------------------
// <copyright file="WindowsStore.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace Pamux.GameDev.Tools.Tabs
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Pamux.GameDev.Tools.Models;
    using System.Diagnostics;
    using System.Drawing;
    using System.ComponentModel;
    using Pamux.GameDev.Tools.AssetUtils;
    using Pamux.GameDev.Lib.Extensions;

    /// <summary>
    /// WindowsStore publishing and maintenance utilities
    /// </summary>
    public partial class AssetLibrary : UserControl
    {
        public static AssetLibrary INSTANCE;

        private TreeNode treeAssetContents_ContextMenuNode;
        private string harvestPath;

        public static readonly IList<AssetMetaData> AllAssets = new List<AssetMetaData>();
        public static IList<AssetMetaData> FilteredAssets { get; private set; }

        private static readonly IList<AssetMetaData> FilteredAssets_A = new List<AssetMetaData>();
        private static readonly IList<AssetMetaData> FilteredAssets_B = new List<AssetMetaData>();

        ToolStripMenuItem toolStripItemAssetStoreSearch = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItemOpenAssetFolder = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItemOpenAssetMetaDataFolder = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItemViewAssetMetaData = new ToolStripMenuItem();

        ToolStripMenuItem toolStripItemHarvestAsset = new ToolStripMenuItem();

        ContextMenuStrip resultsContextMenuStrip;
        ContextMenuStrip treeAssetContentsContextMenuStrip;

        public AssetLibrary()
        {
            InitializeComponent();

            treeAssetContentsContextMenuStrip = new ContextMenuStrip();
            treeAssetContentsContextMenuStrip.Opening += new CancelEventHandler(treeAssetContentsContextMenuStrip_Opening);
            toolStripItemHarvestAsset.Click += new EventHandler(toolStripItemHarvestAsset_Click);


            resultsContextMenuStrip = new ContextMenuStrip();
            toolStripItemAssetStoreSearch.Text = "Name search in Unity AssetStore";
            toolStripItemAssetStoreSearch.Click += new EventHandler(toolStripItemAssetStoreSearch_Click);

            toolStripItemOpenAssetFolder.Text = "Open Asset Folder in Explorer";
            toolStripItemOpenAssetFolder.Click += new EventHandler(toolStripItemOpenAssetFolder_Click);

            toolStripItemOpenAssetMetaDataFolder.Text = "Open Asset MetaData Folder in Explorer";
            toolStripItemOpenAssetMetaDataFolder.Click += new EventHandler(toolStripItemOpenAssetMetaDataFolder_Click);

            toolStripItemViewAssetMetaData.Text = "View  Asset MetaData";
            toolStripItemViewAssetMetaData.Click += new EventHandler(toolStripItemViewAssetMetaData_Click);

            resultsContextMenuStrip.Items.Add(toolStripItemAssetStoreSearch);
            resultsContextMenuStrip.Items.Add("-");
            resultsContextMenuStrip.Items.Add(toolStripItemOpenAssetFolder);
            resultsContextMenuStrip.Items.Add(toolStripItemOpenAssetMetaDataFolder);
            resultsContextMenuStrip.Items.Add("-");
            resultsContextMenuStrip.Items.Add(toolStripItemViewAssetMetaData);
        }

        private void treeAssetContents_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeAssetContents_ContextMenuNode = treeAssetContents.GetNodeAt(e.X, e.Y);
        }

        private void treeAssetContentsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            harvestPath = null;
            treeAssetContentsContextMenuStrip.Items.Clear();

            if (treeAssetContents_ContextMenuNode == null)
            {
                if (treeAssetContents == null || treeAssetContents.SelectedNode == null)
                {
                    e.Cancel = true;
                    return;
                }

                treeAssetContents_ContextMenuNode = treeAssetContents.SelectedNode;
            }

            try
            {
                if (treeAssetContents_ContextMenuNode.Nodes.Count != 0)
                {
                    e.Cancel = true;
                    return;
                }

                treeAssetContents.SelectedNode = treeAssetContents_ContextMenuNode;
                
               
                toolStripItemHarvestAsset.Text = $"Harvest Asset: {treeAssetContents_ContextMenuNode.Text}";
                harvestPath = treeAssetContents_ContextMenuNode.FullPath;
                treeAssetContentsContextMenuStrip.Items.Add(toolStripItemHarvestAsset);
                e.Cancel = false;
            }
            finally
            {
                treeAssetContents_ContextMenuNode = null;
            }
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

        private void LoadGDOData(string dbName, CheckedListBox.ObjectCollection items)
        {
            foreach (var aLine in File.ReadAllLines(GetGDODataPath(dbName)))
            {
                if (string.IsNullOrWhiteSpace(aLine))
                {
                    continue;
                }
                var line = aLine.Trim();
                if (line.StartsWith("#"))
                {
                    continue;
                }
                items.Add(line);
            }
        }

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
                    AllAssets.Add(new AssetMetaData
                    {
                        FullPath = unityPackageFileFullPath,
                        Company = nameStack[0],
                        AssetSubFolder = nameStack[1],
                    });
                }
            }

            if (nameStack.Count > 0)
            { 
                nameStack.RemoveAt(nameStack.Count - 1);
            }
        }

        static bool isFirstTime = true;

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

            if (results.DataSource == FilteredAssets_B)
            {
                FilteredAssets = FilteredAssets_A;
            }
            else
            {
                FilteredAssets = FilteredAssets_B;
            }

            FilteredAssets.Clear();
            var query = new AssetQuery(textQuery.Text);
            foreach (AssetMetaData a in AllAssets)
            {
                if (a.Match(query))
                {
                    FilteredAssets.Add(a);
                }
            }
            results.DataSource = FilteredAssets;

            if (isFirstTime)
            {
                isFirstTime = false;
                results.Columns[0].MinimumWidth = 300;
                results.Columns[1].MinimumWidth = 250;
                results.Columns[2].MinimumWidth = 250;
            }

            File.WriteAllText(Settings.LocalHtmlPath, GenerateHTML());
            // browser.Url = Settings.LocalHtmlUri;
        }

        private string GenerateHTML()
        {
            return string.Format("<!DOCTYPE html><html lang=\"en\" dir=\"ltr\" class=\"client-nojs\"><head>{0}</head><body>{1}</body></html>", GetHTMLHead(), GetHTMLBody());
        }

        private string GetHTMLBody()
        {

            return GetHTMLDiv("search-results", "multi-line-results-search-results", GetAllSearchResults());
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

        private void ShowMetaData(AssetMetaData ap)
        {
            treeAssetContents.Tag = ap;
            treeAssetContents.Nodes.Clear();
            var mappedNodes = new Dictionary<string, TreeNode>();

            foreach (string asset in ap.Assets)
            {
                var assetParts = asset.Split('/');
                var path = "";

                TreeNode currentNode = null;
                for (int i = 0; i < assetParts.Length; ++i)
                {
                    path += $"{assetParts[i]}/";


                    if (mappedNodes.ContainsKey(path))
                    {
                        currentNode = mappedNodes[path];
                    }
                    else
                    {
                        if (currentNode == null)
                        {
                            currentNode = mappedNodes[path] = treeAssetContents.Nodes.Add(assetParts[i]);
                        }
                        else
                        {
                            currentNode = mappedNodes[path] = currentNode.Nodes.Add(assetParts[i]);
                        }

                        currentNode.ContextMenuStrip = treeAssetContentsContextMenuStrip;
                    }


                }
            }

            if (treeAssetContents.Nodes.Count > 0)
            {
                treeAssetContents.Nodes[0].Expand();
                foreach (TreeNode n in treeAssetContents.Nodes[0].Nodes)
                {
                    n.Expand();
                }
            }
        }

        private void lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Process.Start(e.Link.LinkData as string);
        }


        private void results_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            foreach (DataGridViewColumn column in results.Columns)
            {
                column.ContextMenuStrip = resultsContextMenuStrip;
            }

            e.ContextMenuStrip = resultsContextMenuStrip;

        }

        private DataGridViewCellEventArgs mouseLocation;

        private AssetMetaData AssetAtMouseLocation => FilteredAssets[mouseLocation.RowIndex];

        private void toolStripItemHarvestAsset_Click(object sender, EventArgs args)
        {
            if (string.IsNullOrWhiteSpace(harvestPath))
            {
                return;
            }

            var assetMetaData = treeAssetContents.Tag as AssetMetaData;
            if (assetMetaData == null)
            {
                return;
            }

            var unityAsset = new UnityAssetProxy(assetMetaData.UnityPackage, harvestPath);
            unityAsset.Harvest();
        }

        private void toolStripItemAssetStoreSearch_Click(object sender, EventArgs args)
        {
            DoAssetStoreSearch(AssetAtMouseLocation);

            //results.Rows[mouseLocation.RowIndex].Cells[mouseLocation.ColumnIndex]
        }

        private void toolStripItemOpenAssetMetaDataFolder_Click(object sender, EventArgs e)
        {
            Process.Start(AssetAtMouseLocation.MetaDataFolder);
        }

        private void toolStripItemViewAssetMetaData_Click(object sender, EventArgs e)
        {
            Process.Start(AssetAtMouseLocation.MetaDataPath);
        }

        private void toolStripItemOpenAssetFolder_Click(object sender, EventArgs e)
        {
            Process.Start(AssetAtMouseLocation.UnityPackageFolder);
        }

        private void DoAssetStoreSearch(AssetMetaData assetPackage)
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
        private void results_CellMouseEnter(object sender,
            DataGridViewCellEventArgs location)
        {
            mouseLocation = location;
        }

        private void results_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Process.Start(AssetAtMouseLocation.UnityPackageFolder);
        }

        private void results_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            ShowMetaData(FilteredAssets[e.RowIndex]);
        }


    }
}