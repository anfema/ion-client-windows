namespace Anfema.Ion.Utils
{
    class LockWithCounter
    {
        public int counter { get; set; } = 0;
        public AsyncLock asyncLock { get; } = new AsyncLock();
    }
}
