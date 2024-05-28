using CustomSynchronizationPrimitives.Contracts;
using System.Threading;

namespace CustomSynchronizationPrimitives.SynchronizationPrimitives
{
    public class MonitorHandler : ILockHandler
    {
        private readonly object _lock = new object();
        public void Enter()
        {
            Monitor.Enter(_lock);
        }

        public void Exit()
        {
            Monitor.Exit(_lock);
        }

        public bool TryEnter(int timeout)
        {
            return Monitor.TryEnter(_lock, timeout);
        }
    }
}
