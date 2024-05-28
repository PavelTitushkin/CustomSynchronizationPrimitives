using CustomSynchronizationPrimitives.Contracts;
using System.Threading;

namespace CustomSynchronizationPrimitives.SynchronizationPrimitives
{
    public class SpinLockHandler : ILockHandler
    {
        private int _lockState = 0; // 0 - unlocked, 1 - locked

        public void Enter()
        {
            while (true)
            {
                if (Interlocked.CompareExchange(ref _lockState, 1, 0) == 0)
                {
                    return;
                }
            }
        }

        public void Exit()
        {
            Volatile.Write(ref _lockState, 0);
        }

        public bool TryEnter(int millisecondsTimeout)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (true)
            {
                if (Interlocked.CompareExchange(ref _lockState, 1, 0) == 0)
                {
                    return true;
                }
                if (sw.ElapsedMilliseconds >= millisecondsTimeout)
                {
                    return false;
                }
            }
        }
    }
}
