

namespace ZigZag.Core.Parameters
{
    public interface INumericalParameter
    {
        public abstract long GetInt(int index);
        public abstract double GetFloat(int index);
    }
}
