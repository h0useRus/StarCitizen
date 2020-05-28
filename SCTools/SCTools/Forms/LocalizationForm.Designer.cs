namespace NSW.StarCitizen.Tools.Forms
{
    partial class LocalizationForm
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
            this.btnLocalization = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLocalization
            // 
            this.btnLocalization.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocalization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLocalization.Location = new System.Drawing.Point(12, 12);
            this.btnLocalization.Name = "btnLocalization";
            this.btnLocalization.Size = new System.Drawing.Size(242, 23);
            this.btnLocalization.TabIndex = 1;
            this.btnLocalization.Text = "Включить поддержку локализации";
            this.btnLocalization.UseVisualStyleBackColor = true;
            this.btnLocalization.Click += new System.EventHandler(this.btnLocalization_Click);
            // 
            // LocalizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 192);
            this.Controls.Add(this.btnLocalization);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LocalizationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Star Citizen : Локализация";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLocalization;
    }
}