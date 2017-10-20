namespace Pamux.GameDev.Tools
{
    using Pamux.GameDev.Tools.Tabs;

    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabHome = new System.Windows.Forms.TabPage();
            this.home = new Pamux.GameDev.Tools.Tabs.Home();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.settings = new Pamux.GameDev.Tools.Tabs.Settings();
            this.tabWindowsStore = new System.Windows.Forms.TabPage();
            this.windowsStore1 = new Pamux.GameDev.Tools.Tabs.WindowsStore();
            this.tabMain.SuspendLayout();
            this.tabHome.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabWindowsStore.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabHome);
            this.tabMain.Controls.Add(this.tabWindowsStore);
            this.tabMain.Controls.Add(this.tabSettings);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(854, 500);
            this.tabMain.TabIndex = 0;
            // 
            // tabHome
            // 
            this.tabHome.Controls.Add(this.home);
            this.tabHome.Location = new System.Drawing.Point(4, 22);
            this.tabHome.Name = "tabHome";
            this.tabHome.Padding = new System.Windows.Forms.Padding(3);
            this.tabHome.Size = new System.Drawing.Size(846, 474);
            this.tabHome.TabIndex = 0;
            this.tabHome.Text = "Home";
            this.tabHome.UseVisualStyleBackColor = true;
            // 
            // home
            // 
            this.home.Dock = System.Windows.Forms.DockStyle.Fill;
            this.home.Location = new System.Drawing.Point(3, 3);
            this.home.Name = "home";
            this.home.Size = new System.Drawing.Size(840, 468);
            this.home.TabIndex = 0;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.settings);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(846, 474);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // settings
            // 
            this.settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settings.Location = new System.Drawing.Point(3, 3);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(840, 468);
            this.settings.TabIndex = 0;
            // 
            // tabWindowsStore
            // 
            this.tabWindowsStore.Controls.Add(this.windowsStore1);
            this.tabWindowsStore.Location = new System.Drawing.Point(4, 22);
            this.tabWindowsStore.Name = "tabWindowsStore";
            this.tabWindowsStore.Padding = new System.Windows.Forms.Padding(3);
            this.tabWindowsStore.Size = new System.Drawing.Size(846, 474);
            this.tabWindowsStore.TabIndex = 2;
            this.tabWindowsStore.Text = "Windows Store";
            this.tabWindowsStore.UseVisualStyleBackColor = true;
            // 
            // windowsStore1
            // 
            this.windowsStore1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.windowsStore1.Location = new System.Drawing.Point(3, 3);
            this.windowsStore1.Name = "windowsStore1";
            this.windowsStore1.Size = new System.Drawing.Size(840, 468);
            this.windowsStore1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 500);
            this.Controls.Add(this.tabMain);
            this.Name = "MainForm";
            this.Text = "Pamux GameDev Tools";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabMain.ResumeLayout(false);
            this.tabHome.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabWindowsStore.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabHome;
        private System.Windows.Forms.TabPage tabSettings;
        private Home home;
        private Settings settings;
        private System.Windows.Forms.TabPage tabWindowsStore;
        private WindowsStore windowsStore1;
    }
}