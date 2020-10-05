using System.Runtime.InteropServices;


namespace ZigZagEditor.NativeInterop
{
    static class ObjectInterop
    {
        public const string LibraryName = "ZigZagEditorNative";

        public delegate bool AddObjectDelegate(ulong objectTypeID, ulong parentObjectID);
        public delegate bool RemoveObjectDelegate(ulong objectID);

        [DllImport(LibraryName)]
        public static extern void installObjectDelegates(AddObjectDelegate add, RemoveObjectDelegate remove);

        [DllImport(LibraryName)]
        public static extern void onNewObjectTypeAdded(string name, ulong id, ObjectTypeCategory category);

        [DllImport(LibraryName)]
        public static extern void onObjectCreated(ulong newObjectID, ulong parentObjectID);

        [DllImport(LibraryName)]
        public static extern void onObjectDestroyed(ulong objectID);
    }
}
