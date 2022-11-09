using System;
using System.Collections.Concurrent;

namespace SpiderWin.Services
{
    public class DelayedTaskHandler
    {
        protected static Lazy<DelayedTaskHandler> _instance = new Lazy<DelayedTaskHandler>(() => new DelayedTaskHandler());
        private bool _isWorking = false;
        private static object _lock = new object();
        private DelayedTaskHandler()
        {

        }
        public static DelayedTaskHandler GetInstance() => _instance.Value;

        ConcurrentDictionary<string, Action> _queue = new ConcurrentDictionary<string, Action>();
        public void AddTask(string key, Action action)
        {
            if (!_queue.ContainsKey(key))
            {
                _queue.TryAdd(key, action);
            }
            Consume();
        }

        private void ConsumeCore()
        {
            Task.Delay(2000).ContinueWith(t =>
            {
                if (_queue.Keys.Count > 0)
                {
                    _queue.TryRemove(_queue.Keys.First(), out var action);
                    action?.Invoke();
                }
                ConsumeCore();
            });
        }

        public void Consume()
        {
            if (!_isWorking)
            {
                lock (_lock)
                {
                    if (!_isWorking)
                    {
                        _isWorking = true;
                        ConsumeCore();
                    }
                }
            }
        }
    }
}