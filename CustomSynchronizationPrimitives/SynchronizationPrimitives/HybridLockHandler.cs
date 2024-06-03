using System;
using System.Threading;

namespace CustomSynchronizationPrimitives.SynchronizationPrimitives
{
    public class HybridLockHandler
    {
        private object _locker = new object();
        private SpinLockHandler _spinLockHandler = new SpinLockHandler();
        private bool _enteredLocker;
        public HybridLock Enter()
        {
            _enteredLocker = _spinLockHandler.TryEnter(100);
            if (_enteredLocker)
            {
                return new HybridLock(this, LockType.SpinLock);
            }
            else
            {
                Monitor.Enter(_locker);

                return new HybridLock(this, LockType.Monitor);
            }
        }

        public void Exit(LockType lockType)
        {
            if (lockType == LockType.SpinLock)
            {
                _spinLockHandler.Exit();
            }
            else
            {
                Monitor.Exit(_locker);
            }
        }

        public HybridLock TryEnter(int timeout)
        {
            _enteredLocker = _spinLockHandler.TryEnter(timeout);
            if (_enteredLocker)
            {
                return new HybridLock(this, LockType.SpinLock);
            }
            else if (Monitor.TryEnter(_locker, timeout))
            {
                return new HybridLock(this, LockType.Monitor);
            }
            else
            {
                throw new TimeoutException();
            }
        }

        public ref struct HybridLock
        {
            private readonly HybridLockHandler _lockHandler;
            private LockType _lockType;

            public HybridLock(HybridLockHandler lockHandler, LockType lockType)
            {
                _lockHandler = lockHandler;
                _lockType = lockType;
            }

            public void Dispose()
            {
                _lockHandler.Exit(_lockType);
            }
        }

        public enum LockType
        {
            SpinLock,
            Monitor
        }
    }
}
