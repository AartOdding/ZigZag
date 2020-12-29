using System.Text.Json;
using System.Threading;


namespace ZigZag.Core
{
    public abstract class ZObject
    {
        internal ZObject()
        {
            ID = Interlocked.Increment(ref NEXT_ID);
        }

        public long ID
        {
            get;
        }

        private static long NEXT_ID = 4000;
    }
}
