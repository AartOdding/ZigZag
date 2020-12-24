using System;
using ZigZag.Core;


namespace ZigZag.Text
{
    public class Print : Node
    {

        public readonly TextDataInput Input = new TextDataInput();

        public override void Update()
        {
            if (!(Input.ConnectedOutput is null))
            {
                foreach (string line in Input.ConnectedOutput.Lines)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
