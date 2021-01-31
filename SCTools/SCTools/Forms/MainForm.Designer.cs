using NSW.StarCitizen.Tools.Properties;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tbGamePath = new System.Windows.Forms.TextBox();
            this.cbGameModes = new System.Windows.Forms.ComboBox();
            this.gbGameInfo = new System.Windows.Forms.GroupBox();
            this.tbGameVersion = new System.Windows.Forms.TextBox();
            this.lblGameVersion = new System.Windows.Forms.Label();
            this.tbGameMode = new System.Windows.Forms.TextBox();
            this.lblGameMode = new System.Windows.Forms.Label();
            this.gbButtonMenu = new System.Windows.Forms.GroupBox();
            this.btnUpdateLocalization = new System.Windows.Forms.Button();
            this.btnLocalization = new System.Windows.Forms.Button();
            this.btnAppUpdate = new System.Windows.Forms.Button();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.cbLanguage = new System.Windows.Forms.ComboBox();
            this.niTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmTrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAppName = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.miRunOnStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.miRunMinimized = new System.Windows.Forms.ToolStripMenuItem();
            this.miRunTopMost = new System.Windows.Forms.ToolStripMenuItem();
            this.miUseHttpProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.cbMenuLanguage = new System.Windows.Forms.ToolStripComboBox();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miMoveLiveToPtu = new System.Windows.Forms.ToolStripMenuItem();
            this.miMovePtuToLive = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExitApp = new System.Windows.Forms.ToolStripMenuItem();
            this.cbGeneralRunWithWindows = new System.Windows.Forms.CheckBox();
            this.cbGeneralRunMinimized = new System.Windows.Forms.CheckBox();
            this.lblMinutes = new System.Windows.Forms.Label();
            this.cbRefreshTime = new System.Windows.Forms.ComboBox();
            this.cbCheckNewVersions = new System.Windows.Forms.CheckBox();
            this.gbApplicationUpdate = new System.Windows.Forms.GroupBox();
            this.gbGameInfo.SuspendLayout();
            this.gbButtonMenu.SuspendLayout();
            this.cmTrayMenu.SuspendLayout();
            this.gbApplicationUpdate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbGamePath
            // 
            this.tbGamePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGamePath.BackColor = System.Drawing.SystemColors.Info;
            this.tbGamePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGamePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGamePath.Location = new System.Drawing.Point(12, 12);
            this.tbGamePath.Name = "tbGamePath";
            this.tbGamePath.ReadOnly = true;
            this.tbGamePath.Size = new System.Drawing.Size(380, 21);
            this.tbGamePath.TabIndex = 1;
            this.tbGamePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbGamePath.WordWrap = false;
            this.tbGamePath.Click += new System.EventHandler(this.btnGamePath_Click);
            // 
            // cbGameModes
            // 
            this.cbGameModes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbGameModes.BackColor = System.Drawing.SystemColors.Info;
            this.cbGameModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGameModes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbGameModes.FormattingEnabled = true;
            this.cbGameModes.Location = new System.Drawing.Point(398, 12);
            this.cbGameModes.Name = "cbGameModes";
            this.cbGameModes.Size = new System.Drawing.Size(74, 21);
            this.cbGameModes.TabIndex = 2;
            this.cbGameModes.SelectionChangeCommitted += new System.EventHandler(this.cbGameModes_SelectionChangeCommitted);
            // 
            // gbGameInfo
            // 
            this.gbGameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGameInfo.Controls.Add(this.tbGameVersion);
            this.gbGameInfo.Controls.Add(this.lblGameVersion);
            this.gbGameInfo.Controls.Add(this.tbGameMode);
            this.gbGameInfo.Controls.Add(this.lblGameMode);
            this.gbGameInfo.Location = new System.Drawing.Point(12, 39);
            this.gbGameInfo.Name = "gbGameInfo";
            this.gbGameInfo.Size = new System.Drawing.Size(200, 108);
            this.gbGameInfo.TabIndex = 3;
            this.gbGameInfo.TabStop = false;
            // 
            // tbGameVersion
            // 
            this.tbGameVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGameVersion.BackColor = System.Drawing.SystemColors.Window;
            this.tbGameVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGameVersion.Location = new System.Drawing.Point(9, 72);
            this.tbGameVersion.Name = "tbGameVersion";
            this.tbGameVersion.ReadOnly = true;
            this.tbGameVersion.Size = new System.Drawing.Size(185, 20);
            this.tbGameVersion.TabIndex = 3;
            this.tbGameVersion.TabStop = false;
            // 
            // lblGameVersion
            // 
            this.lblGameVersion.AutoSize = true;
            this.lblGameVersion.Location = new System.Drawing.Point(6, 56);
            this.lblGameVersion.Name = "lblGameVersion";
            this.lblGameVersion.Size = new System.Drawing.Size(72, 13);
            this.lblGameVersion.TabIndex = 2;
            this.lblGameVersion.Text = "Game version";
            // 
            // tbGameMode
            // 
            this.tbGameMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGameMode.BackColor = System.Drawing.SystemColors.Window;
            this.tbGameMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGameMode.Location = new System.Drawing.Point(9, 33);
            this.tbGameMode.Name = "tbGameMode";
            this.tbGameMode.ReadOnly = true;
            this.tbGameMode.Size = new System.Drawing.Size(185, 20);
            this.tbGameMode.TabIndex = 1;
            this.tbGameMode.TabStop = false;
            // 
            // lblGameMode
            // 
            this.lblGameMode.AutoSize = true;
            this.lblGameMode.Location = new System.Drawing.Point(6, 16);
            this.lblGameMode.Name = "lblGameMode";
            this.lblGameMode.Size = new System.Drawing.Size(64, 13);
            this.lblGameMode.TabIndex = 0;
            this.lblGameMode.Text = "Game mode";
            // 
            // gbButtonMenu
            // 
            this.gbButtonMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbButtonMenu.Controls.Add(this.btnUpdateLocalization);
            this.gbButtonMenu.Controls.Add(this.btnLocalization);
            this.gbButtonMenu.Location = new System.Drawing.Point(218, 39);
            this.gbButtonMenu.Name = "gbButtonMenu";
            this.gbButtonMenu.Size = new System.Drawing.Size(254, 108);
            this.gbButtonMenu.TabIndex = 4;
            this.gbButtonMenu.TabStop = false;
            // 
            // btnUpdateLocalization
            // 
            this.btnUpdateLocalization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateLocalization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateLocalization.Location = new System.Drawing.Point(6, 72);
            this.btnUpdateLocalization.Name = "btnUpdateLocalization";
            this.btnUpdateLocalization.Size = new System.Drawing.Size(242, 23);
            this.btnUpdateLocalization.TabIndex = 4;
            this.btnUpdateLocalization.Text = "Check for update...";
            this.btnUpdateLocalization.UseVisualStyleBackColor = true;
            this.btnUpdateLocalization.Click += new System.EventHandler(this.btnUpdateLocalization_Click);
            // 
            // btnLocalization
            // 
            this.btnLocalization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocalization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocalization.Location = new System.Drawing.Point(6, 30);
            this.btnLocalization.Name = "btnLocalization";
            this.btnLocalization.Size = new System.Drawing.Size(242, 23);
            this.btnLocalization.TabIndex = 3;
            this.btnLocalization.Text = "Localization";
            this.btnLocalization.UseVisualStyleBackColor = true;
            this.btnLocalization.Click += new System.EventHandler(this.btnLocalization_Click);
            // 
            // btnAppUpdate
            // 
            this.btnAppUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAppUpdate.Location = new System.Drawing.Point(9, 19);
            this.btnAppUpdate.Name = "btnAppUpdate";
            this.btnAppUpdate.Size = new System.Drawing.Size(445, 23);
            this.btnAppUpdate.TabIndex = 4;
            this.btnAppUpdate.Text = "Check for application updates";
            this.btnAppUpdate.UseVisualStyleBackColor = true;
            this.btnAppUpdate.Click += new System.EventHandler(this.btnAppUpdate_Click);
            // 
            // lblLanguage
            // 
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(221, 246);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(55, 13);
            this.lblLanguage.TabIndex = 3;
            this.lblLanguage.Text = "Language";
            // 
            // cbLanguage
            // 
            this.cbLanguage.BackColor = System.Drawing.SystemColors.Info;
            this.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLanguage.FormattingEnabled = true;
            this.cbLanguage.Location = new System.Drawing.Point(224, 265);
            this.cbLanguage.Name = "cbLanguage";
            this.cbLanguage.Size = new System.Drawing.Size(248, 21);
            this.cbLanguage.TabIndex = 9;
            this.cbLanguage.SelectionChangeCommitted += new System.EventHandler(this.cbLanguage_SelectionChangeCommitted);
            // 
            // niTray
            // 
            this.niTray.ContextMenuStrip = this.cmTrayMenu;
            this.niTray.Icon = ((System.Drawing.Icon)(resources.GetObject("niTray.Icon")));
            this.niTray.Visible = true;
            this.niTray.BalloonTipClicked += new System.EventHandler(this.niTray_BalloonTipClicked);
            this.niTray.MouseClick += new System.Windows.Forms.MouseEventHandler(this.niTray_MouseClick);
            // 
            // cmTrayMenu
            // 
            this.cmTrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAppName,
            this.tsSeparator2,
            this.miTools,
            this.miSettings,
            this.tsSeparator1,
            this.miExitApp});
            this.cmTrayMenu.Name = "cmTrayMenu";
            this.cmTrayMenu.Size = new System.Drawing.Size(181, 126);
            this.cmTrayMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmTrayMenu_Opening);
            // 
            // miAppName
            // 
            this.miAppName.Image = ((System.Drawing.Image)(resources.GetObject("miAppName.Image")));
            this.miAppName.Name = "miAppName";
            this.miAppName.Size = new System.Drawing.Size(180, 22);
            this.miAppName.Text = "SCTools";
            this.miAppName.Click += new System.EventHandler(this.miAppName_Click);
            // 
            // tsSeparator2
            // 
            this.tsSeparator2.Name = "tsSeparator2";
            this.tsSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // miSettings
            // 
            this.miSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRunOnStartup,
            this.miRunMinimized,
            this.miRunTopMost,
            this.miUseHttpProxy,
            this.cbMenuLanguage});
            this.miSettings.Name = "miSettings";
            this.miSettings.Size = new System.Drawing.Size(180, 22);
            this.miSettings.Text = "Settings";
            // 
            // miRunOnStartup
            // 
            this.miRunOnStartup.CheckOnClick = true;
            this.miRunOnStartup.Name = "miRunOnStartup";
            this.miRunOnStartup.Size = new System.Drawing.Size(194, 22);
            this.miRunOnStartup.Text = "Run On Windows startup";
            this.miRunOnStartup.Click += new System.EventHandler(this.miRunOnStartup_Click);
            // 
            // miRunMinimized
            // 
            this.miRunMinimized.CheckOnClick = true;
            this.miRunMinimized.Name = "miRunMinimized";
            this.miRunMinimized.Size = new System.Drawing.Size(194, 22);
            this.miRunMinimized.Text = "Run minimized";
            this.miRunMinimized.Click += new System.EventHandler(this.miRunMinimized_Click);
            // 
            // miRunTopMost
            // 
            this.miRunTopMost.CheckOnClick = true;
            this.miRunTopMost.Name = "miRunTopMost";
            this.miRunTopMost.Size = new System.Drawing.Size(194, 22);
            this.miRunTopMost.Text = "Always on top";
            this.miRunTopMost.Click += new System.EventHandler(this.miRunTopMost_Click);
            // 
            // miUseHttpProxy
            // 
            this.miUseHttpProxy.CheckOnClick = true;
            this.miUseHttpProxy.Name = "miUseHttpProxy";
            this.miUseHttpProxy.Size = new System.Drawing.Size(194, 22);
            this.miUseHttpProxy.Text = "Use Http Proxy";
            this.miUseHttpProxy.Click += new System.EventHandler(this.miUseHttpProxy_Click);
            // 
            // cbMenuLanguage
            // 
            this.cbMenuLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMenuLanguage.Name = "cbMenuLanguage";
            this.cbMenuLanguage.Size = new System.Drawing.Size(121, 21);
            this.cbMenuLanguage.SelectedIndexChanged += new System.EventHandler(this.cbMenuLanguage_SelectedIndexChanged);
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMoveLiveToPtu,
            this.miMovePtuToLive});
            this.miTools.Name = "miTools";
            this.miTools.Size = new System.Drawing.Size(180, 22);
            this.miTools.Text = "Tools";
            // 
            // miMoveLiveToPtu
            // 
            this.miMoveLiveToPtu.Name = "miMoveLiveToPtu";
            this.miMoveLiveToPtu.Size = new System.Drawing.Size(180, 22);
            this.miMoveLiveToPtu.Text = "Move LIVE to PTU";
            this.miMoveLiveToPtu.Click += new System.EventHandler(this.miMoveLiveToPtu_Click);
            // 
            // miMovePtuToLive
            // 
            this.miMovePtuToLive.Name = "miMovePtuToLive";
            this.miMovePtuToLive.Size = new System.Drawing.Size(180, 22);
            this.miMovePtuToLive.Text = "Move PTU to LIVE";
            this.miMovePtuToLive.Click += new System.EventHandler(this.miMovePtuToLive_Click);
            // 
            // tsSeparator1
            // 
            this.tsSeparator1.Name = "tsSeparator1";
            this.tsSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // miExitApp
            // 
            this.miExitApp.Name = "miExitApp";
            this.miExitApp.Size = new System.Drawing.Size(180, 22);
            this.miExitApp.Text = "Quit";
            this.miExitApp.Click += new System.EventHandler(this.miExitApp_Click);
            // 
            // cbGeneralRunWithWindows
            // 
            this.cbGeneralRunWithWindows.AutoSize = true;
            this.cbGeneralRunWithWindows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbGeneralRunWithWindows.Location = new System.Drawing.Point(12, 244);
            this.cbGeneralRunWithWindows.Name = "cbGeneralRunWithWindows";
            this.cbGeneralRunWithWindows.Size = new System.Drawing.Size(142, 17);
            this.cbGeneralRunWithWindows.TabIndex = 7;
            this.cbGeneralRunWithWindows.Text = "Run On Windows startup";
            this.cbGeneralRunWithWindows.UseVisualStyleBackColor = true;
            this.cbGeneralRunWithWindows.CheckedChanged += new System.EventHandler(this.cbGeneralRunWithWindows_CheckedChanged);
            // 
            // cbGeneralRunMinimized
            // 
            this.cbGeneralRunMinimized.AutoSize = true;
            this.cbGeneralRunMinimized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbGeneralRunMinimized.Location = new System.Drawing.Point(12, 269);
            this.cbGeneralRunMinimized.Name = "cbGeneralRunMinimized";
            this.cbGeneralRunMinimized.Size = new System.Drawing.Size(91, 17);
            this.cbGeneralRunMinimized.TabIndex = 8;
            this.cbGeneralRunMinimized.Text = "Run minimized";
            this.cbGeneralRunMinimized.UseVisualStyleBackColor = true;
            this.cbGeneralRunMinimized.CheckedChanged += new System.EventHandler(this.cbGeneralRunMinimized_CheckedChanged);
            // 
            // lblMinutes
            // 
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Location = new System.Drawing.Point(264, 60);
            this.lblMinutes.Name = "lblMinutes";
            this.lblMinutes.Size = new System.Drawing.Size(43, 13);
            this.lblMinutes.TabIndex = 11;
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
            this.cbRefreshTime.Location = new System.Drawing.Point(212, 52);
            this.cbRefreshTime.Name = "cbRefreshTime";
            this.cbRefreshTime.Size = new System.Drawing.Size(46, 21);
            this.cbRefreshTime.TabIndex = 6;
            this.cbRefreshTime.SelectionChangeCommitted += new System.EventHandler(this.cbRefreshTime_SelectionChangeCommitted);
            // 
            // cbCheckNewVersions
            // 
            this.cbCheckNewVersions.AutoSize = true;
            this.cbCheckNewVersions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCheckNewVersions.Location = new System.Drawing.Point(9, 56);
            this.cbCheckNewVersions.Name = "cbCheckNewVersions";
            this.cbCheckNewVersions.Size = new System.Drawing.Size(158, 17);
            this.cbCheckNewVersions.TabIndex = 5;
            this.cbCheckNewVersions.Text = "Check for new version every";
            this.cbCheckNewVersions.UseVisualStyleBackColor = true;
            this.cbCheckNewVersions.CheckedChanged += new System.EventHandler(this.cbCheckNewVersions_CheckedChanged);
            // 
            // gbApplicationUpdate
            // 
            this.gbApplicationUpdate.Controls.Add(this.btnAppUpdate);
            this.gbApplicationUpdate.Controls.Add(this.lblMinutes);
            this.gbApplicationUpdate.Controls.Add(this.cbCheckNewVersions);
            this.gbApplicationUpdate.Controls.Add(this.cbRefreshTime);
            this.gbApplicationUpdate.Location = new System.Drawing.Point(12, 153);
            this.gbApplicationUpdate.Name = "gbApplicationUpdate";
            this.gbApplicationUpdate.Size = new System.Drawing.Size(460, 85);
            this.gbApplicationUpdate.TabIndex = 5;
            this.gbApplicationUpdate.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 298);
            this.Controls.Add(this.gbApplicationUpdate);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.cbGeneralRunMinimized);
            this.Controls.Add(this.cbLanguage);
            this.Controls.Add(this.cbGeneralRunWithWindows);
            this.Controls.Add(this.gbButtonMenu);
            this.Controls.Add(this.gbGameInfo);
            this.Controls.Add(this.cbGameModes);
            this.Controls.Add(this.tbGamePath);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Star Citizen : Utils";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.gbGameInfo.ResumeLayout(false);
            this.gbGameInfo.PerformLayout();
            this.gbButtonMenu.ResumeLayout(false);
            this.cmTrayMenu.ResumeLayout(false);
            this.gbApplicationUpdate.ResumeLayout(false);
            this.gbApplicationUpdate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbGamePath;
        private System.Windows.Forms.ComboBox cbGameModes;
        private System.Windows.Forms.GroupBox gbGameInfo;
        private System.Windows.Forms.TextBox tbGameMode;
        private System.Windows.Forms.Label lblGameMode;
        private System.Windows.Forms.TextBox tbGameVersion;
        private System.Windows.Forms.Label lblGameVersion;
        private System.Windows.Forms.GroupBox gbButtonMenu;
        private System.Windows.Forms.Button btnLocalization;
        private System.Windows.Forms.NotifyIcon niTray;
        private System.Windows.Forms.CheckBox cbGeneralRunWithWindows;
        private System.Windows.Forms.CheckBox cbGeneralRunMinimized;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cbLanguage;
        private System.Windows.Forms.Button btnAppUpdate;
        private System.Windows.Forms.Label lblMinutes;
        private System.Windows.Forms.ComboBox cbRefreshTime;
        private System.Windows.Forms.CheckBox cbCheckNewVersions;
        private System.Windows.Forms.GroupBox gbApplicationUpdate;
        private System.Windows.Forms.ContextMenuStrip cmTrayMenu;
        private System.Windows.Forms.ToolStripMenuItem miExitApp;
        private System.Windows.Forms.ToolStripSeparator tsSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripMenuItem miRunMinimized;
        private System.Windows.Forms.ToolStripMenuItem miRunOnStartup;
        private System.Windows.Forms.ToolStripSeparator tsSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miAppName;
        private System.Windows.Forms.ToolStripComboBox cbMenuLanguage;
        private System.Windows.Forms.ToolStripMenuItem miRunTopMost;
        private System.Windows.Forms.ToolStripMenuItem miUseHttpProxy;
        private System.Windows.Forms.Button btnUpdateLocalization;
        private System.Windows.Forms.ToolStripMenuItem miTools;
        private System.Windows.Forms.ToolStripMenuItem miMoveLiveToPtu;
        private System.Windows.Forms.ToolStripMenuItem miMovePtuToLive;
    }
}