using System;
using System.Runtime.InteropServices;


namespace ZigZagEditor.NativeInterop
{
    class MainloopInterop
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
        public static extern IntPtr ZigZagGetProcAddress(string procedure);
    }
}
