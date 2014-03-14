using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using NLog;

using SimpleScale.HeadNode;
using SimpleScale.Common;
using SimpleScale.WorkerNode;
using SimpleScale.Queues;

using TestApp.Mandelbrot;

namespace TestApp.Mandelbrot
{
    public partial class MandelbrotUserControl : UserControl
    {
        private static Logger _logger;

        private const string JobsQueueName = "PixelCalculationWork";
        private const string JobsCompletedQueueName = "PixelCalculationWorkCompleted";

        Bitmap _mandelbrotBitmap;
        private IQueueManager<MandelbrotCalculationInput, MandelbrotCalculationResult> _queueManager;
        private HeadNode<MandelbrotCalculationInput, MandelbrotCalculationResult> _headNode;

        public MandelbrotUserControl()
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
            _queueManager = new MemoryQueueManager<MandelbrotCalculationInput, MandelbrotCalculationResult> { SleepInterval = 0 };
            //_queueManager = CreateServiceBusQueue();
            //_queueManager = CreateRabbitMqQueue();
        }

        private void startHeadNodeButton_Click(object sender, EventArgs e)
        {
            CreateHeadNode();
            this.startHeadNodeButton.Enabled = false;
        }

        private void mandelbrotPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_mandelbrotBitmap, 0, 0, mandelbrotPictureBox.Width, mandelbrotPictureBox.Height);
        }

        private void CreateHeadNode()
        {
            _headNode = new HeadNode<MandelbrotCalculationInput, MandelbrotCalculationResult>(_queueManager);
            _headNode.JobComplete += HeadNodeJobComplete;
            _headNode.StartHeadNode();
        }

        void HeadNodeJobComplete(object sender, JobCompleteEventArgs<MandelbrotCalculationResult> e)
        {
            PaintPixel(e.Result.Data);
            //_logger.Info("Job " + e.Result.Id + " complete in batch " + e.Result.BatchId + ".");
        }

        private IQueueManager<MandelbrotCalculationInput, MandelbrotCalculationResult> CreateServiceBusQueue()
        {
            var serviceBusConnectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            return new ServiceBusQueueManager<MandelbrotCalculationInput, MandelbrotCalculationResult>(serviceBusConnectionString,
                JobsQueueName, JobsCompletedQueueName);
        }

        private IQueueManager<MandelbrotCalculationInput, MandelbrotCalculationResult> CreateRabbitMqQueue()
        {
            return new RabbitMqQueueManager<MandelbrotCalculationInput, MandelbrotCalculationResult>("localhost",
                JobsQueueName, JobsCompletedQueueName);
        }

        private void startWorkerButton_Click(object sender, EventArgs e)
        {
            var workerNode = new WorkerNode<MandelbrotCalculationInput, MandelbrotCalculationResult>(_queueManager, new MandelbrotCalculator());
            workerNode.StartAsync();
            var nodeCount = int.Parse(workerNodesLabel.Text);
            workerNodesLabel.Text = (++nodeCount).ToString();
        }

        private void PaintPixel(MandelbrotCalculationResult pixelResult)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => PaintPixel(pixelResult)));
                return;
            }
            var memoryStream = new MemoryStream(pixelResult.JpgImage);
            var jpgImage = Image.FromStream(memoryStream);
            var bitmap = new Bitmap(jpgImage);
            CopyRegionIntoImage(bitmap, new Rectangle(0 ,0, bitmap.Width, bitmap.Height), ref _mandelbrotBitmap,
                new Rectangle(0, pixelResult.Y, bitmap.Width, bitmap.Height));
            this.Refresh();
        }

        private static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }

        private void generateMandelbrotButton_Click(object sender, EventArgs e)
        {
            _mandelbrotBitmap = new Bitmap(mandelbrotPictureBox.Width, mandelbrotPictureBox.Height);
            var inputList = InputGenerator.GenerateListOfInputs(mandelbrotPictureBox.Width, mandelbrotPictureBox.Height);
            var batch = new Batch<MandelbrotCalculationInput>(inputList);
            _headNode.RunBatch(batch);
        }

        private void logTextBox_TextChanged(object sender, EventArgs e)
        {
            logTextBox.SelectionStart = logTextBox.Text.Length;
            logTextBox.ScrollToCaret();
        }
    }
}
