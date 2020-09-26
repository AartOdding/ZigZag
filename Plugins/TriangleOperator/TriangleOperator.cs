using System;
using System.IO;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using ZigZag.Plugins;

namespace TriangleOperator
{
    public class TriangleOperator : ZigZag.Operator
    {
        private readonly float[] m_vertices = {
            -0.5f, -0.5f, //Bottom-left vertex
             0.5f, -0.5f, //Bottom-right vertex
             0.0f,  0.5f  //Top vertex
        };

        private int m_vbo;
        private int m_vao;
        private int m_shader;

        private readonly ZigZag.Plugins.OpenGL.Texture m_outputTexture;

        public TriangleOperator()
        {
            m_outputTexture = new ZigZag.Plugins.OpenGL.Texture();
            m_outputTexture.SetParent(this);
        }

        public override void Load()
        {
            m_outputTexture.Load();

            m_vao = GL.GenVertexArray();
            GL.BindVertexArray(m_vao);

            m_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, m_vertices.Length * sizeof(float), m_vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader,
 @"
#version 330

layout(location = 0) in vec2 vertex;

void main()
{
    gl_Position = vec4(vertex, 0.0, 1.0);
}
");
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader,
@"
#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
} 
");
            GL.CompileShader(fragmentShader);

            m_shader = GL.CreateProgram();
            GL.AttachShader(m_shader, vertexShader);
            GL.AttachShader(m_shader, fragmentShader);
            GL.LinkProgram(m_shader);
        }

        public override void UnLoad()
        {
            GL.DeleteVertexArray(m_vao);
            GL.DeleteBuffer(m_vbo);
        }

        public override void Execute()
        {
            //Console.WriteLine("hello");
            m_outputTexture.BindFrameBuffer();

            GL.ClearColor(0, 0, 0.5f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(m_shader);
            GL.BindVertexArray(m_vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }

    }
}
