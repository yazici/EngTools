namespace Pamux.GameDev.Tools.Tabs
{
    partial class SettingsControl
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
            this.txtVoiceSaveDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtVoiceSaveDirectory
            // 
            this.txtVoiceSaveDirectory.Location = new System.Drawing.Point(135, 28);
            this.txtVoiceSaveDirectory.Name = "txtVoiceSaveDirectory";
            this.txtVoiceSaveDirectory.Size = new System.Drawing.Size(288, 20);
            this.txtVoiceSaveDirectory.TabIndex = 0;
            this.txtVoiceSaveDirectory.Text = "D:\\Workspace\\Voice";
            this.txtVoiceSaveDirectory.TextChanged += new System.EventHandler(this.txtVoiceSaveDirectory_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Voice Save Directory";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtVoiceSaveDirectory);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(800, 535);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtVoiceSaveDirectory;
        private System.Windows.Forms.Label label1;
    }
}
