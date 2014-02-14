namespace TestApp
{
    partial class MandelbrotForm
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
            ((System.ComponentModel.ISupportInitialize)(this.mandelbrotPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mandelbrotPictureBox
            // 
            this.mandelbrotPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mandelbrotPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mandelbrotPictureBox.Name = "mandelbrotPictureBox";
            this.mandelbrotPictureBox.Size = new System.Drawing.Size(603, 329);
            this.mandelbrotPictureBox.TabIndex = 0;
            this.mandelbrotPictureBox.TabStop = false;
            this.mandelbrotPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.mandelbrotPictureBox_Paint);
            // 
            // startWorkerButton
            // 
            this.startWorkerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startWorkerButton.Location = new System.Drawing.Point(609, 49);
            this.startWorkerButton.Name = "startWorkerButton";
            this.startWorkerButton.Size = new System.Drawing.Size(87, 43);
            this.startWorkerButton.TabIndex = 1;
            this.startWorkerButton.Text = "Start Worker Node";
            this.startWorkerButton.UseVisualStyleBackColor = true;
            this.startWorkerButton.Click += new System.EventHandler(this.startWorkerButton_Click);
            // 
            // generateMandelbrotButton
            // 
            this.generateMandelbrotButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateMandelbrotButton.Location = new System.Drawing.Point(609, 98);
            this.generateMandelbrotButton.Name = "generateMandelbrotButton";
            this.generateMandelbrotButton.Size = new System.Drawing.Size(87, 43);
            this.generateMandelbrotButton.TabIndex = 2;
            this.generateMandelbrotButton.Text = "Generate";
            this.generateMandelbrotButton.UseVisualStyleBackColor = true;
            this.generateMandelbrotButton.Click += new System.EventHandler(this.generateMandelbrotButton_Click);
            // 
            // startHeadNodeButton
            // 
            this.startHeadNodeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startHeadNodeButton.Location = new System.Drawing.Point(608, 0);
            this.startHeadNodeButton.Name = "startHeadNodeButton";
            this.startHeadNodeButton.Size = new System.Drawing.Size(87, 43);
            this.startHeadNodeButton.TabIndex = 3;
            this.startHeadNodeButton.Text = "Start Head Node";
            this.startHeadNodeButton.UseVisualStyleBackColor = true;
            this.startHeadNodeButton.Click += new System.EventHandler(this.startHeadNodeButton_Click);
            // 
            // MandelbrotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 329);
            this.Controls.Add(this.startHeadNodeButton);
            this.Controls.Add(this.generateMandelbrotButton);
            this.Controls.Add(this.startWorkerButton);
            this.Controls.Add(this.mandelbrotPictureBox);
            this.Name = "MandelbrotForm";
            this.Text = "Mandelbrot";
            this.Load += new System.EventHandler(this.MandelbrotForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mandelbrotPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mandelbrotPictureBox;
        private System.Windows.Forms.Button startWorkerButton;
        private System.Windows.Forms.Button generateMandelbrotButton;
        private System.Windows.Forms.Button startHeadNodeButton;
    }
}