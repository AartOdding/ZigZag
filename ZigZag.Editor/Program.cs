using System;
using System.IO;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL4;

using ZigZagEditor.NativeInterop;

namespace ZigZagEditor
{
    public class NativeGlfwBinding : OpenTK.IBindingsContext
    {
        public IntPtr GetProcAddress(string procName)
        {
            return MainloopInterop.ZigZagGetProcAddress(procName);
        }
    }

    class Program
    {
        static bool AddObject(ulong objectTypeID, ulong parentObjectID)
        {
            return false;
        }

        static bool RemoveObject(ulong objectID)
        {
            return false;
        }

        static void Main(string[] args)
        {
            ulong projectID = Identifier.Create();

            ImportResolver.Install();

            MainloopInterop.initialize();
            ObjectInterop.installObjectDelegates(AddObject, RemoveObject);
            var str = "project";
            ProjectInterop.onProjectCreated(str, projectID);

            /*
            What you want to do is call GL.LoadBindings with an IBindingsContext. 
            Maybe the built in GLFWBindingsContext will work, otherwise it's pretty easy to implement
            */
            //OpenTK.Windowing.GraphicsLibraryFramework.GLFWBindingsContext c = new OpenTK.Windowing.GraphicsLibraryFramework.GLFWBindingsContext();
            GL.LoadBindings(new NativeGlfwBinding());

            var m_textureId = GL.GenTexture();
            var m_textureId2 = GL.GenTexture();
            var m_textureId3 = GL.GenTexture();
            var m_textureId4 = GL.GenTexture();
            var m_textureId5 = GL.GenTexture();
            GL.ClearColor(1, 1, 1, 1);

            //OpenTK.Windowing.GraphicsLibraryFramework.ContextApi.

            var rootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            var pluginDir = Path.Combine(rootDir.FullName, "plugins");

            var netCore31Debug = Path.Combine("bin", "Debug", "netcoreapp3.1");

            var pluginTextureRootDir = Path.Combine(pluginDir, "OpenGL", "Texture");
            var pluginTextureDir = Path.Combine(pluginTextureRootDir, netCore31Debug, "Texture.dll");
            var pluginTriangleDir = Path.Combine(pluginDir, "TriangleOperator", netCore31Debug, "TriangleOperator.dll");

            PluginLoader.Load(pluginTextureDir);
            PluginLoader.Load(pluginTriangleDir);

            //var triangle = (ZigZag.Operator)Activator.CreateInstance(pluginTriangle.m_operators[0]);

            /*            GameWindowSettings gws = new GameWindowSettings();
                        gws.RenderFrequency = 60;
                        gws.UpdateFrequency = 60;
                        gws.IsMultiThreaded = false;

                        NativeWindowSettings nws = new NativeWindowSettings();
                        nws.API = ContextAPI.OpenGL;
                        nws.Profile = ContextProfile.Core;
                        nws.Flags = ContextFlags.ForwardCompatible;
                        nws.APIVersion = new Version(3, 3);*/

            //triangle.Load();

            while (true)
            {
                MainloopInterop.render();

                //triangle.Execute();

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

                if (MainloopInterop.shouldQuit())
                {
                    break;
                }
            }

            //triangle.UnLoad();

            MainloopInterop.shutdown();


            //Window w = new Window(gws, nws);
            //w.op = (ZigZag.Operator)triangle;
            //w.Run();
        }
    }
}
