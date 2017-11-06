namespace Pamux.GameDev.Tools.Tabs
{
    partial class AssetLibrary
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.results = new System.Windows.Forms.DataGridView();
            this.treeAssetContents = new System.Windows.Forms.TreeView();
            this.panelQuery = new System.Windows.Forms.Panel();
            this.textQuery = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.results)).BeginInit();
            this.panelQuery.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.78279F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.21721F));
            this.tableLayoutPanel.Controls.Add(this.results, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.treeAssetContents, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.panelQuery, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.800312F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.19969F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(976, 641);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // results
            // 
            this.results.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.results.Dock = System.Windows.Forms.DockStyle.Left;
            this.results.Location = new System.Drawing.Point(3, 53);
            this.results.Name = "results";
            this.results.Size = new System.Drawing.Size(597, 585);
            this.results.TabIndex = 12;
            this.results.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.results_CellContextMenuStripNeeded);
            this.results.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.results_CellMouseDoubleClick);
            this.results.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.results_CellMouseEnter);
            this.results.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.results_RowEnter);
            // 
            // treeAssetContents
            // 
            this.treeAssetContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeAssetContents.Location = new System.Drawing.Point(606, 53);
            this.treeAssetContents.Name = "treeAssetContents";
            this.treeAssetContents.Size = new System.Drawing.Size(367, 585);
            this.treeAssetContents.TabIndex = 11;
            this.treeAssetContents.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeAssetContents_NodeMouseClick);
            // 
            // panelQuery
            // 
            this.panelQuery.BackColor = System.Drawing.Color.Coral;
            this.tableLayoutPanel.SetColumnSpan(this.panelQuery, 2);
            this.panelQuery.Controls.Add(this.textQuery);
            this.panelQuery.Controls.Add(this.label1);
            this.panelQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelQuery.Location = new System.Drawing.Point(3, 3);
            this.panelQuery.Name = "panelQuery";
            this.panelQuery.Size = new System.Drawing.Size(970, 44);
            this.panelQuery.TabIndex = 10;
            // 
            // textQuery
            // 
            this.textQuery.Location = new System.Drawing.Point(54, 10);
            this.textQuery.Name = "textQuery";
            this.textQuery.Size = new System.Drawing.Size(575, 20);
            this.textQuery.TabIndex = 4;
            this.textQuery.TextChanged += new System.EventHandler(this.txtQuery_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Query";
            // 
            // AssetLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "AssetLibrary";
            this.Size = new System.Drawing.Size(976, 641);
            this.Load += new System.EventHandler(this.AssetLibrary_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.results)).EndInit();
            this.panelQuery.ResumeLayout(false);
            this.panelQuery.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panelQuery;
        private System.Windows.Forms.TextBox textQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView results;
        private System.Windows.Forms.TreeView treeAssetContents;
    }
}
