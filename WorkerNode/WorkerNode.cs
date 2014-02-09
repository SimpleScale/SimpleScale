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
        private readonly IMapJob<T, U> _mapJob;

        public event WorkerProgressHandler WorkerProgressEvent;
        public delegate void WorkerProgressHandler(WorkerProgressEventArgs e);

        public WorkerNode(IQueueManager<T, U> queueManager, IMapJob<T, U> mapJob)
        {
            _queueManager = queueManager;
            _mapJob = mapJob;
        }

        public void StartAsync(CancellationTokenSource cancellationTokenSource)
        {
            var token = cancellationTokenSource.Token;
            Task.Factory.StartNew(() => Start(token), token);
        }

        public void Start(CancellationToken cancellationToken)
        {
            LogAndRaiseProgressEvent(ProgressType.WorkerNodeStarted, "Worker node started");
            while (true)
            {
                try
                {
                    Job<T> job;
                    U resultData;
                    var messageProcessed = _queueManager.ReadJobAndDoWork(_mapJob.DoWork, out job, out resultData);
                    if (!messageProcessed)
                        continue;

                    var result = new Result<U>(resultData, job.Id, job.BatchId);
                    _queueManager.AddCompleteJob(result);
                    LogAndRaiseProgressEvent(ProgressType.WorkCompleted, "Work completed", job.BatchId, job.Id);
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (Exception ex)
                {
                    LogAndRaiseProgressEvent(ProgressType.WorkFailed, ex.Message);
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
