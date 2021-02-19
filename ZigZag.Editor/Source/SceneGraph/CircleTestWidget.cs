using System;
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

        protected override void MousePressEvent(MousePressEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                e.Subscribe = true;
            }
        }

        protected override void MouseDragEvent(MouseDragEvent e)
        {
            Position += e.Delta;
        }

        protected override void MouseReleaseEvent(MouseReleaseEvent e)
        {
        }
    }
}
