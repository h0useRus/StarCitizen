using NSW.StarCitizen.Tools.Lib.Helpers;

namespace NSW.StarCitizen.Tools.Forms
{
    partial class GameSettingsForm
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
            if (disposing)
            {
                components?.Dispose();
                DisposableUtils.Dispose(_settingControls);
                _settingControls = null;
                var profilesSource = cbProfiles.DataSource;
                cbProfiles.DataSource = null;
                DisposableUtils.Dispose(profilesSource);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSettingsForm));
            this.cmGameSetting = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miResetSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopySetting = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopyAllSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miChangedOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.btnResetPage = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.btnNewProfile = new System.Windows.Forms.Button();
            this.btnRenameProfile = new System.Windows.Forms.Button();
            this.btnDeleteProfile = new System.Windows.Forms.Button();
            this.lblProfile = new System.Windows.Forms.Label();
            this.lblSettingsDbRepoUrl = new System.Windows.Forms.LinkLabel();
            this.lblReportIssues = new System.Windows.Forms.Label();
            this.tabCategories = new NSW.StarCitizen.Tools.Controls.TabControlEx();
            this.cmGameSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmGameSetting
            // 
            this.cmGameSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miResetSetting,
            this.miCopySetting,
            this.miCopyAllSettings,
            this.toolStripSeparator2,
            this.miChangedOnly});
            this.cmGameSetting.Name = "cmGameSetting";
            this.cmGameSetting.Size = new System.Drawing.Size(156, 98);
            this.cmGameSetting.Opened += new System.EventHandler(this.cmGameSetting_Opened);
            // 
            // miResetSetting
            // 
            this.miResetSetting.Name = "miResetSetting";
            this.miResetSetting.Size = new System.Drawing.Size(155, 22);
            this.miResetSetting.Text = "Reset Setting";
            this.miResetSetting.Click += new System.EventHandler(this.miResetSetting_Click);
            // 
            // miCopySetting
            // 
            this.miCopySetting.Name = "miCopySetting";
            this.miCopySetting.Size = new System.Drawing.Size(155, 22);
            this.miCopySetting.Text = "Copy Setting";
            this.miCopySetting.Click += new System.EventHandler(this.miCopySetting_Click);
            // 
            // miCopyAllSettings
            // 
            this.miCopyAllSettings.Name = "miCopyAllSettings";
            this.miCopyAllSettings.Size = new System.Drawing.Size(155, 22);
            this.miCopyAllSettings.Text = "Copy All Settings";
            this.miCopyAllSettings.Click += new System.EventHandler(this.miCopyAllSettings_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(152, 6);
            // 
            // miChangedOnly
            // 
            this.miChangedOnly.CheckOnClick = true;
            this.miChangedOnly.Name = "miChangedOnly";
            this.miChangedOnly.Size = new System.Drawing.Size(155, 22);
            this.miChangedOnly.Text = "Changed Only";
            this.miChangedOnly.CheckedChanged += new System.EventHandler(this.miChangedOnly_CheckedChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(660, 511);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnResetAll
            // 
            this.btnResetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetAll.Enabled = false;
            this.btnResetAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetAll.Location = new System.Drawing.Point(4, 511);
            this.btnResetAll.Name = "btnResetAll";
            this.btnResetAll.Size = new System.Drawing.Size(115, 23);
            this.btnResetAll.TabIndex = 2;
            this.btnResetAll.Text = "Reset All";
            this.btnResetAll.UseVisualStyleBackColor = true;
            this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);
            // 
            // btnResetPage
            // 
            this.btnResetPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetPage.Enabled = false;
            this.btnResetPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetPage.Location = new System.Drawing.Point(125, 511);
            this.btnResetPage.Name = "btnResetPage";
            this.btnResetPage.Size = new System.Drawing.Size(115, 23);
            this.btnResetPage.TabIndex = 3;
            this.btnResetPage.Text = "Reset at Page";
            this.btnResetPage.UseVisualStyleBackColor = true;
            this.btnResetPage.Click += new System.EventHandler(this.btnResetPage_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
            // 
            // cbProfiles
            // 
            this.cbProfiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbProfiles.FormattingEnabled = true;
            this.cbProfiles.Location = new System.Drawing.Point(125, 8);
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new System.Drawing.Size(236, 21);
            this.cbProfiles.TabIndex = 4;
            this.cbProfiles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbProfiles_DrawItem);
            this.cbProfiles.SelectionChangeCommitted += new System.EventHandler(this.cbProfiles_SelectionChangeCommitted);
            // 
            // btnNewProfile
            // 
            this.btnNewProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewProfile.Location = new System.Drawing.Point(4, 35);
            this.btnNewProfile.Name = "btnNewProfile";
            this.btnNewProfile.Size = new System.Drawing.Size(115, 23);
            this.btnNewProfile.TabIndex = 5;
            this.btnNewProfile.Text = "New...";
            this.btnNewProfile.UseVisualStyleBackColor = true;
            this.btnNewProfile.Click += new System.EventHandler(this.btnNewProfile_Click);
            // 
            // btnRenameProfile
            // 
            this.btnRenameProfile.Enabled = false;
            this.btnRenameProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameProfile.Location = new System.Drawing.Point(125, 35);
            this.btnRenameProfile.Name = "btnRenameProfile";
            this.btnRenameProfile.Size = new System.Drawing.Size(115, 23);
            this.btnRenameProfile.TabIndex = 6;
            this.btnRenameProfile.Text = "Rename...";
            this.btnRenameProfile.UseVisualStyleBackColor = true;
            this.btnRenameProfile.Click += new System.EventHandler(this.btnRenameProfile_Click);
            // 
            // btnDeleteProfile
            // 
            this.btnDeleteProfile.Enabled = false;
            this.btnDeleteProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteProfile.Location = new System.Drawing.Point(246, 35);
            this.btnDeleteProfile.Name = "btnDeleteProfile";
            this.btnDeleteProfile.Size = new System.Drawing.Size(115, 23);
            this.btnDeleteProfile.TabIndex = 7;
            this.btnDeleteProfile.Text = "Delete";
            this.btnDeleteProfile.UseVisualStyleBackColor = true;
            this.btnDeleteProfile.Click += new System.EventHandler(this.btnDeleteProfile_Click);
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblProfile.Location = new System.Drawing.Point(3, 14);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(45, 15);
            this.lblProfile.TabIndex = 8;
            this.lblProfile.Text = "Profile:";
            // 
            // lblSettingsDbRepoUrl
            // 
            this.lblSettingsDbRepoUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSettingsDbRepoUrl.Location = new System.Drawing.Point(367, 40);
            this.lblSettingsDbRepoUrl.Name = "lblSettingsDbRepoUrl";
            this.lblSettingsDbRepoUrl.Size = new System.Drawing.Size(408, 13);
            this.lblSettingsDbRepoUrl.TabIndex = 9;
            this.lblSettingsDbRepoUrl.TabStop = true;
            this.lblSettingsDbRepoUrl.Text = "repoUrl";
            this.lblSettingsDbRepoUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSettingsDbRepoUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblSettingsDbRepoUrl_LinkClicked);
            // 
            // lblReportIssues
            // 
            this.lblReportIssues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReportIssues.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblReportIssues.Location = new System.Drawing.Point(363, 8);
            this.lblReportIssues.Name = "lblReportIssues";
            this.lblReportIssues.Size = new System.Drawing.Size(408, 32);
            this.lblReportIssues.TabIndex = 10;
            this.lblReportIssues.Text = "report";
            this.lblReportIssues.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabCategories
            // 
            this.tabCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCategories.ContextMenuStrip = this.cmGameSetting;
            this.tabCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabCategories.ItemSize = new System.Drawing.Size(0, 20);
            this.tabCategories.Location = new System.Drawing.Point(0, 71);
            this.tabCategories.Name = "tabCategories";
            this.tabCategories.Padding = new System.Drawing.Point(3, 3);
            this.tabCategories.SelectedIndex = 0;
            this.tabCategories.Size = new System.Drawing.Size(779, 434);
            this.tabCategories.TabIndex = 0;
            // 
            // GameSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 540);
            this.Controls.Add(this.lblReportIssues);
            this.Controls.Add(this.lblSettingsDbRepoUrl);
            this.Controls.Add(this.lblProfile);
            this.Controls.Add(this.btnDeleteProfile);
            this.Controls.Add(this.btnRenameProfile);
            this.Controls.Add(this.btnNewProfile);
            this.Controls.Add(this.cbProfiles);
            this.Controls.Add(this.btnResetPage);
            this.Controls.Add(this.btnResetAll);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabCategories);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(375, 200);
            this.Name = "GameSettingsForm";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GameSettingsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameSettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.GameSettingsForm_Load);
            this.cmGameSetting.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NSW.StarCitizen.Tools.Controls.TabControlEx tabCategories;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.Button btnResetPage;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip cmGameSetting;
        private System.Windows.Forms.ToolStripMenuItem miChangedOnly;
        private System.Windows.Forms.ToolStripMenuItem miResetSetting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miCopySetting;
        private System.Windows.Forms.ToolStripMenuItem miCopyAllSettings;
        private System.Windows.Forms.ComboBox cbProfiles;
        private System.Windows.Forms.Button btnNewProfile;
        private System.Windows.Forms.Button btnRenameProfile;
        private System.Windows.Forms.Button btnDeleteProfile;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.LinkLabel lblSettingsDbRepoUrl;
        private System.Windows.Forms.Label lblReportIssues;

        public object DispoableUtils { get; private set; }
    }
}