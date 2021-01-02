using System.Threading;


namespace ZigZagEditor
{
    class Identifier
    {
        public static ulong Create()
        {
            return (ulong)Interlocked.Increment(ref globalNextValue);
        }

        private static long globalNextValue = 5000;

    }
}
