namespace NSW.StarCitizen.Tools.Forms
{
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tbGamePath = new System.Windows.Forms.TextBox();
            this.cbGameModes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // tbGamePath
            // 
            this.tbGamePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGamePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGamePath.Location = new System.Drawing.Point(12, 12);
            this.tbGamePath.Name = "tbGamePath";
            this.tbGamePath.ReadOnly = true;
            this.tbGamePath.Size = new System.Drawing.Size(454, 21);
            this.tbGamePath.TabIndex = 1;
            this.tbGamePath.Text = "Нажмите здесь, чтобы выбрать путь до папки Star Citizen";
            this.tbGamePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbGamePath.Click += new System.EventHandler(this.btnGamePath_Click);
            // 
            // cbGameModes
            // 
            this.cbGameModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGameModes.FormattingEnabled = true;
            this.cbGameModes.Location = new System.Drawing.Point(473, 12);
            this.cbGameModes.Name = "cbGameModes";
            this.cbGameModes.Size = new System.Drawing.Size(73, 21);
            this.cbGameModes.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 298);
            this.Controls.Add(this.cbGameModes);
            this.Controls.Add(this.tbGamePath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbGamePath;
        private System.Windows.Forms.ComboBox cbGameModes;
    }
}