using System;
using System.Threading;

namespace CustomSynchronizationPrimitives.SynchronizationPrimitives
{
    public class HybridLockHandler
    {
        private readonly object _locker = new object();
        private SpinLockHandler _spinLockHandler = new SpinLockHandler();
        private bool _enteredMonitor;

        public HybridLock Enter()
        {
            _enteredMonitor = Monitor.TryEnter(_locker);
            if (!_enteredMonitor)
            {
                _spinLockHandler.Enter();
            }

            return new HybridLock(this);
        }

        public void Exit()
        {
            if (_enteredMonitor)
            {
                Monitor.Exit(_locker);
                _enteredMonitor = false;
            }
            else
            {
                _spinLockHandler.Exit();
            }
        }

        public HybridLock TryEnter(int timeout)
        {
            _enteredMonitor = Monitor.TryEnter(_locker, timeout);
            if (_enteredMonitor)
            {
                if (!_spinLockHandler.TryEnter(timeout))
                {
                    throw new TimeoutException();
                }
            }

            return new HybridLock(this);
        }

        public struct HybridLock : IDisposable
        {
            private readonly HybridLockHandler _lockHandler;

            public HybridLock(HybridLockHandler lockHandler)
            {
                _lockHandler = lockHandler;
            }

            public void Dispose()
            {
                _lockHandler.Exit();
            }
        }
    }
}
