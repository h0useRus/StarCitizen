namespace NSW.StarCitizen.Tools.Forms
{
    partial class ProgressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStop = new System.Windows.Forms.Button();
            this.prTaskProgress = new System.Windows.Forms.ProgressBar();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.lblTaskInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(149, 69);
            this.btnStop.MinimumSize = new System.Drawing.Size(75, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 25);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // prTaskProgress
            // 
            this.prTaskProgress.Location = new System.Drawing.Point(12, 34);
            this.prTaskProgress.Name = "prTaskProgress";
            this.prTaskProgress.Size = new System.Drawing.Size(350, 23);
            this.prTaskProgress.TabIndex = 1;
            // 
            // lblTaskName
            // 
            this.lblTaskName.AutoSize = true;
            this.lblTaskName.Location = new System.Drawing.Point(12, 15);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(62, 13);
            this.lblTaskName.TabIndex = 2;
            this.lblTaskName.Text = "Task Name";
            // 
            // lblTaskInfo
            // 
            this.lblTaskInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTaskInfo.Location = new System.Drawing.Point(12, 15);
            this.lblTaskInfo.Name = "lblTaskInfo";
            this.lblTaskInfo.Size = new System.Drawing.Size(350, 16);
            this.lblTaskInfo.TabIndex = 3;
            this.lblTaskInfo.Text = "Task Info";
            this.lblTaskInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnStop;
            this.ClientSize = new System.Drawing.Size(374, 106);
            this.Controls.Add(this.lblTaskName);
            this.Controls.Add(this.lblTaskInfo);
            this.Controls.Add(this.prTaskProgress);
            this.Controls.Add(this.btnStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProgressForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgressForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProgressForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ProgressBar prTaskProgress;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.Label lblTaskInfo;
    }
}