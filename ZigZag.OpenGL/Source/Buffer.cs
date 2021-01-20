using System;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;


namespace ZigZag.OpenGL
{
    public class Buffer
    {
        public Buffer(
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
                throw new Exception("Cannot bind released object.");
            }
            if (m_buffer == 0)
            {
                m_buffer = GL.GenBuffer();
            }
            if (m_buffer == 0)
            {
                throw new Exception("Failed to create Buffer Object");
            }
            GL.BindBuffer(Target, m_buffer);
        }

        public void Release()
        {
            if (m_released)
            {
                throw new Exception("Cannot release object twice");
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

        // Important: dataCount is the amount of objects, not bytes
        public void SetData<T>(ref T data, int dataCount) where T : struct
        {
            Bind();
            if (m_buffer == 0)
            {
                int sizeOfT = Marshal.SizeOf(typeof(T));
                GL.BufferData(Target, dataCount * sizeOfT, ref data, Usage);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private int m_buffer = 0;
        private int m_bufferSizeBytes = 0;
        private bool m_released = false;
    }
}
