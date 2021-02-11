
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
            // tabCategories
            // 
            this.tabCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCategories.ContextMenuStrip = this.cmGameSetting;
            this.tabCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabCategories.ItemSize = new System.Drawing.Size(0, 20);
            this.tabCategories.Location = new System.Drawing.Point(0, 8);
            this.tabCategories.Name = "tabCategories";
            this.tabCategories.Padding = new System.Drawing.Point(3, 3);
            this.tabCategories.SelectedIndex = 0;
            this.tabCategories.Size = new System.Drawing.Size(779, 497);
            this.tabCategories.TabIndex = 0;
            // 
            // GameSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 540);
            this.Controls.Add(this.btnResetPage);
            this.Controls.Add(this.btnResetAll);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabCategories);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(375, 200);
            this.Name = "GameSettingsForm";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GameSettingsForm";
            this.Load += new System.EventHandler(this.GameSettingsForm_Load);
            this.cmGameSetting.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}