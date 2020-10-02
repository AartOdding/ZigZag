using OpenTK.Graphics.OpenGL;
using System;
using System.Runtime.InteropServices;

namespace ZigZag.Plugins.OpenGL
{

    internal class NativeGui
    {
        [DllImport("ZigZagEditorNative.dll")]
        public static extern IntPtr ZigZagGetProcAddress(string s);
    }

    public class NativeGlfwBinding : OpenTK.IBindingsContext
    {

        public IntPtr GetProcAddress(string procName)
        {
            return NativeGui.ZigZagGetProcAddress(procName);
        }
    }

    public class Texture : DataSource
    {

        public override void Load()
        {
            //GL.LoadBindings(new NativeGlfwBinding());

            m_textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, m_textureId);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, 640, 480, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            m_frameBufferId = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_frameBufferId);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, m_textureId, 0);
        }

        public override void UnLoad()
        {
            GL.DeleteFramebuffer(m_frameBufferId);
            GL.DeleteTexture(m_textureId);
        }

        public void BindTexture(int index)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + index);
            GL.BindTexture(TextureTarget.Texture2D, m_textureId);
        }

        public void BindFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, m_frameBufferId);
            GL.Viewport(0, 0, 640, 480);
        }

        private int m_textureId = 0;
        private int m_frameBufferId = 0;

    }
}
