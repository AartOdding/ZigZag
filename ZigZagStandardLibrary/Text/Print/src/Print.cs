using System;


namespace ZigZag
{
    namespace Text
    {
        public class Print : ProcessNode
        {

            public InputNode<TextData> Input;

            public override void Process()
            {
                if (Input.Node != null)
                {
                    foreach (string line in Input.Node.Lines)
                    {
                        Console.WriteLine(line);
                    }
                }
            }

        }
    }
}
