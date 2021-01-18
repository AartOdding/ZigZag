using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (m_buffer == 0)
            {
                m_buffer = GL.GenBuffer();
            }
            GL.BindBuffer(Target, m_buffer);
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

        public void CleanUp()
        {

        }

        private int m_buffer = 0;
        private int m_bufferSizeBytes = 0;
    }
}
