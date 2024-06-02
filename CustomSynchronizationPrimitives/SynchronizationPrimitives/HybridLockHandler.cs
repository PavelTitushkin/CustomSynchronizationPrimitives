using System.Threading;

namespace CustomSynchronizationPrimitives.SynchronizationPrimitives
{
    public class HybridLockHandler
    {
        private readonly object _locker = new object();
        private SpinLockHandler _spinLockHandler = new SpinLockHandler();

        public void Enter()
        {
            if (!Monitor.TryEnter(_locker))
            {
                _spinLockHandler.Enter();
            }
        }

        public void Exit()
        {
            if (Monitor.IsEntered(_locker))
            {
                Monitor.Exit(_locker);
            }
            else
            {
                _spinLockHandler.Exit();
            }
        }

        public bool TryEnter(int timeout)
        {
            if (Monitor.TryEnter(_locker))
            {
                return true;
            }
            else
            {
                return _spinLockHandler.TryEnter(timeout);
            }
        }
    }
}
