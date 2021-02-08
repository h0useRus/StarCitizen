
namespace NSW.StarCitizen.Tools.Controls
{
    partial class ComboboxSetting
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCaption = new System.Windows.Forms.Label();
            this.cbValue = new NSW.StarCitizen.Tools.Controls.AutoWidthCombobox();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCaption.AutoSize = true;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCaption.Location = new System.Drawing.Point(8, 9);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(41, 15);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "label1";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbValue
            // 
            this.cbValue.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.cbValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbValue.Location = new System.Drawing.Point(200, 8);
            this.cbValue.MinimumSize = new System.Drawing.Size(75, 0);
            this.cbValue.Name = "cbValue";
            this.cbValue.Size = new System.Drawing.Size(140, 21);
            this.cbValue.TabIndex = 1;
            this.cbValue.SelectedIndexChanged += new System.EventHandler(this.Combobox_SelectedIndexChanged);
            // 
            // ComboboxSetting
            // 
            this.Controls.Add(this.cbValue);
            this.Controls.Add(this.lblCaption);
            this.Name = "ComboboxSetting";
            this.Padding = new System.Windows.Forms.Padding(5, 8, 10, 5);
            this.Size = new System.Drawing.Size(350, 35);
            this.DoubleClick += new System.EventHandler(this.ComboboxSetting_DoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCaption;
        private AutoWidthCombobox cbValue;
    }
}
