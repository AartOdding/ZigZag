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
            if (m_released)
            {
                throw new Exception("Buffer object has been released");
            }
            if (m_buffer == 0)
            {
                m_buffer = GL.GenBuffer();
            }
            if (m_buffer == 0)
            {
                throw new Exception("Failed to create buffer object");
            }
            GL.BindBuffer(Target, m_buffer);
        }

        public void Release()
        {
            if (m_released)
            {
                throw new Exception("Buffer object has been released already");
            }
            if (m_buffer != 0)
            {
                GL.DeleteBuffer(m_buffer);
                m_buffer = 0;
                m_released = true;
            }
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

        private int m_buffer = 0;
        private int m_bufferSizeBytes = 0;
        private bool m_released = false;
    }
}
