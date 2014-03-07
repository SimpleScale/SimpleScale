using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TestApp.Factors;
using TestApp.Mandelbrot;

namespace TestApp
{
    public partial class MainForm : Form
    {
        CountFactorsUserControl countFactorsControl = new CountFactorsUserControl();
        MandelbrotUserControl mandelbrotControl = new MandelbrotUserControl();

        public MainForm()
        {
            InitializeComponent();
        }

        private void CountFactorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MainPanel.Controls.Clear();
            this.MainPanel.Controls.Add(countFactorsControl);
            countFactorsControl.Dock = DockStyle.Fill;
        }

        private void mandelbrotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MainPanel.Controls.Clear();
            this.MainPanel.Controls.Add(mandelbrotControl);
            mandelbrotControl.Dock = DockStyle.Fill;

        }
    }
}
