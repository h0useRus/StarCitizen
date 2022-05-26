
namespace NSW.StarCitizen.Tools.Forms
{
    partial class LauncherForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherForm));
            this.btnRunGame = new System.Windows.Forms.Button();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.btnImportProfile = new System.Windows.Forms.Button();
            this.btnRemoveProfile = new System.Windows.Forms.Button();
            this.lblProfile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRunGame
            // 
            this.btnRunGame.FlatAppearance.BorderSize = 0;
            this.btnRunGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunGame.ImageKey = "start";
            this.btnRunGame.ImageList = this.imgList;
            this.btnRunGame.Location = new System.Drawing.Point(464, 19);
            this.btnRunGame.Name = "btnRunGame";
            this.btnRunGame.Size = new System.Drawing.Size(28, 31);
            this.btnRunGame.TabIndex = 1;
            this.btnRunGame.UseVisualStyleBackColor = true;
            this.btnRunGame.Click += new System.EventHandler(this.btnRunGame_Click);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "start");
            this.imgList.Images.SetKeyName(1, "stop");
            // 
            // cbProfiles
            // 
            this.cbProfiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfiles.FormattingEnabled = true;
            this.cbProfiles.Location = new System.Drawing.Point(12, 25);
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new System.Drawing.Size(446, 21);
            this.cbProfiles.TabIndex = 0;
            this.cbProfiles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbProfiles_DrawItem);
            this.cbProfiles.SelectedIndexChanged += new System.EventHandler(this.cbProfiles_SelectedIndexChanged);
            // 
            // btnImportProfile
            // 
            this.btnImportProfile.Location = new System.Drawing.Point(12, 52);
            this.btnImportProfile.Name = "btnImportProfile";
            this.btnImportProfile.Size = new System.Drawing.Size(175, 23);
            this.btnImportProfile.TabIndex = 2;
            this.btnImportProfile.Text = "Import Current";
            this.btnImportProfile.UseVisualStyleBackColor = true;
            this.btnImportProfile.Click += new System.EventHandler(this.btnImportCurrentProfile_Click);
            // 
            // btnRemoveProfile
            // 
            this.btnRemoveProfile.Location = new System.Drawing.Point(193, 52);
            this.btnRemoveProfile.Name = "btnRemoveProfile";
            this.btnRemoveProfile.Size = new System.Drawing.Size(127, 23);
            this.btnRemoveProfile.TabIndex = 3;
            this.btnRemoveProfile.Text = "Delete";
            this.btnRemoveProfile.UseVisualStyleBackColor = true;
            this.btnRemoveProfile.Click += new System.EventHandler(this.btnRemoveProfile_Click);
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Location = new System.Drawing.Point(12, 9);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(70, 13);
            this.lblProfile.TabIndex = 5;
            this.lblProfile.Text = "Game Profile:";
            // 
            // LauncherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 86);
            this.Controls.Add(this.lblProfile);
            this.Controls.Add(this.btnRemoveProfile);
            this.Controls.Add(this.btnImportProfile);
            this.Controls.Add(this.cbProfiles);
            this.Controls.Add(this.btnRunGame);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LauncherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Launcher";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LauncherForm_FormClosed);
            this.Load += new System.EventHandler(this.LauncherForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunGame;
        private System.Windows.Forms.ComboBox cbProfiles;
        private System.Windows.Forms.Button btnImportProfile;
        private System.Windows.Forms.Button btnRemoveProfile;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.ImageList imgList;
    }
}