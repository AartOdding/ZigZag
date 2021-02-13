using System;
using System.Collections.Generic;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public readonly struct Geometry
    {
        public Geometry(
            List<Vertex2> vertices,
            List<uint> indices,
            List<uint> vertexCounts)
        {
            m_vertices = vertices.ToArray();
            m_indices = indices.ToArray();
            m_vertexCounts = vertexCounts.ToArray();

            if (vertices.Count == 0)
            {
                m_boundingBox = new Rectangle();
            }
            else
            {
                float minX = float.MaxValue;
                float minY = float.MaxValue;
                float maxX = float.MinValue;
                float maxY = float.MinValue;

                foreach (var vertex in vertices)
                {
                    minX = Math.Min(minX, vertex.PosX);
                    minY = Math.Min(minY, vertex.PosY);
                    maxX = Math.Max(maxX, vertex.PosX);
                    maxY = Math.Max(maxY, vertex.PosY);
                }

                m_boundingBox = new Rectangle(minX, minY, maxX - minX, maxY - minY);
            }
        }

        public readonly IReadOnlyList<Vertex2> Vertices
        {
            get
            {
                return m_vertices;
            }
        }
        public readonly IReadOnlyList<uint> Indices
        {
            get
            {
                return m_indices;
            }
        }
        public readonly IReadOnlyList<uint> VertexCounts
        {
            get
            {
                return m_vertexCounts;
            }
        }

        public readonly Rectangle BoundingBox
        {
            get
            {
                return m_boundingBox;
            }
        }

        public readonly ref Vertex2 FirstVertex
        {
            get
            {
                return ref m_vertices[0];
            }
        }

        public readonly ref uint FirstIndex
        {
            get
            {
                return ref m_indices[0];
            }
        }

        private readonly Vertex2[] m_vertices;
        private readonly uint[] m_indices;
        private readonly uint[] m_vertexCounts;
        private readonly Rectangle m_boundingBox;
    }
}
