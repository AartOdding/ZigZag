using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace ZigZag.OpenGL
{

    public readonly struct VertexAttribute
    {
        public VertexAttribute(
            int index, 
            int componentCount, 
            VertexAttribPointerType type, 
            Buffer buffer, 
            int strideInBytes = 0, 
            int offsetInBufferInBytes = 0)
        {

        }

        public VertexAttribute(
            int index,
            int componentCount,
            VertexAttribPointerType type,
            Buffer buffer,
            bool normalize,
            int strideInBytes = 0,
            int offsetInBufferInBytes = 0)
        {

        }
    }
}
