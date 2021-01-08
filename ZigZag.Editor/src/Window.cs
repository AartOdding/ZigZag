using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using ImGuiNET;


namespace ZigZag.Editor
{
    public class Window : GameWindow
    {
        private readonly float[] m_vertices =
        {
            -0.5f, -0.5f, 0.0f, // Bottom-left vertex
             0.5f, -0.5f, 0.0f, // Bottom-right vertex
             0.0f,  0.5f, 0.0f  // Top vertex
        };

        private int m_vertexBufferObject;
        private int m_vertexArrayObject;

        private IntPtr m_imguiContext;

        private Shader m_shader;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            m_shader = new Shader(Resources.Shaders.ImGuiVertexShaderSource, Resources.Shaders.ImGuiFragmentShaderSource);
            m_shader.Use();

            m_vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(m_vertexArrayObject);

            m_vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, m_vertices.Length * sizeof(float), m_vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            m_imguiContext = ImGui.CreateContext();
            ImGui.SetCurrentContext(m_imguiContext);

            ImGui.GetIO().Fonts.AddFontDefault();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            m_shader.Use();
            GL.BindVertexArray(m_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(m_vertexBufferObject);
            GL.DeleteVertexArray(m_vertexArrayObject);

            GL.DeleteProgram(m_shader.Handle);
        }
    }
}
