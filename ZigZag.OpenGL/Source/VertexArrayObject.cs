using System;
using OpenTK.Graphics.OpenGL;


namespace ZigZag.OpenGL
{
    public class VertexArrayObject
    {
        public void Bind()
        {
            if (m_released)
            {
                throw new Exception("Vertex array object has been released");
            }
            if (m_vertexArray == 0)
            {
                m_vertexArray = GL.GenVertexArray();
            }
            if (m_vertexArray == 0)
            {
                throw new Exception("Failed to create vertex array object");
            }
            GL.BindVertexArray(m_vertexArray);
        }

        public void Release()
        {
            if (m_released)
            {
                throw new Exception("Vertex array object has been released already");
            }
            if (m_vertexArray != 0)
            {
                GL.DeleteVertexArray(m_vertexArray);
                m_vertexArray = 0;
                m_released = true;
            }
        }

        public void AttributeFloat(
            int index,
            int componentCount,
            BufferObject buffer,
            VertexAttribPointerType bufferDataType,
            int strideInBytes = 0,
            int offsetInBufferInBytes = 0)
        {
            AttributeFloat(index, componentCount, buffer, bufferDataType, strideInBytes, offsetInBufferInBytes);
        }

        public void AttributeFloat(
            int index,
            int componentCount,
            BufferObject buffer,
            VertexAttribPointerType bufferDataType,
            bool normalize = false,
            int strideInBytes = 0,
            int offsetInBufferInBytes = 0)
        {
            if (buffer is null || buffer.Target != BufferTarget.ArrayBuffer)
            {
                throw new ArgumentException("Invalid buffer.");
            }
            Bind();
            buffer.Bind();
            GL.VertexAttribPointer(index, componentCount, bufferDataType, normalize, strideInBytes, offsetInBufferInBytes);
            GL.EnableVertexAttribArray(index);
        }

        public void AttributeInt(
            int index,
            int componentCount,
            BufferObject buffer,
            VertexAttribIntegerType bufferDataType,
            int strideInBytes = 0,
            int offsetInBufferInBytes = 0)
        {
            if (buffer is null || buffer.Target != BufferTarget.ArrayBuffer)
            {
                throw new ArgumentException("Invalid buffer.");
            }
            Bind();
            buffer.Bind();
            GL.VertexAttribIPointer(index, componentCount, bufferDataType, strideInBytes, (IntPtr)offsetInBufferInBytes);
            GL.EnableVertexAttribArray(index);
        }

        private int m_vertexArray;
        private bool m_released = false;
    }
}
