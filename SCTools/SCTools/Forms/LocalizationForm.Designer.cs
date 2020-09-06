namespace NSW.StarCitizen.Tools.Forms
{
    partial class LocalizationForm
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
            this.lblMinutes = new System.Windows.Forms.Label();
            this.cbRefreshTime = new System.Windows.Forms.ComboBox();
            this.cbCheckNewVersions = new System.Windows.Forms.CheckBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.cbLanguages = new System.Windows.Forms.ComboBox();
            this.lblCurrentLanguage = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblServerVersion = new System.Windows.Forms.Label();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.cbVersions = new System.Windows.Forms.ComboBox();
            this.cbRepository = new System.Windows.Forms.ComboBox();
            this.btnLocalizationDisable = new System.Windows.Forms.Button();
            this.lblSelectedVersion = new System.Windows.Forms.Label();
            this.tbCurrentVersion = new System.Windows.Forms.TextBox();
            this.btnManage = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMinutes
            // 
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Location = new System.Drawing.Point(270, 178);
            this.lblMinutes.Name = "lblMinutes";
            this.lblMinutes.Size = new System.Drawing.Size(43, 13);
            this.lblMinutes.TabIndex = 9;
            this.lblMinutes.Text = "minutes";
            // 
            // cbRefreshTime
            // 
            this.cbRefreshTime.BackColor = System.Drawing.SystemColors.Info;
            this.cbRefreshTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRefreshTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRefreshTime.FormattingEnabled = true;
            this.cbRefreshTime.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "30",
            "60"});
            this.cbRefreshTime.Location = new System.Drawing.Point(218, 175);
            this.cbRefreshTime.Name = "cbRefreshTime";
            this.cbRefreshTime.Size = new System.Drawing.Size(46, 21);
            this.cbRefreshTime.TabIndex = 10;
            this.cbRefreshTime.SelectedIndexChanged += new System.EventHandler(this.cbRefreshTime_SelectedIndexChanged);
            // 
            // cbCheckNewVersions
            // 
            this.cbCheckNewVersions.AutoSize = true;
            this.cbCheckNewVersions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCheckNewVersions.Location = new System.Drawing.Point(9, 177);
            this.cbCheckNewVersions.Name = "cbCheckNewVersions";
            this.cbCheckNewVersions.Size = new System.Drawing.Size(158, 17);
            this.cbCheckNewVersions.TabIndex = 9;
            this.cbCheckNewVersions.Text = "Check for new version every";
            this.cbCheckNewVersions.UseVisualStyleBackColor = true;
            this.cbCheckNewVersions.CheckedChanged += new System.EventHandler(this.cbCheckNewVersions_CheckedChanged);
            // 
            // btnInstall
            // 
            this.btnInstall.Enabled = false;
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstall.Location = new System.Drawing.Point(9, 92);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(469, 23);
            this.btnInstall.TabIndex = 6;
            this.btnInstall.Text = "Install selected version";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // cbLanguages
            // 
            this.cbLanguages.BackColor = System.Drawing.SystemColors.Info;
            this.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLanguages.FormattingEnabled = true;
            this.cbLanguages.Location = new System.Drawing.Point(117, 148);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(147, 21);
            this.cbLanguages.TabIndex = 7;
            this.cbLanguages.SelectionChangeCommitted += new System.EventHandler(this.cbLanguages_SelectionChangeCommitted);
            // 
            // lblCurrentLanguage
            // 
            this.lblCurrentLanguage.AutoSize = true;
            this.lblCurrentLanguage.Location = new System.Drawing.Point(6, 151);
            this.lblCurrentLanguage.Name = "lblCurrentLanguage";
            this.lblCurrentLanguage.Size = new System.Drawing.Size(91, 13);
            this.lblCurrentLanguage.TabIndex = 5;
            this.lblCurrentLanguage.Text = "Current language:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(406, 39);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(72, 47);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblServerVersion
            // 
            this.lblServerVersion.AutoSize = true;
            this.lblServerVersion.Location = new System.Drawing.Point(6, 68);
            this.lblServerVersion.Name = "lblServerVersion";
            this.lblServerVersion.Size = new System.Drawing.Size(95, 13);
            this.lblServerVersion.TabIndex = 2;
            this.lblServerVersion.Text = "Available versions:";
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Location = new System.Drawing.Point(6, 15);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(92, 13);
            this.lblCurrentVersion.TabIndex = 0;
            this.lblCurrentVersion.Text = "Source repository:";
            // 
            // cbVersions
            // 
            this.cbVersions.BackColor = System.Drawing.SystemColors.Info;
            this.cbVersions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVersions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbVersions.FormattingEnabled = true;
            this.cbVersions.Location = new System.Drawing.Point(117, 65);
            this.cbVersions.Name = "cbVersions";
            this.cbVersions.Size = new System.Drawing.Size(283, 21);
            this.cbVersions.TabIndex = 4;
            this.cbVersions.SelectionChangeCommitted += new System.EventHandler(this.cbVersions_SelectionChangeCommitted);
            // 
            // cbRepository
            // 
            this.cbRepository.BackColor = System.Drawing.SystemColors.Info;
            this.cbRepository.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbRepository.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRepository.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRepository.FormattingEnabled = true;
            this.cbRepository.Location = new System.Drawing.Point(117, 12);
            this.cbRepository.Name = "cbRepository";
            this.cbRepository.Size = new System.Drawing.Size(283, 21);
            this.cbRepository.TabIndex = 1;
            this.cbRepository.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbRepository_DrawItem);
            this.cbRepository.SelectionChangeCommitted += new System.EventHandler(this.cbRepository_SelectionChangeCommitted);
            // 
            // btnLocalizationDisable
            // 
            this.btnLocalizationDisable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocalizationDisable.Location = new System.Drawing.Point(273, 146);
            this.btnLocalizationDisable.Name = "btnLocalizationDisable";
            this.btnLocalizationDisable.Size = new System.Drawing.Size(205, 23);
            this.btnLocalizationDisable.TabIndex = 8;
            this.btnLocalizationDisable.Text = "Disable localization support";
            this.btnLocalizationDisable.UseVisualStyleBackColor = true;
            this.btnLocalizationDisable.Click += new System.EventHandler(this.btnLocalizationDisable_Click);
            // 
            // lblSelectedVersion
            // 
            this.lblSelectedVersion.AutoSize = true;
            this.lblSelectedVersion.Location = new System.Drawing.Point(6, 42);
            this.lblSelectedVersion.Name = "lblSelectedVersion";
            this.lblSelectedVersion.Size = new System.Drawing.Size(81, 13);
            this.lblSelectedVersion.TabIndex = 13;
            this.lblSelectedVersion.Text = "Current version:";
            // 
            // tbCurrentVersion
            // 
            this.tbCurrentVersion.BackColor = System.Drawing.SystemColors.Info;
            this.tbCurrentVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCurrentVersion.Location = new System.Drawing.Point(117, 39);
            this.tbCurrentVersion.Name = "tbCurrentVersion";
            this.tbCurrentVersion.ReadOnly = true;
            this.tbCurrentVersion.Size = new System.Drawing.Size(283, 20);
            this.tbCurrentVersion.TabIndex = 3;
            // 
            // btnManage
            // 
            this.btnManage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManage.Location = new System.Drawing.Point(406, 10);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(72, 23);
            this.btnManage.TabIndex = 2;
            this.btnManage.Text = "Manage";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            // 
            // btnUninstall
            // 
            this.btnUninstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUninstall.Location = new System.Drawing.Point(9, 92);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(469, 23);
            this.btnUninstall.TabIndex = 6;
            this.btnUninstall.Text = "Uninstall localization";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Visible = false;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // LocalizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 203);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.tbCurrentVersion);
            this.Controls.Add(this.lblSelectedVersion);
            this.Controls.Add(this.btnLocalizationDisable);
            this.Controls.Add(this.cbRepository);
            this.Controls.Add(this.cbVersions);
            this.Controls.Add(this.lblMinutes);
            this.Controls.Add(this.cbRefreshTime);
            this.Controls.Add(this.cbCheckNewVersions);
            this.Controls.Add(this.lblCurrentVersion);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.lblServerVersion);
            this.Controls.Add(this.cbLanguages);
            this.Controls.Add(this.lblCurrentLanguage);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnUninstall);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocalizationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Star Citizen : Localization";
            this.Load += new System.EventHandler(this.LocalizationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblServerVersion;
        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Label lblCurrentLanguage;
        private System.Windows.Forms.ComboBox cbLanguages;
        private System.Windows.Forms.Label lblMinutes;
        private System.Windows.Forms.ComboBox cbRefreshTime;
        private System.Windows.Forms.CheckBox cbCheckNewVersions;
        private System.Windows.Forms.ComboBox cbVersions;
        private System.Windows.Forms.ComboBox cbRepository;
        private System.Windows.Forms.Button btnLocalizationDisable;
        private System.Windows.Forms.Label lblSelectedVersion;
        private System.Windows.Forms.TextBox tbCurrentVersion;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.Button btnUninstall;
    }
}