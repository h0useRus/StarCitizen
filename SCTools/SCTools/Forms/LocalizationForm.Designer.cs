using NSW.StarCitizen.Tools.Properties;

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
            this.label1 = new System.Windows.Forms.Label();
            this.cbLocalizationRefreshTime = new System.Windows.Forms.ComboBox();
            this.cbLocalizationCheckNewVersions = new System.Windows.Forms.CheckBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.cbCurrentLanguage = new System.Windows.Forms.ComboBox();
            this.lblCurrentLanguage = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblServerVersion = new System.Windows.Forms.Label();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.cbLocalizationVersions = new System.Windows.Forms.ComboBox();
            this.cbRepository = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(270, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "minutes";
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
            this.cbLocalizationRefreshTime.Location = new System.Drawing.Point(218, 126);
            this.cbLocalizationRefreshTime.Name = "cbLocalizationRefreshTime";
            this.cbLocalizationRefreshTime.Size = new System.Drawing.Size(46, 21);
            this.cbLocalizationRefreshTime.TabIndex = 8;
            this.cbLocalizationRefreshTime.SelectedIndexChanged += new System.EventHandler(this.cbLocalizationRefreshTime_SelectedIndexChanged);
            // 
            // cbLocalizationCheckNewVersions
            // 
            this.cbLocalizationCheckNewVersions.AutoSize = true;
            this.cbLocalizationCheckNewVersions.Location = new System.Drawing.Point(9, 128);
            this.cbLocalizationCheckNewVersions.Name = "cbLocalizationCheckNewVersions";
            this.cbLocalizationCheckNewVersions.Size = new System.Drawing.Size(150, 17);
            this.cbLocalizationCheckNewVersions.TabIndex = 7;
            this.cbLocalizationCheckNewVersions.Text = global::NSW.StarCitizen.Tools.Properties.Resources.Localization_AutomaticCheck_Text;
            this.cbLocalizationCheckNewVersions.UseVisualStyleBackColor = true;
            this.cbLocalizationCheckNewVersions.CheckedChanged += new System.EventHandler(this.cbLocalizationCheckNewVersions_CheckedChanged);
            // 
            // btnInstall
            // 
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstall.Location = new System.Drawing.Point(9, 64);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(464, 23);
            this.btnInstall.TabIndex = 3;
            this.btnInstall.Text = global::NSW.StarCitizen.Tools.Properties.Resources.Localization_InstallVersion_Text;
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // cbCurrentLanguage
            // 
            this.cbCurrentLanguage.BackColor = System.Drawing.SystemColors.Info;
            this.cbCurrentLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrentLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCurrentLanguage.FormattingEnabled = true;
            this.cbCurrentLanguage.Location = new System.Drawing.Point(117, 99);
            this.cbCurrentLanguage.Name = "cbCurrentLanguage";
            this.cbCurrentLanguage.Size = new System.Drawing.Size(147, 21);
            this.cbCurrentLanguage.TabIndex = 6;
            this.cbCurrentLanguage.SelectedIndexChanged += new System.EventHandler(this.cbCurrentLanguage_SelectedIndexChanged);
            // 
            // lblCurrentLanguage
            // 
            this.lblCurrentLanguage.AutoSize = true;
            this.lblCurrentLanguage.Location = new System.Drawing.Point(6, 102);
            this.lblCurrentLanguage.Name = "lblCurrentLanguage";
            this.lblCurrentLanguage.Size = new System.Drawing.Size(91, 13);
            this.lblCurrentLanguage.TabIndex = 5;
            this.lblCurrentLanguage.Text = "Current language:";
            // 
            // btnRefresh
            // 
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(406, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(67, 46);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = global::NSW.StarCitizen.Tools.Properties.Resources.Localization_Refresh_Text;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblServerVersion
            // 
            this.lblServerVersion.AutoSize = true;
            this.lblServerVersion.Location = new System.Drawing.Point(6, 41);
            this.lblServerVersion.Name = "lblServerVersion";
            this.lblServerVersion.Size = new System.Drawing.Size(90, 13);
            this.lblServerVersion.TabIndex = 2;
            this.lblServerVersion.Text = "Selected Version:";
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
            // cbLocalizationVersions
            // 
            this.cbLocalizationVersions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocalizationVersions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLocalizationVersions.FormattingEnabled = true;
            this.cbLocalizationVersions.Location = new System.Drawing.Point(117, 37);
            this.cbLocalizationVersions.Name = "cbLocalizationVersions";
            this.cbLocalizationVersions.Size = new System.Drawing.Size(283, 21);
            this.cbLocalizationVersions.TabIndex = 10;
            // 
            // cbRepository
            // 
            this.cbRepository.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRepository.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRepository.FormattingEnabled = true;
            this.cbRepository.Location = new System.Drawing.Point(117, 12);
            this.cbRepository.Name = "cbRepository";
            this.cbRepository.Size = new System.Drawing.Size(283, 21);
            this.cbRepository.TabIndex = 11;
            // 
            // LocalizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 157);
            this.Controls.Add(this.cbRepository);
            this.Controls.Add(this.cbLocalizationVersions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLocalizationRefreshTime);
            this.Controls.Add(this.cbLocalizationCheckNewVersions);
            this.Controls.Add(this.lblCurrentVersion);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.lblServerVersion);
            this.Controls.Add(this.cbCurrentLanguage);
            this.Controls.Add(this.lblCurrentLanguage);
            this.Controls.Add(this.btnRefresh);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocalizationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Star Citizen : Localization";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblServerVersion;
        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Label lblCurrentLanguage;
        private System.Windows.Forms.ComboBox cbCurrentLanguage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLocalizationRefreshTime;
        private System.Windows.Forms.CheckBox cbLocalizationCheckNewVersions;
        private System.Windows.Forms.ComboBox cbLocalizationVersions;
        private System.Windows.Forms.ComboBox cbRepository;
    }
}