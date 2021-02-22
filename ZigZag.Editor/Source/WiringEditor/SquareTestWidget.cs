using System;
using ZigZag.Mathematics;
using ZigZag.SceneGraph;


namespace ZigZag.Editor.WiringEditor
{
    class SquareTestWidget : GeometryNode
    {
        public SquareTestWidget(Node parent) : base(parent)
        {
            MouseButtonPressEvent += OnMousePressEvent;
            MouseButtonDragEvent += OnMouseDragEvent;
            MouseButtonReleaseEvent += OnMouseReleaseEvent;

            GeometryBuilder builder = new GeometryBuilder();
            builder.Color = new Color(0, 200, 120, 120);
            builder.AddRectangle(new Rectangle(-200, -200, 400, 400));
            Geometry = builder.Build();
        }

        private float m_rotation;

        public void OnMousePressEvent(MouseButtonPressEvent e)
        {
            m_rotation += 0.3f;
            Transform = Transform2D.CreateRotation(m_rotation);
        }

        private void OnMouseDragEvent(MouseButtonDragEvent e)
        {
        }

        private void OnMouseReleaseEvent(MouseButtonReleaseEvent e)
        {
        }
    }
}
