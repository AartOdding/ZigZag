using System;
using System.Reflection;
using System.Runtime.InteropServices;


namespace ZigZagEditor
{

    static class ZigZagEditorNative
    {
        public const string LibraryName = "ZigZagEditorNative";

        [DllImport(LibraryName)]
        public static extern void initialize();


        [DllImport(LibraryName)]
        public static extern void render();


        [DllImport(LibraryName)]
        public static extern void shutdown();


        [DllImport(LibraryName)]
        public static extern bool shouldQuit();


        [DllImport(LibraryName)]
        public static extern IntPtr ZigZagGetProcAddress(string s);
    }
}
