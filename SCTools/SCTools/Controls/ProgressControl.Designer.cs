namespace NSW.StarCitizen.Tools.Controls
{
    partial class ProgressControl
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
            this.pbProgress = new NSW.StarCitizen.Tools.Controls.TextProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.CustomText = "";
            this.pbProgress.Location = new System.Drawing.Point(3, 3);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.ProgressColor = System.Drawing.SystemColors.Highlight;
            this.pbProgress.Size = new System.Drawing.Size(389, 23);
            this.pbProgress.TabIndex = 0;
            this.pbProgress.TextColor = System.Drawing.SystemColors.ControlText;
            this.pbProgress.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pbProgress.VisualMode = NSW.StarCitizen.Tools.Controls.ProgressBarDisplayMode.CustomText;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(398, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pbProgress);
            this.Name = "ProgressControl";
            this.Size = new System.Drawing.Size(485, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private TextProgressBar pbProgress;
        private System.Windows.Forms.Button btnCancel;
    }
}
