using System;
using ZigZag.Mathematics;
using ZigZag.SceneGraph;


namespace ZigZag.Editor.WiringEditor
{
    class CircleTestWidget : GeometryNode
    {
        public CircleTestWidget(Node parent) : base(parent)
        {
            MouseButtonPressEvent += OnMousePressEvent;
            MouseButtonDragEvent += OnMouseDragEvent;
            MouseButtonReleaseEvent += OnMouseReleaseEvent;
            MouseDoubleClickEvent += OnMouseDoubleClickEvent;

            BuildGeometry(new Color(0, 200, 120, 120));
        }

        private void BuildGeometry(Color color)
        {
            GeometryBuilder builder = new GeometryBuilder();
            builder.Color = color;
            builder.AddEllipse(new Vector2(0, 0), 50, 50);
            Geometry = builder.Build();
        }

        private void OnMousePressEvent(MouseButtonPressEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                e.Accept();
            }
            else
            {
                e.Decline();
            }
        }

        private void OnMouseDragEvent(MouseButtonDragEvent e)
        {
            Position += e.Delta;
        }

        private void OnMouseReleaseEvent(MouseButtonReleaseEvent e)
        {
        }

        private void OnMouseDoubleClickEvent(MouseDoubleClickEvent e)
        {
            Random rnd = new Random();
            BuildGeometry(new Color(rnd.Next(255), rnd.Next(255), rnd.Next(255), 120));
        }
    }
}
