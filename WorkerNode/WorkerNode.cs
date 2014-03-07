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

    public class WorkerNode<T, U>
    {
        public static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IQueueManager<T, U> _queueManager;
        private readonly IJobRunner<T, U> _mapJob;

        public event WorkerProgressHandler WorkerProgressEvent;
        public delegate void WorkerProgressHandler(WorkerProgressEventArgs e);

        public WorkerNode(IQueueManager<T, U> queueManager, IJobRunner<T, U> mapJob)
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
                Job<T> job = null;
                try
                {
                    
                    U resultData;
                    var messageProcessed = _queueManager.ReadJobAndDoWork(_mapJob.DoWork, out job, out resultData);
                    if (!messageProcessed)
                        continue;

                    var result = new Result<U>(resultData, job.Id, job.BatchId, null);
                    _queueManager.AddCompleteJob(result);
                    LogAndRaiseProgressEvent(ProgressType.WorkCompleted, "Work completed", job.BatchId, job.Id);
                }
                catch (Exception ex)
                {
                    Result<U> result;
                    if (job != null)
                        result = new Result<U>(default(U), job.Id, job.BatchId, ex);
                    else
                        result = new Result<U>(default(U), Job<T>.UnknownJobId, Guid.Empty, ex);
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
