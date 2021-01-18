using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.SceneGraph;
using ZigZag.SceneGraph.Math;
using OpenTK.Graphics.OpenGL;


namespace ZigZag.Editor.SceneGraph
{
    class GeometryDrawData : IDisposable
    {
        public GeometryDrawData(ref Geometry geometry, int zStart)
        {
            m_geometry = geometry;
            m_vertexArray = GL.GenVertexArray();
            m_vertexBuffer = GL.GenBuffer();
            m_indexBuffer = GL.GenBuffer();
            m_vertexDepthBuffer = GL.GenBuffer();
        }

        private Geometry m_geometry;
        private int m_vertexArray;
        private int m_vertexBuffer;
        private int m_indexBuffer;
        private int m_vertexDepthBuffer;

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        ~GeometryDrawData()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
