using System;
using OpenTK.Graphics.OpenGL;


namespace ZigZag.OpenGL
{
    public class VertexArrayObject
    {
        public VertexArrayObject()
        {
            m_vertexArray = GL.GenVertexArray();

            if (m_vertexArray == 0)
            {
                throw new Exception("Failed to create vertex array object.");
            }
        }

        public void Bind()
        {
            if (m_vertexArray == 0)
            {
                throw new Exception("Vertex array object was not succesfully allocated, or has already been released.");
            }
            GL.BindVertexArray(m_vertexArray);
        }

        public void Release()
        {
            if (m_vertexArray == 0)
            {
                throw new Exception("Vertex array object was not succesfully allocated, or has already been released.");
            }
            GL.DeleteVertexArray(m_vertexArray);
            m_vertexArray = 0;
        }

        public void SetFloatAttribute(
            int index,
            int componentCount,
            BufferObject buffer,
            VertexAttribPointerType bufferDataType,
            int strideInBytes = 0,
            int offsetInBufferInBytes = 0)
        {
            SetFloatAttribute(index, componentCount, buffer, bufferDataType, false, strideInBytes, offsetInBufferInBytes);
        }

        public void SetFloatAttribute(
            int index,
            int componentCount,
            BufferObject buffer,
            VertexAttribPointerType bufferDataType,
            bool normalize = false,
            int strideInBytes = 0,
            int offsetInBufferInBytes = 0)
        {
            if (buffer.Target != BufferTarget.ArrayBuffer)
            {
                throw new ArgumentException("Invalid buffer target.");
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
            if (buffer.Target != BufferTarget.ArrayBuffer)
            {
                throw new ArgumentException("Invalid buffer target.");
            }
            Bind();
            buffer.Bind();
            GL.VertexAttribIPointer(index, componentCount, bufferDataType, strideInBytes, (IntPtr)offsetInBufferInBytes);
            GL.EnableVertexAttribArray(index);
        }

        public void SetIndexBuffer(BufferObject buffer)
        {
            if (buffer.Target != BufferTarget.ElementArrayBuffer)
            {
                throw new ArgumentException("Invalid buffer target.");
            }
            Bind();
            buffer.Bind();
        }

        private int m_vertexArray = 0;
    }
}
