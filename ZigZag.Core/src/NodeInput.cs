
namespace ZigZag.Core
{
    public abstract class NodeInput : AbstractNode
    {

        public abstract bool Accepts(NodeOutput output);

        public NodeOutput ConnectedNodeOutput
        {
            get;
            internal set;
        }

    }
}
