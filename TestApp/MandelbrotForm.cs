using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using NLog;

using SimpleScale.HeadNode;
using SimpleScale.Common;
using SimpleScale.WorkerNode;
using SimpleScale.Queues;

using TestApp.Mandelbrot;

namespace TestApp
{
    public partial class MandelbrotForm : Form
    {
        private static Logger _logger;

        Bitmap _mandelbrotBitmap;
        private IQueueManager<PixelCalculationInput, PixelCalculationResult> _queueManager;
        private HeadNode<PixelCalculationInput, PixelCalculationResult> _headNode;

        public MandelbrotForm()
        {
            InitializeComponent();
        }

        private void MandelbrotForm_Load(object sender, EventArgs e)
        {
            //_logger = LogManager.GetCurrentClassLogger();

            _mandelbrotBitmap = new Bitmap(mandelbrotPictureBox.Width, mandelbrotPictureBox.Height);

            CreateQueueManger();
        }

        private void CreateQueueManger()
        {
            //_queueManager = new MemoryQueueManager<PixelCalculationInput, PixelCalculationResult> { SleepInterval = 0 };
            
            _queueManager = CreateServiceBusQueue();
        }

        private void startHeadNodeButton_Click(object sender, EventArgs e)
        {
            CreateHeadNode();
        }

        private void mandelbrotPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_mandelbrotBitmap, 0, 0, mandelbrotPictureBox.Width, mandelbrotPictureBox.Height);
        }

        private void CreateHeadNode()
        {
            _headNode = new HeadNode<PixelCalculationInput, PixelCalculationResult>(_queueManager);
            _headNode.JobComplete += HeadNodeJobComplete;
            _headNode.StartHeadNode();
        }

        void HeadNodeJobComplete(object sender, JobCompleteEventArgs<PixelCalculationResult> e)
        {
            PaintPixel(e.Result.Data);
            //_logger.Info("Job " + e.Result.Id + " complete in batch " + e.Result.BatchId + ".");
        }

        private IQueueManager<PixelCalculationInput, PixelCalculationResult> CreateServiceBusQueue()
        {
            var workQueueName = "PixelCalculationWork";
            var workCompletedQueueName = "PixelCalculationWorkCompleted";
            var serviceBusConnectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            return new ServiceBusQueueManager<PixelCalculationInput, PixelCalculationResult>(serviceBusConnectionString,
                workQueueName, workCompletedQueueName);
        }

        private void startWorkerButton_Click(object sender, EventArgs e)
        {
            var workerNode = new WorkerNode<PixelCalculationInput, PixelCalculationResult>(_queueManager, new PixelCalculator());
            workerNode.StartAsync();
        }

        private void PaintPixel(PixelCalculationResult pixelResult)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => PaintPixel(pixelResult)));
                return;
            }

            for (int x = 0; x < pixelResult.Colours.Length; x++)
            {
                var colour = GetColor(pixelResult.Colours[x]);
                _mandelbrotBitmap.SetPixel(x, pixelResult.Y, colour);
            }
            this.Refresh();
        }

        private Color GetColor(double value)
        {
            const double MaxColor = 256;
            const double ContrastValue = 0.2;
            return Color.FromArgb(0, 0,
                (int)(MaxColor * Math.Pow(value, ContrastValue)));
        }

        private void generateMandelbrotButton_Click(object sender, EventArgs e)
        {
            _mandelbrotBitmap = new Bitmap(mandelbrotPictureBox.Width, mandelbrotPictureBox.Height);
            var inputList = PixelInputGenerator.GenerateListOfInputs(mandelbrotPictureBox.Width, mandelbrotPictureBox.Height);
            var batch = new Batch<PixelCalculationInput>(inputList);
            _headNode.RunBatch(batch);
        }
    }
}
