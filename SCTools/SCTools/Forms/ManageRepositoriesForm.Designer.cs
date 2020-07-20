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
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvRepositories
            // 
            this.lvRepositories.FullRowSelect = true;
            this.lvRepositories.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvRepositories.HideSelection = false;
            this.lvRepositories.HoverSelection = true;
            this.lvRepositories.Location = new System.Drawing.Point(12, 12);
            this.lvRepositories.MultiSelect = false;
            this.lvRepositories.Name = "lvRepositories";
            this.lvRepositories.Size = new System.Drawing.Size(443, 173);
            this.lvRepositories.TabIndex = 0;
            this.lvRepositories.UseCompatibleStateImageBehavior = false;
            this.lvRepositories.View = System.Windows.Forms.View.Details;
            this.lvRepositories.SelectedIndexChanged += new System.EventHandler(this.lvRepositories_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(15, 205);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(278, 20);
            this.tbName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "GitHub URL";
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(15, 244);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(278, 20);
            this.tbUrl.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(299, 205);
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
            this.btnRemove.Location = new System.Drawing.Point(380, 205);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 59);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // ManageRepositoriesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 272);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvRepositories);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageRepositoriesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Star Citizen : Localization repositories";
            this.Load += new System.EventHandler(this.ManageRepositoriesForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvRepositories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
    }
}