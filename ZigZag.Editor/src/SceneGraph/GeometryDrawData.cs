using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.SceneGraph;
using ZigZag.Mathematics;
using ZigZag.OpenGL;
using OpenTK.Graphics.OpenGL;


namespace ZigZag.Editor.SceneGraph
{
    class GeometryDrawData
    {
        public GeometryDrawData(ref Geometry geometry, uint zStart)
        {
            m_vertexArray = new VertexArrayObject();
            m_vertexBuffer = new BufferObject(BufferTarget.ArrayBuffer);
            m_indexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer);
            /*
            List<Vertex3> vertices = new List<Vertex3>(geometry.Vertices.Count);

            if (geometry.VertexCounts.Count > 0)
            {
                uint currentGroup = 0;
                uint groupEnd = geometry.VertexCounts[0];

                for (int i = 0; i < geometry.Vertices.Count; ++i)
                {
                    if (i >= groupEnd)
                    {
                        // increment groupEnd with the size of the new current group
                        groupEnd += geometry.VertexCounts[(int)++currentGroup];
                    }
                    vertices.Add(new Vertex3(geometry.Vertices[i], zStart + currentGroup));
                }
            }*/

            m_vertexBuffer.SetData(ref geometry.FirstVertex, geometry.Vertices.Count);
            m_indexBuffer.SetData(ref geometry.FirstIndex, geometry.Indices.Count);
            m_vertexArray.SetAttribute(0, 2, AttributeMapping.FloatToFloat, m_vertexBuffer, 12, 0);
            m_vertexArray.SetAttribute(1, 4, AttributeMapping.UInt8ToFloatNormalized, m_vertexBuffer, 12, 8);
            m_vertexArray.SetIndexBuffer(m_indexBuffer);

            VertexCount = geometry.Vertices.Count;
            IndexCount = geometry.Indices.Count;
        }

        public void Bind()
        {
            m_vertexArray.Bind();
        }

        public void Release()
        {
            m_vertexArray.Release();
            m_vertexBuffer.Release();
            m_indexBuffer.Release();
        }

        public int VertexCount { get; }
        public int IndexCount { get; }

        private VertexArrayObject m_vertexArray;
        private BufferObject m_vertexBuffer;
        private BufferObject m_indexBuffer;
    }
}
