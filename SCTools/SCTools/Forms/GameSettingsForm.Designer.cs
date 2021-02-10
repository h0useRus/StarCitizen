
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
            this.tabCategories = new System.Windows.Forms.TabControl();
            this.cmGameSetting = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miChangedOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miResetSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.miResetAtPage = new System.Windows.Forms.ToolStripMenuItem();
            this.miResetAll = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnResetAll = new System.Windows.Forms.Button();
            this.btnResetPage = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmGameSetting.SuspendLayout();
            this.SuspendLayout();
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
            // cmGameSetting
            // 
            this.cmGameSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miResetSelected,
            this.toolStripSeparator2,
            this.miResetAtPage,
            this.miResetAll,
            this.toolStripSeparator1,
            this.miChangedOnly});
            this.cmGameSetting.Name = "cmGameSetting";
            this.cmGameSetting.Size = new System.Drawing.Size(181, 126);
            this.cmGameSetting.Opened += new System.EventHandler(this.cmGameSetting_Opened);
            // 
            // miChangedOnly
            // 
            this.miChangedOnly.CheckOnClick = true;
            this.miChangedOnly.Name = "miChangedOnly";
            this.miChangedOnly.Size = new System.Drawing.Size(180, 22);
            this.miChangedOnly.Text = "Changed Only";
            this.miChangedOnly.CheckedChanged += new System.EventHandler(this.miChangedOnly_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // miResetSelected
            // 
            this.miResetSelected.Name = "miResetSelected";
            this.miResetSelected.Size = new System.Drawing.Size(180, 22);
            this.miResetSelected.Text = "Reset Selected";
            this.miResetSelected.Click += new System.EventHandler(this.miResetSelected_Click);
            // 
            // miResetAtPage
            // 
            this.miResetAtPage.Name = "miResetAtPage";
            this.miResetAtPage.Size = new System.Drawing.Size(180, 22);
            this.miResetAtPage.Text = "Reset at Page";
            this.miResetAtPage.Click += new System.EventHandler(this.miResetAtPage_Click);
            // 
            // miResetAll
            // 
            this.miResetAll.Name = "miResetAll";
            this.miResetAll.Size = new System.Drawing.Size(180, 22);
            this.miResetAll.Text = "Reset All";
            this.miResetAll.Click += new System.EventHandler(this.miResetAll_Click);
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
            this.toolTip.IsBalloon = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
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
            this.MinimumSize = new System.Drawing.Size(375, 200);
            this.Name = "GameSettingsForm";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GameSettingsForm";
            this.Load += new System.EventHandler(this.GameSettingsForm_Load);
            this.cmGameSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCategories;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.Button btnResetPage;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip cmGameSetting;
        private System.Windows.Forms.ToolStripMenuItem miChangedOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miResetSelected;
        private System.Windows.Forms.ToolStripMenuItem miResetAtPage;
        private System.Windows.Forms.ToolStripMenuItem miResetAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}