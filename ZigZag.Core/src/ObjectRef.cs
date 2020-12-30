using System.Text.Json;
using ZigZag.Core.Serialization;


namespace ZigZag.Core
{
    public class ObjectRef<T> where T : ZObject
    {
        public T Object
        {
            get;
            set;
        }

        internal long DeserializedID
        {
            get;
            set;
        }
    }
}
