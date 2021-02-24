using ZigZag.Mathematics;
using ZigZag.SceneGraph;


namespace ZigZag.Editor.WiringEditor
{
    class NodeBlockWidget : GeometryNode
    {
        public NodeBlockWidget()
        {
            dragger = new Dragger(this);

            GeometryBuilder builder = new GeometryBuilder();

            builder.Color = new Color(20, 20, 60, 255);
            builder.FillMode = FillMode.Filled;
            builder.AddRectangle(new Rectangle(-100, -70, 200, 140));

            builder.LinePlacement = LinePlacement.Centered;
            builder.Color = new Color(160, 160, 160, 50);
            builder.FillMode = FillMode.Outline;
            builder.AddRectangle(new Rectangle(-100, -70, 200, 140));

            Geometry = builder.Build();
        }

        private Dragger dragger;
    }
}
