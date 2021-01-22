using System;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using ZigZag.OpenGL;
using System.IO;


namespace ZigZag.Editor
{
    public static class ImGuiRendererIntegration
    {
        public static void Initialize()
        {
            m_shader = new Shader(Resources.Shaders.ImGuiVertexShaderSource, Resources.Shaders.ImGuiFragmentShaderSource);
            m_shader.SetInt("active_texture", 0);

            m_vertexBuffer = new BufferObject(BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);
            m_indexBuffer = new BufferObject(BufferTarget.ElementArrayBuffer, BufferUsageHint.StreamDraw);

            m_vertexArray = new VertexArrayObject();
            m_vertexArray.SetFloatAttribute(0, 2, m_vertexBuffer, VertexAttribPointerType.Float, 20, 0);
            m_vertexArray.SetFloatAttribute(1, 2, m_vertexBuffer, VertexAttribPointerType.Float, 20, 8);
            m_vertexArray.SetFloatAttribute(2, 4, m_vertexBuffer, VertexAttribPointerType.UnsignedByte, true, 20, 16);
            m_vertexArray.SetIndexBuffer(m_indexBuffer);
        }

        public static void Shutdown()
        {
            m_vertexArray.Release();
            m_vertexBuffer.Release();
            m_indexBuffer.Release();
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

            m_vertexArray.Bind();

            for (int i = 0; i < drawData.CmdListsCount; ++i)
            {
                var cmdList = drawData.CmdListsRange[i];

                m_vertexBuffer.SetData<ImDrawVert>(cmdList.VtxBuffer.Data, cmdList.VtxBuffer.Size);
                m_indexBuffer.SetData<ushort>(cmdList.IdxBuffer.Data, cmdList.IdxBuffer.Size);

                for (int j = 0; j < cmdList.CmdBuffer.Size; ++j)
                {
                    var cmd = cmdList.CmdBuffer[j];

                    int elemCount = (int)cmd.ElemCount;
                    int offset = (int)cmd.IdxOffset;
                    GL.DrawElements(PrimitiveType.Triangles, elemCount, DrawElementsType.UnsignedShort, offset * sizeof(ushort));
                }
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
        private static VertexArrayObject m_vertexArray;
        private static BufferObject m_vertexBuffer;
        private static BufferObject m_indexBuffer;
        private static int m_fontsTextureHandle = 0;
    }
}
