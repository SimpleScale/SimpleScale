using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NLog;

using SimpleScale.Common;
using SimpleScale.Queues;

namespace SimpleScale.WorkerNode
{

    public class WorkerNode<InputT, ResultU>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IQueueManager<InputT, ResultU> _queueManager;
        private readonly IJobRunner<InputT, ResultU> _mapJob;

        public event WorkerProgressHandler WorkerProgressEvent;
        public delegate void WorkerProgressHandler(WorkerProgressEventArgs e);

        public WorkerNode(IQueueManager<InputT, ResultU> queueManager, IJobRunner<InputT, ResultU> mapJob)
        {
            _queueManager = queueManager;
            _mapJob = mapJob;
        }

        public void StartAsync()
        {
            Task.Factory.StartNew(Start);
        }

        public void Start()
        {
            LogAndRaiseProgressEvent(ProgressType.WorkerNodeStarted, "Worker node started");
            while (true)
            {
                Job<InputT> job = null;
                try
                {

                    ResultU resultData;
                    var messageProcessed = _queueManager.ReadJobAndDoWork(_mapJob.DoWork, out job, out resultData);
                    if (!messageProcessed)
                        continue;

                    var result = new Result<ResultU>(resultData, job.Id, job.BatchId, null);
                    _queueManager.AddCompleteJob(result);
                    LogAndRaiseProgressEvent(ProgressType.WorkCompleted, "Work completed", job.BatchId, job.Id);
                }
                catch (Exception ex)
                {
                    Result<ResultU> result;
                    if (job != null)
                        result = new Result<ResultU>(default(ResultU), job.Id, job.BatchId, ex);
                    else
                        result = new Result<ResultU>(default(ResultU), Job<InputT>.UnknownJobId, Guid.Empty, ex);
                    _queueManager.AddCompleteJob(result);
                    _logger.Error(ex);
                }
            }
        }

        private void RaiseMessageEvent(ProgressType progressType)
        {
            LogAndRaiseProgressEvent(progressType, String.Empty);
        }

        private void LogAndRaiseProgressEvent(ProgressType progressType, string message, Nullable<Guid> batchId = null, int jobId = -1)
        {
            var threadIdText = "(Thread Id -" + Thread.CurrentThread.ManagedThreadId + ").";
            var jobAndBatchText = "";
            if (batchId.HasValue)
                jobAndBatchText = "Job '" + jobId + "' in batch '" + batchId.Value + ". ";
            var fullMessage = threadIdText + jobAndBatchText + message + ".";
            _logger.Info(fullMessage);
            if (WorkerProgressEvent != null)
                WorkerProgressEvent(new WorkerProgressEventArgs { Message = fullMessage, MessageType = progressType, ThreadId = Thread.CurrentThread.ManagedThreadId, BatchId = batchId, JobId = jobId });
        }
    }
}
