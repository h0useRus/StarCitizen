namespace NSW.StarCitizen.Tools.Forms
{
    partial class ManageRepositoriesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageRepositoriesForm));
            this.lvRepositories = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.lblPath = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.tabRepositories = new System.Windows.Forms.TabControl();
            this.tabPageUserRepositories = new System.Windows.Forms.TabPage();
            this.tabPageStdRepositories = new System.Windows.Forms.TabPage();
            this.lvStdRepositories = new System.Windows.Forms.ListView();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.tabRepositories.SuspendLayout();
            this.tabPageUserRepositories.SuspendLayout();
            this.tabPageStdRepositories.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvRepositories
            // 
            this.lvRepositories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvRepositories.FullRowSelect = true;
            this.lvRepositories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvRepositories.HideSelection = false;
            this.lvRepositories.Location = new System.Drawing.Point(5, 6);
            this.lvRepositories.MultiSelect = false;
            this.lvRepositories.Name = "lvRepositories";
            this.lvRepositories.Size = new System.Drawing.Size(478, 211);
            this.lvRepositories.SmallImageList = this.imageList;
            this.lvRepositories.TabIndex = 0;
            this.lvRepositories.UseCompatibleStateImageBehavior = false;
            this.lvRepositories.View = System.Windows.Forms.View.Details;
            this.lvRepositories.SelectedIndexChanged += new System.EventHandler(this.lvRepositories_SelectedIndexChanged);
            this.lvRepositories.DoubleClick += new System.EventHandler(this.lvRepositories_DoubleClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "GitHub");
            this.imageList.Images.SetKeyName(1, "Folder");
            this.imageList.Images.SetKeyName(2, "Gitee");
            this.imageList.Images.SetKeyName(3, "Delete");
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 256);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(12, 273);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(359, 20);
            this.tbName.TabIndex = 2;
            // 
            // lblPath
            // 
            this.lblPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(9, 296);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(65, 13);
            this.lblPath.TabIndex = 3;
            this.lblPath.Text = "GitHub URL";
            // 
            // tbUrl
            // 
            this.tbUrl.AllowDrop = true;
            this.tbUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUrl.Location = new System.Drawing.Point(12, 312);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(359, 20);
            this.tbUrl.TabIndex = 4;
            this.tbUrl.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbUrl_DragDrop);
            this.tbUrl.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbUrl_DragEnter);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(387, 306);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(103, 26);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnRemove.ImageKey = "Delete";
            this.btnRemove.ImageList = this.imageList;
            this.btnRemove.Location = new System.Drawing.Point(464, 267);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(26, 26);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // tabRepositories
            // 
            this.tabRepositories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabRepositories.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabRepositories.Controls.Add(this.tabPageUserRepositories);
            this.tabRepositories.Controls.Add(this.tabPageStdRepositories);
            this.tabRepositories.Location = new System.Drawing.Point(3, 1);
            this.tabRepositories.Name = "tabRepositories";
            this.tabRepositories.SelectedIndex = 0;
            this.tabRepositories.Size = new System.Drawing.Size(497, 252);
            this.tabRepositories.TabIndex = 7;
            this.tabRepositories.SelectedIndexChanged += new System.EventHandler(this.tabRepositories_SelectedIndexChanged);
            // 
            // tabPageUserRepositories
            // 
            this.tabPageUserRepositories.Controls.Add(this.lvRepositories);
            this.tabPageUserRepositories.Location = new System.Drawing.Point(4, 25);
            this.tabPageUserRepositories.Name = "tabPageUserRepositories";
            this.tabPageUserRepositories.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUserRepositories.Size = new System.Drawing.Size(489, 223);
            this.tabPageUserRepositories.TabIndex = 0;
            this.tabPageUserRepositories.Text = "User";
            this.tabPageUserRepositories.UseVisualStyleBackColor = true;
            // 
            // tabPageStdRepositories
            // 
            this.tabPageStdRepositories.Controls.Add(this.lvStdRepositories);
            this.tabPageStdRepositories.Location = new System.Drawing.Point(4, 25);
            this.tabPageStdRepositories.Name = "tabPageStdRepositories";
            this.tabPageStdRepositories.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStdRepositories.Size = new System.Drawing.Size(489, 223);
            this.tabPageStdRepositories.TabIndex = 1;
            this.tabPageStdRepositories.Text = "Standard";
            this.tabPageStdRepositories.UseVisualStyleBackColor = true;
            // 
            // lvStdRepositories
            // 
            this.lvStdRepositories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvStdRepositories.FullRowSelect = true;
            this.lvStdRepositories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvStdRepositories.HideSelection = false;
            this.lvStdRepositories.Location = new System.Drawing.Point(5, 6);
            this.lvStdRepositories.MultiSelect = false;
            this.lvStdRepositories.Name = "lvStdRepositories";
            this.lvStdRepositories.Size = new System.Drawing.Size(478, 211);
            this.lvStdRepositories.SmallImageList = this.imageList;
            this.lvStdRepositories.TabIndex = 1;
            this.lvStdRepositories.UseCompatibleStateImageBehavior = false;
            this.lvStdRepositories.View = System.Windows.Forms.View.Details;
            this.lvStdRepositories.SelectedIndexChanged += new System.EventHandler(this.lvStdRepositories_SelectedIndexChanged);
            this.lvStdRepositories.DoubleClick += new System.EventHandler(this.lvStdRepositories_DoubleClick);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Location = new System.Drawing.Point(387, 267);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(26, 26);
            this.btnDown.TabIndex = 8;
            this.btnDown.TabStop = false;
            this.btnDown.Text = "D";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Location = new System.Drawing.Point(419, 267);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(26, 26);
            this.btnUp.TabIndex = 9;
            this.btnUp.TabStop = false;
            this.btnUp.Text = "U";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // ManageRepositoriesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 341);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.tabRepositories);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.lblName);
            this.DoubleBuffered = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 347);
            this.Name = "ManageRepositoriesForm";
            this.RestoreLocation = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Star Citizen : Localization repositories";
            this.Load += new System.EventHandler(this.ManageRepositoriesForm_Load);
            this.tabRepositories.ResumeLayout(false);
            this.tabPageUserRepositories.ResumeLayout(false);
            this.tabPageStdRepositories.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvRepositories;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.TabControl tabRepositories;
        private System.Windows.Forms.TabPage tabPageUserRepositories;
        private System.Windows.Forms.TabPage tabPageStdRepositories;
        private System.Windows.Forms.ListView lvStdRepositories;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ImageList imageList;
    }
}