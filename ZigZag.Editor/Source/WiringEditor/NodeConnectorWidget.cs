using ZigZag.Mathematics;
using ZigZag.SceneGraph;


namespace ZigZag.Editor.WiringEditor
{
    class NodeConnectorWidget : GeometryNode
    {
        public NodeConnectorWidget(bool isLeftSide, int type)
        {
            m_leftSide = isLeftSide;
            m_type = type;

            GeometryBuilder builder = new GeometryBuilder();

            builder.LinePlacement = LinePlacement.Centered;
            builder.Color = new Color(60, 60, 60, 255);
            builder.FillMode = FillMode.Outline;
            builder.AddRectangle(new Rectangle(-5, -8, 10, 16));

            Geometry = builder.Build();

            MouseButtonPressEvent += OnMousePressEvent;
            MouseButtonDragEvent += OnMouseDragEvent;
        }

        private void OnMousePressEvent(MouseButtonPressEvent e)
        {
            e.SetAccepted(e.Button == MouseButton.Left);
        }

        private void OnMouseDragEvent(MouseButtonDragEvent e)
        {
            System.Console.WriteLine("drag");
        }

        private bool m_leftSide;
        private int m_type;
    }
}
