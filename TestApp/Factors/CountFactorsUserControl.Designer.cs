namespace TestApp.Factors
{
    partial class CountFactorsUserControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.workerNodesLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startWorkerNodeButton
            // 
            this.startWorkerNodeButton.Location = new System.Drawing.Point(0, 56);
            this.startWorkerNodeButton.Name = "startWorkerNodeButton";
            this.startWorkerNodeButton.Size = new System.Drawing.Size(120, 50);
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
            this.logTextBox.Location = new System.Drawing.Point(126, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(680, 374);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Text = "";
            this.logTextBox.TextChanged += new System.EventHandler(this.LogTextBoxTextChanged);
            // 
            // addBatchToQueueButton
            // 
            this.addBatchToQueueButton.Location = new System.Drawing.Point(0, 112);
            this.addBatchToQueueButton.Name = "addBatchToQueueButton";
            this.addBatchToQueueButton.Size = new System.Drawing.Size(120, 50);
            this.addBatchToQueueButton.TabIndex = 3;
            this.addBatchToQueueButton.Text = "Add Batch to Queue";
            this.addBatchToQueueButton.UseVisualStyleBackColor = true;
            this.addBatchToQueueButton.Click += new System.EventHandler(this.AddBatchToQueueButtonClick);
            // 
            // startHeadNodeButton
            // 
            this.startHeadNodeButton.Location = new System.Drawing.Point(0, 0);
            this.startHeadNodeButton.Name = "startHeadNodeButton";
            this.startHeadNodeButton.Size = new System.Drawing.Size(120, 50);
            this.startHeadNodeButton.TabIndex = 4;
            this.startHeadNodeButton.Text = "Start Head Node";
            this.startHeadNodeButton.UseVisualStyleBackColor = true;
            this.startHeadNodeButton.Click += new System.EventHandler(this.StartHeadNodeButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Worker Nodes:";
            // 
            // workerNodesLabel
            // 
            this.workerNodesLabel.AutoSize = true;
            this.workerNodesLabel.Location = new System.Drawing.Point(88, 168);
            this.workerNodesLabel.Name = "workerNodesLabel";
            this.workerNodesLabel.Size = new System.Drawing.Size(13, 13);
            this.workerNodesLabel.TabIndex = 6;
            this.workerNodesLabel.Text = "0";
            // 
            // CountFactorsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.workerNodesLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startHeadNodeButton);
            this.Controls.Add(this.addBatchToQueueButton);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.startWorkerNodeButton);
            this.Name = "CountFactorsUserControl";
            this.Size = new System.Drawing.Size(809, 377);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startWorkerNodeButton;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Button addBatchToQueueButton;
        private System.Windows.Forms.Button startHeadNodeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label workerNodesLabel;
    }
}

