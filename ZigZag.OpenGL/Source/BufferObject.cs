using System;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;


namespace ZigZag.OpenGL
{
    public class BufferObject
    {
        public BufferObject(
            BufferTarget target = BufferTarget.ArrayBuffer, 
            BufferUsageHint usage = BufferUsageHint.DynamicDraw)
        {
            Target = target;
            Usage = usage;

            m_buffer = GL.GenBuffer();

            if (m_buffer == 0)
            {
                throw new Exception("Failed to create buffer object.");
            }
        }

        public BufferTarget Target
        {
            get;
        }

        public BufferUsageHint Usage
        {
            get;
        }

        public void Bind()
        {
            if (m_buffer == 0)
            {
                throw new Exception("Buffer object was not succesfully allocated, or has already been released.");
            }
            GL.BindBuffer(Target, m_buffer);
        }

        public void Release()
        {
            if (m_buffer == 0)
            {
                throw new Exception("Buffer object was not succesfully allocated, or has already been released.");
            }
            GL.DeleteBuffer(m_buffer);
            m_buffer = 0;
        }

        public void Allocate(int sizeInBytes)
        {
            Bind();
            GL.BufferData(Target, sizeInBytes, IntPtr.Zero, Usage);
            m_bufferSizeBytes = sizeInBytes;
        }

        public void SetData<T>(ref T firstValue, int valueCount) where T : struct
        {
            Bind();
            int totalSize = valueCount * Marshal.SizeOf(typeof(T));

            if (m_bufferSizeBytes < totalSize)
            {
                GL.BufferData(Target, totalSize, ref firstValue, Usage);
                m_bufferSizeBytes = totalSize;
            }
            else
            {
                GL.BufferSubData(Target, IntPtr.Zero, totalSize, ref firstValue);
            }
        }

        public void SetData<T>(IntPtr address, int valueCount) where T : struct
        {
            Bind();
            int totalSize = valueCount * Marshal.SizeOf(typeof(T));

            if (m_bufferSizeBytes < totalSize)
            {
                GL.BufferData(Target, totalSize, address, Usage);
                m_bufferSizeBytes = totalSize;
            }
            else
            {
                GL.BufferSubData(Target, IntPtr.Zero, totalSize, address);
            }
        }

        public void SetData(IntPtr address, int sizeInBytes)
        {
            Bind();

            if (m_bufferSizeBytes < sizeInBytes)
            {
                GL.BufferData(Target, sizeInBytes, address, Usage);
                m_bufferSizeBytes = sizeInBytes;
            }
            else
            {
                GL.BufferSubData(Target, IntPtr.Zero, sizeInBytes, address);
            }
        }

        private int m_buffer = 0;
        private int m_bufferSizeBytes = 0;
    }
}
