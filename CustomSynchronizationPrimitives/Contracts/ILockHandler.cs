namespace CustomSynchronizationPrimitives.Contracts
{
    public interface ILockHandler
    {
        void Enter();
        void Exit();
        bool TryEnter(int timeout);
    }
}
