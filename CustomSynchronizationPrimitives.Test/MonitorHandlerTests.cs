using CustomSynchronizationPrimitives.SynchronizationPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSynchronizationPrimitives.Test
{
    public class MonitorHandlerTests
    {
        [Fact]
        public void EnterShouldLock()
        {
            // Arrange
            var handler = new MonitorHandler();

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
            var handler = new MonitorHandler();

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
        public void TryEnter_ShouldReturnTrue_IfLockAcquiredWithinTimeout()
        {
            // Arrange
            var handler = new MonitorHandler();

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
        public void TryEnter_ShouldReturnFalse_IfLockNotAcquiredWithinTimeout()
        {
            // Arrange
            var handler = new MonitorHandler();
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
