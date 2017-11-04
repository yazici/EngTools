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

    /// <summary>
    /// WindowsStore publishing and maintenance utilities
    /// </summary>
    public partial class AssetLibrary : UserControl
    {
        public static AssetLibrary INSTANCE;

        public static readonly IList<AssetPackage> AllAssets = new List<AssetPackage>();
        public static IList<AssetPackage> FilteredAssets { get; private set; }

        private static readonly IList<AssetPackage> FilteredAssets_A = new List<AssetPackage>();
        private static readonly IList<AssetPackage> FilteredAssets_B = new List<AssetPackage>();

        ToolStripMenuItem toolStripItemAssetStoreSearch = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItemOpenAssetFolder = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItemOpenAssetMetaDataFolder = new ToolStripMenuItem();
        ToolStripMenuItem toolStripItemViewAssetMetaData = new ToolStripMenuItem();
        
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();


        public AssetLibrary()
        {
            InitializeComponent();

            contextMenuStrip.Items.Add(toolStripItemAssetStoreSearch);
            contextMenuStrip.Items.Add("-");
            contextMenuStrip.Items.Add(toolStripItemOpenAssetFolder);
            contextMenuStrip.Items.Add(toolStripItemOpenAssetMetaDataFolder);
            contextMenuStrip.Items.Add("-");
            contextMenuStrip.Items.Add(toolStripItemViewAssetMetaData);
            

            toolStripItemAssetStoreSearch.Text = "Name search in Unity AssetStore";
            toolStripItemAssetStoreSearch.Click += new EventHandler(toolStripItemAssetStoreSearch_Click);

            toolStripItemOpenAssetFolder.Text = "Open Asset Folder in Explorer";
            toolStripItemOpenAssetFolder.Click += new EventHandler(toolStripItemOpenAssetFolder_Click);

            toolStripItemOpenAssetMetaDataFolder.Text = "Open Asset MetaData Folder in Explorer";
            toolStripItemOpenAssetMetaDataFolder.Click += new EventHandler(toolStripItemOpenAssetMetaDataFolder_Click);

            toolStripItemViewAssetMetaData.Text = "View  Asset MetaData";
            toolStripItemViewAssetMetaData.Click += new EventHandler(toolStripItemViewAssetMetaData_Click);
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


        private void UpdateAssetLibraryDatabase()
        {
            AllAssets.Clear();
            var rootFolderLength = Settings.Unity3DAssetsFolderPath.Length + 1;
            foreach (var companyPath in Directory.EnumerateDirectories(Settings.Unity3DAssetsFolderPath))
            {
                var companyFolder = companyPath.Substring(rootFolderLength);
                var companyPathLength = companyPath.Length + 1;
                foreach (var assetPath in Directory.EnumerateDirectories(companyPath))
                {
                    var assetFolder = assetPath.Substring(companyPathLength);
                    var assetPathLength = assetPath.Length + 1;
                    foreach (var assetFilePath in Directory.EnumerateFiles(assetPath, "*.unitypackage"))
                    {
                        var a = AssetPackage.CreateFromPath(assetFilePath,
                                                            Settings.Unity3DAssetsFolderPath,
                                                            companyFolder,
                                                            assetFolder, assetFilePath.Substring(assetPathLength));
                        AllAssets.Add(a);
                    }

                }
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
                UpdateAssetLibraryDatabase();
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
            var  query = new AssetQuery(textQuery.Text);
            foreach (AssetPackage a in AllAssets)
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

        public static string GetHTMLDiv(string className, string id, string content)
        {
            return GetHTMLTag("div", className, id, content);
        }

        public static string GetHTMLAnchor(string className, string id, string href, string image, string content)
        {
            string otherAttributes = string.Format(" href=\"{0}\"", href);

            return GetHTMLTag("a", className, id, otherAttributes, content);
        }
        public static string GetHTMLTag(string tag, string className, string id, string content)
        {
            return GetHTMLTag(tag, className, id, "", content);
        }
        public static string GetHTMLTag(string tag, string className, string id, string otherAttributes, string content)
        {
            return string.Format("<{0} class=\"{1}\" id=\"{2}\"{3}>{4}</{0}>", tag, className, id, otherAttributes, content);
        }

        private string GetHTMLHead()
        {

            return "<meta charset=\"UTF-8\" /><link href=\"local.css\" rel=\"stylesheet\" />";
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

        private void ShowMetaData(AssetPackage ap)
        {
            tvAssetContents.Nodes.Clear();
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
                            currentNode = mappedNodes[path] = tvAssetContents.Nodes.Add(assetParts[i]);
                        }
                        else
                        {
                            currentNode = mappedNodes[path] = currentNode.Nodes.Add(assetParts[i]);
                        }
                    }
                }
            }

            if (tvAssetContents.Nodes.Count > 0)
            {
                tvAssetContents.Nodes[0].Expand();
                foreach (TreeNode n in tvAssetContents.Nodes[0].Nodes)
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
                column.ContextMenuStrip = contextMenuStrip;
                
            }

            e.ContextMenuStrip = contextMenuStrip;

        }

        private DataGridViewCellEventArgs mouseLocation;

        private AssetPackage AssetAtMouseLocation => FilteredAssets[mouseLocation.RowIndex];
        
        
        // Change the cell's color.
        private void toolStripItemAssetStoreSearch_Click(object sender, EventArgs args)
        {
            DoAssetStoreSearch(AssetAtMouseLocation);
            
            //results.Rows[mouseLocation.RowIndex].Cells[mouseLocation.ColumnIndex]
        }

        private void toolStripItemOpenAssetMetaDataFolder_Click(object sender, EventArgs e)
        {
            Process.Start(AssetAtMouseLocation.AssetMetaDataFolder);
        }

        private void toolStripItemViewAssetMetaData_Click(object sender, EventArgs e)
        {
            Process.Start(AssetAtMouseLocation.AssetMetaDataPath);
        }

        private void toolStripItemOpenAssetFolder_Click(object sender, EventArgs e)
        {
            Process.Start(AssetAtMouseLocation.AssetFolder);
        }

        private void DoAssetStoreSearch(AssetPackage assetPackage)
        {
            var nameParts = assetPackage.name.Split(' ');
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
            Process.Start(AssetAtMouseLocation.AssetFolder);
        }

        private void results_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            ShowMetaData(FilteredAssets[e.RowIndex]);
        }

        
    }
}