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
            this.gbLocalization = new System.Windows.Forms.GroupBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.cbCurrentLanguage = new System.Windows.Forms.ComboBox();
            this.lblCurrentLanguage = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tbServerVersion = new System.Windows.Forms.TextBox();
            this.lblServerVersion = new System.Windows.Forms.Label();
            this.tbCurrentVersion = new System.Windows.Forms.TextBox();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLocalizationRefreshTime = new System.Windows.Forms.ComboBox();
            this.cbLocalizationCheckNewVersions = new System.Windows.Forms.CheckBox();
            this.gbLocalization.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLocalization
            // 
            this.gbLocalization.Controls.Add(this.label1);
            this.gbLocalization.Controls.Add(this.cbLocalizationRefreshTime);
            this.gbLocalization.Controls.Add(this.cbLocalizationCheckNewVersions);
            this.gbLocalization.Controls.Add(this.btnInstall);
            this.gbLocalization.Controls.Add(this.cbCurrentLanguage);
            this.gbLocalization.Controls.Add(this.lblCurrentLanguage);
            this.gbLocalization.Controls.Add(this.btnRefresh);
            this.gbLocalization.Controls.Add(this.tbServerVersion);
            this.gbLocalization.Controls.Add(this.lblServerVersion);
            this.gbLocalization.Controls.Add(this.tbCurrentVersion);
            this.gbLocalization.Controls.Add(this.lblCurrentVersion);
            this.gbLocalization.Location = new System.Drawing.Point(12, 12);
            this.gbLocalization.Name = "gbLocalization";
            this.gbLocalization.Size = new System.Drawing.Size(480, 164);
            this.gbLocalization.TabIndex = 2;
            this.gbLocalization.TabStop = false;
            this.gbLocalization.Text = "Локализация";
            // 
            // btnInstall
            // 
            this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstall.Location = new System.Drawing.Point(10, 69);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(464, 23);
            this.btnInstall.TabIndex = 3;
            this.btnInstall.Text = "Установить последнюю версию";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // cbCurrentLanguage
            // 
            this.cbCurrentLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCurrentLanguage.BackColor = System.Drawing.SystemColors.Info;
            this.cbCurrentLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrentLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCurrentLanguage.FormattingEnabled = true;
            this.cbCurrentLanguage.Location = new System.Drawing.Point(118, 104);
            this.cbCurrentLanguage.Name = "cbCurrentLanguage";
            this.cbCurrentLanguage.Size = new System.Drawing.Size(147, 21);
            this.cbCurrentLanguage.TabIndex = 6;
            this.cbCurrentLanguage.SelectedIndexChanged += new System.EventHandler(this.cbCurrentLanguage_SelectedIndexChanged);
            // 
            // lblCurrentLanguage
            // 
            this.lblCurrentLanguage.AutoSize = true;
            this.lblCurrentLanguage.Location = new System.Drawing.Point(7, 107);
            this.lblCurrentLanguage.Name = "lblCurrentLanguage";
            this.lblCurrentLanguage.Size = new System.Drawing.Size(84, 13);
            this.lblCurrentLanguage.TabIndex = 5;
            this.lblCurrentLanguage.Text = "Текущий язык:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(407, 17);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(67, 46);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tbServerVersion
            // 
            this.tbServerVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbServerVersion.Location = new System.Drawing.Point(118, 43);
            this.tbServerVersion.Name = "tbServerVersion";
            this.tbServerVersion.ReadOnly = true;
            this.tbServerVersion.Size = new System.Drawing.Size(283, 20);
            this.tbServerVersion.TabIndex = 3;
            // 
            // lblServerVersion
            // 
            this.lblServerVersion.AutoSize = true;
            this.lblServerVersion.Location = new System.Drawing.Point(7, 46);
            this.lblServerVersion.Name = "lblServerVersion";
            this.lblServerVersion.Size = new System.Drawing.Size(105, 13);
            this.lblServerVersion.TabIndex = 2;
            this.lblServerVersion.Text = "Последняя версия:";
            // 
            // tbCurrentVersion
            // 
            this.tbCurrentVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCurrentVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCurrentVersion.Location = new System.Drawing.Point(118, 17);
            this.tbCurrentVersion.Name = "tbCurrentVersion";
            this.tbCurrentVersion.ReadOnly = true;
            this.tbCurrentVersion.Size = new System.Drawing.Size(283, 20);
            this.tbCurrentVersion.TabIndex = 1;
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Location = new System.Drawing.Point(7, 20);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(94, 13);
            this.lblCurrentVersion.TabIndex = 0;
            this.lblCurrentVersion.Text = "Текущая версия:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "минут ";
            // 
            // cbLocalizationRefreshTime
            // 
            this.cbLocalizationRefreshTime.BackColor = System.Drawing.SystemColors.Info;
            this.cbLocalizationRefreshTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocalizationRefreshTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLocalizationRefreshTime.FormattingEnabled = true;
            this.cbLocalizationRefreshTime.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "30",
            "60"});
            this.cbLocalizationRefreshTime.Location = new System.Drawing.Point(219, 131);
            this.cbLocalizationRefreshTime.Name = "cbLocalizationRefreshTime";
            this.cbLocalizationRefreshTime.Size = new System.Drawing.Size(46, 21);
            this.cbLocalizationRefreshTime.TabIndex = 8;
            this.cbLocalizationRefreshTime.SelectedIndexChanged += new System.EventHandler(this.cbLocalizationRefreshTime_SelectedIndexChanged);
            // 
            // cbLocalizationCheckNewVersions
            // 
            this.cbLocalizationCheckNewVersions.AutoSize = true;
            this.cbLocalizationCheckNewVersions.Location = new System.Drawing.Point(10, 133);
            this.cbLocalizationCheckNewVersions.Name = "cbLocalizationCheckNewVersions";
            this.cbLocalizationCheckNewVersions.Size = new System.Drawing.Size(203, 17);
            this.cbLocalizationCheckNewVersions.TabIndex = 7;
            this.cbLocalizationCheckNewVersions.Text = "Автоматически проверять каждые";
            this.cbLocalizationCheckNewVersions.UseVisualStyleBackColor = true;
            this.cbLocalizationCheckNewVersions.CheckedChanged += new System.EventHandler(this.cbLocalizationCheckNewVersions_CheckedChanged);
            // 
            // LocalizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 185);
            this.Controls.Add(this.gbLocalization);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocalizationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Star Citizen : Локализация";
            this.gbLocalization.ResumeLayout(false);
            this.gbLocalization.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbLocalization;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox tbServerVersion;
        private System.Windows.Forms.Label lblServerVersion;
        private System.Windows.Forms.TextBox tbCurrentVersion;
        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Label lblCurrentLanguage;
        private System.Windows.Forms.ComboBox cbCurrentLanguage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLocalizationRefreshTime;
        private System.Windows.Forms.CheckBox cbLocalizationCheckNewVersions;
    }
}