namespace TestApp
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
            this.startWorkerNodeButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.addBatchToQueueButton = new System.Windows.Forms.Button();
            this.startHeadNodeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startWorkerNodeButton
            // 
            this.startWorkerNodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startWorkerNodeButton.Location = new System.Drawing.Point(211, 266);
            this.startWorkerNodeButton.Name = "startWorkerNodeButton";
            this.startWorkerNodeButton.Size = new System.Drawing.Size(193, 90);
            this.startWorkerNodeButton.TabIndex = 0;
            this.startWorkerNodeButton.Text = "Start Worker Node";
            this.startWorkerNodeButton.UseVisualStyleBackColor = true;
            this.startWorkerNodeButton.Click += new System.EventHandler(this.StartWorkerNodeButtonClick);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(12, 12);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(790, 248);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Text = "";
            this.logTextBox.TextChanged += new System.EventHandler(this.logTextBox_TextChanged);
            // 
            // addBatchToQueueButton
            // 
            this.addBatchToQueueButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addBatchToQueueButton.Location = new System.Drawing.Point(410, 266);
            this.addBatchToQueueButton.Name = "addBatchToQueueButton";
            this.addBatchToQueueButton.Size = new System.Drawing.Size(193, 90);
            this.addBatchToQueueButton.TabIndex = 3;
            this.addBatchToQueueButton.Text = "Add Batch to Queue";
            this.addBatchToQueueButton.UseVisualStyleBackColor = true;
            this.addBatchToQueueButton.Click += new System.EventHandler(this.addBatchToQueueButtonClick);
            // 
            // startHeadNodeButton
            // 
            this.startHeadNodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startHeadNodeButton.Location = new System.Drawing.Point(12, 266);
            this.startHeadNodeButton.Name = "startHeadNodeButton";
            this.startHeadNodeButton.Size = new System.Drawing.Size(193, 90);
            this.startHeadNodeButton.TabIndex = 4;
            this.startHeadNodeButton.Text = "Start Head Node";
            this.startHeadNodeButton.UseVisualStyleBackColor = true;
            this.startHeadNodeButton.Click += new System.EventHandler(this.startHeadNodeButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 368);
            this.Controls.Add(this.startHeadNodeButton);
            this.Controls.Add(this.addBatchToQueueButton);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.startWorkerNodeButton);
            this.Name = "MainForm";
            this.Text = "Test App";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startWorkerNodeButton;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Button addBatchToQueueButton;
        private System.Windows.Forms.Button startHeadNodeButton;
    }
}

