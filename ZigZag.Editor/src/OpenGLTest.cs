using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;


namespace ZigZag.Editor
{
    class OpenGLTest
    {
        public void Setup()
        {
            string vertSource = File.ReadAllText("../../../Resources/Shaders/test.vert.txt");
            string fragSource = File.ReadAllText("../../../Resources/Shaders/test.frag.txt");
            m_shader = new Shader(vertSource, fragSource);

            var verts = new float[] { 30, 200, 300, 70, 400, 400 };

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, 6 * 4, verts, BufferUsageHint.DynamicDraw);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            
            GL.BindVertexArray(0);
        }
        
        public void Draw(int w, int h)
        {
            //var mat = Matrix4.CreateOrthographicOffCenter(0, w, h, 0, -1, 1);
            //var mat = Matrix4.CreateOrthographic(w, h, 0, 1);

            m_shader.Use();
            m_shader.SetVector2("viewport_min", new Vector2(0, 0));
            m_shader.SetVector2("viewport_max", new Vector2(w, h));
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 3);
        }

        Shader m_shader;
        int vao, vbo;

    }
}
