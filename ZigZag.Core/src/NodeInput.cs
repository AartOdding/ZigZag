using System.Diagnostics;


namespace ZigZag.Core
{
    public abstract class NodeInput : ZObject
    {
        public NodeInput()
        {
        }

        public NodeInput(Node node)
        {
            // No need to check for loops
            Debug.Assert(!(node is null));
            Node = node;
        }

        public NodeInput(string name)
        {
            Name = name;
        }

        public NodeInput(Node node, string name)
        {
            // No need to check for loops
            Debug.Assert(!(node is null));
            Node = node;
            Name = name;
        }

        public string Name
        {
            get;
            internal set;
        }

        public Node Node
        {
            get;
            internal set;
        }

        public NodeOutput ConnectedOutput
        {
            get;
            internal set;
        }

        public abstract void Update();

        public abstract bool Accepts(NodeOutput output);


    }
}
