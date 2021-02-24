using System;
using ZigZag.SceneGraph;
using ZigZag.SceneGraph.Widgets;


namespace ZigZag.Editor.WiringEditor
{
    class Surface : InfinitePlane
    {
        public Surface()
        {
            Init();
        }

        public Surface(Node parent) : base(parent)
        {
            Init();
        }

        private void Init()
        {
            ConsecutiveClicksEvent += OnConsecutiveClicks;
        }

        private void OnConsecutiveClicks(ConsecutiveClicksEvent e)
        {
            if (e.ClickCount == 2)
            {
                var block = new NodeBlockWidget();
                AddChild(block);
                block.Position = e.Position;

                Console.WriteLine("HELOOO");
            }
        }

    }
}
