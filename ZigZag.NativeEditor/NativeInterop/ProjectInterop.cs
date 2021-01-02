using System.Runtime.InteropServices;


namespace ZigZagEditor.NativeInterop
{
    static class ProjectInterop
    {
        public const string LibraryName = "ZigZagEditorNative";

        [DllImport(LibraryName)]
        public static extern void onProjectCreated(string name, ulong projectID);

        [DllImport(LibraryName)]
        public static extern void onProjectCleared();
    }
}
