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
            this.lvRepositories = new System.Windows.Forms.ListView();
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
            this.tabRepositories.SuspendLayout();
            this.tabPageUserRepositories.SuspendLayout();
            this.tabPageStdRepositories.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvRepositories
            // 
            this.lvRepositories.FullRowSelect = true;
            this.lvRepositories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvRepositories.HideSelection = false;
            this.lvRepositories.HoverSelection = true;
            this.lvRepositories.Location = new System.Drawing.Point(5, 6);
            this.lvRepositories.MultiSelect = false;
            this.lvRepositories.Name = "lvRepositories";
            this.lvRepositories.Size = new System.Drawing.Size(450, 178);
            this.lvRepositories.TabIndex = 0;
            this.lvRepositories.UseCompatibleStateImageBehavior = false;
            this.lvRepositories.View = System.Windows.Forms.View.Details;
            this.lvRepositories.SelectedIndexChanged += new System.EventHandler(this.lvRepositories_SelectedIndexChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 223);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(12, 240);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(279, 20);
            this.tbName.TabIndex = 2;
            // 
            // lblPath
            // 
            this.lblPath.AutoSize = true;
            this.lblPath.Location = new System.Drawing.Point(9, 263);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(65, 13);
            this.lblPath.TabIndex = 3;
            this.lblPath.Text = "GitHub URL";
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(12, 279);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(279, 20);
            this.tbUrl.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(306, 240);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 59);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Location = new System.Drawing.Point(387, 240);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 59);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // tabRepositories
            // 
            this.tabRepositories.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabRepositories.Controls.Add(this.tabPageUserRepositories);
            this.tabRepositories.Controls.Add(this.tabPageStdRepositories);
            this.tabRepositories.Location = new System.Drawing.Point(3, 1);
            this.tabRepositories.Name = "tabRepositories";
            this.tabRepositories.SelectedIndex = 0;
            this.tabRepositories.Size = new System.Drawing.Size(474, 219);
            this.tabRepositories.TabIndex = 7;
            this.tabRepositories.SelectedIndexChanged += new System.EventHandler(this.tabRepositories_SelectedIndexChanged);
            // 
            // tabPageUserRepositories
            // 
            this.tabPageUserRepositories.Controls.Add(this.lvRepositories);
            this.tabPageUserRepositories.Location = new System.Drawing.Point(4, 25);
            this.tabPageUserRepositories.Name = "tabPageUserRepositories";
            this.tabPageUserRepositories.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUserRepositories.Size = new System.Drawing.Size(466, 190);
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
            this.tabPageStdRepositories.Size = new System.Drawing.Size(466, 190);
            this.tabPageStdRepositories.TabIndex = 1;
            this.tabPageStdRepositories.Text = "Standard";
            this.tabPageStdRepositories.UseVisualStyleBackColor = true;
            // 
            // lvStdRepositories
            // 
            this.lvStdRepositories.FullRowSelect = true;
            this.lvStdRepositories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvStdRepositories.HideSelection = false;
            this.lvStdRepositories.HoverSelection = true;
            this.lvStdRepositories.Location = new System.Drawing.Point(5, 6);
            this.lvStdRepositories.MultiSelect = false;
            this.lvStdRepositories.Name = "lvStdRepositories";
            this.lvStdRepositories.Size = new System.Drawing.Size(450, 178);
            this.lvStdRepositories.TabIndex = 1;
            this.lvStdRepositories.UseCompatibleStateImageBehavior = false;
            this.lvStdRepositories.View = System.Windows.Forms.View.Details;
            this.lvStdRepositories.SelectedIndexChanged += new System.EventHandler(this.lvStdRepositories_SelectedIndexChanged);
            // 
            // ManageRepositoriesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 308);
            this.Controls.Add(this.tabRepositories);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblPath);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageRepositoriesForm";
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
    }
}