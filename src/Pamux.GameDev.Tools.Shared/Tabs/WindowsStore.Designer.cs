namespace Pamux.GameDev.Tools.Tabs
{
    partial class WindowsStore
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
            this.buttonCreateScaledImages = new System.Windows.Forms.Button();
            this.buttonSaveScreenshotFromClipboard = new System.Windows.Forms.Button();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.buttonPeriodicScreenSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCreateScaledImages
            // 
            this.buttonCreateScaledImages.Location = new System.Drawing.Point(28, 22);
            this.buttonCreateScaledImages.Name = "buttonCreateScaledImages";
            this.buttonCreateScaledImages.Size = new System.Drawing.Size(283, 23);
            this.buttonCreateScaledImages.TabIndex = 0;
            this.buttonCreateScaledImages.Text = "Create Scaled Windows Store Image Assets";
            this.buttonCreateScaledImages.UseVisualStyleBackColor = true;
            this.buttonCreateScaledImages.Click += new System.EventHandler(this.ButtonCreateScaledImagesClick);
            // 
            // buttonSaveScreenshotFromClipboard
            // 
            this.buttonSaveScreenshotFromClipboard.Location = new System.Drawing.Point(28, 62);
            this.buttonSaveScreenshotFromClipboard.Name = "buttonSaveScreenshotFromClipboard";
            this.buttonSaveScreenshotFromClipboard.Size = new System.Drawing.Size(283, 23);
            this.buttonSaveScreenshotFromClipboard.TabIndex = 1;
            this.buttonSaveScreenshotFromClipboard.Text = "Save ScreenShot from my Clipboard";
            this.buttonSaveScreenshotFromClipboard.UseVisualStyleBackColor = true;
            this.buttonSaveScreenshotFromClipboard.Click += new System.EventHandler(this.ButtonSaveScreenshotFromClipboardClick);
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(155, 257);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(537, 20);
            this.textMessage.TabIndex = 2;
            // 
            // buttonPeriodicScreenSave
            // 
            this.buttonPeriodicScreenSave.Location = new System.Drawing.Point(28, 104);
            this.buttonPeriodicScreenSave.Name = "buttonPeriodicScreenSave";
            this.buttonPeriodicScreenSave.Size = new System.Drawing.Size(283, 23);
            this.buttonPeriodicScreenSave.TabIndex = 3;
            this.buttonPeriodicScreenSave.Text = "Save ScreenShot Periodically";
            this.buttonPeriodicScreenSave.UseVisualStyleBackColor = true;
            this.buttonPeriodicScreenSave.Click += new System.EventHandler(this.ButtonPeriodicScreenSaveClick);
            // 
            // WindowsStore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonPeriodicScreenSave);
            this.Controls.Add(this.textMessage);
            this.Controls.Add(this.buttonSaveScreenshotFromClipboard);
            this.Controls.Add(this.buttonCreateScaledImages);
            this.Name = "WindowsStore";
            this.Size = new System.Drawing.Size(976, 641);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCreateScaledImages;
        private System.Windows.Forms.Button buttonSaveScreenshotFromClipboard;
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.Button buttonPeriodicScreenSave;
    }
}
