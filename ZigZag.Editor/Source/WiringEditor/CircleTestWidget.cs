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

            GeometryBuilder builder = new GeometryBuilder();
            builder.Color = new Color(0, 200, 120, 120);
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
                e.Ignore();
            }
        }

        private void OnMouseDragEvent(MouseButtonDragEvent e)
        {
            Position += e.Delta;
        }

        private void OnMouseReleaseEvent(MouseButtonReleaseEvent e)
        {
        }
    }
}
