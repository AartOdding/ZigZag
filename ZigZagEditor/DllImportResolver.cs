using System;
using System.Reflection;
using System.Runtime.InteropServices;


namespace ZigZagEditor
{
    static class DllImportResolver
    {
        public static void Install()
        {
            NativeLibrary.SetDllImportResolver(typeof(DllImportResolver).Assembly, ImportResolver);
        }

        private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == ZigZagEditorNative.LibraryName)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return NativeLibrary.Load("../../../native/build_output/Debug/ZigZagEditorNative.dll");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return NativeLibrary.Load("../../../native/build_output/Debug/ZigZagEditorNative.so");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return NativeLibrary.Load("../../../native/build_output/Debug/ZigZagEditorNative.dylib");
                }
                else
                {
                    return IntPtr.Zero;
                }
            }
            else
            {
                return NativeLibrary.Load(libraryName, assembly, searchPath);
            }
        }
    }
}
