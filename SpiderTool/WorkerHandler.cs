using System.Collections.Concurrent;

namespace SpiderTool
{
    public class WorkerHandler
    {
        private object lockObj = new object();
        private static Lazy<WorkerHandler> lazy = new Lazy<WorkerHandler>(() => new WorkerHandler());
        private WorkerHandler()
        {

        }

        public static WorkerHandler GetInstance()
        {
            return lazy.Value;
        }


        readonly ConcurrentDictionary<int, CancellationTokenSource> workStatusSource = new ConcurrentDictionary<int, CancellationTokenSource>();
        public CancellationTokenSource this[int taskId] => workStatusSource[taskId];
        public CancellationTokenSource GetOrAdd(int taskId)
        {
            lock (lockObj)
            {
                if (workStatusSource.ContainsKey(taskId))
                    return workStatusSource[taskId];

                var tokenSource = new CancellationTokenSource();
                workStatusSource.TryAdd(taskId, tokenSource);
                return tokenSource;
            }
        }

        public void Cancel(int taskId)
        {
            if (workStatusSource.ContainsKey(taskId))
            {
                workStatusSource[taskId].Cancel();
            }
        }
        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="taskId"></param>
        public void Return(int taskId)
        {
            if (workStatusSource.ContainsKey(taskId))
            {
                if (workStatusSource.TryRemove(taskId, out var d))
                {
                    d.Dispose();
                }
            }
        }

        public void CancelAll()
        {
            lock (lockObj)
            {
                var allItems = workStatusSource.Keys.ToList();
                foreach (var item in allItems)
                {
                    workStatusSource[item].Cancel();
                }
            }
        }
    }
}
