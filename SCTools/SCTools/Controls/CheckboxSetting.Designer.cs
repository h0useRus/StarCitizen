
namespace NSW.StarCitizen.Tools.Controls
{
    partial class CheckboxSetting
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
            this.cbValue = new System.Windows.Forms.CheckBox();
            this.lblCaption = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbValue
            // 
            this.cbValue.AutoSize = true;
            this.cbValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.cbValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbValue.Location = new System.Drawing.Point(328, 5);
            this.cbValue.Name = "cbValue";
            this.cbValue.Size = new System.Drawing.Size(12, 25);
            this.cbValue.TabIndex = 0;
            this.cbValue.UseVisualStyleBackColor = true;
            this.cbValue.CheckStateChanged += new System.EventHandler(this.cbValue_CheckStateChanged);
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCaption.Location = new System.Drawing.Point(8, 9);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(41, 15);
            this.lblCaption.TabIndex = 1;
            this.lblCaption.Text = "label2";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CheckboxSetting
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblCaption);
            this.Controls.Add(this.cbValue);
            this.Name = "CheckboxSetting";
            this.Padding = new System.Windows.Forms.Padding(5, 5, 10, 5);
            this.Size = new System.Drawing.Size(350, 35);
            this.DoubleClick += new System.EventHandler(this.CheckboxSetting_DoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbValue;
        private System.Windows.Forms.Label lblCaption;
    }
}
