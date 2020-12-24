using System.Collections.Generic;
using ZigZag.Core;


namespace ZigZag.Text
{
    public class TextDataInput : InputNode
    {
        public TextData ConnectedOutput
        {
            get;
            set;
        }

        public override bool Accepts(OutputNode node)
        {
            return node.GetType() == typeof(TextData);
        }
    }

    public class TextData : OutputNode
    {

        public List<string> Lines = new List<string>();

    }
}
