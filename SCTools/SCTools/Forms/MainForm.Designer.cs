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
            this.gbGameInfo = new System.Windows.Forms.GroupBox();
            this.tbGameVersion = new System.Windows.Forms.TextBox();
            this.lblGameVersion = new System.Windows.Forms.Label();
            this.tbGameMode = new System.Windows.Forms.TextBox();
            this.lblGameMode = new System.Windows.Forms.Label();
            this.gbButtonMenu = new System.Windows.Forms.GroupBox();
            this.btnUserSettings = new System.Windows.Forms.Button();
            this.btnLocalization = new System.Windows.Forms.Button();
            this.gbGameInfo.SuspendLayout();
            this.gbButtonMenu.SuspendLayout();
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
            this.tbGamePath.Text = "Нажмите здесь, чтобы выбрать путь до папки Star Citizen";
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
            this.cbGameModes.SelectedIndexChanged += new System.EventHandler(this.cbGameModes_SelectedIndexChanged);
            // 
            // gbGameInfo
            // 
            this.gbGameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbGameInfo.Controls.Add(this.tbGameVersion);
            this.gbGameInfo.Controls.Add(this.lblGameVersion);
            this.gbGameInfo.Controls.Add(this.tbGameMode);
            this.gbGameInfo.Controls.Add(this.lblGameMode);
            this.gbGameInfo.Location = new System.Drawing.Point(12, 39);
            this.gbGameInfo.Name = "gbGameInfo";
            this.gbGameInfo.Size = new System.Drawing.Size(200, 110);
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
            // 
            // lblGameVersion
            // 
            this.lblGameVersion.AutoSize = true;
            this.lblGameVersion.Location = new System.Drawing.Point(6, 56);
            this.lblGameVersion.Name = "lblGameVersion";
            this.lblGameVersion.Size = new System.Drawing.Size(88, 13);
            this.lblGameVersion.TabIndex = 2;
            this.lblGameVersion.Text = "Версия клиента";
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
            // 
            // lblGameMode
            // 
            this.lblGameMode.AutoSize = true;
            this.lblGameMode.Location = new System.Drawing.Point(6, 16);
            this.lblGameMode.Name = "lblGameMode";
            this.lblGameMode.Size = new System.Drawing.Size(87, 13);
            this.lblGameMode.TabIndex = 0;
            this.lblGameMode.Text = "Игровой режим";
            // 
            // gbButtonMenu
            // 
            this.gbButtonMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbButtonMenu.Controls.Add(this.btnUserSettings);
            this.gbButtonMenu.Controls.Add(this.btnLocalization);
            this.gbButtonMenu.Location = new System.Drawing.Point(218, 39);
            this.gbButtonMenu.Name = "gbButtonMenu";
            this.gbButtonMenu.Size = new System.Drawing.Size(254, 110);
            this.gbButtonMenu.TabIndex = 4;
            this.gbButtonMenu.TabStop = false;
            // 
            // btnUserSettings
            // 
            this.btnUserSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUserSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserSettings.Location = new System.Drawing.Point(6, 69);
            this.btnUserSettings.Name = "btnUserSettings";
            this.btnUserSettings.Size = new System.Drawing.Size(242, 23);
            this.btnUserSettings.TabIndex = 1;
            this.btnUserSettings.Text = "Настройки клиента";
            this.btnUserSettings.UseVisualStyleBackColor = true;
            this.btnUserSettings.Visible = false;
            // 
            // btnLocalization
            // 
            this.btnLocalization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocalization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocalization.Location = new System.Drawing.Point(6, 30);
            this.btnLocalization.Name = "btnLocalization";
            this.btnLocalization.Size = new System.Drawing.Size(242, 23);
            this.btnLocalization.TabIndex = 0;
            this.btnLocalization.Text = "Локализация";
            this.btnLocalization.UseVisualStyleBackColor = true;
            this.btnLocalization.Click += new System.EventHandler(this.btnLocalization_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 161);
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
            this.Text = "Star Citizen : Утилиты";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbGameInfo.ResumeLayout(false);
            this.gbGameInfo.PerformLayout();
            this.gbButtonMenu.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnUserSettings;
        private System.Windows.Forms.Button btnLocalization;
    }
}