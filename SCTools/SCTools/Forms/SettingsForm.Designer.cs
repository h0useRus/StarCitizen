namespace NSW.StarCitizen.Tools.Forms
{
    partial class SettingsForm
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
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.cbGeneralRunWithWindows = new System.Windows.Forms.CheckBox();
            this.cbGeneralRunMinimized = new System.Windows.Forms.CheckBox();
            this.cbGeneralCloseToTray = new System.Windows.Forms.CheckBox();
            this.gbLocalization = new System.Windows.Forms.GroupBox();
            this.cbLocalizationCheckNewVersions = new System.Windows.Forms.CheckBox();
            this.cbLocalizationAutoDownload = new System.Windows.Forms.CheckBox();
            this.cbLocalizationRefreshTime = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbGeneral.SuspendLayout();
            this.gbLocalization.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbGeneral
            // 
            this.gbGeneral.Controls.Add(this.cbGeneralCloseToTray);
            this.gbGeneral.Controls.Add(this.cbGeneralRunMinimized);
            this.gbGeneral.Controls.Add(this.cbGeneralRunWithWindows);
            this.gbGeneral.Location = new System.Drawing.Point(12, 12);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Size = new System.Drawing.Size(507, 90);
            this.gbGeneral.TabIndex = 0;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "Основные";
            // 
            // cbGeneralRunWithWindows
            // 
            this.cbGeneralRunWithWindows.AutoSize = true;
            this.cbGeneralRunWithWindows.Location = new System.Drawing.Point(6, 42);
            this.cbGeneralRunWithWindows.Name = "cbGeneralRunWithWindows";
            this.cbGeneralRunWithWindows.Size = new System.Drawing.Size(196, 17);
            this.cbGeneralRunWithWindows.TabIndex = 0;
            this.cbGeneralRunWithWindows.Text = "Запускать при загрузке Windows";
            this.cbGeneralRunWithWindows.UseVisualStyleBackColor = true;
            // 
            // cbGeneralRunMinimized
            // 
            this.cbGeneralRunMinimized.AutoSize = true;
            this.cbGeneralRunMinimized.Location = new System.Drawing.Point(6, 65);
            this.cbGeneralRunMinimized.Name = "cbGeneralRunMinimized";
            this.cbGeneralRunMinimized.Size = new System.Drawing.Size(128, 17);
            this.cbGeneralRunMinimized.TabIndex = 1;
            this.cbGeneralRunMinimized.Text = "Запускать свернуто";
            this.cbGeneralRunMinimized.UseVisualStyleBackColor = true;
            // 
            // cbGeneralCloseToTray
            // 
            this.cbGeneralCloseToTray.AutoSize = true;
            this.cbGeneralCloseToTray.Location = new System.Drawing.Point(6, 19);
            this.cbGeneralCloseToTray.Name = "cbGeneralCloseToTray";
            this.cbGeneralCloseToTray.Size = new System.Drawing.Size(214, 17);
            this.cbGeneralCloseToTray.TabIndex = 2;
            this.cbGeneralCloseToTray.Text = "Сворачивать в область уведомлений";
            this.cbGeneralCloseToTray.UseVisualStyleBackColor = true;
            // 
            // gbLocalization
            // 
            this.gbLocalization.Controls.Add(this.label1);
            this.gbLocalization.Controls.Add(this.cbLocalizationRefreshTime);
            this.gbLocalization.Controls.Add(this.cbLocalizationCheckNewVersions);
            this.gbLocalization.Controls.Add(this.cbLocalizationAutoDownload);
            this.gbLocalization.Location = new System.Drawing.Point(12, 108);
            this.gbLocalization.Name = "gbLocalization";
            this.gbLocalization.Size = new System.Drawing.Size(507, 69);
            this.gbLocalization.TabIndex = 3;
            this.gbLocalization.TabStop = false;
            this.gbLocalization.Text = "Локализация";
            // 
            // cbLocalizationCheckNewVersions
            // 
            this.cbLocalizationCheckNewVersions.AutoSize = true;
            this.cbLocalizationCheckNewVersions.Location = new System.Drawing.Point(6, 19);
            this.cbLocalizationCheckNewVersions.Name = "cbLocalizationCheckNewVersions";
            this.cbLocalizationCheckNewVersions.Size = new System.Drawing.Size(203, 17);
            this.cbLocalizationCheckNewVersions.TabIndex = 2;
            this.cbLocalizationCheckNewVersions.Text = "Автоматически проверять каждые";
            this.cbLocalizationCheckNewVersions.UseVisualStyleBackColor = true;
            // 
            // cbLocalizationAutoDownload
            // 
            this.cbLocalizationAutoDownload.AutoSize = true;
            this.cbLocalizationAutoDownload.Location = new System.Drawing.Point(6, 42);
            this.cbLocalizationAutoDownload.Name = "cbLocalizationAutoDownload";
            this.cbLocalizationAutoDownload.Size = new System.Drawing.Size(234, 17);
            this.cbLocalizationAutoDownload.TabIndex = 1;
            this.cbLocalizationAutoDownload.Text = "Автоматически скачивать новую версию";
            this.cbLocalizationAutoDownload.UseVisualStyleBackColor = true;
            // 
            // cbLocalizationRefreshTime
            // 
            this.cbLocalizationRefreshTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.cbLocalizationRefreshTime.Location = new System.Drawing.Point(215, 17);
            this.cbLocalizationRefreshTime.Name = "cbLocalizationRefreshTime";
            this.cbLocalizationRefreshTime.Size = new System.Drawing.Size(46, 21);
            this.cbLocalizationRefreshTime.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(267, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "минут ";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 190);
            this.Controls.Add(this.gbLocalization);
            this.Controls.Add(this.gbGeneral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Настройки программы";
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            this.gbLocalization.ResumeLayout(false);
            this.gbLocalization.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.CheckBox cbGeneralCloseToTray;
        private System.Windows.Forms.CheckBox cbGeneralRunMinimized;
        private System.Windows.Forms.CheckBox cbGeneralRunWithWindows;
        private System.Windows.Forms.GroupBox gbLocalization;
        private System.Windows.Forms.CheckBox cbLocalizationCheckNewVersions;
        private System.Windows.Forms.CheckBox cbLocalizationAutoDownload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLocalizationRefreshTime;
    }
}