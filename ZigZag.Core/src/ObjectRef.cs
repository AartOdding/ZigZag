

namespace ZigZag.Core
{
    public class ObjectRef<T> where T : ZObject
    {
        public ObjectRef() { }
        public ObjectRef(T obj)
        {
            Object = obj;
        }

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
