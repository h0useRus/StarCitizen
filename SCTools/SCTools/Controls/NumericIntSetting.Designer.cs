
namespace NSW.StarCitizen.Tools.Controls
{
    partial class NumericIntSetting
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
            this.lblValue = new System.Windows.Forms.Label();
            this.numControl = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numControl)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCaption.Location = new System.Drawing.Point(8, 9);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(41, 15);
            this.lblCaption.TabIndex = 1;
            this.lblCaption.Text = "label1";
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblValue.Location = new System.Drawing.Point(249, 9);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(41, 15);
            this.lblValue.TabIndex = 3;
            this.lblValue.Text = "label2";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numControl
            // 
            this.numControl.AutoSize = true;
            this.numControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.numControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numControl.Location = new System.Drawing.Point(290, 9);
            this.numControl.Margin = new System.Windows.Forms.Padding(0);
            this.numControl.MinimumSize = new System.Drawing.Size(50, 0);
            this.numControl.Name = "numControl";
            this.numControl.Size = new System.Drawing.Size(50, 20);
            this.numControl.TabIndex = 3;
            this.numControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numControl.TextChanged += new System.EventHandler(this.numControl_TextChanged);
            this.numControl.ValueChanged += new System.EventHandler(this.numControl_ValueChanged);
            // 
            // NumericIntSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.numControl);
            this.Controls.Add(this.lblCaption);
            this.Name = "NumericIntSetting";
            this.Padding = new System.Windows.Forms.Padding(5, 9, 10, 5);
            this.Size = new System.Drawing.Size(350, 35);
            this.DoubleClick += new System.EventHandler(this.NumericIntSetting_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.numControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.NumericUpDown numControl;
    }
}
