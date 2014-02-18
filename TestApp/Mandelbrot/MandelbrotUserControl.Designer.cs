namespace TestApp.Mandelbrot
{
    partial class MandelbrotUserControl
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
            this.mandelbrotPictureBox = new System.Windows.Forms.PictureBox();
            this.startWorkerButton = new System.Windows.Forms.Button();
            this.generateMandelbrotButton = new System.Windows.Forms.Button();
            this.startHeadNodeButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.workerNodesLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mandelbrotPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mandelbrotPictureBox
            // 
            this.mandelbrotPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mandelbrotPictureBox.Location = new System.Drawing.Point(126, 3);
            this.mandelbrotPictureBox.Name = "mandelbrotPictureBox";
            this.mandelbrotPictureBox.Size = new System.Drawing.Size(792, 313);
            this.mandelbrotPictureBox.TabIndex = 0;
            this.mandelbrotPictureBox.TabStop = false;
            this.mandelbrotPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.mandelbrotPictureBox_Paint);
            // 
            // startWorkerButton
            // 
            this.startWorkerButton.Location = new System.Drawing.Point(0, 56);
            this.startWorkerButton.Name = "startWorkerButton";
            this.startWorkerButton.Size = new System.Drawing.Size(120, 50);
            this.startWorkerButton.TabIndex = 1;
            this.startWorkerButton.Text = "Start Worker Node";
            this.startWorkerButton.UseVisualStyleBackColor = true;
            this.startWorkerButton.Click += new System.EventHandler(this.startWorkerButton_Click);
            // 
            // generateMandelbrotButton
            // 
            this.generateMandelbrotButton.Location = new System.Drawing.Point(0, 112);
            this.generateMandelbrotButton.Name = "generateMandelbrotButton";
            this.generateMandelbrotButton.Size = new System.Drawing.Size(120, 50);
            this.generateMandelbrotButton.TabIndex = 2;
            this.generateMandelbrotButton.Text = "Generate";
            this.generateMandelbrotButton.UseVisualStyleBackColor = true;
            this.generateMandelbrotButton.Click += new System.EventHandler(this.generateMandelbrotButton_Click);
            // 
            // startHeadNodeButton
            // 
            this.startHeadNodeButton.Location = new System.Drawing.Point(0, 0);
            this.startHeadNodeButton.Name = "startHeadNodeButton";
            this.startHeadNodeButton.Size = new System.Drawing.Size(120, 50);
            this.startHeadNodeButton.TabIndex = 3;
            this.startHeadNodeButton.Text = "Start Head Node";
            this.startHeadNodeButton.UseVisualStyleBackColor = true;
            this.startHeadNodeButton.Click += new System.EventHandler(this.startHeadNodeButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(126, 311);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(917, 88);
            this.logTextBox.TabIndex = 4;
            this.logTextBox.Text = "";
            this.logTextBox.TextChanged += new System.EventHandler(this.logTextBox_TextChanged);
            // 
            // workerNodesLabel
            // 
            this.workerNodesLabel.AutoSize = true;
            this.workerNodesLabel.Location = new System.Drawing.Point(88, 168);
            this.workerNodesLabel.Name = "workerNodesLabel";
            this.workerNodesLabel.Size = new System.Drawing.Size(13, 13);
            this.workerNodesLabel.TabIndex = 8;
            this.workerNodesLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Worker Nodes:";
            // 
            // MandelbrotUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.workerNodesLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.startHeadNodeButton);
            this.Controls.Add(this.generateMandelbrotButton);
            this.Controls.Add(this.startWorkerButton);
            this.Controls.Add(this.mandelbrotPictureBox);
            this.Name = "MandelbrotUserControl";
            this.Size = new System.Drawing.Size(917, 402);
            this.Load += new System.EventHandler(this.MandelbrotForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mandelbrotPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox mandelbrotPictureBox;
        private System.Windows.Forms.Button startWorkerButton;
        private System.Windows.Forms.Button generateMandelbrotButton;
        private System.Windows.Forms.Button startHeadNodeButton;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Label workerNodesLabel;
        private System.Windows.Forms.Label label1;
    }
}