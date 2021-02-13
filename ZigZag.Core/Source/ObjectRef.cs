

namespace ZigZag.Core
{
    public interface IObjectRef
    {
        internal void SetObject(object obj);
    }

    public class ObjectRef<T> : IObjectRef
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

        void IObjectRef.SetObject(object obj)
        {
            Object = (T)obj;
        }
    }
}
