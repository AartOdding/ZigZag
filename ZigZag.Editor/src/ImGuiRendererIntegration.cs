using System;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using System.IO;


namespace ZigZag.Editor
{
    public static class ImGuiRendererIntegration
    {
        public static void Initialize()
        {
            string vertSource = File.ReadAllText("../../../Resources/Shaders/imgui.vert.txt");
            string fragSource = File.ReadAllText("../../../Resources/Shaders/imgui.frag.txt");

            m_shader = new Shader(vertSource, fragSource);
            m_shader.SetInt("active_texture", 0);

            m_vertexArray = GL.GenVertexArray();
            m_vertexBuffer = GL.GenBuffer();
            m_indexBuffer = GL.GenBuffer();

            GL.BindVertexArray(m_vertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_vertexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_indexBuffer);

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float,        false, 20, 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float,        false, 20, 8);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.UnsignedByte, true,  20, 16);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            GL.BindVertexArray(0);
        }

        public static void Shutdown()
        {

        }

        public static void Render(ImDrawDataPtr drawData, int w, int h)
        {
            var io = ImGui.GetIO();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            m_shader.Use();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, m_fontsTextureHandle);

            m_shader.SetVector2("viewport_min", new Vector2(0, 0));
            m_shader.SetVector2("viewport_max", new Vector2(w, h));

            GL.BindVertexArray(m_vertexArray);

            for (int i = 0; i < drawData.CmdListsCount; ++i)
            {
                var cmdList = drawData.CmdListsRange[i];

                GL.BindBuffer(BufferTarget.ArrayBuffer, m_vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, 
                    cmdList.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(),
                    cmdList.VtxBuffer.Data, BufferUsageHint.DynamicDraw);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_indexBuffer);
                GL.BufferData(BufferTarget.ElementArrayBuffer, 
                    cmdList.IdxBuffer.Size * sizeof(ushort),
                    cmdList.IdxBuffer.Data, BufferUsageHint.DynamicDraw);

                for (int j = 0; j < cmdList.CmdBuffer.Size; ++j)
                {
                    var cmd = cmdList.CmdBuffer[j];

                    int elemCount = (int)cmd.ElemCount;
                    int offset = (int)cmd.IdxOffset;
                    GL.DrawElements(PrimitiveType.Triangles, elemCount, DrawElementsType.UnsignedShort, offset * sizeof(ushort));
                }

                //cmdList.VtxBuffer.Size



                //var d = drawList.VtxBuffer

                //drawList.VtxBuffer[0].
            }
        }

        public static void CreateFontsTexture()
        {
            DeleteFontsTexture();

            //ImGui.GetIO().Fonts.GetTexDataAsAlpha8(out IntPtr pixels, out int width, out int height);
            ImGui.GetIO().Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height);

            m_fontsTextureHandle = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, m_fontsTextureHandle);
            //GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, width, height, 0, PixelFormat.Red, PixelType.UnsignedByte, pixels);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public static void DeleteFontsTexture()
        {
            if (m_fontsTextureHandle != 0)
            {
                GL.DeleteTexture(m_fontsTextureHandle);
                m_fontsTextureHandle = 0;
            }
        }

        private static Shader m_shader;
        private static int m_fontsTextureHandle = 0;
        private static int m_vertexArray = 0;
        private static int m_vertexBuffer = 0;
        private static int m_indexBuffer = 0;
    }
}
