using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;


namespace ZigZag.OpenGL
{
    public class VertexData
    {

        public void Attribute(
            int index,
            int componentCount,
            VertexAttribPointerType type,
            Buffer buffer,
            int strideInBytes = 0,
            int offsetInBufferInBytes = 0)
        {

        }

        public void Attribute(
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
