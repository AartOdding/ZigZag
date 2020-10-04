using System;
using System.Reflection;
using System.Runtime.InteropServices;


namespace ZigZagEditor
{
    static class ImportResolver
    {
        public static void Install()
        {
            NativeLibrary.SetDllImportResolver(typeof(ImportResolver).Assembly, Resolve);
        }

        private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == ZigZagEditor.NativeInterop.MainloopInterop.LibraryName)
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
