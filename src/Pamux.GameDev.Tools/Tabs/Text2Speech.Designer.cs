namespace Pamux.GameDev.Tools.Tabs
{
    partial class Text2Speech
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
            this.textMessage = new System.Windows.Forms.TextBox();
            this.buttonSpeak = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(54, 30);
            this.textMessage.Name = "textMessage";
            this.textMessage.Size = new System.Drawing.Size(537, 20);
            this.textMessage.TabIndex = 2;
            // 
            // buttonSpeak
            // 
            this.buttonSpeak.Location = new System.Drawing.Point(598, 26);
            this.buttonSpeak.Name = "buttonSpeak";
            this.buttonSpeak.Size = new System.Drawing.Size(75, 23);
            this.buttonSpeak.TabIndex = 3;
            this.buttonSpeak.Text = "&Speak";
            this.buttonSpeak.UseVisualStyleBackColor = true;
            this.buttonSpeak.Click += new System.EventHandler(this.ButtonSpeakClick);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(598, 55);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // Text2Speech
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonSpeak);
            this.Controls.Add(this.textMessage);
            this.Name = "Text2Speech";
            this.Size = new System.Drawing.Size(976, 641);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.Button buttonSpeak;
        private System.Windows.Forms.Button buttonSave;
    }
}
