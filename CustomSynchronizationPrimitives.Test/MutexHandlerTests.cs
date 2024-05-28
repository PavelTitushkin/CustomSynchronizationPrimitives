using CustomSynchronizationPrimitives.SynchronizationPrimitives;

namespace CustomSynchronizationPrimitives.Test
{
    public class MutexHandlerTests
    {
        [Fact]
        public void EnterShouldLock()
        {
            // Arrange
            var handler = new MutexHandler();

            // Act
            handler.Enter();
            bool lockAcquired = false;

            var t = new Thread(() =>
            {
                lockAcquired = handler.TryEnter(1000); // 1 second
            });

            t.Start();
            t.Join();

            // Assert
            Assert.False(lockAcquired);
            handler.Exit();
        }

        [Fact]
        public void ExitShouldUnlock()
        {
            // Arrange
            var handler = new MutexHandler();

            // Act
            handler.Enter();
            handler.Exit();
            bool lockAcquired = false;

            var t = new Thread(() =>
            {
                lockAcquired = handler.TryEnter(1000); // 1 second
                if (lockAcquired)
                {
                    handler.Exit();
                }
            });

            t.Start();
            t.Join();

            // Assert
            Assert.True(lockAcquired);
        }

        [Fact]
        public void TryEnterShouldReturnTrueIfLockAcquiredWithinTimeout()
        {
            // Arrange
            var handler = new MutexHandler();

            // Act
            bool lockAcquired = handler.TryEnter(1000); // 1 second

            // Assert
            Assert.True(lockAcquired);
            if (lockAcquired)
            {
                handler.Exit();
            }
        }

        [Fact]
        public void TryEnterShouldReturnFalseIfLockNotAcquiredWithinTimeout()
        {
            // Arrange
            var handler = new MutexHandler();
            handler.Enter();

            // Act
            bool lockAcquired = false;

            var t = new Thread(() =>
            {
                lockAcquired = handler.TryEnter(1000); // 1 second
            });

            t.Start();
            t.Join();

            // Assert
            Assert.False(lockAcquired);
            handler.Exit();
        }
    }
}
