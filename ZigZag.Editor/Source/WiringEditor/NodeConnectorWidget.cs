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

            Build(new Color(60, 60, 60, 255));

            MouseButtonPressEvent += OnMousePressEvent;
            MouseButtonDragEvent += OnMouseDragEvent;
            HoverEnterEvent += OnHoverEnter;
            HoverLeaveEvent += OnHoverLeave;
        }

        private void Build(Color color)
        {
            GeometryBuilder builder = new GeometryBuilder();

            builder.LinePlacement = LinePlacement.Centered;
            builder.Color = color;
            builder.FillMode = FillMode.Outline;
            builder.AddRectangle(new Rectangle(-5, -8, 10, 16));

            Geometry = builder.Build();
        }

        private void OnHoverEnter(HoverEnterEvent e)
        {
            Build(new Color(100, 60, 60, 255));
        }

        private void OnHoverLeave(HoverLeaveEvent e)
        {
            Build(new Color(60, 60, 60, 255));
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
