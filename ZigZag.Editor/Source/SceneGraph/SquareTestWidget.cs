using System;
using ZigZag.Mathematics;
using ZigZag.SceneGraph;


namespace ZigZag.Editor.SceneGraph
{
    class SquareTestWidget : GeometryNode
    {
        public SquareTestWidget(Node parent) : base(parent)
        {
            GeometryBuilder builder = new GeometryBuilder();
            builder.Color = new Color(0, 200, 120, 120);
            builder.AddRectangle(new Rectangle(-200, -200, 400, 400));
            Geometry = builder.Build();
        }

        private float m_rotation;

        protected override void MousePressEvent(MousePressEvent e, out bool consume, out bool subscribe)
        {
            m_rotation += 0.3f;
            Transform = Transform2D.CreateRotation(m_rotation);
            Console.WriteLine($"Square press: {e.Position}");
            consume = true;
            subscribe = true;
        }

        protected override void MouseDragEvent(MouseDragEvent e)
        {
            base.MouseDragEvent(e);
        }

        protected override void MouseReleaseEvent(MouseReleaseEvent e)
        {
            Console.WriteLine($"Square release: {e.Position}");
            base.MouseReleaseEvent(e);
        }
    }
}
