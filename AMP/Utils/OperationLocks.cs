using System;
using System.Collections.Generic;

namespace Anfema.Ion.Utils
{
    public class OperationLocks
    {
        private Dictionary<String, LockWithCounter> ongoingOperations = new Dictionary<string, LockWithCounter>();
        private readonly object syncLock = new object();

        public AsyncLock ObtainLock( String filePath )
        {
            lock ( syncLock )
            {
                LockWithCounter lockWithCounter = null;
                if( !ongoingOperations.TryGetValue( filePath, out lockWithCounter ) )
                {
                    lockWithCounter = new LockWithCounter();
                    ongoingOperations.Add( filePath, lockWithCounter );
                }
                lockWithCounter.counter++;
                return lockWithCounter.asyncLock;
            }
        }

        public void ReleaseLock( String filePath )
        {
            lock ( syncLock )
            {
                LockWithCounter lockWithCounter = null;
                if( ongoingOperations.TryGetValue( filePath, out lockWithCounter ) )
                {
                    lockWithCounter.counter--;

                    if( lockWithCounter.counter <= 0 )
                    {
                        ongoingOperations.Remove( filePath );
                    }
                }
            }
        }
    }
}
