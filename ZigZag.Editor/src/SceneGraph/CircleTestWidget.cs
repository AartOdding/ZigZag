using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.Mathematics;
using ZigZag.SceneGraph;


namespace ZigZag.Editor.SceneGraph
{
    class CircleTestWidget : GeometryNode
    {
        public CircleTestWidget(Node parent) : base(parent)
        {
            GeometryBuilder builder = new GeometryBuilder();
            builder.Color = new Color(0, 200, 120, 120);
            builder.AddEllipse(new Vector2(0, 0), 50, 50);
            Geometry = builder.Build();
        }

        protected override void MousePressEvent(MousePressEvent e, out bool consume, out bool subscribe)
        {
            Console.WriteLine("Mouse event accepted");
            consume = true;
            subscribe = true;
        }

        protected override void MouseDragEvent(MouseDragEvent e)
        {
            base.MouseDragEvent(e);
        }

        protected override void MouseReleaseEvent(MouseReleaseEvent e)
        {
            base.MouseReleaseEvent(e);
        }
    }
}
