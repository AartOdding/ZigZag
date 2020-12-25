using System.Text.Json;
using System.Threading;


namespace ZigZag.Core
{
    public abstract class ZObject
    {
        internal ZObject()
        {
            Identity = Interlocked.Increment(ref NEXT_ID);
        }

        public long Identity
        {
            get;
        }

        internal abstract void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options);
        internal abstract void ReadJson(ref Utf8JsonReader reader, JsonSerializerOptions options);

        private static long NEXT_ID = 4000;
    }
}
