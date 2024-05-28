using CustomSynchronizationPrimitives.Contracts;
using System.Threading;

namespace CustomSynchronizationPrimitives.SynchronizationPrimitives
{
    public class MutexHandler : ILockHandler
    {
        private readonly Mutex _mutex = new Mutex();

        public void Enter()
        {
            _mutex.WaitOne();
        }

        public void Exit()
        {
            _mutex.ReleaseMutex();
        }

        public bool TryEnter(int millisecondsTimeout)
        {
            return _mutex.WaitOne(millisecondsTimeout);
        }
    }
}
