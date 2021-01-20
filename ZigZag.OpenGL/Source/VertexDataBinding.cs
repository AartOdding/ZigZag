using System;
using OpenTK.Graphics.OpenGL;


namespace ZigZag.OpenGL
{
    public class VertexDataBinding
    {
        public void Bind()
        {
            if (m_released)
            {
                throw new Exception("Cannot bind released object.");
            }
            if (m_vertexArray == 0)
            {
                m_vertexArray = GL.GenVertexArray();
            }
            if (m_vertexArray == 0)
            {
                throw new Exception("Failed to create Vertex Array Object");
            }
            GL.BindVertexArray(m_vertexArray);
        }

        public void Release()
        {
            if (m_released)
            {
                throw new Exception("Cannot release object twice");
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
            Buffer buffer,
            VertexAttribPointerType bufferDataType,
            int strideInBytes = 0,
            int offsetInBufferInBytes = 0)
        {
            AttributeFloat(index, componentCount, buffer, bufferDataType, strideInBytes, offsetInBufferInBytes);
        }

        public void AttributeFloat(
            int index,
            int componentCount,
            Buffer buffer,
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
            Buffer buffer,
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
