using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public class GeometryBuilder
    {
        public Color Color { get; set; } = new Color(1, 1, 1, 1);
        public FillMode FillMode { get; set; } = FillMode.Filled;
        public LinePlacement LinePlacement { get; set; } = LinePlacement.Centered;
        public float LineWidth { get; set; } = 3;

        public void AddRectangle(Rectangle rect)
        {
            if (FillMode == FillMode.Filled)
            {
                AddRectangleFilled(rect);
            }
            else if (FillMode == FillMode.Outline)
            {
                AddRectangleOutline(rect);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void AddEllipse(Vector2 position, float width, float height, int segments = 32)
        {
            if (FillMode == FillMode.Filled)
            {
                AddEllipseFilled(position, width, height, segments);
            }
            else if (FillMode == FillMode.Outline)
            {
                //AddEllipseOutline(position, width, height, segments);
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void AddRoundedRectangle(Rectangle rect, float roundingRadius)
        {

        }

        public Geometry Build()
        {
            return new Geometry(
                m_vertices,
                m_indices,
                m_vertexCounts);
        }

        public void Clear()
        {
            m_vertices.Clear();
            m_indices.Clear();
            m_vertexCounts.Clear();
        }

        private void AddRectangleFilled(Rectangle rect)
        {
            var startCount = m_vertices.Count;
            var color = Color.U32();
            var tl = AddVertex(rect.TopLeft(), color);
            var tr = AddVertex(rect.TopRight(), color);
            var bl = AddVertex(rect.BottomLeft(), color);
            var br = AddVertex(rect.BottomRight(), color);
            AddTriangleFromIndices(tl, tr, bl);
            AddTriangleFromIndices(tr, br, bl);
            m_vertexCounts.Add((uint)(m_vertices.Count - startCount));
        }

        private void AddRectangleOutline(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        private void AddEllipseFilled(Vector2 centre, float width, float height, int segments)
        {
            var startCount = m_vertices.Count;
            var color = Color.U32();
            var widthOverTwo = width / 2;
            var heightOverTwo = height / 2;

            var sin = TrigTable.Sin(segments);
            var cos = TrigTable.Cos(segments);

            var vcentre = AddVertex(centre, color);
            var vtop = AddVertex(centre + new Vector2(heightOverTwo, 0), color);
            var vprev = vtop;

            for (int i = 1; i < segments; ++i)
            {
                var vcurr = AddVertex(new Vector2(cos[i] * widthOverTwo + centre.X, sin[i] * heightOverTwo + centre.Y), color);
                AddTriangleFromIndices(vcentre, vprev, vcurr);
                vprev = vcurr;
            }
            AddTriangleFromIndices(vcentre, vprev, vtop);

            m_vertexCounts.Add((uint)(m_vertices.Count - startCount));
        }

        private void AddEllipseOutline(Vector2 centre, float width, float height)
        {
            throw new NotImplementedException();
        }

        private uint AddVertex(Vector2 pos, uint color)
        {
            m_vertices.Add(new Vertex2(pos, color));
            return (uint)(m_vertices.Count - 1);
        }

        private void AddTriangleFromIndices(uint a, uint b, uint c)
        {
            m_indices.Add(a);
            m_indices.Add(b);
            m_indices.Add(c);
        }

        private readonly List<Vertex2> m_vertices = new List<Vertex2>();
        private readonly List<uint> m_indices = new List<uint>();
        private readonly List<uint> m_vertexCounts = new List<uint>();
    }
}
