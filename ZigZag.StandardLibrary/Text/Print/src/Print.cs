using System;
using ZigZag.Core;


namespace ZigZag.Text
{
    public class Print : ProcessNode
    {

        public readonly TextDataInput Input = new TextDataInput();

        public override void Process()
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
