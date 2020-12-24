using System.Collections.Generic;
using ZigZag.Core;


namespace ZigZag.Text
{
    public class TextDataInput : NodeInput
    {
        public TextData ConnectedOutput
        {
            get;
            set;
        }

        public override bool Accepts(NodeOutput node)
        {
            return node.GetType() == typeof(TextData);
        }
    }

    public class TextData : NodeOutput
    {

        public List<string> Lines = new List<string>();

    }
}
