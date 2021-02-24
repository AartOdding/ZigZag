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

            builder.Color = new Color(35, 35, 35, 255);
            builder.FillMode = FillMode.Filled;
            builder.AddRectangle(new Rectangle(-100, -70, 200, 140));

            builder.LinePlacement = LinePlacement.Centered;
            builder.Color = new Color(60, 60, 60, 255);
            builder.FillMode = FillMode.Outline;
            builder.AddRectangle(new Rectangle(-100, -70, 200, 140));

            Geometry = builder.Build();

            var i1 = new NodeConnectorWidget(true, 1);
            var i2 = new NodeConnectorWidget(true, 1);
            i1.Position = new Vector2(-105, -20);
            i2.Position = new Vector2(-105, 20);
            AddChild(i1);
            AddChild(i2);

            var o1 = new NodeConnectorWidget(false, 1);
            var o2 = new NodeConnectorWidget(false, 1);
            o1.Position = new Vector2(105, -20);
            o2.Position = new Vector2(105, 20);
            AddChild(o1);
            AddChild(o2);
        }

        public override bool Contains(Vector2 point)
        {
            return base.Contains(point) || GetChildrenAt(point).Count > 0;
        }

        private Dragger dragger;
    }
}
