
namespace ZigZag.Core
{
    public abstract class InputNode : AbstractNode
    {

        public abstract bool Accepts(OutputNode output);

        public OutputNode ConnectedOutputNode
        {
            get;
            internal set;
        }

    }
}
